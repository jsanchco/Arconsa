import React, { Component, Fragment } from "react";
import { Row } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Edit,
  Inject,
  Toolbar,
  DetailRow,
  Aggregate,
  Resize,
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, EMBARGOS, DETAILSEMBARGO } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import {
  TOKEN_KEY,
  getUser,
  getEmbargo
} from "../../services";

L10n.load(data);

class Embargos extends Component {
  embargos = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${EMBARGOS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  detailsEmbargo = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${DETAILSEMBARGO}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  gridEmbargos = null;
  gridDetailsEmbargo = null;
  wrapSettings = { wrapMode: "Content" };
  numericParams = {
    params: {
      decimals: 2,
      format: "N",
      validateDecimalOnType: true,
      showSpinButton: false
    },
  };

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
    this.actionFailure = this.actionFailure.bind(this);
    this.actionBegin = this.actionBegin.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
    this.gridDetailsEmbargoActionComplete =
      this.gridDetailsEmbargoActionComplete.bind(this);
    this.clickHandlerEmbargos = this.clickHandlerEmbargos.bind(this);
    this.rowSelectedEmbargos = this.rowSelectedEmbargos.bind(this);
    this.beforePrint = this.beforePrint.bind(this);

    this.format = { type: "dateTime", format: "dd/MM/yyyy" };
    this.queryEmbargos = new Query().addParams("userId", props.userId);

    this.gridDetailsEmbargo = {
      columns: [
        {
          field: "id",
          isPrimaryKey: true,
          isIdentity: true,
          visible: false,
        },
        {
          field: "embargoId",
          visible: false,
        },
        {
          field: "datePay",
          headerText: "Fecha Pago",
          width: 70,
          editType: "datepickeredit",
          type: "date",
          format: this.format,
          textAlign: "Center",
        },
        {
          field: "amount",
          headerText: "Cantidad",
          width: 70,
          fotmat: "N2",
          editType: "numericedit",
          edit: this.numericParams,
        },
        {
          field: "observations",
          headerText: "Observaciones",
          width: "200",
        },
      ],
      aggregates: [
        {
          columns: [
            {
              type: "Sum",
              field: "amount",
              format: "N2",
              footerTemplate: this.footerSumAmount,
            },
          ],
        },
      ],
      dataSource: this.detailsEmbargo,
      queryString: "embargoId",
      locale: "es-US",
      toolbar: this.toolbarOptions,
      editSettings: this.editSettings,
      actionFailure: this.actionFailure,
      allowGrouping: false,
      ref: (g) => (this.gridDetailsEmbargo = g),
      allowTextWrap: true,
      textWrapSettings: this.wrapSettings,
      actionComplete: this.gridDetailsEmbargoActionComplete,
      actionBegin: this.gridDetailsEmbargoActionBegin,
      load: this.loadGridDetailsEmbargo
    };

    this.rowSelectedEmbargos = null;
    this.rowSelectedDetailsEmbargo = null;
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

  loadGridDetailsEmbargo() {
    this.query = [];
    this.query = new Query().addParams(
      "embargoId",
      this.parentDetails.parentRowData.id
    );
  }

  rowSelectedEmbargos() {
    const selectedRecords = this.gridEmbargos.getSelectedRecords();
    this.rowSelectedEmbargos = selectedRecords[0];
  }

  rowSelectedDetailsEmbargo() {
    const selectedRecords = this.gridDetailsEmbargo.getSelectedRecords();
    this.rowSelectedDetailsEmbargo = selectedRecords[0];
  }

  clickHandlerEmbargos(args) {
    if (this.rowSelectedEmbargos === null) {
      this.props.showMessage({
        statusText: "Debes seleccionar un embargo",
        responseText: "Debes seleccionar un embargo",
        type: "danger",
      });
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

  actionBegin(args) {
    if (args.requestType === "save") {
      var cols = this.gridEmbargos.columns;
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
    }
    if (args.requestType === "delete") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });
    }
  }

  gridDetailsEmbargoActionComplete(args) {
    if (args.requestType === "save") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });

      getEmbargo(args.data.embargoId).then((result) => {
        this.gridEmbargos.setRowData(args.data.embargoId, result);
      });

      var childGridElements =
        this.gridEmbargos.element.querySelectorAll(".e-detailrow");
      for (var i = 0; i < childGridElements.length; i++) {
        let element = childGridElements[i];
        let childGridObj = element.querySelector(".e-grid").ej2_instances[0];
        if (
          childGridObj.parentDetails.parentRowData.id === args.data.embargoId
        ) {
          childGridObj.refresh();
          break;
        }
      }
    }
    if (args.requestType === "delete") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });

      getEmbargo(args.data[0].embargoId).then((result) => {
        this.gridEmbargos.setRowData(args.data[0].embargoId, result);
      });
    }
  }

  gridDetailsEmbargoActionBegin(args) {
    if (args.requestType === "add") {
      args.data.embargoId = this.parentDetails.parentRowData.id;
    }

    if (args.requestType === "save") {
      var date = args.data.datePay;
      args.data.datePay = new Date(
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

  footerSumAmount(args) {
    return <span>Total: {args.Sum}€</span>;
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

  render() {
    return (
      <Fragment>
        <div className="animated fadeIn">
          <div
            className="card"
            style={{ marginRight: "60px", marginTop: "20px" }}
          >
            <div className="card-header">
              <i className="icon-layers"></i> Embargos
            </div>
            <div className="card-body"></div>
            <Row>
              <GridComponent
                dataSource={this.embargos}
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
                allowGrouping={false}
                ref={(g) => (this.gridEmbargos = g)}
                query={this.queryEmbargos}
                allowTextWrap={true}
                textWrapSettings={this.wrapSettings}
                childGrid={this.gridDetailsEmbargo}
                dataBound={this.dataBound}
                // toolbarClick={this.clickHandlerEmbargos}
                rowSelected={this.rowSelectedEmbargos}
                allowResizing={true}
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
                    field="identifier"
                    headerText="Identificador"
                    width="70"
                  />
                  <ColumnDirective
                    field="issuingEntity"
                    headerText="Entidad Emisora"
                    width="70"
                  />
                  <ColumnDirective
                    field="accountNumber"
                    headerText="Nª Cuenta"
                    width="70"
                  />
                  <ColumnDirective
                    field="notificationDate"
                    headerText="Fecha Notificación"
                    width="110"
                    editType="datepickeredit"
                    type="date"
                    format={this.format}
                    textAlign="Center"
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
                  {/* <ColumnDirective
                    field="endDate"
                    headerText="Fecha Fin"
                    width="80"
                    editType="datepickeredit"
                    type="date"
                    format={this.format}
                    textAlign="Center"
                    visible={false}
                  /> */}
                  <ColumnDirective
                    field="observations"
                    headerText="Observaciones"
                    width="100"
                  />
                  <ColumnDirective
                    field="total"
                    headerText="Total"
                    width="70"
                    fotmat="N2"
                    textAlign="left"
                    editType="numericedit"
                    edit={this.numericParams}
                  />
                  <ColumnDirective
                    field="remaining"
                    headerText="Restante"
                    width="70"
                    allowEditing={false}
                  />
                  <ColumnDirective field="paid" visible={false} />
                  <ColumnDirective
                    field="userId"
                    defaultValue={this.props.userId}
                    visible={false}
                  />
                </ColumnsDirective>
                <Inject
                  services={[Toolbar, Edit, DetailRow, Aggregate, Resize]}
                />
              </GridComponent>
            </Row>
          </div>
        </div>
      </Fragment>
    );
  }
}

Embargos.propTypes = {};

export default Embargos;
