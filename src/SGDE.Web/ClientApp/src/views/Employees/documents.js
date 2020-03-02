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
import { config, DOCUMENTS, TYPES_DOCUMENT } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY } from "../../services";
import ModalSelectFile from "../Modals/modal-select-file";

L10n.load(data);

class Documents extends Component {
  documents = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${DOCUMENTS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  typesDocument = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${TYPES_DOCUMENT}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  grid = null;
  typesDocumentIdRules = { required: true };

  constructor(props) {
    super(props);

    this.state = {
      modal: false
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
    this.toggleModal = this.toggleModal.bind(this);

    this.template = this.gridTemplate;
  }

  gridTemplate(args) {
    if (args.file !== null && args.file !== "") {
      return (
        <div>
          <span className="dot-green" onClick={() => {this.toggleModal()}}></span>
        </div>
      );
    } else {
      return (
        <div>
          <span className="dot-red" onClick={() => {this.toggleModal()}}></span>
        </div>
      );
    }
  }

  toggleModal() {
    this.setState({
      modal: !this.state.modal
    });
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
      <Fragment>
          <ModalSelectFile
          isOpen={this.state.modal}
          toggle={this.toggleModal}
          updatePhoto={this.updatePhoto}
          userId={this.props.user.id}
          type="image"
        />
        <div className="animated fadeIn">
          <div className="card" style={{ marginRight: "60px" }}>
            <div className="card-header">
              <i className="icon-layers"></i> Cursos
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
                  marginTop: 20,
                  marginBottom: 20
                }}
                actionFailure={this.actionFailure}
                actionComplete={this.actionComplete}
                allowGrouping={false}
                rowSelected={this.rowSelected}
                ref={g => (this.grid = g)}
                query={new Query().addParams("userId", this.props.user.id)}
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
                    field="fileName"
                    headerText="Nombre del Documento"
                    width="100"
                    allowEditing={false}
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
                    field="typeDocumentId"
                    headerText="Tipo de Documento"
                    width="100"
                    editType="dropdownedit"
                    foreignKeyValue="name"
                    foreignKeyField="id"
                    validationRules={this.typesDocumentIdRules}
                    dataSource={this.typesDocument}
                  />
                  <ColumnDirective
                    headerText="Archivo"
                    width="100"
                    template={this.template}
                    textAlign="Center"
                    allowEditing={false}
                  />
                  <ColumnDirective
                    field="userId"
                    headerText="User"
                    width="100"
                    defaultValue={this.props.user.id}
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

Documents.propTypes = {};

export default Documents;
