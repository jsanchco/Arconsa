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
  DetailRow
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, WORKBUDGETDATAS, WORKBUDGETS } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import {
  TOKEN_KEY,
  updateDocumentInWorkBudget,
  base64ToArrayBuffer,
  saveByteArray,
  getWorkBudgetData
} from "../../services";
import ModalSelectWorkBudget from "../Modals/modal-select-work-budget";

L10n.load(data);

class WorkBudgets1 extends Component {
  workBudgetDatas = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${WORKBUDGETDATAS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  workBudgets = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${WORKBUDGETS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  numericParams = {
    params: {
      decimals: 2,
      format: "N",
      validateDecimalOnType: true,
      showSpinButton: false,
    },
  };

  gridWorkBudgetData = null;

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

    this.toolbarWorkBudgetDataOptions = [
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
    this.pageSettings = { pageCount: 10, pageSize: 10 };
    this.actionFailureWorkBudgetData =
      this.actionFailureWorkBudgetData.bind(this);
    this.actionCompleteWorkBudgetData =
      this.actionCompleteWorkBudgetData.bind(this);
    this.actionBeginWorkBudgetData = this.actionBeginWorkBudgetData.bind(this);
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
    this.actionCompleteGridWorkBudget =
      this.actionCompleteGridWorkBudget.bind(this);

    this.query = new Query().addParams("workId", props.workId);

    this.format = { type: "dateTime", format: "dd/MM/yyyy" };

    this.typeBudgets = [{ id: "Version X" }, { id: "Definitivo" }, { id: "Modificado" }];

    this.gridWorkBudget = {
      columns: [
        {
          field: "id",
          isPrimaryKey: true,
          isIdentity: true,
          visible: false,
        },
        {
          field: "workBudgetDataId",
          visible: false,
        },
        {
          field: "type",
          headerText: "Tipo",
          width: "70",
          editType: "dropdownedit",
          foreignKeyValue: "id",
          foreignKeyField: "id",
          validationRules: this.typeRules,
          dataSource: new DataManager(this.typeBudgets),
          edit: this.editType,
        },
        {
          field: "date",
          headerText: "Fecha",
          width: "100",
          type: "date",
          format: this.format,
          editType: "datepickeredit",
          validationRules: { required: true }
        },
        {
          field: "name",
          headerText: "Nombre",
          allowEditing: false,
          width: "100",
        },
        {
          field: "totalContract",
          headerText: "Total Contrato",
          width: "100",
          editType: "numericedit",
          textAlign: "right",
          edit: this.numericParams,
          validationRules: this.typeRules,
        },
        {
          field: "fileName",
          headerText: "Archivo",
          width: "100",
          template: this.templateFile,
          textAlign: "Center",
          allowEditing: false,
        },
        {
          field: "hasFile",
          headerText: "Presupuesto Adjunto",
          width: "100",
          visible: false,
          template: this.templateHasFile,
        },
      ],
      aggregates: [
        {
          columns: [
            {
              type: "Sum",
              field: "units",
              format: "N2",
              footerTemplate: this.footerSumUnits,
            },
            {
              type: "Sum",
              field: "unitsAccumulated",
              format: "N2",
              footerTemplate: this.footerSumUnits,
            },
            {
              type: "Sum",
              field: "unitsTotal",
              format: "N2",
              footerTemplate: this.footerSumUnits,
            },
            {
              type: "Sum",
              field: "amountUnits",
              format: "N2",
              footerTemplate: this.footerSumAmountUnits,
            },
            {
              type: "Sum",
              field: "amountAccumulated",
              format: "N2",
              footerTemplate: this.footerSumAmountAccumulated,
            },
            {
              type: "Sum",
              field: "amountTotal",
              format: "N2",
              footerTemplate: this.footerSumAmountTotal,
            },
          ],
        },
      ],
      dataSource: this.workBudgets,
      queryString: "workBudgetDataId",
      locale: "es-US",
      toolbar: [
        "Add",
        "Edit",
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
      ],
      actionFailure: this.actionFailureGridWorkBudget,
      allowGrouping: false,
      ref: (g) => (this.gridWorkBudget = g),
      allowTextWrap: true,
      textWrapSettings: this.wrapSettings,
      actionComplete: this.actionCompleteGridWorkBudget,
      actionBegin: this.actionBeginGridWorkBudget,
      editSettings: this.editSettings,
      toolbarClick: this.clickHandlerGridWorkBudget,
      props: this.props,
      load: this.loadGridWorkBudget,
      state: this.state,
      toggleModal: this.toggleModal,
      downloadDocuments: this.downloadDocuments,
      page: this
    };
  }

  actionFailureWorkBudgetData(args) {
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

  actionBeginWorkBudgetData(args) {
    if (args.requestType === "add") {
      args.data.workId = this.props.workId;
    }

    if (args.requestType === "save") {
      var cols = this.gridWorkBudgetData.columns;
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

  actionCompleteWorkBudgetData(args) {
    if (args.requestType === "save") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });
      this.setState({ rowSelected: null });
      this.gridWorkBudgetData.clearSelection();
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

  actionFailureGridWorkBudget(args) {
    let error = Array.isArray(args) ? args[0].error : args.error;
    if (Array.isArray(error)) {
      error = error[0].error;
    } else if (error.message != null) {
      error.statusText = args.error.message;
      error.responseText = args.error.message;
    } else if (error.statusText == null) {
      error.statusText = error.error.statusText;
      error.responseText = JSON.parse(error.error.responseText).Message;
    }

    this.props.showMessage({
      statusText: error.statusText,
      responseText: error.responseText,
      type: "danger",
    });
  }

  actionCompleteGridWorkBudget(args) {
    if (args.requestType === "save") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });

      getWorkBudgetData(args.data.workBudgetDataId).then((result) => {
        this.gridWorkBudgetData.setRowData(args.data.workBudgetDataId, result);
      });

      var childGridElements =
        this.gridWorkBudgetData.element.querySelectorAll(".e-detailrow");
      for (var i = 0; i < childGridElements.length; i++) {
        let element = childGridElements[i];
        let childGridObj = element.querySelector(".e-grid").ej2_instances[0];
        if (
          childGridObj.parentDetails.parentRowData.id ===
          args.data.workBudgetDataId
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

      getWorkBudgetData(args.data[0].workBudgetDataId).then((result) => {
        this.gridWorkBudgetData.setRowData(
          args.data[0].workBudgetDataId,
          result
        );
      });
    }
    if (args.requestType === "refresh") {
      var gridWorkBudgetData = document.getElementById("gridWorkBudgetData");

      if (args.rows != null && Array.isArray(args.rows)) {
        getWorkBudgetData(args.rows[0].data.workBudgetDataId).then((result) => {
          this.gridWorkBudgetData.setRowData(
            args.rows[0].data.workBudgetDataId,
            result
          );
        });
      } else if (gridWorkBudgetData.invoiceIdCleaned != null) {
        getWorkBudgetData(gridWorkBudgetData.invoiceIdCleaned).then(
          (result) => {
            this.gridInvoice.setRowData(
              gridWorkBudgetData.invoiceIdCleaned,
              result
            );
          }
        );
      }
    }
  }

  actionBeginGridWorkBudget(args) {
    if (args.requestType === "add") {
      args.data.workBudgetDataId = this.parentDetails.parentRowData.id;
      args.data.workId = this.parentDetails.parentRowData.workId;
    }
  }

  loadGridWorkBudget() {
    this.query = [];
    this.query = new Query().addParams(
      "workBudgetDataId",
      this.parentDetails.parentRowData.id
    );
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
        (item) => item.type === "Definitivo" || item.type === "Modificado"
      );
    } else if (Array.isArray(args)) {
      values = args.filter(
        (item) => item.type === "Definitivo" || item.type === "Modificado"
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

  clickHandler(args) {}

  clickHandlerGridWorkBudget(args) {
    if (args.item.id === "UploadFile") {
      const selectedRecords = this.getSelectedRecords();
      if (Array.isArray(selectedRecords) && selectedRecords.length === 1) {
        this.page.setState( { rowSelected: selectedRecords[0] } );
        // this.state.rowSelected = selectedRecords[0];
        this.toggleModal();
      } else {
        this.page.setState( { rowSelected: null } );
        // this.state.rowSelected = null;
        // this.setState({ rowSelected: null });
        this.props.showMessage({
          statusText: "Debes seleccionar un solo registro",
          responseText: "Debes seleccionar un solo registro",
          type: "danger",
        });
      }
    }

    if (args.item.id === "DownloadFile") {
      const selectedRecords = this.getSelectedRecords();
      if (Array.isArray(selectedRecords) && selectedRecords.length > 0) {
        this.downloadDocuments(selectedRecords);
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
    documentSelected.name = args.fileName;
    documentSelected.typeFile = args.file.type;

    updateDocumentInWorkBudget(documentSelected).then(() => {
      this.gridWorkBudgetData.setRowData(
        this.state.rowSelected.id,
        documentSelected
      );

      var childGridElements =
        this.gridWorkBudgetData.element.querySelectorAll(".e-detailrow");
      for (var i = 0; i < childGridElements.length; i++) {
        let element = childGridElements[i];
        let childGridObj = element.querySelector(".e-grid").ej2_instances[0];
        if (
          childGridObj.parentDetails.parentRowData.id === documentSelected.workBudgetDataId
        ) {
          childGridObj.refresh();
          break;
        }
      }
    });
  }

  downloadDocuments(selectedRecords) {
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
                dataSource={this.workBudgetDatas}
                locale="es-US"
                allowPaging={true}
                pageSettings={this.pageSettings}
                toolbar={this.toolbarWorkBudgetDataOptions}
                toolbarClick={this.clickHandler}
                editSettings={this.editSettings}
                style={{
                  marginLeft: 30,
                  marginRight: 30,
                  marginTop: -20,
                  marginBottom: 20,
                  overflow: "auto",
                }}
                actionFailure={this.actionFailureWorkBudgetData}
                actionComplete={this.actionCompleteWorkBudgetData}
                actionBegin={this.actionBeginWorkBudgetData}
                ref={(g) => (this.gridWorkBudgetData = g)}
                query={this.query}
                beforePrint={this.beforePrint}
                printComplete={this.printComplete}
                childGrid={this.gridWorkBudget}
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
                  <ColumnDirective field="workId" visible={false} />
                  <ColumnDirective
                    field="reference"
                    headerText="Referencia"
                    width="100"
                    validationRules={{ required: true }}
                  />
                  <ColumnDirective
                    field="workName"
                    headerText="Obra"
                    allowEditing={false}
                    width="100"
                    visible={false}
                  />
                  <ColumnDirective
                    field="description"
                    headerText="Descripción"
                    width="100"
                  />
                  <ColumnDirective
                    field="total"
                    headerText="Total"
                    width="100"
                    textAlign="Right"
                    headerTextAlign="Left"
                    allowEditing={false}
                  />
                </ColumnsDirective>

                <Inject
                  services={[Page, Toolbar, Edit, Aggregate, DetailRow]}
                />
              </GridComponent>
            </Row>
          </div>
        </div>
      </Fragment>
    );
  }
}

WorkBudgets1.propTypes = {};

export default WorkBudgets1;
