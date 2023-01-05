import React, { Component, Fragment } from "react";
import {
  Breadcrumb,
  BreadcrumbItem,
  Container,
  Row,
  Col,
  FormGroup,
  Label,
} from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Edit,
  Inject,
  Toolbar,
  Page,
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, CLIENTS } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { connect } from "react-redux";
import { AppSwitch } from "@coreui/react";
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

  constructor(props) {
    super(props);

    this.state = {
      rowSelected: null,
      showAllClients: true,
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
    this.pageSettings = {
      pageCount: 10,
      pageSize: 10,
      currentPage: props.currentPageClients,
    };
    this.searchSettings = {
      key: props.currentSearchClients,
    };
    this.actionFailure = this.actionFailure.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
    this.clickHandler = this.clickHandler.bind(this);
    this.dataBound = this.dataBound.bind(this);
    this.handleInputChange = this.handleInputChange.bind(this);

    this.query = new Query().addParams("allClients", this.state.showAllClients);
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

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;

    if (name === "allClients") {
      this.setState({
        showAllClients: !target.checked,
      });
    }
  }

  dataBound() {
    this.props.setCurrentPageClients(this.grid.pageSettings.currentPage);
    this.props.setCurrentSearchClients(this.grid.searchSettings.key);
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

  componentDidUpdate(prevProps, prevState) {
    if (prevState.showAllClients !== this.state.showAllClients) {
      this.grid.query = new Query().addParams(
        "allClients",
        this.state.showAllClients
      );
      this.grid.refresh();
    }
  }

  idClientTemplate(args) {
    return (
      <div>
        <span><strong>{args.idClient}</strong></span>
      </div>
    );
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
          <BreadcrumbItem active>Clientes</BreadcrumbItem>
        </Breadcrumb>

        <Container fluid>
          <div className="animated fadeIn">
            <div className="card">
              <div className="card-header">
                <i className="icon-chart"></i> Clientes
              </div>
              <Row>
                <Col style={{ textAlign: "right", marginRight: 30 }}>
                  <FormGroup style={{}}>
                    <Label
                      htmlFor="allClients"
                      style={{ verticalAlign: "bottom" }}
                    >
                      Solo clientes activos&nbsp;
                    </Label>
                    <AppSwitch
                      className={"mx-1 mt-4"}
                      variant={"pill"}
                      color={"primary"}
                      label
                      id="allClients"
                      name="allClients"
                      placeholder="Solo clientes activos"
                      onChange={this.handleInputChange}
                      dataOn="Si"
                      dataOff="No"
                    />
                  </FormGroup>
                </Col>
              </Row>

              <Row>
                <GridComponent
                  dataSource={this.clients}
                  id="Clients"
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
                    // marginTop: -20,
                    marginBottom: 20,
                    overflow: "auto",
                  }}
                  actionFailure={this.actionFailure}
                  actionComplete={this.actionComplete}
                  rowSelected={this.rowSelected}
                  ref={(g) => (this.grid = g)}
                  query={this.query}
                  allowTextWrap={true}
                  textWrapSettings={this.wrapSettings}
                  // enablePersistence={true}
                  dataBound={this.dataBound}
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
                      field="idClient"
                      headerText="Id Cliente"
                      width="110"
                      allowEditing={false}
                      template={this.idClientTemplate}
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
                    <ColumnDirective field="active" visible={false} />
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
    currentPageClients: state.applicationReducer.currentPageClients,
    currentSearchClients: state.applicationReducer.currentSearchClients,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
  setCurrentPageClients: (currentPageClients) =>
    dispatch(ACTION_APPLICATION.setCurrentPageClients(currentPageClients)),
  setCurrentSearchClients: (currentSearchClients) =>
    dispatch(ACTION_APPLICATION.setCurrentSearchClients(currentSearchClients)),
});

export default connect(mapStateToProps, mapDispatchToProps)(Clients);
