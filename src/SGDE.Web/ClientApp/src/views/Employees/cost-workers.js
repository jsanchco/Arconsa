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
import { config, COSTWORKERS, PROFESSIONSBYUSER } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY } from "../../services";

L10n.load(data);

class CostWorkers extends Component {
  trainings = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${COSTWORKERS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  professions = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${PROFESSIONSBYUSER}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  numericParams1 = {
    params: {
      // decimals: 5,
      format: "N5",
      //validateDecimalOnType: true,
      showSpinButton: false
    },
  };

  numericParams2 = {
    params: {
      // decimals: 5,
      format: "N5",
      // validateDecimalOnType: true,
      showSpinButton: false
    },
  };

  numericParams3 = {
    params: {
      // decimals: 5,
      format: "N5",
      // validateDecimalOnType: true,
      showSpinButton: false
    },
  };

  numericParams4 = {
    params: {
      // decimals: 5,
      format: "N5",
      // validateDecimalOnType: true,
      showSpinButton: false
    },
  };

  numericParams5 = {
    params: {
      // decimals: 5,
      format: "N5",
      // validateDecimalOnType: true,
      showSpinButton: false
    },
  };

  grid = null;
  // ddlProfessions = null;
  wrapSettings = { wrapMode: "Content" };

  proffesionIdRules = { required: true };

  constructor(props) {
    super(props);

    this.state = {
      trainings: null,
    };

    this.toolbarOptions = ["Add", "Edit", "Delete", "Update", "Cancel"];
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

    this.template = this.gridTemplate;

    this.format = { type: "dateTime", format: "dd/MM/yyyy" };
    this.query = new Query().addParams("userId", props.userId);
    this.queryProfessions = {
      params: {
        query: new Query().addParams("userId", props.userId),
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
    if (day < 10) day = "0" + day;

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
                  marginBottom: 20,
                }}
                actionFailure={this.actionFailure}
                actionComplete={this.actionComplete}
                actionBegin={this.actionBegin}
                allowGrouping={false}
                rowSelected={this.rowSelected}
                ref={(g) => (this.grid = g)}
                query={this.query}
                allowTextWrap={true}
                textWrapSettings={this.wrapSettings}
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
                    width="80"
                    editType="datepickeredit"
                    type="date"
                    format={this.format}
                    textAlign="Center"
                  />
                  <ColumnDirective
                    field="endDate"
                    headerText="Fecha Fin"
                    width="80"
                    editType="datepickeredit"
                    type="date"
                    format={this.format}
                    textAlign="Center"
                  />
                  <ColumnDirective
                    field="professionId"
                    headerText="Puesto"
                    width="100"
                    editType="dropdownedit"
                    foreignKeyValue="name"
                    foreignKeyField="id"
                    dataSource={this.professions}
                    edit={this.queryProfessions}
                    allowFiltering={true}
                    validationRules={this.proffesionIdRules}
                  />
                  <ColumnDirective
                    headerText="Precio"
                    textAlign="Center"
                    columns={[
                      // {
                      //   field: "priceHourOrdinaryS",
                      //   headerText: "Ordinaria",
                      //   width: "70"
                      // },
                      // {
                      //   field: "priceHourExtraS",
                      //   headerText: "Extra",
                      //   width: "70"
                      // },
                      // {
                      //   field: "priceHourFestiveS",
                      //   headerText: "Festivo",
                      //   width: "70",
                      // },
                      // {
                      //   field: "priceHourNocturnalS",
                      //   headerText: "Nocturno",
                      //   width: "70",
                      // },
                      // {
                      //   field: "priceDailyS",
                      //   headerText: "Diario",
                      //   width: "70"
                      // },
                      
                      {
                        field: "priceHourOrdinary",
                        headerText: "Ordinaria",
                        width: "70",
                        textAlign: "left",
                        editType: "numericedit",
                        edit: this.numericParams1,
                      },
                      {
                        field: "priceHourExtra",
                        headerText: "Extra",
                        width: "70",
                        textAlign: "left",
                        editType: "numericedit",
                        edit: this.numericParams2,
                      },
                      {
                        field: "priceHourFestive",
                        headerText: "Festivo",
                        width: "70",
                        textAlign: "left",
                        editType: "numericedit",
                        edit: this.numericParams3,
                      },
                      {
                        field: "priceHourNocturnal",
                        headerText: "Nocturno",
                        width: "70",
                        textAlign: "left",
                        editType: "numericedit",
                        edit: this.numericParams4,
                      },
                      {
                        field: "priceDaily",
                        headerText: "Diario",
                        width: "70",
                        textAlign: "left",
                        editType: "numericedit",
                        edit: this.numericParams5,
                      }    
                    ]}
                  />
                  <ColumnDirective
                    field="observations"
                    headerText="Observaciones"
                    width="100"
                  />
                  <ColumnDirective
                    field="userId"
                    headerText="User"
                    width="100"
                    defaultValue={this.props.userId}
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
