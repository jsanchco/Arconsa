import React, { Component, Fragment } from "react";
import PropTypes from "prop-types";
import { Button, Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import { Row } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Inject,
  Toolbar,
  Page,
  ForeignKey,
  Group
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { DialogComponent } from "@syncfusion/ej2-react-popups";
import { config, WORKERSHIRING } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY, updateWorkersInWork } from "../../services";
import "./modal-worker.css";

L10n.load(data);

class ModalWorkers extends Component {
  workers = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${WORKERSHIRING}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  grid = null;

  constructor(props) {
    super(props);

    this.state = {
      workers: null,
      hideConfirmDialog: false,
      workersSelected: null,
      selectedRecords: null,
      selectedRowIndex: []
    };

    this._handleOnClick = this._handleOnClick.bind(this);
    this.dialogClose = this.dialogClose.bind(this);
    this.actionFailure = this.actionFailure.bind(this);
    this.onDataBound = this.onDataBound.bind(this);
    this.onRowDataBound = this.onRowDataBound.bind(this);

    this.selectionSettings = {
      checkboxMode: "ResetOnRowClick",
      persistSelection: true
    };
    this.pageSettings = { pageCount: 10, pageSize: 10 };
    this.toolbarOptions = ["Search"];

    this.confirmButton = [
      {
        click: () => {
          this.setState({ hideConfirmDialog: false });

          const selectedRecords = this.grid.getSelectedRecords();
          updateWorkersInWork(selectedRecords, this.props.workSelected.id);
        },
        buttonModel: { content: "Si", isPrimary: true }
      },
      {
        click: () => {
          this.setState({ hideConfirmDialog: false });
        },
        buttonModel: { content: "No" }
      }
    ];

    this.animationSettings = { effect: "None" };
  }

  _handleOnClick() {
    this.setState({ hideConfirmDialog: true });
  }

  dialogClose() {
    this.setState({
      hideConfirmDialog: false
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

  stateTemplate(args) {
    if (args.state === 0) {
      return (
        <div>
          <span className="dot-red"></span>
        </div>
      );
    }

    if (args.state === 1) {
      return (
        <div>
          <span className="dot-green"></span>
        </div>
      );
    }

    if (args.state === 2) {
      return (
        <div>
          <span className="dot-orange"></span>
        </div>
      );
    }
  }

  onDataBound() {
    console.log("dataBound!!!");
  }

  onRowDataBound(args) {
    if (args.data.state === 0) {
      let { selectedRowIndex } = this.state;
      selectedRowIndex.push(args.data.id);

      this.setState({ selectedRowIndex: selectedRowIndex });
    }
  }

  render() {
    let title = "";
    let query = null;
    if (
      this.props.workSelected !== null &&
      this.props.workSelected !== undefined
    ) {
      title = ` Trabajadores [${this.props.workSelected.name}]`;
      query = new Query().addParams("workId", this.props.workSelected.id);
    }

    return (
      <Fragment>
        <DialogComponent
          id="confirmDialog"
          header="Validar Cambios"
          visible={this.state.hideConfirmDialog}
          showCloseIcon={true}
          animationSettings={this.animationSettings}
          width="500px"
          content="¿Estás seguro de hacer estas modificaciones?"
          ref={dialog => (this.confirmDialogInstance = dialog)}
          target="#target-works"
          buttons={this.confirmButton}
          close={this.dialogClose.bind(this)}
        ></DialogComponent>
        <Modal
          isOpen={this.props.isOpen}
          toggle={this.props.toggle}
          className={"modal-lg"}
        >
          <ModalHeader toggle={this.props.toggle}>{title}</ModalHeader>
          <ModalBody>
            <Row>
              <GridComponent
                dataSource={this.workers}
                locale="es-US"
                allowPaging={true}
                pageSettings={this.pageSettings}
                actionFailure={this.actionFailure}
                toolbar={this.toolbarOptions}
                style={{
                  marginLeft: 30,
                  marginRight: 30,
                  marginTop: 20,
                  marginBottom: 20
                }}
                ref={g => (this.grid = g)}
                selectionSettings={this.selectionSettings}
                query={query}
                dataBound={this.onDataBound}
                rowDataBound={this.onRowDataBound}
                selectedRowIndex={[5, 8]}
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
                  <ColumnDirective type="checkbox" width="50"></ColumnDirective>
                  <ColumnDirective
                    field="name"
                    headerText="Nombre"
                    width="100"
                  />
                  <ColumnDirective field="dni" headerText="DNI" width="100" />
                  <ColumnDirective
                    field="workName"
                    headerText="Obra Asignada"
                    width="100"
                  />
                  <ColumnDirective
                    field="state"
                    headerText="Estado"
                    width="100"
                    textAlign="Center"
                    template={this.stateTemplate}
                  />
                </ColumnsDirective>
                <Inject services={[ForeignKey, Group, Page, Toolbar]} />
              </GridComponent>
            </Row>
          </ModalBody>
          <ModalFooter>
            <Button color="primary" onClick={this._handleOnClick}>
              Guardar
            </Button>
            <Button
              color="secondary"
              style={{ marginLeft: "10px" }}
              onClick={this.props.toggle}
            >
              Cancelar
            </Button>
          </ModalFooter>
        </Modal>
      </Fragment>
    );
  }
}

ModalWorkers.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  toggle: PropTypes.func.isRequired,
  workSelected: PropTypes.object,
  showMessage: PropTypes.func.isRequired
};

export default ModalWorkers;
