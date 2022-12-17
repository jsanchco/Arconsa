import React, { Component, Fragment } from "react";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Edit,
  Inject,
  Toolbar,
  Page,
  ForeignKey,
  ContextMenu,
  Group,
  Resize,
} from "@syncfusion/ej2-react-grids";
import { AppSwitch } from "@coreui/react";
import {
  Breadcrumb,
  BreadcrumbItem,
  Container,
  FormGroup,
  Label,
  Row,
  Col,
} from "reactstrap";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, WORKS, CLIENTSWITHOUTFILTER } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import { TOKEN_KEY, updateDatesWork } from "../../services";
import ModalWorkers from "../Modals/modal-workers";
import ModalMassiveSigningWorks from "../Modals/modal-massive-signing-works";
import Legend from "../../components/legend";

L10n.load(data);

class Works extends Component {
  works = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${WORKS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  clients = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${CLIENTSWITHOUTFILTER}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  grid = null;
  clientIdRules = { required: true };
  contextMenuItems = [
    { text: "Cerrar Obra", target: ".e-content", id: "closeWork" },
    { text: "Abrir Obra", target: ".e-content", id: "openWork" },
  ];
  numericParams = {
    params: {
      decimals: 0,
      format: "N",
      validateDecimalOnType: true,
      showSpinButton: false
    },
  };
  wrapSettings = { wrapMode: "Content" };

  constructor(props) {
    super(props);

    this.state = {
      works: null,
      clients: null,
      rowSelected: null,
      rowSelectedindex: null,
      modal: false,
      modalTemplateSigning: false,
      showCloseWorks: true,
    };

    this.toolbarOptions = [
      "Add",
      "Edit",
      "Delete",
      "Update",
      "Cancel",
      // {
      //   text: "Plantilla Automática",
      //   tooltipText: "Plantilla para a generación de Fichajes Automática",
      //   prefixIcon: "e-custom-icons e-details",
      //   id: "TemplateSigning",
      // },
      {
        text: "Detalles",
        tooltipText: "Detalles",
        prefixIcon: "e-custom-icons e-details",
        id: "Details",
      },
      {
        text: "Añadir/Quitar trabajadores",
        tooltipText: "Añadir/Quitar Trabajadores",
        prefixIcon: "e-custom-icons e-file-workers",
        id: "Workers",
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
      currentPage: props.currentPageWorks,
    };
    this.searchSettings = {
      key: props.currentSearchWorks,
    };
    this.actionFailure = this.actionFailure.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
    this.contextMenuClick = this.contextMenuClick.bind(this);
    this.rowSelected = this.rowSelected.bind(this);
    this.contextMenuOpen = this.contextMenuOpen.bind(this);
    this.statusTemplate = this.statusTemplate.bind(this);
    this.dateTemplate = this.dateTemplate.bind(this);
    this.clientTemplate = this.clientTemplate.bind(this);
    this.clickHandler = this.clickHandler.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
    this.toggleModalTemplateSigning =
      this.toggleModalTemplateSigning.bind(this);
    this.handleInputChange = this.handleInputChange.bind(this);
    this.dataBound = this.dataBound.bind(this);

    this.selectionSettings = {
      checkboxMode: "ResetOnRowClick",
      type: "Single",
    };

    this.query = new Query().addParams(
      "showCloseWorks",
      this.state.showCloseWorks
    );

    this.editClients = {
      params: {
        popupWidth: "auto",
      },
    };

    this.typeWork = [{ id: "HO" }, { id: "PA" }, { id: "MA" }];
  }

  componentDidUpdate(prevProps, prevState) {
    if (prevState.showWorksClosed !== this.state.showWorksClosed) {
      console.log("showWorksClosed -> ", this.state.showWorksClosed);
      this.grid.query = new Query().addParams(
        "showCloseWorks",
        this.state.showWorksClosed
      );
      this.grid.refresh();
    }

    // if (this.enablePersistence === true) {
    //   this.enablePersistence = false;
    //   var stateGrid = window.localStorage.getItem("gridWorks");
    //   if (stateGrid !== null && stateGrid !== undefined) {
    //     var model = JSON.parse(stateGrid);
    //     this.grid.setProperties(model);
    //   }
    // }
  }

  dataBound() {
    this.props.setCurrentPageWorks(this.grid.pageSettings.currentPage);
    this.props.setCurrentSearchWorks(this.grid.searchSettings.key);
  }

  clickHandler(args) {
    if (args.item.id === "Details") {
      const selectedRecords = this.grid.getSelectedRecords();
      if (Array.isArray(selectedRecords) && selectedRecords.length === 1) {
        this.setState({ rowSelected: selectedRecords[0] });

        this.props.history.push({
          pathname: "/works/detailwork/" + selectedRecords[0].id,
          state: {
            work: selectedRecords[0],
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

    if (args.item.id === "Workers") {
      const selectedRecords = this.grid.getSelectedRecords();
      if (Array.isArray(selectedRecords) && selectedRecords.length === 1) {
        this.setState({ rowSelected: selectedRecords[0] });
        this.toggleModal();
      } else {
        this.setState({ rowSelected: null });
        this.props.showMessage({
          statusText: "Debes seleccionar un solo registro",
          responseText: "Debes seleccionar un solo registro",
          type: "danger",
        });
      }
    }

    if (args.item.id === "TemplateSigning") {
      const selectedRecords = this.grid.getSelectedRecords();
      if (Array.isArray(selectedRecords) && selectedRecords.length === 1) {
        this.setState({ rowSelected: selectedRecords[0] });
        this.toggleModalTemplateSigning();
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

  toggleModal() {
    this.setState({
      modal: !this.state.modal,
    });
  }

  toggleModalTemplateSigning() {
    this.setState({
      modalTemplateSigning: !this.state.modalTemplateSigning,
    });
  }

  contextMenuOpen() {
    const { rowSelected } = this.state;
    var contextMenuObj = this.grid.contextMenuModule.contextMenu;
    if (rowSelected && rowSelected.open) {
      // contextMenuObj.enableItems(["Cerrar Obra"], false);
      contextMenuObj.hideItems(["Abrir Obra"]);
      contextMenuObj.showItems(["Cerrar Obra"]);
    } else {
      // contextMenuObj.enableItems(["Abrir Obra"], false);
      contextMenuObj.hideItems(["Cerrar Obra"]);
      contextMenuObj.showItems(["Abrir Obra"]);
    }
  }

  statusTemplate(args) {
    switch (args.status) {
      case "Abierta":
        return (
          <div>
            <span className="dot-green"></span>
          </div>
        );

      case "Cerrada":
        return (
          <div>
            <span className="dot-red"></span>
          </div>
        );
          
      case "Juridico":
        return (
          <div>
            <span className="dot-orange"></span>
          </div>
        );
        
      default:
        return (
          <div>
            <span className="dot-green"></span>
          </div>
        );  
    }
  }

  formatDate(args) {
    if (args === null || args === "") {
      return "";
    }

    let day = args.getDate();
    if (day < 10) day = "0" + day;

    const month = args.getMonth() + 1;
    const year = args.getFullYear();

    if (month < 10) {
      return `${day}/0${month}/${year}`;
    } else {
      return `${day}/${month}/${year}`;
    }
  }

  dateTemplate(args) {
    // const titleOpen = `${this.formatDate(args.openDate)}`;
    // const titleClose = `${this.formatDate(args.closeDate)}`;
    return (
      <div>
        <div style={{ display: "flex" }}>
          <div style={{ textAlign: "left", width: "30%" }}>Aper.:</div>
          <div style={{ textAlign: "left", width: "70%" }}>{args.openDate}</div>
        </div>
        <div style={{ display: "flex" }}>
          <div style={{ textAlign: "left", width: "30%" }}>Cier.:</div>
          <div style={{ textAlign: "left", width: "70%" }}>
            {args.closeDate}
          </div>
        </div>
      </div>
    );
  }

  clientTemplate(args) {
    return ( 
      <div> 
        <a rel='nofollow' href={"/#/clients/detailclient/" + args.clientId}>{args.clientName}</a>
      </div> 
    ); 
  }  

  contextMenuClick(args) {
    const workSelected = this.state.rowSelected;
    if (args.item.id === "openWork") {
      workSelected.open = true;
      updateDatesWork(workSelected).then(() => {
        this.grid.setCellValue(workSelected.id, "closeDate", null);
        this.grid.setCellValue(
          workSelected.id,
          "openDate",
          this.formatDate(new Date())
        );
        this.grid.setCellValue(workSelected.id, "open", true);
      });
    }
    if (args.item.id === "closeWork") {
      workSelected.open = false;
      updateDatesWork(workSelected).then(() => {
        this.grid.setCellValue(
          workSelected.id,
          "closeDate",
          this.formatDate(new Date())
        );
        this.grid.setCellValue(workSelected.id, "open", false);
      });
    }
  }

  rowSelected() {
    const selectedRecords = this.grid.getSelectedRecords();
    const selectedRowIndex = this.grid.getSelectedRowIndexes();
    this.setState({
      rowSelected: selectedRecords[0],
      rowSelectedindex: selectedRowIndex[0],
    });
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

  headerCellInfo(args) {
    args.node.getElementsByClassName("e-checkbox-wrapper")[0] &&
      args.node.getElementsByClassName("e-checkbox-wrapper")[0].remove();
  }

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;

    if (name === "worksClosed") {
      this.setState({
        showWorksClosed: !target.checked,
      });
    }
  }

  render() {
    return (
      <Fragment>
        <Breadcrumb>
          {/*eslint-disable-next-line*/}
          <BreadcrumbItem>
            <a href="/#">Inicio</a>
          </BreadcrumbItem>
          {/* eslint-disable-next-line*/}
          <BreadcrumbItem active>Obras</BreadcrumbItem>
        </Breadcrumb>

        <ModalWorkers
          isOpen={this.state.modal}
          toggle={this.toggleModal}
          workSelected={this.state.rowSelected}
          showMessage={this.props.showMessage}
        />

        {this.state.rowSelected ? (
          <ModalMassiveSigningWorks
            isOpen={this.state.modalTemplateSigning}
            toggle={this.toggleModalTemplateSigning}
            userId={this.props.userId}
            workId={this.state.rowSelected.id}
            workName={this.state.rowSelected.name}
            showMessage={this.props.showMessage}
          />
        ) : null}

        <Container fluid>
          <div className="animated fadeIn" id="target-works">
            <div className="card">
              <div className="card-header">
                <i className="icon-globe"></i> Obras
              </div>
              <div className="card-body"></div>

              <div
                style={{
                  marginLeft: "35px",
                  marginTop: "-20px",
                  marginBottom: "30px",
                }}
              >
                <Row>
                  <Col xs="9">
                    <Legend
                      elements={[
                        { color: "dot-green", text: "Abierta" },
                        { color: "dot-orange", text: "Juridico" },
                        { color: "dot-red", text: "Cerrada" },
                      ]}
                    />
                  </Col>
                  <Col xs="3">
                    <FormGroup style={{ marginTop: -20 }}>
                      <Label
                        htmlFor="worksClosed"
                        style={{ verticalAlign: "bottom" }}
                      >
                        Mostrar obras cerradas&nbsp;
                      </Label>
                      <AppSwitch
                        className={"mx-1 mt-4"}
                        variant={"pill"}
                        color={"primary"}
                        label
                        id="worksClosed"
                        name="worksClosed"
                        placeholder="obras cerradas"
                        onChange={this.handleInputChange}
                        dataOn="No"
                        dataOff="Si"
                      />
                    </FormGroup>
                  </Col>
                </Row>
              </div>

              <div>
                <GridComponent
                  dataSource={this.works}
                  id="Works"
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
                    overflow: "auto",
                  }}
                  actionFailure={this.actionFailure}
                  actionComplete={this.actionComplete}
                  allowGrouping={true}
                  rowSelected={this.rowSelected}
                  ref={(g) => (this.grid = g)}
                  contextMenuItems={this.contextMenuItems}
                  contextMenuOpen={this.contextMenuOpen}
                  contextMenuClick={this.contextMenuClick}
                  // selectionSettings={this.selectionSettings}
                  headerCellInfo={this.headerCellInfo}
                  allowTextWrap={true}
                  textWrapSettings={this.wrapSettings}
                  allowResizing={true}
                  query={this.query}
                  // enablePersistence={true}
                  dataBound={this.dataBound}
                >
                  <ColumnsDirective>
                    {/* <ColumnDirective
                      type="checkbox"
                      width="50"
                    ></ColumnDirective> */}
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
                    <ColumnDirective
                      field="address"
                      headerText="Dirección"
                      width="200"
                    />
                    {/* <ColumnDirective
                  field="estimatedDuration"
                  headerText="Duración Estimada"
                  width="100"
                /> */}
                    {/* <ColumnDirective
                      field="worksToRealize"
                      headerText="Tipo"
                      width="50"
                    /> */}
                    <ColumnDirective
                      field="worksToRealize"
                      headerText="Tipo"
                      width="70"
                      editType="dropdownedit"
                      foreignKeyValue="id"
                      foreignKeyField="id"
                      dataSource={new DataManager(this.typeWork)}
                    />
                    <ColumnDirective
                      field="numberPersonsRequested"
                      headerText="Personas"
                      width="50"
                      textAlign="right"
                      allowEditing={false}
                    />
                    <ColumnDirective
                      field="clientId"
                      headerText="Cliente"
                      width="100"
                      editType="dropdownedit"
                      foreignKeyValue="name"
                      foreignKeyField="id"
                      validationRules={this.clientIdRules}
                      dataSource={this.clients}
                      edit={this.editClients}
                      template={this.clientTemplate}
                    />
                    <ColumnDirective
                      field="closeDate"
                      headerText="Fechas"
                      width="100"
                      template={this.dateTemplate}
                      textAlign="Center"
                      allowEditing={false}
                      defaultValue={true}
                    />
                    <ColumnDirective
                      field="status"
                      headerText="Estado"
                      width="100"
                      template={this.statusTemplate}
                      textAlign="Center"
                      allowEditing={false}
                    />
                    <ColumnDirective field="openDate" visible={false} />
                    <ColumnDirective field="closeDate" visible={false} />
                    <ColumnDirective field="percentageIVA" visible={false} />
                  </ColumnsDirective>
                  <Inject
                    services={[
                      Group,
                      ContextMenu,
                      ForeignKey,
                      Page,
                      Toolbar,
                      Edit,
                      Resize,
                    ]}
                  />
                </GridComponent>
              </div>
            </div>
          </div>
        </Container>
      </Fragment>
    );
  }
}

Works.propTypes = {};

const mapStateToProps = (state) => {
  return {
    currentPageWorks: state.applicationReducer.currentPageWorks,
    currentSearchWorks: state.applicationReducer.currentSearchWorks,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
  setCurrentPageWorks: (currentPageWorks) =>
    dispatch(ACTION_APPLICATION.setCurrentPageWorks(currentPageWorks)),
  setCurrentSearchWorks: (currentSearchWorks) =>
    dispatch(ACTION_APPLICATION.setCurrentSearchWorks(currentSearchWorks)),
});

export default connect(mapStateToProps, mapDispatchToProps)(Works);
