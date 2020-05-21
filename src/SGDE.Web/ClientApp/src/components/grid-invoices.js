import React, { Component, Fragment } from "react";
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
  AggregatesDirective,
} from "@syncfusion/ej2-react-grids";
import {
  createSpinner,
  showSpinner,
  hideSpinner,
} from "@syncfusion/ej2-popups";
import { setValue } from "@syncfusion/ej2-base";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { DialogComponent } from "@syncfusion/ej2-react-popups";
import { config, INVOICES } from "../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../locales/locale.json";
import { TOKEN_KEY } from "../services";
import {
  base64ToArrayBuffer,
  saveByteArray,
  printInvoice,
  billPayment,
} from "../services";

L10n.load(data);

class GridInvoice extends Component {
  invoices = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${INVOICES}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  wrapSettings = { wrapMode: "Content" };

  grid = null;

  constructor(props) {
    super(props);

    this.state = {
      hideConfirmDialog: false,
    };

    this.selectedRow = null;
    this.toolbarOptions = [
      "Delete",
      // "Update",
      // "Cancel",
      {
        text: "Imprimir Factura",
        tooltipText: "Imprimir Factura",
        prefixIcon: "e-custom-icons e-print",
        id: "PrintInvoice",
      },
      {
        text: "Anular Factura",
        tooltipText: "Anular Factura",
        prefixIcon: "e-custom-icons e-empty",
        id: "CancelInvoice",
      }
    ];
    if (props.showViewInvoice === true) {
      this.toolbarOptions.push({
        text: "Ver Factura",
        tooltipText: "Ver Factura",
        prefixIcon: "e-custom-icons e-file-upload",
        id: "ViewInvoice",
      });
    }

    this.editSettings = {
      showDeleteConfirmDialog: true,
      allowAdding: false,
      allowDeleting: true,
    };
    this.numericParams = {
      params: {
        decimals: 2,
        format: "N",
        validateDecimalOnType: true,
      },
    };
    this.pageSettings = { pageCount: 10, pageSize: 10 };
    this.actionFailure = this.actionFailure.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
    this.clickHandler = this.clickHandler.bind(this);
    this.billPayment = this.billPayment.bind(this);
    this.rowSelected = this.rowSelected.bind(this);

    this.confirmButton = [
      {
        click: () => {
          this.setState({ hideConfirmDialog: false });
          this.billPayment();
        },
        buttonModel: { content: "Si", isPrimary: true },
      },
      {
        click: () => {
          this.setState({ hideConfirmDialog: false });
        },
        buttonModel: { content: "No" },
      },
    ];
    this.animationSettings = { effect: "None" };
    if (props.workId) {
      this.query = new Query().addParams("workId", props.workId);
    }
    if (props.clientId) {
      this.query = new Query().addParams("clientId", props.clientId);
    }
  }

  componentDidUpdate(prevProps) {
    if (prevProps.update !== this.props.update) {
      this.grid.refresh();
    }
  }

  dialogClose() {
    this.setState({
      hideConfirmDialog: false,
    });
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
      setValue("retentions", args.data.retentions, this.selectedRow);
      this.grid.aggregateModule.refresh(this.selectedRow);

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

  clickHandler(args) {
    const selectedRecords = this.grid.getSelectedRecords();
    if (args.item.id === "PrintInvoice") {
      if (selectedRecords.length === 0) {
        this.props.showMessage({
          statusText: "Debes seleccionar una factura",
          responseText: "Debes seleccionar una factura",
          type: "danger",
        });
      } else {
        const element = document.getElementById("gridInvoices");

        createSpinner({
          target: element,
        });
        showSpinner(element);

        printInvoice(selectedRecords[0].id)
          .then((result) => {
            const fileArr = base64ToArrayBuffer(result.file);
            saveByteArray(result.fileName, fileArr, result.typeFile);
            hideSpinner(element);
          })
          .catch((error) => {
            hideSpinner(element);
          });
      }
    }

    if (args.item.id === "CancelInvoice") {
      if (selectedRecords.length === 0) {
        this.props.showMessage({
          statusText: "Debes seleccionar una factura",
          responseText: "Debes seleccionar una factura",
          type: "danger",
        });
      } else {
        this.setState({ hideConfirmDialog: true });
      }
    }

    if (args.item.id === "ViewInvoice") {
      if (selectedRecords.length === 0) {
        this.props.showMessage({
          statusText: "Debes seleccionar una factura",
          responseText: "Debes seleccionar una factura",
          type: "danger",
        });
      } else {
        this.props.updateForm(selectedRecords[0].id);
        this.props.toggleForm();
      }
    }
  }

  billPayment() {
    const element = document.getElementById("gridInvoices");

    createSpinner({
      target: element,
    });
    showSpinner(element);

    billPayment(this.grid.getSelectedRecords()[0].id)
      .then(() => {
        this.props.showMessage({
          statusText: "200",
          responseText: "Operación realizada con éxito",
          type: "success",
        });
        this.grid.refresh();
        hideSpinner(element);
      })
      .catch((error) => {
        this.props.showMessage({
          statusText: "Ha ocurrido un error en la operación",
          responseText: "Ha ocurrido un error en la operación",
          type: "danger",
        });
        hideSpinner(element);
      });
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

  changeRetentions(args) {
    setValue("retentions", args.value, this.selectedRow);
    if (this.grid) {
      this.grid.aggregateModule.refresh(this.selectedRow);
    }
  }

  rowSelected() {
    if (this.grid) {
      this.selectedRow = this.grid.getSelectedRecords()[0];
    }
  }

  render() {
    return (
      <Fragment>
        <DialogComponent
          id="confirmDialog"
          header="Abonar Factura"
          visible={this.state.hideConfirmDialog}
          showCloseIcon={true}
          animationSettings={this.animationSettings}
          width="500px"
          content="¿Estás seguro de querer Abonar esta factura?"
          ref={(dialog) => (this.confirmDialogInstance = dialog)}
          target="#gridInvoices"
          buttons={this.confirmButton}
          close={this.dialogClose.bind(this)}
        ></DialogComponent>

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
            marginBottom: 20,
          }}
          actionFailure={this.actionFailure}
          actionComplete={this.actionComplete}
          allowGrouping={false}
          rowSelected={this.rowSelected}
          ref={(g) => (this.grid = g)}
          query={this.query}
          allowTextWrap={true}
          textWrapSettings={this.wrapSettings}
          id="gridInvoices"
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
              field="invoiceNumber"
              width="100"
              visible={false}
            />
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
              field="issueDate"
              headerText="F. Emisión"
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
              allowEditing={false}
            />
            <ColumnDirective
              field="ivaTaxBase"
              headerText="IVA"
              width="100"
              allowEditing={false}
            />
            <ColumnDirective
              field="total"
              headerText="Total"
              width="100"
              allowEditing={false}
            />
            <ColumnDirective
              field="retentions"
              headerText="Retenciones"
              width="100"
              fotmat="N2"
              editType="numericedit"
              edit={this.numericParams}
            />
            <ColumnDirective field="typeInvoice" visible={false} />
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
                  field="ivaTaxBase"
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

                <AggregateColumnDirective
                  field="retentions"
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
      </Fragment>
    );
  }
}

GridInvoice.propTypes = {
  workId: PropTypes.number,
  clientId: PropTypes.number,
  update: PropTypes.number,
  showMessage: PropTypes.func.isRequired,
  updateForm: PropTypes.func,
  showViewInvoice: PropTypes.bool.isRequired,
  toggleForm: PropTypes.func.isRequired
};

export default GridInvoice;
