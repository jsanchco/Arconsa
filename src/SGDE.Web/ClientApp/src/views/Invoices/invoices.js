import React, { Component, Fragment } from "react";
import { Breadcrumb, BreadcrumbItem, Container, Row } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Edit,
  Inject,
  Toolbar,
  Page,
  Resize,
  DetailRow,
  Aggregate,
} from "@syncfusion/ej2-react-grids";
import {
  createSpinner,
  showSpinner,
  hideSpinner,
} from "@syncfusion/ej2-popups";
import { DataManager, WebApiAdaptor } from "@syncfusion/ej2-data";
import {
  config,
  INVOICES,
  CLIENTSLITE,
  WORKSLITE,
  WORKBUDGETSLITE,
  DETAILSINVOICE,
} from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import {
  TOKEN_KEY,
  getInvoice,
  base64ToArrayBuffer,
  saveByteArray,
  printInvoice,
} from "../../services";
import { Query } from "@syncfusion/ej2-data";
import { DropDownList } from "@syncfusion/ej2-dropdowns";

L10n.load(data);

class Invoices extends Component {
  invoices = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${INVOICES}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  clients = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${CLIENTSLITE}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  works = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${WORKSLITE}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  workBudgets = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${WORKBUDGETSLITE}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  detailsInvoice = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${DETAILSINVOICE}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  gridInvoice = null;

  requeridIdRules = { required: true };

  wrapSettings = { wrapMode: "Content" };

  constructor(props) {
    super(props);

    this.clientsElem = null;
    this.clientsObj = null;
    this.worksElem = null;
    this.worksObj = null;
    this.workBudgetsElem = null;
    this.worksBudgetObj = null;

    this.toolbarOptions = [
      "Add",
      "Edit",
      "Delete",
      "Update",
      "Cancel",
      // {
      //   text: "Detalles",
      //   tooltipText: "Detalles",
      //   prefixIcon: "e-custom-icons e-details",
      //   id: "Details",
      // },
      "Print",
      {
        text: "Imprimir Factura",
        tooltipText: "Imprimir Factura",
        prefixIcon: "e-custom-icons e-print",
        id: "PrintInvoice",
      },
      "Search",
    ];
    this.editSettings = {
      showDeleteConfirmDialog: true,
      allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      newRowPosition: "Top",
    };
    this.pageSettings = {
      pageCount: 10,
      pageSize: 10,
      currentPage: props.currentPageInvoices,
    };
    this.searchSettings = {
      fields: ["name"],
      key: props.currentSearchInvoices,
    };
    this.numericParams = {
      params: {
        decimals: 2,
        format: "N",
        validateDecimalOnType: true,
        showSpinButton: false,
      },
    };
    this.formatDate = { type: "dateTime", format: "dd/MM/yyyy" };
    this.expandGridRow = null;
    this.rowSelectedInvoice = null;
    this.rowSelectedDetailsInvoice = null;
    this.editClients = {
      create: () => {
        this.clientsElem = document.createElement("input");
        return this.clientsElem;
      },
      destroy: () => {
        this.clientsObj.destroy();
      },
      read: () => {
        return this.clientsObj.value;
      },
      write: (args) => {
        this.clientsObj = new DropDownList({
          change: () => {
            this.worksObj.enabled = true;
            const tempQuery = new Query().addParams(
              "clientId",
              this.clientsObj.value
            );
            this.worksObj.query = tempQuery;
            this.worksObj.text = "";
            this.worksObj.value = null;
            this.workBudgetsObj.text = "";
            this.workBudgetsObj.value = null;
            this.worksObj.dataBind();
          },
          dataSource: new DataManager({
            adaptor: new WebApiAdaptor(),
            url: `${config.URL_API}/${CLIENTSLITE}`,
            headers: [
              { Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) },
            ],
          }),
          fields: { value: "id", text: "name" },
          floatLabelType: "Never",
          placeholder: "Selecciona Cliente",
          popupWidth: "auto",
          allowFiltering: true,
          filtering: this.handleFilteringClients.bind(this),
        });
        this.clientsObj.appendTo(this.clientsElem);
        if (
          args.rowData != null &&
          args.rowData.clientId != null &&
          args.rowData.clientId !== 0
        ) {
          this.clientsObj.value = args.rowData.clientId;
        }
      },
    };
    this.editWorks = {
      create: () => {
        this.worksElem = document.createElement("input");
        return this.worksElem;
      },
      destroy: () => {
        this.worksObj.destroy();
      },
      read: () => {
        return this.worksObj.value;
      },
      write: (args) => {
        this.worksObj = new DropDownList({
          change: () => {
            this.workBudgetsObj.enabled = true;
            const tempQuery = new Query().addParams(
              "workId",
              this.worksObj.value
            );
            this.workBudgetsObj.query = tempQuery;
            this.workBudgetsObj.text = "";
            this.workBudgetsObj.value = null;
            this.workBudgetsObj.dataBind();
          },
          dataSource: new DataManager({
            adaptor: new WebApiAdaptor(),
            url: `${config.URL_API}/${WORKSLITE}`,
            headers: [
              { Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) },
            ],
          }),
          enabled: false,
          fields: { value: "id", text: "name" },
          floatLabelType: "Never",
          placeholder: "Selecciona Obra",
          popupWidth: "auto",
          allowFiltering: true,
          filtering: this.handleFilteringWorks.bind(this),
        });
        this.worksObj.appendTo(this.worksElem);
        if (
          args.rowData != null &&
          args.rowData.workId != null &&
          args.rowData.workId !== 0
        ) {
          this.worksObj.value = args.rowData.workId;
        }
      },
    };
    this.editWorkBudgets = {
      create: () => {
        this.workBudgetsElem = document.createElement("input");
        return this.workBudgetsElem;
      },
      destroy: () => {
        this.workBudgetsObj.destroy();
      },
      read: () => {
        return this.workBudgetsObj.value;
      },
      write: (args) => {
        this.workBudgetsObj = new DropDownList({
          dataSource: new DataManager({
            adaptor: new WebApiAdaptor(),
            url: `${config.URL_API}/${WORKBUDGETSLITE}`,
            headers: [
              { Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) },
            ],
          }),
          enabled: false,
          fields: { value: "id", text: "name" },
          floatLabelType: "Never",
          placeholder: "Selecciona Presupuesto",
          popupWidth: "auto",
        });
        this.workBudgetsObj.appendTo(this.workBudgetsElem);
        if (
          args.rowData != null &&
          args.rowData.workBudgetId != null &&
          args.rowData.workBudgetId !== 0
        ) {
          this.workBudgetsObj.value = args.rowData.workId;
        }
      },
    };

    this.actionBegin = this.actionBegin.bind(this);
    this.actionFailure = this.actionFailure.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
    this.dataBoundGridInvoice = this.dataBoundGridInvoice.bind(this);
    this.fnRowSelectedInvoice = this.fnRowSelectedInvoice.bind(this);
    this.gridDetailsInvoiceActionComplete =
      this.gridDetailsInvoiceActionComplete.bind(this);
    this.detailDataBound = this.detailDataBound.bind(this);
    this.clickHandlerGridInvoice = this.clickHandlerGridInvoice.bind(this);

    this.gridDetailsInvoice = {
      columns: [
        {
          field: "id",
          isPrimaryKey: true,
          isIdentity: true,
          visible: false,
        },
        {
          field: "invoiceId",
          visible: false,
        },
        {
          field: "servicesPerformed",
          headerText: "Servicios Prestados",
          width: "100",
          textAlign: "left",
        },
        {
          headerText: "UNIDADES",
          textAlign: "center",
          columns: [
            {
              field: "nameUnit",
              headerText: "Medida",
              width: "100",
              textAlign: "center",
            },
            {
              field: "units",
              headerText: "Trámite",
              width: "100",
              fotmat: "N2",
              textAlign: "right",
              editType: "numericedit",
              edit: this.numericParams,
            },
            {
              field: "unitsAccumulated",
              headerText: "Anteriores",
              width: "100",
              fotmat: "N2",
              textAlign: "right",
              editType: "numericedit",
              edit: this.numericParams,
              allowEditing: false,
              defaultValue: 0,
            },
            {
              field: "unitsTotal",
              headerText: "Origen",
              width: "100",
              fotmat: "N2",
              textAlign: "right",
              editType: "numericedit",
              edit: this.numericParams,
              allowEditing: false,
            },
          ],
        },
        {
          headerText: "IMPORTES",
          textAlign: "center",
          columns: [
            {
              field: "priceUnity",
              headerText: "Precio Unidad",
              width: "100",
              fotmat: "N2",
              textAlign: "right",
              editType: "numericedit",
              edit: this.numericParams,
            },
            {
              field: "amountUnits",
              headerText: "Trámite",
              width: "100",
              fotmat: "N2",
              textAlign: "right",
              editType: "numericedit",
              edit: this.numericParams,
              allowEditing: false,
            },
            {
              field: "amountAccumulated",
              headerText: "Anteriores",
              width: "100",
              fotmat: "N2",
              textAlign: "right",
              editType: "numericedit",
              edit: this.numericParams,
              allowEditing: false,
              defaultValue: 0,
            },
            {
              field: "amountTotal",
              headerText: "Origen",
              width: "100",
              fotmat: "N2",
              textAlign: "right",
              editType: "numericedit",
              edit: this.numericParams,
              allowEditing: false,
            },
          ],
        },
      ],
      aggregates: [
        {
          columns: [
            {
              type: "Sum",
              field: "units",
              format: "N2",
              footerTemplate: this.footerSumUnits,
            },
            {
              type: "Sum",
              field: "unitsAccumulated",
              format: "N2",
              footerTemplate: this.footerSumUnits,
            },
            {
              type: "Sum",
              field: "unitsTotal",
              format: "N2",
              footerTemplate: this.footerSumUnits,
            },
            {
              type: "Sum",
              field: "amountUnits",
              format: "N2",
              footerTemplate: this.footerSumAmountUnits,
            },
            {
              type: "Sum",
              field: "amountAccumulated",
              format: "N2",
              footerTemplate: this.footerSumAmountAccumulated,
            },
            {
              type: "Sum",
              field: "amountTotal",
              format: "N2",
              footerTemplate: this.footerSumAmountTotal,
            },
          ],
        },
      ],
      dataSource: this.detailsInvoice,
      queryString: "invoiceId",
      locale: "es-US",
      toolbar: [
        "Add",
        "Edit",
        "Delete",
        "Update",
        "Cancel",
        {
          text: "Detalle por Horas",
          tooltipText: "detalle por horas",
          prefixIcon: "e-custom-icons e-details",
          id: "DetailByHours",
        },
        {
          text: "Limpiar",
          tooltipText: "limpiar",
          prefixIcon: "e-custom-icons e-empty",
          id: "EmptyDetails",
        },
        {
          text: "Importar Factura Anterior",
          tooltipText: "importar factura anterior",
          prefixIcon: "e-custom-icons e-file-workers",
          id: "PreviousInvoice",
        },
      ],
      actionFailure: this.gridDetailsInvoiceActionFailure,
      allowGrouping: false,
      ref: (g) => (this.gridDetailsInvoice = g),
      allowTextWrap: true,
      textWrapSettings: this.wrapSettings,
      load: this.loadGridDetailsInvoice,
      actionComplete: this.gridDetailsInvoiceActionComplete,
      actionBegin: this.gridDetailsInvoiceActionBegin,
      dataBound: this.dataBoundDetailsInvoice,
      editSettings: this.editSettings,
      rowSelected: this.fnRowSelectedDetailsInvoice,
      toolbarClick: this.clickHandlerGridDetailsInvoice,
      props: this.props,
    };
  }

  dataBoundDetailsInvoice(args) {
    console.log();
  }

  loadGridDetailsInvoice() {
    this.query = [];
    this.query = new Query().addParams(
      "invoiceId",
      this.parentDetails.parentRowData.id
    );
  }

  clickHandlerGridDetailsInvoice(args) {
    if (args.item.id === "DetailByHours") {
      this.query = [];
      this.query = new Query()
        .addParams("invoiceId", this.parentDetails.parentRowData.id)
        .addParams("detailByHours", true);
    }

    if (args.item.id === "EmptyDetails") {
      var gridInvoices = document.getElementById("gridInvoices");
      gridInvoices.invoiceIdCleaned = this.parentDetails.parentRowData.id;

      this.query = [];
      this.query = new Query()
        .addParams("invoiceId", this.parentDetails.parentRowData.id)
        .addParams("emptyDetails", true);
    }

    if (args.item.id === "PreviousInvoice") {
      this.query = [];
      this.query = new Query()
        .addParams("invoiceId", this.parentDetails.parentRowData.id)
        .addParams("previousInvoice", true);
    }
  }

  dataBoundGridInvoice() {
    this.props.setCurrentPageInvoices(
      this.gridInvoice.pageSettings.currentPage
    );
    this.props.setCurrentSearchInvoices(this.gridInvoice.searchSettings.key);

    if (this.expandGridRow != null) {
      let rowIndex = parseInt(this.expandGridRow.getAttribute("aria-rowindex"));
      this.gridInvoice.detailRowModule.expand(rowIndex);
    }
  }

  fnRowSelectedInvoice() {
    const selectedRecords = this.gridInvoice.getSelectedRecords();
    this.rowSelectedInvoice = selectedRecords[0];
  }

  fnRowSelectedDetailsInvoice() {
    const selectedRecords = this.getSelectedRecords();
    this.rowSelectedDetailsInvoice = selectedRecords[0];
  }

  actionFailure(args) {
    let error = Array.isArray(args) ? args[0].error : args.error;
    if (Array.isArray(error)) {
      error = error[0].error;
    }

    this.props.showMessage({
      statusText: error.statusText,
      responseText: error.responseText,
      type: "danger",
    });
  }

  actionBegin(args) {
    if (args.requestType === "save") {
      var cols = this.gridInvoice.columns;
      for (var i = 0; i < cols.length; i++) {
        if (cols[i].type === "date") {
          var date = args.data[cols[i].field];
          if (date != null) {
            args.data[cols[i].field] = new Date(
              Date.UTC(
                date.getFullYear(),
                date.getMonth(),
                date.getDate(),
                date.getHours(),
                date.getMilliseconds()
              )
            );
          }
        }
      }
    }
  }

  actionComplete(args) {
    if (args.requestType === "save") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });
    }
    if (args.requestType === "delete") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });
    }
  }

  gridDetailsInvoiceActionFailure(args) {
    let error = Array.isArray(args) ? args[0].error : args.error;
    if (Array.isArray(error)) {
      error = error[0].error;
    } else if (error.message != null) {
      error.statusText = args.error.message;
      error.responseText = args.error.message;
    } else if (error.statusText == null) {
      error.statusText = error.error.statusText;
      error.responseText = JSON.parse(error.error.responseText).Message;
    }

    // this.query = [];
    // this.query = new Query().addParams(
    //   "invoiceId",
    //   this.parentDetails.parentRowData.id
    // );

    this.props.showMessage({
      statusText: error.statusText,
      responseText: error.responseText,
      type: "danger",
    });
  }

  gridDetailsInvoiceActionComplete(args) {
    if (args.requestType === "save") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });

      getInvoice(args.data.invoiceId).then((result) => {
        this.gridInvoice.setRowData(args.data.invoiceId, result);
      });

      var childGridElements =
        this.gridInvoice.element.querySelectorAll(".e-detailrow");
      for (var i = 0; i < childGridElements.length; i++) {
        let element = childGridElements[i];
        let childGridObj = element.querySelector(".e-grid").ej2_instances[0];
        if (
          childGridObj.parentDetails.parentRowData.id === args.data.invoiceId
        ) {
          childGridObj.refresh();
          break;
        }
      }
    }
    if (args.requestType === "delete") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });

      getInvoice(args.data[0].invoiceId).then((result) => {
        this.gridInvoice.setRowData(args.data[0].invoiceId, result);
      });
    }
    if (args.requestType === "refresh") {
      var gridInvoices = document.getElementById("gridInvoices");

      if (args.rows != null && Array.isArray(args.rows)) {
        getInvoice(args.rows[0].data.invoiceId).then((result) => {
          this.gridInvoice.setRowData(args.rows[0].data.invoiceId, result);
        });
      } else if (gridInvoices.invoiceIdCleaned != null) {
        getInvoice(gridInvoices.invoiceIdCleaned).then((result) => {
          this.gridInvoice.setRowData(gridInvoices.invoiceIdCleaned, result);
        });
      }
    }
  }

  gridDetailsInvoiceActionBegin(args) {
    if (args.requestType === "add") {
      args.data.invoiceId = this.parentDetails.parentRowData.id;
    }
    if (args.requestType === "save") {
      this.query = [];
      this.query = new Query().addParams(
        "invoiceId",
        this.parentDetails.parentRowData.id
      );
    }
    if (args.requestType === "delete") {
      this.query = [];
      this.query = new Query().addParams(
        "invoiceId",
        this.parentDetails.parentRowData.id
      );
    }
  }

  handleFilteringClients(e) {
    let query = new Query();
    query =
      e.text !== "" ? query.where("name", "contains", e.text, true) : query;
    e.updateData(this.clients, query);
  }

  handleFilteringWorks(e) {
    let query = new Query();
    query =
      e.text !== "" ? query.where("name", "contains", e.text, true) : query;
    e.updateData(this.works, query);
  }

  templateIVA(args) {
    let total = Math.round((args.ivaTaxBase + Number.EPSILON) * 100) / 100;
    return <span>{total}</span>;
  }

  footerSumUnits(args) {
    let amount = Number(args.Sum);
    amount = Math.round((amount + Number.EPSILON) * 100) / 100;

    if (isNaN(amount)) {
      amount = args.Sum.replace(",", "").replace("$", "");
      amount = Number(amount);
      amount = Math.round((amount + Number.EPSILON) * 100) / 100;
    }

    return <span>Total: {amount}</span>;
  }

  footerSumEuros(args) {
    let amount = Number(args.Sum);
    amount = Math.round((amount + Number.EPSILON) * 100) / 100;

    if (isNaN(amount)) {
      amount = args.Sum.replace(",", "").replace("$", "");
      amount = Number(amount);
      amount = Math.round((amount + Number.EPSILON) * 100) / 100;
    }

    return <span>Total: {amount}€</span>;
  }

  footerSumAmountUnits(args) {
    let amount = Number(args.Sum);
    amount = Math.round((amount + Number.EPSILON) * 100) / 100;

    if (isNaN(amount)) {
      amount = args.Sum.replace(",", "").replace("$", "");
      amount = Number(amount);
      amount = Math.round((amount + Number.EPSILON) * 100) / 100;
    }

    return <span>B. Imponible: {amount}€</span>;
  }

  footerSumAmountAccumulated(args) {
    let amount = Number(args.Sum);
    amount = Math.round((amount + Number.EPSILON) * 100) / 100;

    if (isNaN(amount)) {
      amount = args.Sum.replace(",", "").replace("$", "");
      amount = Number(amount);
      amount = Math.round((amount + Number.EPSILON) * 100) / 100;
    }

    return <span>Cert. Anterior: {amount}€</span>;
  }

  footerSumAmountTotal(args) {
    let amount = Number(args.Sum);
    amount = Math.round((amount + Number.EPSILON) * 100) / 100;

    if (isNaN(amount)) {
      amount = args.Sum.replace(",", "").replace("$", "");
      amount = Number(amount);
      amount = Math.round((amount + Number.EPSILON) * 100) / 100;
    }

    return <span>Cert. Origen: {amount}€</span>;
  }

  // footerTaxBase(args) {
  //   let amount = Number(args.Sum);
  //   amount = Math.round((amount + Number.EPSILON) * 100) / 100;

  //   if (isNaN(amount)) {
  //     amount = args.Sum.replace(",", "").replace("$", "");
  //     amount = Number(amount);
  //     amount = Math.round((amount + Number.EPSILON) * 100) / 100;
  //   }

  //   return <span>B. Imponible: {amount}€</span>;
  // }

  customAggregateFn(args) {
    let total = 0;
    for (let cont = 0; cont < args.result.length; cont++) {
      total += Number(args.result[cont].units * args.result[cont].priceUnity);
    }

    return total;
  }

  detailDataBound(args) {
    console.log();
  }

  clickHandlerGridInvoice(args) {
    const selectedRecords = this.gridInvoice.getSelectedRecords();
    if (args.item.id === "PrintInvoice") {
      if (selectedRecords.length === 0) {
        this.props.showMessage({
          statusText: "Debes seleccionar una factura",
          responseText: "Debes seleccionar una factura",
          type: "danger",
        });
      } else {
        const element = document.getElementById("gridInvoices");

        createSpinner({
          target: element,
        });
        showSpinner(element);

        printInvoice(selectedRecords[0].id)
          .then((result) => {
            const fileArr = base64ToArrayBuffer(result.file);
            saveByteArray(result.fileName, fileArr, result.typeFile);
            hideSpinner(element);
          })
          .catch((error) => {
            hideSpinner(element);
          });
      }
    }
  }

  render() {
    return (
      <Fragment>
        <Breadcrumb>
          {/*eslint-disable-next-line*/}
          <BreadcrumbItem>
            <a href="#">Inicio</a>
          </BreadcrumbItem>
          {/* eslint-disable-next-line*/}
          <BreadcrumbItem active>Facturas</BreadcrumbItem>
        </Breadcrumb>

        <Container fluid>
          <div className="animated fadeIn">
            <div className="card">
              <div className="card-header">
                <i className="cui-file"></i> Facturas
              </div>
              <div className="card-body"></div>
              <Row>
                <GridComponent
                  dataSource={this.invoices}
                  id="gridInvoices"
                  locale="es"
                  allowPaging={true}
                  pageSettings={this.pageSettings}
                  searchSettings={this.searchSettings}
                  toolbar={this.toolbarOptions}
                  toolbarClick={this.clickHandlerGridInvoice}
                  editSettings={this.editSettings}
                  style={{
                    marginLeft: 30,
                    marginRight: 30,
                    marginTop: -20,
                    marginBottom: 20,
                    overflow: "auto",
                  }}
                  actionBegin={this.actionBegin}
                  actionFailure={this.actionFailure}
                  actionComplete={this.actionComplete}
                  rowSelected={this.fnRowSelectedInvoice}
                  ref={(g) => (this.gridInvoice = g)}
                  allowTextWrap={true}
                  textWrapSettings={this.wrapSettings}
                  dataBound={this.dataBoundGridInvoice}
                  allowResizing={true}
                  childGrid={this.gridDetailsInvoice}
                  detailDataBound={this.detailDataBound}
                  invoiceIdCleaned={null}
                >
                  <ColumnsDirective>
                    <ColumnDirective
                      field="id"
                      headerText="Id"
                      width="40"
                      isPrimaryKey={true}
                      isIdentity={true}
                      visible={false}
                    />
                    <ColumnDirective
                      field="invoiceNumber"
                      width="100"
                      visible={false}
                    />
                    <ColumnDirective
                      field="name"
                      headerText="Nº Factura"
                      width="100"
                      allowEditing={false}
                    />
                    <ColumnDirective
                      field="startDate"
                      headerText="F. Inicio"
                      width="100"
                      type="date"
                      format={this.formatDate}
                      editType="datepickeredit"
                      validationRules={this.requeridIdRules}
                      // allowEditing={false}
                    />
                    <ColumnDirective
                      field="endDate"
                      headerText="F. Fin"
                      width="100"
                      type="date"
                      format={this.formatDate}
                      editType="datepickeredit"
                      validationRules={this.requeridIdRules}
                      // allowEditing={false}
                    />
                    <ColumnDirective
                      field="issueDate"
                      headerText="F. Emisión"
                      width="100"
                      type="date"
                      format={this.formatDate}
                      editType="datepickeredit"
                      validationRules={this.requeridIdRules}
                      // allowEditing={false}
                    />
                    <ColumnDirective
                      field="clientId"
                      headerText="Cliente"
                      width="100"
                      foreignKeyValue="name"
                      foreignKeyField="id"
                      validationRules={this.requeridIdRules}
                      dataSource={this.clients}
                      edit={this.editClients}
                    />
                    <ColumnDirective
                      field="workId"
                      headerText="Obra"
                      width="100"
                      foreignKeyValue="name"
                      foreignKeyField="id"
                      validationRules={this.requeridIdRules}
                      dataSource={this.works}
                      edit={this.editWorks}
                    />
                    <ColumnDirective
                      field="workBudgetId"
                      headerText="Presupuesto"
                      width="100"
                      foreignKeyValue="name"
                      foreignKeyField="id"
                      // validationRules={this.requeridIdRules}
                      dataSource={this.workBudgets}
                      edit={this.editWorkBudgets}
                    />
                    <ColumnDirective
                      field="taxBase"
                      headerText="B. Impon."
                      width="100"
                      allowEditing={false}
                      defaultValue={0}
                    />
                    <ColumnDirective
                      field="ivaTaxBase"
                      headerText="IVA"
                      width="90"
                      allowEditing={false}
                      defaultValue={0}
                      template={this.templateIVA}
                    />
                    <ColumnDirective
                      field="total"
                      headerText="Total"
                      width="100"
                      allowEditing={false}
                      defaultValue={0}
                    />
                    <ColumnDirective
                      field="retentions"
                      headerText="Ret."
                      width="70"
                      // fotmat="N2"
                      // editType="numericedit"
                      // edit={this.numericParams}
                      allowEditing={false}
                      defaultValue={0}
                    />
                    <ColumnDirective
                      field="payDate"
                      headerText="F. Pago"
                      width="100"
                      type="date"
                      format={this.formatDate}
                      editType="datepickeredit"
                    />
                    <ColumnDirective field="typeInvoice" visible={false} />
                  </ColumnsDirective>
                  <Inject
                    services={[
                      Page,
                      Toolbar,
                      Edit,
                      Resize,
                      DetailRow,
                      Aggregate,
                    ]}
                  />
                </GridComponent>
              </Row>
            </div>
          </div>
        </Container>
      </Fragment>
    );
  }
}

Invoices.propTypes = {};

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
    currentPageInvoices: state.applicationReducer.currentPageInvoices,
    currentSearchInvoices: state.applicationReducer.currentSearchInvoices,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
  setCurrentPageInvoices: (currentPageInvoices) =>
    dispatch(ACTION_APPLICATION.setCurrentPageInvoices(currentPageInvoices)),
  setCurrentSearchInvoices: (currentSearchInvoices) =>
    dispatch(
      ACTION_APPLICATION.setCurrentSearchInvoices(currentSearchInvoices)
    ),
});

export default connect(mapStateToProps, mapDispatchToProps)(Invoices);
