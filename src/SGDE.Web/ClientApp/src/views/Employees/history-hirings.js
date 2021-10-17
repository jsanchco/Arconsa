import React, { Component, Fragment } from "react";
import { Row, Col } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Inject,
  ContextMenu,
  Page,
  Toolbar,
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, HISTORYHIRINGBYUSERID } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import {
  TOKEN_KEY,
  getUser,
  updateUserHiringInWorkByUser,
} from "../../services";
import Legend from "../../components/legend";

L10n.load(data);

class HistoryHirings extends Component {
  userHirings = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${HISTORYHIRINGBYUSERID}`,
    // url: `${config.URL_API}/${USERSHIRING}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  grid = null;
  contextMenuItems = [
    { text: "En Obra", target: ".e-content", id: "inWork" },
    { text: "Fuera de Obra", target: ".e-content", id: "outWork" },
  ];

  wrapSettings = { wrapMode: "Content" };

  constructor(props) {
    super(props);

    this.state = {
      trainings: null,
      rowSelected: null,
      rowSelectedindex: null,
      user: {
        fullname: "",
      },
    };

    this.toolbarOptions = ["Print"];

    this.format = { type: "dateTime", format: "dd/MM/yyyy" };

    this.contextMenuClick = this.contextMenuClick.bind(this);
    this.contextMenuOpen = this.contextMenuOpen.bind(this);
    this.rowSelected = this.rowSelected.bind(this);
    this.beforePrint = this.beforePrint.bind(this);

    this.pageSettings = { pageCount: 10, pageSize: 10 };
    this.query = new Query().addParams("userId", props.userId);
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
      return (
        <div>
          En Obra
        </div>
      );
    } else {
      return (
        <div>
          Fuera de Obra
        </div>
      );
    }
  }

  contextMenuClick(args) {
    const historyHiringSelected = this.state.rowSelected;
    if (args.item.id === "inWork") {
      historyHiringSelected.inWork = true;
      updateUserHiringInWorkByUser(historyHiringSelected).then(() => {
        this.grid.refresh();
      });
    }
    if (args.item.id === "outWork") {
      historyHiringSelected.inWork = false;
      updateUserHiringInWorkByUser(historyHiringSelected).then(() => {
        this.grid.refresh();
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

  beforePrint(args) {
    var div = document.createElement("Div");
    div.innerHTML = this.state.user.fullname;
    div.style.textAlign = "center";
    div.style.color = "red";
    div.style.padding = "10px 0";
    div.style.fontSize = "25px";
    args.element.insertBefore(div, args.element.childNodes[0]);
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

  render() {
    return (
      <Fragment>
        <div className="animated fadeIn">
          <div
            className="card"
            style={{ marginRight: "60px", marginTop: "20px" }}
          >
            <div className="card-header">
              <i className="icon-layers"></i> Historial de Contratación
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
                dataSource={this.userHirings}
                locale="es"
                allowPaging={true}
                pageSettings={this.pageSettings}
                style={{
                  marginLeft: 30,
                  marginRight: 30,
                  marginTop: -20,
                  marginBottom: 20,
                }}
                allowGrouping={false}
                ref={(g) => (this.grid = g)}
                query={this.query}
                allowTextWrap={true}
                toolbar={this.toolbarOptions}
                rowSelected={this.rowSelected}
                textWrapSettings={this.wrapSettings}
                contextMenuItems={this.contextMenuItems}
                contextMenuOpen={this.contextMenuOpen}
                contextMenuClick={this.contextMenuClick}
                beforePrint={this.beforePrint}
                toolbarClick={this.toolbarClick}
                printComplete={this.printComplete}
              >
                <ColumnsDirective>
                  <ColumnDirective
                    field="clientName"
                    headerText="Cliente"
                    width="100"
                  />
                  <ColumnDirective
                    field="workName"
                    headerText="Obra"
                    width="100"
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
                    field="professionName"
                    headerText="Profesión"
                    width="100"
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
                <Inject services={[Page, Toolbar, ContextMenu]} />
              </GridComponent>
            </Row>
          </div>
        </div>
      </Fragment>
    );
  }
}

HistoryHirings.propTypes = {};

export default HistoryHirings;
