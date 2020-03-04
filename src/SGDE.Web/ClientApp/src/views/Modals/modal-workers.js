import React, { Component, Fragment } from "react";
import PropTypes from "prop-types";
import { Button, Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import { Row } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Inject
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor } from "@syncfusion/ej2-data";
import { DialogComponent } from "@syncfusion/ej2-react-popups";
import { config, USERS } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY } from "../../services";
import "./modal-worker.css";

L10n.load(data);

class ModalWorkers extends Component {
  workers = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${USERS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  grid = null;

  constructor(props) {
    super(props);

    this.state = {
      workers: null,
      hideConfirmDialog: false,
      workersSelected: null
    };

    this._handleOnClick = this._handleOnClick.bind(this);
    this.dialogClose = this.dialogClose.bind(this);

    this.selectionSettings = {
      checkboxMode: "ResetOnRowClick"
    };

    this.confirmButton = [
      {
        click: () => {
          this.setState({ hideConfirmDialog: false });

          const selectedRecords = this.grid.getSelectedRecords();
          this.props.updateWorkersInWork(selectedRecords);
          
          this.props.toggle();
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
    //this.props.toggle();
  }

  dialogClose() {
    this.setState({
      hideConfirmDialog: false
    });
  }

  render() {
    let title = "";
    if (
      this.props.workSelected !== null &&
      this.props.workSelected !== undefined
    ) {
      title = ` Trabajadores [${this.props.workSelected.name}]`;
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
                style={{
                  marginLeft: 30,
                  marginRight: 30,
                  marginTop: 20,
                  marginBottom: 20
                }}
                rowSelected={this.rowSelected}
                ref={g => (this.grid = g)}
                selectionSettings={this.selectionSettings}
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
                    field="fullname"
                    headerText="Nombre"
                    width="100"
                  />
                  <ColumnDirective field="dni" headerText="DNI" width="100" />
                  <ColumnDirective
                    field="workName"
                    headerText="Obra Asignada"
                    width="100"
                  />
                </ColumnsDirective>
                <Inject services={[]} />
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
  updateWorkersInWork: PropTypes.func.isRequired
};

export default ModalWorkers;
