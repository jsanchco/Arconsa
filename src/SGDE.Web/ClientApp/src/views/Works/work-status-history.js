import React, { Component, Fragment } from "react";
import { Row } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Edit,
  Inject,
  Toolbar,
  Group,
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, WORKSTATUSHISTORIES } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY } from "../../services";

L10n.load(data);

class WorkStatusHistory extends Component {
  workStatusHistories = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${WORKSTATUSHISTORIES}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  grid = null;

  constructor(props) {
    super(props);

    this.state = {
      modal: false,
      rowSelected: null,
    };

    this.toolbarOptions = [
      "Add",
      "Edit",
      "Delete",
      "Update",
      "Cancel",
      "Print"
    ];
    this.editSettings = {
      showDeleteConfirmDialog: true,
      allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      newRowPosition: "Top",
    };
    this.actionFailure = this.actionFailure.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
    this.actionBegin = this.actionBegin.bind(this);
    this.dataBound = this.dataBound.bind(this);
    this.beforePrint = this.beforePrint.bind(this);

    this.animationSettings = { effect: "None" };

    this.query = new Query().addParams("workId", props.workId);

    this.format = { type: "dateTime", format: "dd/MM/yyyy" };

    this.typeWorkStatusHistory = [
      { id: "Abierta" },
      { id: "Juridico" },
      { id: "Cerrada" },
    ];
    this.typeWorkStatusHistoryParams = {
      params: {
        sortOrder: "none",
        popupWidth: "auto",
      },
    };
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
      this.setState({ rowSelected: null });
      this.grid.clearSelection();
    }
    if (args.requestType === "delete") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });
      this.setState({ rowSelected: null });
    }
  }

  dataBound(args) {}

  beforePrint(args) {
    var div = document.createElement("Div");
    div.innerHTML = this.props.workName;
    div.style.textAlign = "center";
    div.style.color = "red";
    div.style.padding = "10px 0";
    div.style.fontSize = "25px";
    args.element.insertBefore(div, args.element.childNodes[0]);
  }

  printComplete(args) {
  }

  render() {
    return (
      <Fragment>
        <div className="animated fadeIn" id="target-work-status-history">
          <div
            className="card"
            style={{ marginRight: "60px", marginTop: "20px" }}
          >
            <div className="card-header">
              <i className="icon-layers"></i> Historial de Estados
            </div>
            <div className="card-body"></div>
            <Row>
              <GridComponent
                dataSource={this.workStatusHistories}
                locale="es-US"
                toolbar={this.toolbarOptions}
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
                actionBegin={this.actionBegin}
                ref={(g) => (this.grid = g)}
                query={this.query}
                allowGrouping={true}
                dataBound={this.dataBound}
                beforePrint={this.beforePrint}
                printComplete={this.printComplete}
              >
                <ColumnsDirective>
                  <ColumnDirective
                    field="id"
                    headerText="Id"
                    isPrimaryKey={true}
                    isIdentity={true}
                    visible={false}
                  />
                  <ColumnDirective
                    field="dateChange"
                    headerText="Fecha"
                    width="100"
                    type="date"
                    format={this.format}
                    editType="datepickeredit"
                    defaultValue={new Date()}
                    validationRules={{ required: true }}
                  />
                  <ColumnDirective
                    field="value"
                    headerText="Estado"
                    width="70"
                    editType="dropdownedit"
                    foreignKeyValue="id"
                    foreignKeyField="id"
                    dataSource={new DataManager(this.typeWorkStatusHistory)}
                    edit={this.typeWorkStatusHistoryParams}
                    validationRules={{ required: true }}
                  />
                  <ColumnDirective
                    field="observations"
                    headerText="Observaciones"
                    width="300"
                  />
                  <ColumnDirective
                    field="workId"
                    defaultValue={this.props.workId}
                    visible={false}
                  />
                </ColumnsDirective>
                <Inject services={[Toolbar, Edit, Group]} />
              </GridComponent>
            </Row>
          </div>
        </div>
      </Fragment>
    );
  }
}

WorkStatusHistory.propTypes = {};

export default WorkStatusHistory;
