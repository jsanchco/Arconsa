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
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
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
  billPaymentWithAmount,
} from "../../services";
import { DropDownList } from "@syncfusion/ej2-dropdowns";
import { DialogComponent } from "@syncfusion/ej2-react-popups";
import { DialogUtility } from "@syncfusion/ej2-popups";
import ModalCancelInvoice from "../Modals/modal-cancel-invoice";
import ModalInvoicePaymentsHistory from "../Modals/modal-invoice-payments-history";

L10n.load(data);

class Invoices extends Component {
  invoices = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${INVOICES}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  detailsInvoice = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${DETAILSINVOICE}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  gridInvoice = null;
  gridDetailsInvoice = null;

  requeridIdRules = { required: true };

  wrapSettings = { wrapMode: "Content" };

  constructor(props) {
    super(props);

    this.state = {
      hideConfirmDialog: false,
      modalCancelInvoice: false,
      modalInvoicePaymentsHistory: false,
      invoiceSelected: null,
    };

    this.clientsElem = null;
    this.clientsObj = null;
    this.worksElem = null;
    this.worksObj = null;
    this.workBudgetsElem = null;
    this.worksBudgetObj = null;
    this.loadFirstTime = true;

    this.queryInvoices = new Query()
      .addParams("enterpriseId", JSON.parse(localStorage.getItem("enterprise")).id);

    this.toolbarOptions = [
      "Add",
      "Edit",
      "Delete",
      "Update",
      "Cancel",
      "Print",
      {
        text: "Imprimir Factura",
        tooltipText: "Imprimir Factura",
        prefixIcon: "e-custom-icons e-print",
        id: "PrintInvoice",
      },
      {
        text: "Anular Factura",
        tooltipText: "Anular Factura",
        prefixIcon: "e-custom-icons e-empty",
        id: "CancelInvoice",
      },
      {
        text: "Det. Pagos",
        tooltipText: "Detalle de Pagos de Factura",
        prefixIcon: "e-custom-icons e-file-workers",
        id: "detailPayment",
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
        decimals: 5,
        format: "N",
        validateDecimalOnType: true,
        showSpinButton: false,
      },
    };
    this.numericParamsIVA = {
      params: {
        decimals: 5,
        format: "N",
        validateDecimalOnType: true,
        showSpinButton: false,
        min: 0,
        max: 1,
      },
    };
    this.confirmButton = [
      {
        click: () => {
          this.setState({ hideConfirmDialog: false });
        },
        buttonModel: { content: "Si", isPrimary: true },
      },
      {
        click: () => {
          this.setState({ hideConfirmDialog: false });
        },
        buttonModel: { content: "No" },
      },
    ];
    this.animationSettings = { effect: "None" };
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
        const enterpriseId = JSON.parse(localStorage.getItem("enterprise")).id;
        let clients = new DataManager({
          adaptor: new WebApiAdaptor(),
          url: `${config.URL_API}/${CLIENTSLITE}?enterpriseId=${enterpriseId}`,
          headers: [
            { Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) },
          ],
        });
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
          dataSource: clients,
          fields: { value: "id", text: "name" },
          floatLabelType: "Never",
          placeholder: "Selecciona Cliente",
          popupWidth: "auto",
          allowFiltering: true,
          filtering: this.handleFilteringClients.bind(this, clients),
          showClearButton : true
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
        const enterpriseId = JSON.parse(localStorage.getItem("enterprise")).id;
        let works = new DataManager({
          adaptor: new WebApiAdaptor(),
          url: `${config.URL_API}/${WORKSLITE}?enterpriseId=${enterpriseId}`,
          headers: [
            { Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) },
          ],
        });        
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
          open: () => {
            const tempQuery = new Query().addParams(
              "clientId",
              this.clientsObj.value
            );
            this.worksObj.query = tempQuery;    
          },          
          dataSource: works,
          fields: { value: "id", text: "name" },
          floatLabelType: "Never",
          placeholder: "Selecciona Obra",
          popupWidth: "auto",
          allowFiltering: true,
          filtering: this.handleFilteringWorks.bind(
            this,
            works,
            this.clientsObj
          ),
          showClearButton : true
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
          open: () => {
            const tempQuery = new Query().addParams(
              "workId",
              this.worksObj.value
            );
            this.workBudgetsObj.query = tempQuery;    
          },            
          fields: { value: "id", text: "name" },
          floatLabelType: "Never",
          placeholder: "Selecciona Presupuesto",
          popupWidth: "auto",
          showClearButton : true
        });
        this.workBudgetsObj.appendTo(this.workBudgetsElem);
        if (
          args.rowData != null &&
          args.rowData.workBudgetId != null &&
          args.rowData.workBudgetId !== 0
        ) {
          this.workBudgetsObj.value = args.rowData.workBudgetId;
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
    this.billPaymentWithAmount = this.billPaymentWithAmount.bind(this);
    this.clientTemplate = this.clientTemplate.bind(this);
    this.workTemplate = this.workTemplate.bind(this);
    this.workBudgetTemplate = this.workBudgetTemplate.bind(this);
    this.toggleModalCancelInvoice = this.toggleModalCancelInvoice.bind(this);
    this.toggleModalInvoicePaymentsHistory =
      this.toggleModalInvoicePaymentsHistory.bind(this);

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
              width: "130",
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
              field: "iva",
              headerText: "IVA",
              width: "100",
              fotmat: "N2",
              textAlign: "right",
              editType: "numericedit",
              edit: this.numericParamsIVA,
              allowEditing: true,
            },
            {
              field: "amountTotal",
              headerText: "Origen",
              width: "130",
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
          text: "Det. por Horas",
          tooltipText: "detalle por horas",
          prefixIcon: "e-custom-icons e-details",
          id: "DetailByHours",
        },
        {
          text: "Det. por Partidas",
          tooltipText: "detalle por partidas",
          prefixIcon: "e-custom-icons e-details",
          id: "DetailByPartidas",
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
      editSettings: this.editSettings,
      rowSelected: this.fnRowSelectedDetailsInvoice,
      toolbarClick: this.clickHandlerGridDetailsInvoice,
      props: this.props,
    };
  }

  dialogClose() {
    this.setState({
      hideConfirmDialog: false,
    });
  }

  toggleModalCancelInvoice() {
    this.setState({
      modalCancelInvoice: !this.state.modalCancelInvoice,
    });
  }

  toggleModalInvoicePaymentsHistory() {
    if (this.state.modalInvoicePaymentsHistory === true) {
      this.gridInvoice.refresh();
    }
    this.setState({
      modalInvoicePaymentsHistory: !this.state.modalInvoicePaymentsHistory,
    });
  }

  billPaymentWithAmount(issueDate, amount, iva, description) {
    const element = document.getElementById("gridInvoices");

    createSpinner({
      target: element,
    });
    showSpinner(element);

    billPaymentWithAmount({
      invoiceId: this.gridInvoice.getSelectedRecords()[0].id,
      amount: amount,
      iva: iva,
      description: description,
      issueDate: issueDate
    })
      .then(() => {
        this.props.showMessage({
          statusText: "200",
          responseText: "Operación realizada con éxito",
          type: "success",
        });
        this.gridInvoice.refresh();
        hideSpinner(element);
      })
      .catch((error) => {
        this.props.showMessage({
          statusText: "Ha ocurrido un error en la operación",
          responseText: "Ha ocurrido un error en la operación",
          type: "danger",
        });
        hideSpinner(element);
      });
  }

  clientTemplate(args) {
    return (
      <div>
        <a rel="nofollow" href={"/#/clients/detailclient/" + args.clientId}>
          {args.clientName}
        </a>
      </div>
    );
  }

  workTemplate(args) {
    return (
      <div>
        <a rel="nofollow" href={"/#/works/detailwork/" + args.workId}>
          {args.workName}
        </a>
      </div>
    );
  }

  workBudgetTemplate(args) {
    return (
      <div>
        <a rel="nofollow" href={"/#/works/detailwork/" + args.workId + "/2"}>
          {args.workBudgetName}
        </a>
      </div>
    );
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

    if (args.item.id === "DetailByPartidas") {
      this.query = [];
      this.query = new Query()
        .addParams("invoiceId", this.parentDetails.parentRowData.id)
        .addParams("detailByPartidas", true);
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
      var DialogObj = DialogUtility.confirm({
        title: "ATENCION",
        cancelButton: { text: "No" },
        content:
          "Si importas la Factura anterior perderás el detalle de factura que tienes en la actualidad, ¿Deseas continuar?",
        okButton: {
          text: "Si",
          click: () => {
            this.query = [];
            this.query = new Query()
              .addParams("invoiceId", this.parentDetails.parentRowData.id)
              .addParams("previousInvoice", true);

            DialogObj.close();
          },
        },
        showCloseIcon: true,
        position: { Y: 100 },
      });
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

    const errorJSON = JSON.parse(error.responseText);
    this.props.showMessage({
      statusText: error.statusText,
      responseText: errorJSON.Message,
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
      args.data.iva = this.parentDetails.parentRowData.ivaValue;
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

  handleFilteringClients(clients, e) {
    let query = new Query();
    query =
      e.text !== "" ? query.where("name", "contains", e.text, true) : query;
    e.updateData(clients, query);
  }

  handleFilteringWorks(works, clientObj, e) {
    let query = new Query();
    query =
      e.text !== "" ? query.where("name", "contains", e.text, true) : query;

    if (
      clientObj != null &&
      clientObj.itemData != null &&
      clientObj.itemData.id != null
    ) {
      query.addParams("clientId", clientObj.itemData.id);
    }
    e.updateData(works, query);
  }

  templateIVA(args) {
    let total = Math.round((args.ivaTaxBase + Number.EPSILON) * 100) / 100;
    return <span>{total}</span>;
  }

  templateTotalPayment(args) {
    let total = Math.round((args.totalPayment + Number.EPSILON) * 100) / 100;
    return <span>{total}</span>;
  }
  
  templateRemaining(args) {
    if (args.total === 0) {
      return;
    }

    let remaining = Math.round((args.remaining + Number.EPSILON) * 100) / 100;
    let color = "#16a085";
    if (remaining > 0) {
      color = "#e74c3c";      
    } else {
      remaining = "Pagado";
    }

    return <span style = {{ color: color}}>{remaining}</span>;
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

    if (args.item.id === "CancelInvoice") {
      if (selectedRecords.length === 0) {
        this.props.showMessage({
          statusText: "Debes seleccionar una factura",
          responseText: "Debes seleccionar una factura",
          type: "danger",
        });
      } else {
        this.setState({ invoiceSelected: selectedRecords[0] });
        this.toggleModalCancelInvoice();
      }
    }

    if (args.item.id === "detailPayment") {
      if (selectedRecords.length === 0) {
        this.props.showMessage({
          statusText: "Debes seleccionar una factura",
          responseText: "Debes seleccionar una factura",
          type: "danger",
        });
      } else {
        this.setState({ invoiceSelected: selectedRecords[0] });
        this.toggleModalInvoicePaymentsHistory();
      }
    }
  }

  beforePrint(args) {
    var div = document.createElement("Div");
    div.innerHTML = "FACTURAS";
    div.style.textAlign = "center";
    div.style.color = "red";
    div.style.padding = "10px 0";
    div.style.fontSize = "25px";
    args.element.insertBefore(div, args.element.childNodes[0]);
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

        <ModalCancelInvoice
          isOpen={this.state.modalCancelInvoice}
          toggle={this.toggleModalCancelInvoice}
          invoice={this.state.invoiceSelected}
          billPaymentWithAmount={this.billPaymentWithAmount}
          showMessage={this.props.showMessage}
        />

        <ModalInvoicePaymentsHistory
          isOpen={this.state.modalInvoicePaymentsHistory}
          toggle={this.toggleModalInvoicePaymentsHistory}
          invoice={this.state.invoiceSelected}
          showMessage={this.props.showMessage}
        />

        <DialogComponent
          id="confirmDialog"
          header="Abonar Factura"
          visible={this.state.hideConfirmDialog}
          showCloseIcon={true}
          animationSettings={this.animationSettings}
          width="500px"
          content="¿Estás seguro de querer Abonar esta factura?"
          ref={(dialog) => (this.confirmDialogInstance = dialog)}
          buttons={this.confirmButton}
          close={this.dialogClose.bind(this)}
        ></DialogComponent>

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
                  beforePrint={this.beforePrint}
                  query={this.queryInvoices}
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
                      width="120"
                      allowEditing={false}
                    />
                    <ColumnDirective
                      field="startDate"
                      headerText="F. Inicio"
                      width="120"
                      type="date"
                      format={this.formatDate}
                      editType="datepickeredit"
                      validationRules={this.requeridIdRules}
                    />
                    <ColumnDirective
                      field="endDate"
                      headerText="F. Fin"
                      width="120"
                      type="date"
                      format={this.formatDate}
                      editType="datepickeredit"
                      validationRules={this.requeridIdRules}
                    />
                    <ColumnDirective
                      field="issueDate"
                      headerText="F. Emisión"
                      width="120"
                      type="date"
                      format={this.formatDate}
                      editType="datepickeredit"
                      validationRules={this.requeridIdRules}
                    />
                    <ColumnDirective
                      field="clientId"
                      headerText="Cliente"
                      width="100"
                      foreignKeyValue="name"
                      foreignKeyField="id"
                      validationRules={this.requeridIdRules}
                      edit={this.editClients}
                      template={this.clientTemplate}
                    />
                    <ColumnDirective
                      field="workId"
                      headerText="Obra"
                      width="100"
                      foreignKeyValue="name"
                      foreignKeyField="id"
                      validationRules={this.requeridIdRules}
                      edit={this.editWorks}
                      template={this.workTemplate}
                    />
                    <ColumnDirective
                      field="workBudgetId"
                      headerText="Presupuesto"
                      width="140"
                      foreignKeyValue="name"
                      foreignKeyField="id"
                      edit={this.editWorkBudgets}
                      template={this.workBudgetTemplate}
                    />
                    <ColumnDirective
                      field="taxBase"
                      headerText="B. Impon."
                      width="120"
                      allowEditing={false}
                      defaultValue={0}
                      textAlign="Right"
                      headerTextAlign="Left"
                    />
                    <ColumnDirective
                      field="ivaTaxBase"
                      headerText="IVA"
                      width="120"
                      allowEditing={false}
                      defaultValue={0}
                      template={this.templateIVA}
                      textAlign="Right"
                      headerTextAlign="Left"                      
                    />
                    <ColumnDirective
                      field="total"
                      headerText="Total"
                      width="120"
                      allowEditing={false}
                      defaultValue={0}
                      textAlign="Right"
                      headerTextAlign="Left"
                    />
                    <ColumnDirective
                      field="totalPayment"
                      headerText="Tot. Pagado"
                      width="120"
                      allowEditing={false}
                      template={this.templateTotalPayment}
                      textAlign="Right"
                      headerTextAlign="Left"
                      defaultValue={0}
                    />
                    <ColumnDirective
                      field="remainig"
                      headerText="Restante"
                      width="120"
                      allowEditing={false}
                      template={this.templateRemaining}
                      textAlign="Right"
                      headerTextAlign="Left"
                    />                    
                    <ColumnDirective
                      field="payDate"
                      headerText="F. Pago"
                      width="120"
                      type="date"
                      format={this.formatDate}
                      allowEditing={false}
                    />
                    <ColumnDirective
                      field="retentions"
                      headerText="Retenciones"
                      width="110"
                      allowEditing={false}
                      defaultValue={0}
                      textAlign="Right"
                      headerTextAlign="Left"
                    />
                    <ColumnDirective
                      field="invoiceToCancelName"
                      headerText="Cancelada"
                      width="120"
                      allowEditing={false}
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
