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
import { config, ADVANCES } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY } from "../../services";

L10n.load(data);

class Advances extends Component {
  advances = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${ADVANCES}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  grid = null;

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

    this.query = new Query().addParams("userId", props.userId);

    this.format = { type: "dateTime", format: "dd/MM/yyyy" };
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

  templateIsPaid(args) {
    if (args.paid) {
      return <span>Si</span>;
    } else {
      return <span>No</span>;
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
              <i className="icon-layers"></i> Cursos
            </div>
            <div className="card-body"></div>
            <Row>
              <GridComponent
                dataSource={this.advances}
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
                allowGrouping={false}
                rowSelected={this.rowSelected}
                ref={(g) => (this.grid = g)}
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
                    field="concessionDate"
                    headerText="Fecha Concesión"
                    width="100"
                    editType="datepickeredit"
                    type="date"
                    format={this.format}
                    textAlign="Center"
                  />
                  <ColumnDirective
                    field="amount"
                    headerText="Cantidad"
                    width="100"
                  />
                  <ColumnDirective
                    field="observations"
                    headerText="Observaciones"
                    width="100"
                  />                  
                  <ColumnDirective
                    field="payDate"
                    headerText="Fecha Pago"
                    width="100"
                    editType="datepickeredit"
                    type="date"
                    format={this.format}
                    textAlign="Center"
                  />
                  <ColumnDirective
                    field="paid"
                    headerText="Pagado"
                    width="100"
                    allowEditing={false}
                    template={this.templateIsPaid}
                  />
                  <ColumnDirective
                    field="userId"
                    headerText="User"
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

Advances.propTypes = {};

export default Advances;
