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
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor } from "@syncfusion/ej2-data";
import { config, CLIENTS } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import { TOKEN_KEY } from "../../services";

L10n.load(data);

class Clients extends Component {
  clients = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${CLIENTS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  grid = null;
  wrapSettings = { wrapMode: "Content" };
  initialRender = true;

  constructor(props) {
    super(props);

    this.state = {
      rowSelected: null,
    };

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
    this.pageSettings = { pageCount: 10, pageSize: 10 };
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
    if (this.initialRender) {
      var stateGrid = window.localStorage.getItem("gridClients");
        if (stateGrid !== null && stateGrid !== undefined) {
          var model = JSON.parse(stateGrid);
          this.grid.setProperties(model);
        }
      this.initialRender = false;
    }
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

  render() {
    return (
      <Fragment>
        <Breadcrumb class>
          {/*eslint-disable-next-line*/}
          <BreadcrumbItem><a href="#">Inicio</a></BreadcrumbItem>
          {/* eslint-disable-next-line*/}
          <BreadcrumbItem active>Clientes</BreadcrumbItem>
        </Breadcrumb>

        <Container fluid>
          <div className="animated fadeIn">
            <div className="card">
              <div className="card-header">
                <i className="icon-chart"></i> Clientes
              </div>
              <div className="card-body"></div>
              <Row>
                <GridComponent
                  dataSource={this.clients}
                  id="Clients"
                  locale="es"
                  allowPaging={true}
                  pageSettings={this.pageSettings}
                  toolbar={this.toolbarOptions}
                  toolbarClick={this.clickHandler}
                  editSettings={this.editSettings}
                  style={{
                    marginLeft: 30,
                    marginRight: 30,
                    marginTop: -20,
                    marginBottom: 20,
                  }}
                  actionFailure={this.actionFailure}
                  actionComplete={this.actionComplete}
                  rowSelected={this.rowSelected}
                  ref={(g) => (this.grid = g)}
                  allowTextWrap={true}
                  textWrapSettings={this.wrapSettings}
                  // enablePersistence={true}
                  // dataBound={this.dataBound}
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
                      field="name"
                      headerText="Nombre"
                      width="100"
                    />
                    <ColumnDirective field="cif" headerText="CIF" width="100" />
                    <ColumnDirective
                      field="phoneNumber"
                      headerText="Teléfono(s)"
                      width="100"
                    />
                    <ColumnDirective
                      field="address"
                      headerText="Dirección"
                      width="100"
                    />
                  </ColumnsDirective>
                  <Inject services={[Page, Toolbar, Edit]} />
                </GridComponent>
              </Row>
            </div>
          </div>
        </Container>
      </Fragment>
    );
  }
}

Clients.propTypes = {};

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
});

export default connect(mapStateToProps, mapDispatchToProps)(Clients);
