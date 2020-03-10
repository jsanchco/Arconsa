import React, { Component } from "react";
import PropTypes from "prop-types";
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
import { config, REPORTS } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY } from "../../services";

L10n.load(data);

class GridSelection extends Component {
  grid = null;

  constructor(props) {
    super(props);

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
  }

  componentDidUpdate(prevProps) {
    if (prevProps.settings !== this.props.settings) {
      const { settings } = this.props;
      switch (settings.type) {
        case "workers":
          this.grid.dataSource = new DataManager({
            adaptor: new WebApiAdaptor(),
            url: `${config.URL_API}/${REPORTS}`,
            headers: [
              { Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }
            ]
          });
          this.grid.query = new Query()
            .addParams("workerId", settings.selection)
            .addParams("startDate", settings.start)
            .addParams("endDate", settings.start);

          this.grid.refresh();
          break;

        default:
          break;
      }
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

  render() {
    return (
      <GridComponent
        dataSource={null}
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
        allowGrouping={true}
        rowSelected={this.rowSelected}
        ref={g => (this.grid = g)}
        // query={null}
      >
        <ColumnsDirective>
          <ColumnDirective
            field="clientName"
            headerText="Cliente"
            width="100"
          />
          <ColumnDirective field="workName" headerText="Obra" width="100" />
          <ColumnDirective
            field="userName"
            headerText="Trabajador"
            width="100"
          />
          <ColumnDirective field="dateHour" headerText="Fecha" width="100" />
          <ColumnDirective
            field="priceHour"
            headerText="Precio"
            width="100"
            fotmat="C1"
            textAlign="right"
            editType="numericedit"
          />
          <ColumnDirective
            field="priceHourSale"
            headerText="Precio Venta"
            width="100"
            fotmat="C1"
            textAlign="right"
            editType="numericedit"
          />
        </ColumnsDirective>
        <Inject services={[Page, Toolbar, Edit]} />
      </GridComponent>
    );
  }
}

GridSelection.propTypes = {
  dataSource: PropTypes.object.isRequired,
  showMessage: PropTypes.func.isRequired
};

export default GridSelection;
