import React, { Component, Fragment } from "react";
import { Row } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Edit,
  Inject,
  Toolbar,
  Page
} from "@syncfusion/ej2-react-grids";
import { DateTimePickerComponent } from "@syncfusion/ej2-react-calendars";
import { getValue } from "@syncfusion/ej2-base";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, DAILYSIGNINGS, USERSHIRING } from "../../constants";
import { loadCldr, L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY } from "../../services";

import * as numberingSystems from "cldr-data/supplemental/numberingSystems.json";
import * as gregorian from "cldr-data/main/es-US/ca-gregorian.json";
import * as numbers from "cldr-data/main/es-US/numbers.json";
import * as timeZoneNames from "cldr-data/main/es-US/timeZoneNames.json";

loadCldr(numberingSystems, gregorian, numbers, timeZoneNames);

L10n.load(data);

class DailySignings extends Component {
  dailySignings = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${DAILYSIGNINGS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  userHirings = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${USERSHIRING}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  userHiringIdRules = { required: true };
  grid = null;

  constructor(props) {
    super(props);

    this.state = {
      dailySignings: null,
      userHirings: null,
      rowSelected: null
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
    this.actionBegin = this.actionBegin.bind(this);
    this.rowSelected = this.rowSelected.bind(this);
    this.startHourTemplate = this.startHourTemplate.bind(this);
    this.formatDate = this.formatDate.bind(this);

    this.template = this.gridTemplate;
    this.format = { type: "dateTime", format: "dd/MM/yyyy hh:mm" };

    this.queryDailySignings = new Query().addParams("userId", props.user.id);
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

  actionBegin(args) {
    if (args.requestType === "add" || args.requestType === "beginEdit") {
      this.grid.columns[0].edit.params.query.params = [];
      this.grid.columns[0].edit.params.query
        .addParams("userId", this.props.user.id)
        .addParams("workId", this.props.user.workId);
    }

    if (args.requestType === "save") {
      let date = this.formatDate(args.data.startHour);
      args.data.startHour = date;

      if (
        args.data.endHour !== null &&
        args.data.endHour !== "" &&
        args.data.endHour !== undefined
      ) {
        date = this.formatDate(args.data.endHour);
        args.data.endHour = date;
      }
    }
  }

  formatDate(args) {
    var date = new Date(args);
    var month = ("0" + (date.getMonth() + 1)).slice(-2);
    var day = ("0" + date.getDate()).slice(-2);
    var hours = ("0" + date.getHours()).slice(-2);
    var minutes = ("0" + date.getMinutes()).slice(-2);

    return `${[day, month, date.getFullYear()].join("/")} ${hours}:${minutes}`;
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

  startHourTemplate(args) {
    return (
      <DateTimePickerComponent
        value={getValue("startHour", args)}
        id="startHour"
        placeholder="Hora Inicio"
        floatLabelType="Never"
        format="dd/MM/yyyy HH:mm"
      />
    );
  }

  endHourTemplate(args) {
    return (
      <DateTimePickerComponent
        value={getValue("endHour", args)}
        id="endHour"
        placeholder="Hora Fin"
        floatLabelType="Never"
        format="dd/MM/yyyy HH:mm"
      />
    );
  }

  rowSelected() {
    const selectedRecords = this.grid.getSelectedRecords();
    this.setState({ rowSelected: selectedRecords[0] });
  }

  render() {
    return (
      <Fragment>
        <div className="animated fadeIn">
          <div className="card" style={{ marginRight: "60px" }}>
            <div className="card-header">
              <i className="icon-layers"></i> Fichajes
            </div>
            <div className="card-body"></div>
            <Row>
              <GridComponent
                dataSource={this.dailySignings}
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
                actionBegin={this.actionBegin}
                allowGrouping={false}
                rowSelected={this.rowSelected}
                ref={g => (this.grid = g)}
                query={this.queryDailySignings}
              >
                <ColumnsDirective>
                  <ColumnDirective
                    field="userHiringId"
                    headerText="Obra"
                    width="100"
                    editType="dropdownedit"
                    validationRules={this.userHiringIdRules}
                    textAlign="Center"
                    dataSource={this.userHirings}
                    foreignKeyValue="name"
                    foreignKeyField="id"
                  />
                  <ColumnDirective
                    field="id"
                    headerText="Id"
                    width="40"
                    isPrimaryKey={true}
                    isIdentity={true}
                    visible={false}
                  />
                  <ColumnDirective
                    field="startHour"
                    headerText="Hora Inicio"
                    width="100"
                    editType="datetimepickeredit"
                    type="date"
                    format={this.format}
                    textAlign="Center"
                    //editTemplate={this.startHourTemplate}
                  />
                  <ColumnDirective
                    field="endHour"
                    headerText="Hora Fin"
                    width="100"
                    editType="datetimepickeredit"
                    type="date"
                    format={this.format}
                    textAlign="Center"
                    //editTemplate={this.endHourTemplate}
                  />
                  <ColumnDirective
                    field="totalHours"
                    headerText="Horas Totales"
                    width="100"
                    allowEditing={false}
                    textAlign="Right"
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

DailySignings.propTypes = {};

export default DailySignings;
