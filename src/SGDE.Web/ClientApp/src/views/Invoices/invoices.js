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
import { config, INVOICES, CLIENTSLITE, WORKSLITE } from "../../constants";
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
      write: () => {
        this.clientsObj = new DropDownList({
          change: () => {
            this.worksObj.enabled = true;
            const tempQuery = new Query().addParams("clientId", this.clientsObj.value);
            this.worksObj.query = tempQuery;
            this.worksObj.text = "";
            this.worksObj.dataBind();
          },
          dataSource: new DataManager({
            adaptor: new WebApiAdaptor(),
            url: `${config.URL_API}/${CLIENTSLITE}`,
            headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
          }),
          fields: { value: "id", text: "name" },
          floatLabelType: "Never",
          placeholder: "Selecciona Cliente",
          popupWidth: "auto",
          allowFiltering: true,
          filtering: this.handleFilteringClients.bind(this)  
        });
        this.clientsObj.appendTo(this.clientsElem);
      }
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
      write: () => {
        this.worksObj = new DropDownList({
          // change: () => {
          //   this.stateObj.enabled = true;
          //   const tempQuery = new Query().where(
          //     "CustomerID",
          //     "equal",
          //     this.countryObj.value
          //   );
          //   this.stateObj.query = tempQuery;
          //   this.stateObj.text = "";
          //   this.stateObj.dataBind();
          // },
          dataSource: new DataManager({
            adaptor: new WebApiAdaptor(),
            url: `${config.URL_API}/${WORKSLITE}`,
            headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
          }),
          enabled: false,
          fields: { value: "id", text: "name" },
          floatLabelType: "Never",
          placeholder: "Selecciona Obra",
          popupWidth: "auto"
        });
        this.worksObj.appendTo(this.worksElem);
      }      
    };

    this.formatDate = { type: "dateTime", format: "dd/MM/yyyy" };

    this.actionBegin = this.actionBegin.bind(this);
    this.actionFailure = this.actionFailure.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
    this.clickHandler = this.clickHandler.bind(this);
    this.dataBound = this.dataBound.bind(this);
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

  changeClient(args) {

  }

  selectClient(args) {
    this.editWorks.params.query.params = [];
    if (args.itemData.id != null) {
      this.editWorks.params.query.addParams("clientId", args.itemData.id);
    }

    // this.grid.columnModel[7].edit.params.query.params = [];
    // if (args.itemData.id != null) {
    //   this.grid.columnModel[7].edit.params.query.addParams("clientId", args.itemData.id);
    // }
  }

  handleFilteringClients(e) {
    let query = new Query();
    query =
      e.text !== "" ? query.where("name", "contains", e.text, true) : query;
      e.updateData(this.clients, query);
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
                  // rowSelected={this.rowSelected}
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
                      // editType="dropdownedit"
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
                      //editType="dropdownedit"
                      foreignKeyValue="name"
                      foreignKeyField="id"
                      validationRules={this.requeridIdRules}
                      dataSource={this.works}
                      edit={this.editWorks}
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
