import React, { Component, Fragment } from "react";
import { Row } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Edit,
  Inject,
  Toolbar,
  Page,
  ForeignKey,
  ContextMenu
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor } from "@syncfusion/ej2-data";
import { config, WORKS, CLIENTS } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import { TOKEN_KEY, updateWork } from "../../services";

L10n.load(data);

class Works extends Component {
  works = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${WORKS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  clients = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${CLIENTS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  grid = null;
  clientIdRules = { required: true };
  contextMenuItems = [
    { text: "Cerrar Obra", target: ".e-content", id: "closeWork" },
    { text: "Abrir Obra", target: ".e-content", id: "openWork" }
  ];
  numericParams = {
    params: {
      decimals: 0,
      format: "N",
      validateDecimalOnType: true
    }
  };

  constructor(props) {
    super(props);

    this.state = {
      works: null,
      clients: null,
      rowSelected: null,
      rowSelectedindex: null
    };

    this.toolbarOptions = ["Add", "Edit", "Delete", "Update", "Cancel"];
    this.editSettings = {
      showDeleteConfirmDialog: true,
      allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      newRowPosition: "Top"
    };
    this.pageSettings = { pageCount: 10, pageSize: 10 };
    this.actionFailure = this.actionFailure.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
    this.contextMenuClick = this.contextMenuClick.bind(this);
    this.rowSelected = this.rowSelected.bind(this);
    this.contextMenuOpen = this.contextMenuOpen.bind(this);
    this.openTemplate = this.openTemplate.bind(this);
    this.dateTemplate = this.dateTemplate.bind(this);
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

  openTemplate(args) {
    if (args.open === true) {
      return (
        <div>
          <span className="dot-green"></span>
        </div>
      );
    } else {
      return (
        <div>
          <span className="dot-red"></span>
        </div>
      );
    }
  }

  formatDate(args) {
    if (args === null || args === "") {
      return "";
    }

    const day = args.getDate();
    const month = args.getMonth() + 1;
    const year = args.getFullYear();

    if (month < 10) {
      return `${day}/0${month}/${year}`;
    } else {
      return `${day}/${month}/${year}`;
    }
  }

  dateTemplate(args) {
    const titleOpen = `${this.formatDate(args.openDate)}`;
    const titleClose = `${this.formatDate(args.closeDate)}`;
    return (
      <div>
        <div style={{display: "flex"}}>
          <div style={{ textAlign: "left", width: "50%" }}>Apertura:</div>
          <div style={{ textAlign: "left", width: "50%" }}>{titleOpen}</div>
        </div>
        <div style={{display: "flex"}}>
          <div style={{ textAlign: "left", width: "50%" }}>Cierre:</div>
          <div style={{ textAlign: "left", width: "50%" }}>{titleClose}</div>
        </div>
      </div>
    );
  }

  contextMenuClick(args) {
    const workSelected = this.state.rowSelected;
    if (args.item.id === "openWork") {
      workSelected.open = true;
      updateWork(workSelected).then(() => {
        this.grid.setCellValue(workSelected.id, "open", true);
        this.grid.setCellValue(workSelected.id, "closeDate", null);
        this.grid.setCellValue(workSelected.id, "openDate", new Date());
      });
    }
    if (args.item.id === "closeWork") {
      workSelected.open = false;
      updateWork(workSelected).then(() => {
        this.grid.setCellValue(workSelected.id, "open", false);
        this.grid.setCellValue(workSelected.id, "closeDate", new Date());
      });
    }
  }

  rowSelected() {
    const selectedRecords = this.grid.getSelectedRecords();
    const selectedRowIndex = this.grid.getSelectedRowIndexes();
    this.setState({
      rowSelected: selectedRecords[0],
      rowSelectedindex: selectedRowIndex[0]
    });
  }

  actionFailure(args) {
    const error = Array.isArray(args) ? args[0].error : args.error;
    this.props.showMessage({
      statusText: error.statusText,
      responseText: error.responseText,
      type: "danger"
    });
  }

  actionComplete(args) {
    if (args.requestType === "save") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success"
      });
    }
    if (args.requestType === "delete") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success"
      });
    }
  }

  render() {
    return (
      <Fragment>
        <div className="animated fadeIn">
          <div className="card">
            <div className="card-header">
              <i className="icon-globe"></i> Obras
            </div>
            <div className="card-body"></div>
            <Row>
              <GridComponent
                dataSource={this.works}
                locale="es-US"
                allowPaging={true}
                pageSettings={this.pageSettings}
                toolbar={this.toolbarOptions}
                toolbarClick={this.clickHandler}
                editSettings={this.editSettings}
                style={{
                  marginLeft: 30,
                  marginRight: 30,
                  marginTop: 20,
                  marginBottom: 20
                }}
                actionFailure={this.actionFailure}
                actionComplete={this.actionComplete}
                allowGrouping={true}
                rowSelected={this.rowSelected}
                ref={g => (this.grid = g)}
                contextMenuItems={this.contextMenuItems}
                contextMenuOpen={this.contextMenuOpen}
                contextMenuClick={this.contextMenuClick}
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
                  <ColumnDirective
                    field="address"
                    headerText="Dirección"
                    width="100"
                  />
                  <ColumnDirective
                    field="estimatedDuration"
                    headerText="Duración Estimada"
                    width="100"
                  />
                  <ColumnDirective
                    field="worksToRealize"
                    headerText="Trabajos a Realizar"
                    width="100"
                  />
                  <ColumnDirective
                    field="numberPersonsRequested"
                    headerText="Personas"
                    width="100"
                    fotmat="N0"
                    textAlign="right"
                    editType="numericedit"
                    edit={this.numericParams}
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
                  />
                  <ColumnDirective
                    field="closeDate"
                    headerText="Fechas"
                    width="100"
                    template={this.dateTemplate}
                    textAlign="Center"
                    allowEditing={false}
                  />
                  <ColumnDirective
                    field="open"
                    headerText="Abierta/Cerrada"
                    width="100"
                    template={this.openTemplate}
                    textAlign="Center"
                    allowEditing={false}
                    defaultValue={true}
                  />
                  <ColumnDirective field="openDate" visible={false} />
                  <ColumnDirective field="closeDate" visible={false} />
                </ColumnsDirective>
                <Inject
                  services={[ContextMenu, ForeignKey, Page, Toolbar, Edit]}
                />
              </GridComponent>
            </Row>
          </div>
        </div>
      </Fragment>
    );
  }
}

Works.propTypes = {};

const mapStateToProps = state => {
  return {
    errorApplication: state.applicationReducer.error
  };
};

const mapDispatchToProps = dispatch => ({
  showMessage: message => dispatch(ACTION_APPLICATION.showMessage(message))
});

export default connect(mapStateToProps, mapDispatchToProps)(Works);
