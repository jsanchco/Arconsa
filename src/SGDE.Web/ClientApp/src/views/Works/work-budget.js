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
  Aggregate,
  AggregateColumnsDirective,
  AggregateColumnDirective,
  AggregateDirective,
  AggregatesDirective,
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, WORKBUDGETS } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import {
  TOKEN_KEY,
  updateDocumentInWorkBudget,
  base64ToArrayBuffer,
  saveByteArray,
} from "../../services";
import ModalSelectWorkBudget from "../Modals/modal-select-work-budget";

L10n.load(data);

class WorkBudgets extends Component {
  workbudgets = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${WORKBUDGETS}`,
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

  typeRules = { required: true };
  editType = {
    params: {
      popupWidth: "auto",
      sortOrder: "None",
    },
  };

  constructor(props) {
    super(props);

    this.state = {
      modal: false,
      rowSelected: null,
    };

    this.toolbarOptions = [
      "Add",
      // "Edit",
      "Delete",
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
        tooltipText: "Descargar Archivo",
        prefixIcon: "e-custom-icons e-file-download",
        id: "DownloadFile",
      },
      "Print",
    ];
    this.editSettings = {
      showDeleteConfirmDialog: true,
      // allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      newRowPosition: "Top",
    };
    this.pageSettings = { pageCount: 10, pageSize: 10 };
    this.actionFailure = this.actionFailure.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
    this.actionBegin = this.actionBegin.bind(this);
    this.beforePrint = this.beforePrint.bind(this);
    this.footerSumEuros = this.footerSumEuros.bind(this);
    this.templateFile = this.templateFile.bind(this);
    this.templateHasFile = this.templateHasFile.bind(this);
    this.customAggregateTotalContract =
      this.customAggregateTotalContract.bind(this);
    this.clickHandler = this.clickHandler.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
    this.updateDocument = this.updateDocument.bind(this);
    this.downloadDocuments = this.downloadDocuments.bind(this);

    this.query = new Query().addParams("workId", props.workId);

    this.format = { type: "dateTime", format: "dd/MM/yyyy" };

    this.typeBudgets = [
      { id: "Version X" },
      { id: "Definitivo" },
      { id: "Complementario X" },
    ];
  }

  actionFailure(args) {
    let error = Array.isArray(args) ? args[0].error : args.error;
    let message = null;
    if (Array.isArray(error)) {
      error = error[0].error;
      message = JSON.parse(error.responseText);
    }
    if (
      message !== null &&
      message.Message !== null &&
      message.Message !== ""
    ) {
      this.props.showMessage({
        statusText: error.statusText,
        responseText: message.Message,
        type: "danger",
      });
    } else {
      this.props.showMessage({
        statusText: error.statusText,
        responseText: error.responseText,
        type: "danger",
      });
    }
  }

  actionBegin(args) {
    if (args.requestType === "add") {
      if (this.grid.getCurrentViewRecords().length !== 0) {
        args.data.reference = this.grid.getCurrentViewRecords()[0].reference;
      }
    }

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
      if (this.columns[i].field === "hasFile") {
        this.columns[i].visible = true;
      }
    }
  }

  customAggregateTotalContract(args) {
    let values = null;
    if (args.result != null) {
      values = args.result.filter(
        (item) => item.type === "Definitivo" || item.type === "Complementario X"
      );
    } else if (Array.isArray(args)) {
      values = args.filter(
        (item) => item.type === "Definitivo" || item.type === "Complementario X"
      );
    }

    if (values == null || values.length === 0) {
      return 0;
    }

    var sum = values
      .map((item) => item.totalContract)
      .reduce((prev, next) => prev + next);

    sum = Math.round((sum + Number.EPSILON) * 100) / 100;

    return sum;
  }

  footerSumEuros(args) {
    var title = args.Custom;
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

    return <span>Total (D + CX): {title}€</span>;
  }

  templateFile(args) {
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

  templateHasFile(args) {
    if (args.hasFile) {
      return <div>Si</div>;
    } else {
      return <div>No</div>;
    }
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
  }

  toggleModal() {
    this.setState({
      modal: !this.state.modal,
    });
  }

  updateDocument(args) {
    const documentSelected = this.state.rowSelected;
    let remove = args.fileUrl.indexOf("base64,") + 7;

    documentSelected.file = args.fileUrl.substring(remove);
    documentSelected.fileName = args.name;
    documentSelected.typeFile = args.file.type;

    updateDocumentInWorkBudget(documentSelected).then(() => {
      this.grid.setRowData(this.state.rowSelected.id, documentSelected);
    });
  }

  downloadDocuments() {
    const selectedRecords = this.grid.getSelectedRecords();
    let error = null;

    if (Array.isArray(selectedRecords) && selectedRecords.length > 0) {
      selectedRecords.forEach((document) => {
        if (document.file !== null && document.file !== undefined) {
          const fileArr = base64ToArrayBuffer(document.file);
          saveByteArray(document.name, fileArr, document.typeFile);
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

  render() {
    return (
      <Fragment>
        <ModalSelectWorkBudget
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
              <i className="icon-layers"></i> Presupuestos
            </div>
            <div className="card-body"></div>
            <Row>
              <GridComponent
                dataSource={this.workbudgets}
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
                ref={(g) => (this.grid = g)}
                query={this.query}
                beforePrint={this.beforePrint}
                printComplete={this.printComplete}
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
                    field="type"
                    headerText="Tipo"
                    width="70"
                    editType="dropdownedit"
                    foreignKeyValue="id"
                    foreignKeyField="id"
                    validationRules={this.typeRules}
                    dataSource={new DataManager(this.typeBudgets)}
                    edit={this.editType}
                  />
                  <ColumnDirective
                    field="date"
                    headerText="Fecha"
                    width="100"
                    type="date"
                    format={this.format}
                    editType="datepickeredit"
                  />
                  <ColumnDirective
                    field="reference"
                    headerText="Referencia"
                    width="100"
                  />
                  <ColumnDirective
                    field="name"
                    headerText="Nombre"
                    allowEditing={false}
                    width="100"
                  />
                  <ColumnDirective
                    field="totalContract"
                    headerText="Total Contrato"
                    width="100"
                    editType="numericedit"
                    textAlign="right"
                    edit={this.numericParams}
                    validationRules={this.typeRules}
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
                    field="hasFile"
                    headerText="Presupuesto Adjunto"
                    width="100"
                    visible={false}
                    template={this.templateHasFile}
                  />
                  <ColumnDirective
                    field="workId"
                    defaultValue={this.props.workId}
                    visible={false}
                  />
                </ColumnsDirective>

                <AggregatesDirective>
                  <AggregateDirective>
                    <AggregateColumnsDirective>
                      <AggregateColumnDirective
                        field="totalContract"
                        type="Custom"
                        customAggregate={this.customAggregateTotalContract}
                        footerTemplate={this.footerSumEuros}
                      >
                        {" "}
                      </AggregateColumnDirective>
                    </AggregateColumnsDirective>
                  </AggregateDirective>
                </AggregatesDirective>

                <Inject services={[Page, Toolbar, Edit, Aggregate]} />
              </GridComponent>
            </Row>
          </div>
        </div>
      </Fragment>
    );
  }
}

WorkBudgets.propTypes = {};

export default WorkBudgets;
