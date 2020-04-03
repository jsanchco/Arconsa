import React, { Component } from "react";
import PropTypes from "prop-types";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Edit,
  Inject,
  Toolbar,
  Page,
  Aggregate,
  AggregateColumnsDirective,
  AggregateColumnDirective,
  AggregateDirective,
  AggregatesDirective
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, INVOICES } from "../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../locales/locale.json";
import { TOKEN_KEY } from "../services";

L10n.load(data);

class GridInvoice extends Component {
  invoices = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${INVOICES}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  wrapSettings = { wrapMode: "Content" };

  grid = null;

  constructor(props) {
    super(props);

    this.toolbarOptions = ["Edit", "Delete", "Update", "Cancel"];
    this.editSettings = {
      showDeleteConfirmDialog: true,
      allowEditing: true,
      allowAdding: false,
      allowDeleting: true
    };
    this.pageSettings = { pageCount: 10, pageSize: 10 };
    this.actionFailure = this.actionFailure.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
    this.clickHandler = this.clickHandler.bind(this);
  }

  componentDidUpdate(prevProps) {
    if (prevProps.update !== this.props.update) {
      this.grid.refresh();
    }
  }

  actionFailure(args) {
    let error = Array.isArray(args) ? args[0].error : args.error.error;
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

  clickHandler(args) {
    if (args.item.id === "Details") {
      const { rowSelected } = this.state;
      if (rowSelected !== null) {
        this.props.history.push({
          pathname: "/employees/detailemployee",
          state: {
            user: rowSelected
          }
        });
      } else {
        this.props.showMessage({
          statusText: "Debes seleccionar un usuario",
          responseText: "Debes seleccionar un usuario",
          type: "danger"
        });
      }
    }
  }

  footerSumEuros(args) {
    let title = args.Sum;
    title = title.replace(",", ".");
    const index = title.lastIndexOf(".");
    if (index >= 0) {
      title = `${title.substring(0, index)},${title.substring(
        index + 1,
        title.length
      )}`;
    }

    return <span>Total: {title}€</span>;
  }

  render() {
    return (
      <GridComponent
        dataSource={this.invoices}
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
          <ColumnDirective field="invoiceNumber" width="100" visible={false} />
          <ColumnDirective
            field="name"
            headerText="Nº Factura"
            width="100"
            allowEditing={false}
          />
          <ColumnDirective
            field="startDate"
            headerText="F. Inicio"
            width="100"
            allowEditing={false}
          />
          <ColumnDirective
            field="endDate"
            headerText="F. Fin"
            width="100"
            allowEditing={false}
          />
          <ColumnDirective
            field="clientName"
            headerText="Cliente"
            width="100"
            allowEditing={false}
          />
          <ColumnDirective
            field="workName"
            headerText="Obra"
            width="100"
            allowEditing={false}
          />
          <ColumnDirective
            field="taxBase"
            headerText="B. Imponible"
            width="100"
          />
          <ColumnDirective field="iva" headerText="IVA" width="100" />
          <ColumnDirective field="total" headerText="Total" width="100" />
          <ColumnDirective
            field="retentions"
            headerText="Retenciones"
            width="100"
          />
        </ColumnsDirective>

        <AggregatesDirective>
          <AggregateDirective>
            <AggregateColumnsDirective>
              <AggregateColumnDirective
                field="taxBase"
                type="Sum"
                format="N2"
                footerTemplate={this.footerSumEuros}
              >
                {" "}
              </AggregateColumnDirective>

              <AggregateColumnDirective
                field="iva"
                type="Sum"
                format="N2"
                footerTemplate={this.footerSumEuros}
              >
                {" "}
              </AggregateColumnDirective>

              <AggregateColumnDirective
                field="total"
                type="Sum"
                format="N2"
                footerTemplate={this.footerSumEuros}
              >
                {" "}
              </AggregateColumnDirective>
            </AggregateColumnsDirective>
          </AggregateDirective>
        </AggregatesDirective>

        <Inject services={[Page, Toolbar, Edit, Aggregate]} />
      </GridComponent>
    );
  }
}

GridInvoice.propTypes = {
  workId: PropTypes.number,
  clientId: PropTypes.number,
  update: PropTypes.bool,
  showMessage: PropTypes.func.isRequired
};

export default GridInvoice;
