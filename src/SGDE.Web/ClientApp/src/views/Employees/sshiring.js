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
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, SSHIRINGS } from "../../constants";
import { loadCldr, L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY, getUser } from "../../services";

import * as numberingSystems from "cldr-data/supplemental/numberingSystems.json";
import * as gregorian from "cldr-data/main/es-US/ca-gregorian.json";
import * as numbers from "cldr-data/main/es-US/numbers.json";
import * as timeZoneNames from "cldr-data/main/es-US/timeZoneNames.json";

loadCldr(numberingSystems, gregorian, numbers, timeZoneNames);

L10n.load(data);

class SSHirings extends Component {
  sSHirings = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${SSHIRINGS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  startDateRules = { required: true };
  grid = null;

  wrapSettings = { wrapMode: "Content" };

  constructor(props) {
    super(props);

    this.state = {
      rowSelected: null,
      user: {
        fullname: null,
      },      
    };

    this.toolbarOptions = [
      "Add",
      "Edit",
      "Delete",
      "Update",
      "Cancel",
      "Print",
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
    this.actionBegin = this.actionBegin.bind(this);
    this.rowSelected = this.rowSelected.bind(this);
    this.rowDataBound = this.rowDataBound.bind(this);
    this.beforePrint = this.beforePrint.bind(this);

    this.format = { type: "dateTime", format: "dd/MM/yyyy" };

    this.querySSHirings = new Query().addParams("userId", props.userId);

    this.animationSettings = { effect: "None" };
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

  beforePrint(args) {
    var div = document.createElement("Div");
    div.innerHTML = this.state.user.fullname;
    div.style.textAlign = "center";
    div.style.color = "red";
    div.style.padding = "10px 0";
    div.style.fontSize = "25px";
    args.element.insertBefore(div, args.element.childNodes[0]);
  }  

  actionBegin(args) {}

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

  rowSelected() {
    const selectedRecords = this.grid.getSelectedRecords();
    this.setState({ rowSelected: selectedRecords[0] });
  }

  rowDataBound(args) {
  }

  render() {
    return (
      <Fragment>
        <div className="animated fadeIn" id="target-daily-signing">
          <div
            className="card"
            style={{ marginRight: "60px", marginTop: "20px" }}
          >
            <div className="card-header">
              <i className="icon-layers"></i> Historial en la Seguridad Social
            </div>
            <div className="card-body"></div>
            <Row>
              <GridComponent
                dataSource={this.sSHirings}
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
                rowDataBound={this.rowDataBound}
                ref={(g) => (this.grid = g)}
                query={this.querySSHirings}
                allowTextWrap={true}
                textWrapSettings={this.wrapSettings}
                beforePrint={this.beforePrint}
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
                    field="startDate"
                    headerText="Fecha Inicio"
                    width="100"
                    editType="datepickeredit"
                    type="date"
                    format={this.format}
                    textAlign="Center"
                    validationRules={this.startDateRules}
                  />
                  <ColumnDirective
                    field="endDate"
                    headerText="Fecha Fin"
                    width="100"
                    editType="datepickeredit"
                    type="date"
                    format={this.format}
                    textAlign="Center"
                  />
                  <ColumnDirective
                    field="observations"
                    headerText="Observaciones"
                    width="200"
                  />
                  <ColumnDirective
                    field="userId"
                    visible={false}
                    defaultValue={this.props.userId}
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

SSHirings.propTypes = {};

export default SSHirings;
