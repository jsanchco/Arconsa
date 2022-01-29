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
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor } from "@syncfusion/ej2-data";
import {
  config,
  INVOICES,
  CLIENTSLITE,
  WORKSLITE,
  WORKBUDGETSLITE,
} from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import { TOKEN_KEY } from "../../services";
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

  grid = null;

  requeridIdRules = { required: true };

  wrapSettings = { wrapMode: "Content" };

  constructor(props) {
    super(props);

    this.state = {
      rowSelected: null,
    };

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
      {
        text: "Detalles",
        tooltipText: "Detalles",
        prefixIcon: "e-custom-icons e-details",
        id: "Details",
      },
      "Print",
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
      key: props.currentSearchInvoices,
    };
    this.numericParams = {
      params: {
        decimals: 2,
        format: "N",
        validateDecimalOnType: true,
      },
    };
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

    this.formatDate = { type: "dateTime", format: "dd/MM/yyyy" };

    this.actionBegin = this.actionBegin.bind(this);
    this.actionFailure = this.actionFailure.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
    this.clickHandler = this.clickHandler.bind(this);
    this.dataBound = this.dataBound.bind(this);
    this.rowSelected = this.rowSelected.bind(this);
  }

  clickHandler(args) {
    if (args.item.id === "Details") {
      const selectedRecords = this.grid.getSelectedRecords();
      if (Array.isArray(selectedRecords) && selectedRecords.length === 1) {
        this.setState({ rowSelected: selectedRecords[0] });

        this.props.history.push({
          pathname: "/clients/detailclient/" + selectedRecords[0].id,
          state: {
            client: selectedRecords[0],
          },
        });
      } else {
        this.setState({ rowSelected: null });
        this.props.showMessage({
          statusText: "Debes seleccionar un solo registro",
          responseText: "Debes seleccionar un solo registro",
          type: "danger",
        });
      }
    }
  }

  dataBound() {
    this.props.setCurrentPageInvoices(this.grid.pageSettings.currentPage);
    this.props.setCurrentSearchInvoices(this.grid.searchSettings.key);
  }

  rowSelected() {
    const selectedRecords = this.grid.getSelectedRecords();
    this.setState({ rowSelected: selectedRecords[0] });
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
      var cols = this.grid.columns;
      for (var i = 0; i < cols.length; i++) {
        if (cols[i].type === "date") {
          var date = args.data[cols[i].field];
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
                  id="Invoices"
                  locale="es"
                  allowPaging={true}
                  pageSettings={this.pageSettings}
                  searchSettings={this.searchSettings}
                  toolbar={this.toolbarOptions}
                  toolbarClick={this.clickHandler}
                  editSettings={this.editSettings}
                  style={{
                    marginLeft: 30,
                    marginRight: 30,
                    marginTop: -20,
                    marginBottom: 20,
                  }}
                  actionBegin={this.actionBegin}
                  actionFailure={this.actionFailure}
                  actionComplete={this.actionComplete}
                  rowSelected={this.rowSelected}
                  ref={(g) => (this.grid = g)}
                  allowTextWrap={true}
                  textWrapSettings={this.wrapSettings}
                  dataBound={this.dataBound}
                  allowResizing={true}
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
                      // allowEditing={false}
                    />
                    <ColumnDirective
                      field="endDate"
                      headerText="F. Fin"
                      width="100"
                      type="date"
                      format={this.formatDate}
                      editType="datepickeredit"
                      // allowEditing={false}
                    />
                    <ColumnDirective
                      field="issueDate"
                      headerText="F. Emisión"
                      width="100"
                      type="date"
                      format={this.formatDate}
                      editType="datepickeredit"
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
                      validationRules={this.requeridIdRules}
                      dataSource={this.workBudgets}
                      edit={this.editWorkBudgets}
                    />
                    <ColumnDirective
                      field="taxBase"
                      headerText="B. Impon."
                      width="100"
                      allowEditing={false}
                    />
                    <ColumnDirective
                      field="ivaTaxBase"
                      headerText="IVA"
                      width="90"
                      allowEditing={false}
                    />
                    <ColumnDirective
                      field="total"
                      headerText="Total"
                      width="100"
                      allowEditing={false}
                    />
                    <ColumnDirective
                      field="retentions"
                      headerText="Ret."
                      width="70"
                      fotmat="N2"
                      editType="numericedit"
                      edit={this.numericParams}
                    />
                    <ColumnDirective field="typeInvoice" visible={false} />
                  </ColumnsDirective>
                  <Inject services={[Page, Toolbar, Edit, Resize]} />
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
