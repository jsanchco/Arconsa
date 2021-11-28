import React, { Component, Fragment } from "react";
import { Row, Col } from "reactstrap";
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
import { config, HISTORYHIRINGBYWORKID, PROFESSIONSBYUSER } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { 
  TOKEN_KEY, 
  getUser, 
  updateUserHiringInWorkByUser
 } from "../../services";
import Legend from "../../components/legend";

L10n.load(data);

class AuthorizeCancelWorkers extends Component {
  usersHiring = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${HISTORYHIRINGBYWORKID}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  professions = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${PROFESSIONSBYUSER}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });
  grid = null;
  contextMenuItems = [
    { text: "En Obra", target: ".e-content", id: "inWork" },
    { text: "Fuera de Obra", target: ".e-content", id: "outWork" },
  ];


  professionIdRules = { required: false };

  constructor(props) {
    super(props);

    this.state = {
      usersHiring: null,
      rowSelected: null,
      rowSelectedindex: null
    };

    this.toolbarOptions = ["Edit", "Delete", "Update", "Cancel", "Print"];
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
    this.actionBegin = this.actionBegin.bind(this);
    this.beforePrint = this.beforePrint.bind(this);
    this.rowSelected = this.rowSelected.bind(this);
    this.contextMenuClick = this.contextMenuClick.bind(this);
    this.contextMenuOpen = this.contextMenuOpen.bind(this);

    this.template = this.gridTemplate;
    this.format = { type: "dateTime", format: "dd/MM/yyyy" };

    this.query = new Query()
      .addParams("workId", props.workId);     
  }

  componentDidMount() {
    getUser(this.props.userId).then((result) => {
      this.setState({
        user: {
          fullname: result.fullname,
        },
      });
    });
  }

  contextMenuOpen() {
    const { rowSelected } = this.state;
    var contextMenuObj = this.grid.contextMenuModule.contextMenu;
    if (rowSelected && rowSelected.inWork) {
      // contextMenuObj.enableItems(["Cerrar Obra"], false);
      contextMenuObj.hideItems(["En Obra"]);
      contextMenuObj.showItems(["Fuera de Obra"]);
    } else {
      // contextMenuObj.enableItems(["Abrir Obra"], false);
      contextMenuObj.hideItems(["Fuera de Obra"]);
      contextMenuObj.showItems(["En Obra"]);
    }
  }

  contextMenuClick(args) {
    const userHiringSelected = this.state.rowSelected;
    if (userHiringSelected === null || userHiringSelected === undefined) {
      return;
    }
    
    if (args.item.id === "inWork") {
      updateUserHiringInWorkByUser({ userHiringId: userHiringSelected.id, inWork: true }).then(() => {
        this.grid.refresh();
      });
    }
    if (args.item.id === "outWork") {
      updateUserHiringInWorkByUser({ userHiringId: userHiringSelected.id, inWork: false }).then(() => {
        this.grid.refresh();
      });
    }
  }

  inWorkTemplate(args) {
    if (args.inWork === true) {
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

  inWorkStatus(args) {
    if (args.inWork === true) {
      return <div>En Obra</div>;
    } else {
      return <div>Fuera de Obra</div>;
    }
  }

  toolbarClick(args) {
    for (var i = 0; i < this.columns.length; i++) {
      if (this.columns[i].field === "status") {
        this.columns[i].visible = true;
      } else if (this.columns[i].field === "inWork") {
        this.columns[i].visible = false;
      }
    }
  }

  printComplete(args) {
    for (var i = 0; i < this.columns.length; i++) {
      if (this.columns[i].field === "status") {
        this.columns[i].visible = false;
      } else if (this.columns[i].field === "inWork") {
        this.columns[i].visible = true;
      }
    }
  }

  gridTemplate(args) {
    if (args.file !== null && args.file !== "") {
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

  actionBegin(args) {
    if (args.requestType === "beginEdit") {
      this.grid.columnModel[2].edit.params.query.params = [];
      this.grid.columnModel[2].edit.params.query.addParams("userId", args.rowData.userId);
    }

    if (args.requestType === "save") {
      let date = this.formatDate(args.data.startDate);
      args.data.startDate = date;

      if (
        args.data.endDate !== null &&
        args.data.endDate !== "" &&
        args.data.endDate !== undefined
      ) {
        date = this.formatDate(args.data.endDate);
        args.data.endDate = date;
      }
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

  beforePrint(args) {
    var div = document.createElement("Div");
    div.innerHTML = this.props.workName;
    div.style.textAlign = "center";
    div.style.color = "red";
    div.style.padding = "10px 0";
    div.style.fontSize = "25px";
    args.element.insertBefore(div, args.element.childNodes[0]);
  }

  rowSelected() {
    const selectedRecords = this.grid.getSelectedRecords();
    const selectedRowIndex = this.grid.getSelectedRowIndexes();
    this.setState({
      rowSelected: selectedRecords[0],
      rowSelectedindex: selectedRowIndex[0],
    });
  }

  render() {
    let title = ` Contratos [${this.props.workName}]`;

    return (
      <Fragment>
        <div className="animated fadeIn">
          <div
            className="card"
            style={{ marginRight: "60px", marginTop: "20px" }}
          >
            <div className="card-header">
              <i className="icon-layers"></i>
              {title}
            </div>
            <div className="card-body"></div>
            <Row>
              <Col
                xs="9"
                style={{
                  marginTop: "-25px",
                  marginBottom: "25px",
                  marginLeft: "15px",
                }}
              >
                <Legend
                  elements={[
                    { color: "dot-green", text: "En Obra" },
                    { color: "dot-red", text: "Fuera de Obra" },
                  ]}
                />
              </Col>
            </Row>
            <Row>
              <GridComponent
                dataSource={this.usersHiring}
                locale="es-US"
                allowPaging={true}
                pageSettings={this.pageSettings}
                toolbar={this.toolbarOptions}
                editSettings={this.editSettings}
                style={{
                  marginLeft: 30,
                  marginRight: 30,
                  marginTop: -20,
                  marginBottom: 20,
                }}
                actionFailure={this.actionFailure}
                actionComplete={this.actionComplete}
                actionBegin={this.actionBegin}
                allowGrouping={false}
                rowSelected={this.rowSelected}
                ref={(g) => (this.grid = g)}
                query={this.query}
                beforePrint={this.beforePrint}
                printComplete={this.printComplete}
                toolbarClick={this.toolbarClick}
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
                    field="userName"
                    headerText="Trabajador"
                    width="100"
                    allowEditing={false}
                  />
                  <ColumnDirective
                    field="professionId"
                    headerText="Puesto"
                    width="100"
                    editType="dropdownedit"
                    foreignKeyValue="name"
                    foreignKeyField="id"
                    validationRules={this.professionIdRules}
                    dataSource={this.professions}
                  />
                  <ColumnDirective
                    field="startDate"
                    headerText="Fecha Inicio"
                    width="100"
                    type="date"
                    format={this.format}
                    editType="datepickeredit"
                  />
                  <ColumnDirective
                    field="endDate"
                    headerText="Fecha Fin"
                    width="100"
                    type="date"
                    format={this.format}
                    editType="datepickeredit"
                  />
                  <ColumnDirective
                    field="inWork"
                    headerText="Estado"
                    template={this.inWorkTemplate}
                    textAlign="Center"
                    width="100"
                  />
                  <ColumnDirective
                    field="status"
                    headerText="Estado"
                    textAlign="Center"
                    width="100"
                    visible={false}
                  />
                </ColumnsDirective>
                <Inject services={[Page, Toolbar, Edit]} />
              </GridComponent>
            </Row>
          </div>
        </div>
      </Fragment>
    );
  }
}

AuthorizeCancelWorkers.propTypes = {};

export default AuthorizeCancelWorkers;
