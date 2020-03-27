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
import { config, PROFESSIONINCLIENTS, PROFESSIONS } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY } from "../../services";

L10n.load(data);

class ProfessionInClient extends Component {
  professionInClients = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${PROFESSIONINCLIENTS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  professions = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${PROFESSIONS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  grid = null;

  constructor(props) {
    super(props);

    this.state = {
      roles: null
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

    this.query = new Query().addParams("clientId", props.client.id);
    this.professionIdRules = { required: true };
    this.numericParams = {
      params: {
        decimals: 1,
        format: "N",
        validateDecimalOnType: true
      }
    };
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

  render() {
    let title = "";
    if (this.props.client !== null && this.props.client !== undefined) {
      title = ` Precios por Profesión [${this.props.client.name}]`;
    }

    return (
      <Fragment>
        <div className="animated fadeIn">
          <div className="card" style={{ marginRight: "60px", marginTop: "20px" }}>
            <div className="card-header">
              <i className="icon-people"></i> {title}
            </div>
            <div className="card-body"></div>
            <Row>
              <GridComponent
                dataSource={this.professionInClients}
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
                    field="professionId"
                    headerText="Profesión"
                    width="100"
                    editType="dropdownedit"
                    foreignKeyValue="name"
                    foreignKeyField="id"
                    validationRules={this.professionIdRules}
                    dataSource={this.professions}
                  />
                  <ColumnDirective
                    field="priceHourSale"
                    headerText="Precio Venta"
                    width="100"
                    fotmat="N1"
                    textAlign="left"
                    editType="numericedit"
                    edit={this.numericParams}
                  />
                  <ColumnDirective
                    field="clientId"
                    defaultValue={this.props.client.id}
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

ProfessionInClient.propTypes = {};

export default ProfessionInClient;
