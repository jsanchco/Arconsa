import React, { Component, Fragment } from "react";
import { Row } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Inject,
  DetailRow,
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, EMBARGOS, DETAILSEMBARGO } from "../../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../../locales/locale.json";
import {
  TOKEN_KEY,
} from "../../../services";

L10n.load(data);

class TestBlank extends Component {
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
    // this.actionFailure = this.actionFailure.bind(this);
    // this.actionBegin = this.actionBegin.bind(this);
    // this.actionComplete = this.actionComplete.bind(this);
    // this.gridDetailsEmbargoActionComplete =
    //   this.gridDetailsEmbargoActionComplete.bind(this);
    // this.dataBound = this.dataBound.bind(this);
    // this.clickHandlerEmbargos = this.clickHandlerEmbargos.bind(this);
    // this.rowSelectedEmbargos = this.rowSelectedEmbargos.bind(this);
    // this.beforePrint = this.beforePrint.bind(this);
    // this.dataBoundGridDetailsEmbargo = this.dataBoundGridDetailsEmbargo.bind(this);
    // this.loadGridDetailsEmbargo = this.loadGridDetailsEmbargo.bind(this);

    this.format = { type: "dateTime", format: "dd/MM/yyyy" };
    this.queryEmbargos = new Query().addParams("userId", props.userId);

    //this.onLoad = this.onLoad.bind(this);
    this.gridDetailsEmbargo = {
      //   columns: [
      //     {
      //       field: "OrderID",
      //       headerText: "Order ID",
      //       textAlign: "Right",
      //       width: 120,
      //     },
      //     { field: "CustomerID", headerText: "Customer ID", width: 150 },
      //     { field: "ShipCity", headerText: "Ship City", width: 150 },
      //     { field: "ShipName", headerText: "Ship Name", width: 150 },
      //   ],
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
    //   dataSource: detailsEmbargo,
      dataSource: new DataManager({
        adaptor: new WebApiAdaptor(),
        url: `${config.URL_API}/${DETAILSEMBARGO}`,
        headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
      }),
      load() {
        const str = "id";
        this.parentDetails.parentKeyFieldValue =
          this.parentDetails.parentRowData[str];
        // this.dataSource = detailsEmbargo;
      },
      //load: this.onLoad,
      queryString: "embargoId",
    };

    this.rowSelectedEmbargos = null;
    this.rowSelectedDetailsEmbargo = null;
  }

  onLoad() {
    // const str = "id";
    // this.parentDetails.parentKeyFieldValue =
    //   this.parentDetails.parentRowData[str];
    // this.gridDetailsEmbargo.dataSource = detailsEmbargo;
  }

  render() {
    return (
      <Fragment>
        <div className="animated fadeIn">
          <div
            className="card"
            style={{
              marginRight: "60px",
              marginTop: "20px",
              marginLeft: "60px",
            }}
          >
            <div className="card-header">
              <i className="icon-layers"></i> Test Hierachical
            </div>
            <div className="card-body"></div>
            <Row>
              <GridComponent
                dataSource={this.embargos}
                childGrid={this.gridDetailsEmbargo}
                style={{
                  marginLeft: 30,
                  marginRight: 30,
                  marginTop: -20,
                  marginBottom: 20,
                }}
                ref={(g) => (this.gridEmbargos = g)}
                load={this.onLoad}
              >
                <ColumnsDirective>
                  {/* <ColumnDirective
                    field="EmployeeID"
                    headerText="Employee ID"
                    width="120"
                    textAlign="Right"
                  />
                  <ColumnDirective
                    field="FirstName"
                    headerText="First Name"
                    width="150"
                  />
                  <ColumnDirective field="City" headerText="City" width="150" />
                  <ColumnDirective
                    field="Country"
                    headerText="Country"
                    width="150"
                  /> */}

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
                    width="80"
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
                <Inject services={[DetailRow]} />
              </GridComponent>
            </Row>
          </div>
        </div>
      </Fragment>
    );
  }
}

TestBlank.propTypes = {};

export default TestBlank;
