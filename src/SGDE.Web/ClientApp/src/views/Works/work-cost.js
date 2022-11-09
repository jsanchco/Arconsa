import React, { Component, Fragment } from "react";
import { Row } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Edit,
  Inject,
  Toolbar,
  Group,
  Aggregate,
  AggregateColumnsDirective,
  AggregateColumnDirective,
  AggregateDirective,
  AggregatesDirective,
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, WORKCOSTS } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import {
  TOKEN_KEY,
  updateDocumentInWorkCost,
  base64ToArrayBuffer,
  saveByteArray,
} from "../../services";
import ModalSelectWorkCost from "../Modals/modal-select-work-cost";
import { DialogComponent } from "@syncfusion/ej2-react-popups";
import { removeAllWorkCosts, getWorkCost } from "../../services";
import "../Modals/modal-worker.css";

L10n.load(data);

class WorkCosts extends Component {
  workcosts = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${WORKCOSTS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  numericParams = {
    params: {
      decimals: 2,
      format: "N",
      validateDecimalOnType: true,
      showSpinButton: false
    },
  };

  grid = null;

  constructor(props) {
    super(props);

    this.state = {
      modal: false,
      rowSelected: null,
    };

    this.toolbarOptions = [
      "Add",
      "Edit",
      //   "Delete",
      {
        text: "Borrar Seleccionados",
        tooltipText: "Borrar registros seleccionados",
        prefixIcon: "e-custom-icons e-remove",
        id: "RemoveAll",
      },
      "Update",
      "Cancel",
      {
        text: "Subir Archivo",
        tooltipText: "Subir Archivo",
        prefixIcon: "e-custom-icons e-file-upload",
        id: "UploadFile",
      },
      {
        text: "Descargar Archivo(s)",
        tooltipText: "Descargar Archivo(s)",
        prefixIcon: "e-custom-icons e-file-download",
        id: "DownloadFile",
      },
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
    this.actionComplete = this.actionComplete.bind(this);
    this.actionBegin = this.actionBegin.bind(this);
    this.clickHandler = this.clickHandler.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
    this.updateDocument = this.updateDocument.bind(this);
    this.downloadDocuments = this.downloadDocuments.bind(this);
    this.footerSumEuros = this.footerSumEuros.bind(this);
    this.dataBound = this.dataBound.bind(this);
    this.beforePrint = this.beforePrint.bind(this);
    this.templateFile = this.templateFile.bind(this);
    this.templateHasFile = this.templateHasFile.bind(this);
    this.updateWorkCosts = this.updateWorkCosts.bind(this);
    this.dialogClose = this.dialogClose.bind(this);

    this.animationSettings = { effect: "None" };
    this.confirmButton = [
      {
        click: () => {
          const selectedRecords = this.grid.getSelectedRecords();
          if (Array.isArray(selectedRecords) && selectedRecords.length > 0) {
            let result = selectedRecords.map((a) => a.id);
            removeAllWorkCosts(result)
              .then(() => {
                this.setState({ hideConfirmDialog: false });
                this.updateWorkCosts();
              })
              .catch(() => {
                this.setState({ hideConfirmDialog: false });
                this.updateWorkCosts();
              });
          }
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

    this.query = new Query().addParams("workId", props.workId);

    this.format = { type: "dateTime", format: "dd/MM/yyyy" };

    this.selectionSettings = {
      checkboxMode: "ResetOnRowClick",
    };

    this.state = {
      hideConfirmDialog: false,
    };
  }

  templateFile(args) {
    if (args.hasFile) {
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

  templateHasFile(args) {
    if (args.hasFile) {
      return <div>Si</div>;
    } else {
      return <div>No</div>;
    }
  }

  updateWorkCosts() {
    this.grid.refresh();
  }

  dialogClose() {
    this.setState({
      hideConfirmDialog: false,
    });
  }

  clickHandler(args) {
    if (args.item.id === "UploadFile") {
      const selectedRecords = this.grid.getSelectedRecords();
      if (Array.isArray(selectedRecords) && selectedRecords.length === 1) {
        this.setState({ rowSelected: selectedRecords[0] });
        this.toggleModal();
      } else {
        this.setState({ rowSelected: null });
        this.props.showMessage({
          statusText: "Debes seleccionar un solo registro",
          responseText: "Debes seleccionar un solo registro",
          type: "danger",
        });
      }
    }

    if (args.item.id === "DownloadFile") {
      const selectedRecords = this.grid.getSelectedRecords();
      if (Array.isArray(selectedRecords) && selectedRecords.length > 0) {
        this.downloadDocuments();
      } else {
        this.props.showMessage({
          statusText: "Debes seleccionar uno o más de un registro",
          responseText: "Debes seleccionar uno o más de un registro",
          type: "danger",
        });
      }
    }

    if (args.item.id === "RemoveAll") {
      this.setState({ hideConfirmDialog: true });
    }
  }

  toggleModal() {
    this.setState({
      modal: !this.state.modal,
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

  actionBegin(args) {
    if (args.requestType === "save") {
      var cols = this.grid.columns;
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
      this.setState({ rowSelected: null });
      this.grid.clearSelection();
      this.grid.refresh();
    }
    if (args.requestType === "delete") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });
      this.setState({ rowSelected: null });
    }
  }

  updateDocument(args) {
    const documentSelected = this.state.rowSelected;
    let remove = args.fileUrl.indexOf("base64,") + 7;

    documentSelected.file = args.fileUrl.substring(remove);
    documentSelected.fileName = args.fileName;
    documentSelected.typeFile = args.file.type;

    updateDocumentInWorkCost(documentSelected).then(() => {
      this.grid.setRowData(this.state.rowSelected.id, documentSelected);
    });
  }

  downloadDocuments() {
    const selectedRecords = this.grid.getSelectedRecords();
    let error = null;

    if (Array.isArray(selectedRecords) && selectedRecords.length > 0) {
      selectedRecords.forEach((document) => {
        if (document.hasFile) {
          getWorkCost(document.id)
          .then((workCost) => {
            const fileArr = base64ToArrayBuffer(workCost.file);
            saveByteArray(workCost.fileName, fileArr, workCost.typeFile);
          })
          .catch(() => {
            this.setState({ hideConfirmDialog: false });
            this.updateWorkCosts();
          });
        } else {
          error =
            "Algunos de los registros seleccionados no tienen el archivo subido";
        }
      });
    }

    if (error !== null) {
      this.props.showMessage({
        statusText: error,
        responseText: error,
        type: "warning",
      });
    }
  }

  dataBound(args) {}

  beforePrint(args) {
    var div = document.createElement("Div");
    div.innerHTML = this.props.workName;
    div.style.textAlign = "center";
    div.style.color = "red";
    div.style.padding = "10px 0";
    div.style.fontSize = "25px";
    args.element.insertBefore(div, args.element.childNodes[0]);
  }

  printComplete(args) {
    for (var i = 0; i < this.columns.length; i++) {
      if (this.columns[i].field === "fileName") {
        this.columns[i].visible = false;
      }
      if (this.columns[i].id === "selection") {
        this.columns[i].visible = false;
      }
      if (this.columns[i].field === "hasFile") {
        this.columns[i].visible = true;
      }
    }
  }

  footerSumEuros(args) {
    var title = args.Sum;
    if (typeof title !== "string") {
      title = title.toString();
    }

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
      <Fragment>
        <DialogComponent
          id="confirmDialogRemoveAll"
          header="Eliminar Todos"
          visible={this.state.hideConfirmDialog}
          showCloseIcon={true}
          animationSettings={this.animationSettings}
          width="500px"
          content="¿Estás seguro de eliminar estos gastos?"
          ref={(dialog) => (this.confirmDialogInstance = dialog)}
          target="#target-work-costs"
          buttons={this.confirmButton}
          close={this.dialogClose.bind(this)}
        ></DialogComponent>

        <ModalSelectWorkCost
          isOpen={this.state.modal}
          toggle={this.toggleModal}
          updateDocument={this.updateDocument}
          workId={this.props.workId}
          rowSelected={this.state.rowSelected}
          type="document"
        />
        <div className="animated fadeIn" id="target-work-costs">
          <div
            className="card"
            style={{ marginRight: "60px", marginTop: "20px" }}
          >
            <div className="card-header">
              <i className="icon-layers"></i> Gastos
            </div>
            <div className="card-body"></div>
            <Row>
              <GridComponent
                dataSource={this.workcosts}
                locale="es-US"
                toolbar={this.toolbarOptions}
                toolbarClick={this.clickHandler}
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
                ref={(g) => (this.grid = g)}
                query={this.query}
                selectionSettings={this.selectionSettings}
                allowGrouping={true}
                dataBound={this.dataBound}
                beforePrint={this.beforePrint}
                printComplete={this.printComplete}
              >
                <ColumnsDirective>
                  <ColumnDirective type="checkbox" width="50" id="selection" />
                  <ColumnDirective
                    field="id"
                    headerText="Id"
                    width="40"
                    isPrimaryKey={true}
                    isIdentity={true}
                    visible={false}
                  />
                  <ColumnDirective
                    field="typeWorkCost"
                    headerText="Tipo"
                    width="70"
                  />
                  <ColumnDirective
                    field="date"
                    headerText="Fecha"
                    width="100"
                    type="date"
                    format={this.format}
                    editType="datepickeredit"
                    validationRules={{ required: true }}
                  />
                  <ColumnDirective
                    field="description"
                    headerText="Descripción"
                    width="100"
                  />
                  <ColumnDirective
                    field="provider"
                    headerText="Proveedor"
                    width="100"
                  />
                  <ColumnDirective
                    field="numberInvoice"
                    headerText="Nº Factura"
                    width="100"
                    allowGrouping={false}
                  />
                  <ColumnDirective
                    field="taxBase"
                    headerText="Base"
                    width="100"
                    textAlign="right"
                    editType="numericedit"
                    allowGrouping={false}
                    edit={this.numericParams}
                    validationRules={{ required: true }}
                  />
                  <ColumnDirective
                    field="fileName"
                    headerText="Nombre del Documento"
                    visible={false}
                  />
                  <ColumnDirective
                    field="fileName"
                    headerText="Archivo"
                    width="100"
                    template={this.templateFile}
                    textAlign="Center"
                    allowEditing={false}
                  />
                  <ColumnDirective
                    field="workId"
                    defaultValue={this.props.workId}
                    visible={false}
                  />
                  <ColumnDirective field="typeFile" visible={false} />
                  <ColumnDirective
                    field="hasFile"
                    headerText="Factura Adjunta"
                    width="100"
                    visible={false}
                    template={this.templateHasFile}
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
                    </AggregateColumnsDirective>
                  </AggregateDirective>

                  <AggregateDirective>
                    <AggregateColumnsDirective>
                      <AggregateColumnDirective
                        field="taxBase"
                        type="Sum"
                        format="N2"
                        groupCaptionTemplate={this.footerSumEuros}
                      >
                        {" "}
                      </AggregateColumnDirective>
                    </AggregateColumnsDirective>
                  </AggregateDirective>
                </AggregatesDirective>

                <Inject services={[Toolbar, Edit, Group, Aggregate]} />
              </GridComponent>
            </Row>
          </div>
        </div>
      </Fragment>
    );
  }
}

WorkCosts.propTypes = {};

export default WorkCosts;
