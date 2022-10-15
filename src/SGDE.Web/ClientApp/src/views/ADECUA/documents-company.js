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
import { getValue } from "@syncfusion/ej2-base";
import { DataManager, WebApiAdaptor } from "@syncfusion/ej2-data";
import { config, COMPANY_DOCUMENTS } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import {
  TOKEN_KEY,
  updateCompanyDocument,
  base64ToArrayBuffer,
  saveByteArray,
} from "../../services";
import ModalSelectCompanyDocument from "../Modals/modal-select-company-document";

L10n.load(data);

class DocumentsCompany extends Component {
  documents = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${COMPANY_DOCUMENTS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

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
        tooltipText: "Descargar Archivo(s)",
        prefixIcon: "e-custom-icons e-file-download",
        id: "DownloadFile",
      },
      "Search",
    ];
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
    this.clickHandler = this.clickHandler.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
    this.updateDocument = this.updateDocument.bind(this);
    this.downloadDocuments = this.downloadDocuments.bind(this);
    this.rowDataBound = this.rowDataBound.bind(this);

    this.template = this.gridTemplate;

    this.selectionSettings = {
      checkboxMode: "ResetOnRowClick",
    };

    this.format = { type: "dateTime", format: "dd/MM/yyyy" };
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

  actionFailure(args) {
    let error = Array.isArray(args) ? args[0].error : args.error;
    if (Array.isArray(error)) {
      error = error[0].error;
    } else if (error.message != null) {
      error.statusText = args.error.message;
      error.responseText = args.error.message;
    } else if (error.responseText != null) {
      let customError = JSON.parse(error.responseText).Message;
      this.props.showMessage({
        statusText: customError,
        responseText: customError,
        type: "danger",
      });

      return;
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
      this.setState({ rowSelected: null });
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

    updateCompanyDocument(documentSelected).then(() => {
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
          saveByteArray(document.fileName, fileArr, document.typeFile);
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

  rowDataBound(args) {
    if (args.row) {
      var alarm = getValue("alarm", args.data);
      if (alarm) {
        args.row.classList.add("color-orange");
      }
    }
  }

  render() {
    return (
      <Fragment>
        <ModalSelectCompanyDocument
          isOpen={this.state.modal}
          toggle={this.toggleModal}
          updateDocument={this.updateDocument}
          userId={this.props.userId}
          rowSelected={this.state.rowSelected}
          type="document"
        />
        <div className="animated fadeIn">
          <div
            className="card"
            style={{ marginRight: "60px", marginTop: "20px" }}
          >
            <div className="card-header">
              <i className="icon-layers"></i> Documentos de Empresa
            </div>
            <div className="card-body"></div>
            <Row>
              <GridComponent
                dataSource={this.documents}
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
                rowDataBound={this.rowDataBound}
                ref={(g) => (this.grid = g)}
                query={this.query}
                selectionSettings={this.selectionSettings}
              >
                <ColumnsDirective>
                  <ColumnDirective type="checkbox" width="50"></ColumnDirective>
                  <ColumnDirective
                    field="id"
                    headerText="Id"
                    width="40"
                    isPrimaryKey={true}
                    isIdentity={true}
                    visible={false}
                  />
                  <ColumnDirective
                    field="reference"
                    headerText="Referencia"
                    width="100"
                    validationRules={{ required: true }}
                  />
                  <ColumnDirective
                    field="description"
                    headerText="Descripción"
                    width="100"
                  />
                  <ColumnDirective
                    field="observations"
                    headerText="Observaciones"
                    width="100"
                  />
                  <ColumnDirective
                    field="date"
                    headerText="Fecha"
                    width="100"
                    editType="datepickeredit"
                    type="date"
                    format={this.format}
                    textAlign="Center"
                    defaultValue={new Date()}
                    validationRules={{ required: true }}
                  />
                  <ColumnDirective
                    field="dateExpiration"
                    headerText="Fecha Expiración"
                    width="100"
                    editType="datepickeredit"
                    type="date"
                    format={this.format}
                    textAlign="Center"
                  />
                  <ColumnDirective
                    field="dateWarning"
                    headerText="Fecha Aviso"
                    width="100"
                    editType="datepickeredit"
                    type="date"
                    format={this.format}
                    textAlign="Center"
                  />
                  <ColumnDirective
                    field="fileName"
                    headerText="Archivo"
                    width="100"
                    template={this.template}
                    textAlign="Center"
                    allowEditing={false}
                  />
                  {/* <ColumnDirective
                    field="alarm"
                    headerText="Alarma"
                    width="50"
                    textAlign="Center"
                    editType="booleanedit"
                    type="boolean"
                    displayAsCheckBox={true}
                    allowEditing={false}
                  /> */}
                  <ColumnDirective field="typeFile" visible={false} />
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

DocumentsCompany.propTypes = {};

export default DocumentsCompany;
