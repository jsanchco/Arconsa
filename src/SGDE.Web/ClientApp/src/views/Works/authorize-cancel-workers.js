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
import { DatePickerComponent } from "@syncfusion/ej2-react-calendars";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { getValue } from "@syncfusion/ej2-base";
import { config, USERSHIRING } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY } from "../../services";

L10n.load(data);

class AuthorizeCancelWorkers extends Component {
  usersHiring = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${USERSHIRING}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  grid = null;

  numericParams = {
    params: {
      decimals: 1,
      format: "N",
      validateDecimalOnType: true
    }
  };

  constructor(props) {
    super(props);

    this.state = {
      usersHiring: null
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

    this.template = this.gridTemplate;

    this.query = new Query().addParams("workId", props.work.id);
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

  startDateTemplate(args) {
    return (
      <DatePickerComponent
        value={getValue("startDate", args)}
        id="startDateDP"
        placeholder="Fecha Inicio"
        floatLabelType="Never"
        format="dd/MM/yyyy"
      />
    );
  }

  endDateTemplate(args) {
    return (
      <DatePickerComponent
        value={getValue("endDate", args)}
        id="endDateDP"
        placeholder="Fecha Fin"
        floatLabelType="Never"
        format="dd/MM/yyyy"
      />
    );
  }

  render() {
    let title = "";
    if (
      this.props.work !== null &&
      this.props.work !== undefined
    ) {
      title = ` Contratos [${this.props.work.name}]`;
    }

    return (
      <Fragment>
        <div className="animated fadeIn">
          <div className="card" style={{ marginRight: "60px" }}>
            <div className="card-header">
              <i className="icon-layers"></i>{title}
            </div>
            <div className="card-body"></div>
            <Row>
              <GridComponent
                dataSource={this.usersHiring}
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
                    field="userName"
                    headerText="Trabajador"
                    width="100"
                  />
                  <ColumnDirective
                    field="startDate"
                    headerText="Fecha Inicio"
                    width="100"
                    type="date"
                    format="dd/MM/yyyy"
                    editTemplate={this.startDateTemplate}
                  />
                  <ColumnDirective
                    field="endDate"
                    headerText="Fecha Fin"
                    width="100"
                    type="date"
                    format="dd/MM/yyyy"
                    editTemplate={this.endDateTemplate}
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

AuthorizeCancelWorkers.propTypes = {};

export default AuthorizeCancelWorkers;
