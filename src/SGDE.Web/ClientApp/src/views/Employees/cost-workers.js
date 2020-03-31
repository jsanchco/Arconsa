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
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, COSTWORKERS } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY } from "../../services";

L10n.load(data);

class CostWorkers extends Component {
  trainings = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${COSTWORKERS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  numericParams = {
    params: {
      decimals: 2,
      format: "N",
      validateDecimalOnType: true
    }
  };

  grid = null;

  constructor(props) {
    super(props);

    this.state = {
      trainings: null
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

    this.template = this.gridTemplate;

    this.format = { type: "dateTime", format: "dd/MM/yyyy" };
    this.query = new Query().addParams("userId", props.user.id);
  }

  actionFailure(args) {
    let error = Array.isArray(args) ? args[0].error : args.error;
    if (Array.isArray(error)) {
      error = error[0].error;
    }
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

  actionBegin(args) {
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

  formatDate(args) {
    if (args === null || args === "") {
      return "";
    }

    let day = args.getDate();
    if (day < 10)
      day = "0" + day;
    
    const month = args.getMonth() + 1;
    const year = args.getFullYear();

    if (month < 10) {
      return `${day}/0${month}/${year}`;
    } else {
      return `${day}/${month}/${year}`;
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
              <i className="icon-layers"></i> Precios Coste
            </div>
            <div className="card-body"></div>
            <Row>
              <GridComponent
                dataSource={this.trainings}
                locale="es-US"
                allowPaging={true}
                pageSettings={this.pageSettings}
                toolbar={this.toolbarOptions}
                toolbarClick={this.clickHandler}
                editSettings={this.editSettings}
                style={{
                  marginLeft: 30,
                  marginRight: 30,
                  marginTop: -20,
                  marginBottom: 20
                }}
                actionFailure={this.actionFailure}
                actionComplete={this.actionComplete}
                actionBegin={this.actionBegin}
                allowGrouping={false}
                rowSelected={this.rowSelected}
                ref={g => (this.grid = g)}
                query={this.query}
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
                    headerText="Precio"
                    textAlign="Center"
                    columns={[
                      {
                        field: "priceHourOrdinary",
                        headerText: "Ordinario",
                        width: "100",
                        fotmat: "N2",
                        textAlign: "left",
                        editType: "numericedit",
                        edit: this.numericParams
                      },
                      {
                        field: "priceHourExtra",
                        headerText: "Extra",
                        width: "100",
                        fotmat: "N2",
                        textAlign: "left",
                        editType: "numericedit",
                        edit: this.numericParams
                      },
                      {
                        field: "priceHourFestive",
                        headerText: "Festivo",
                        width: "100",
                        fotmat: "N2",
                        textAlign: "left",
                        editType: "numericedit",
                        edit: this.numericParams
                      }
                    ]}
                  />
                  <ColumnDirective
                    field="userId"
                    headerText="User"
                    width="100"
                    defaultValue={this.props.user.id}
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

CostWorkers.propTypes = {};

export default CostWorkers;
