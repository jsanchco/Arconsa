import React, { Component, Fragment } from "react";
import PropTypes from "prop-types";
import { Button, Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import { Row, Col } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Inject,
  Toolbar,
  Page,
  ForeignKey
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { DialogComponent } from "@syncfusion/ej2-react-popups";
import { config, WORKERSHIRING, PROFESSIONS } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY, updateWorkersInWork } from "../../services";
import "./modal-worker.css";
import Legend from "../../components/legend";

L10n.load(data);

class ModalWorkers extends Component {
  workers = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${WORKERSHIRING}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  professions = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${PROFESSIONS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  grid = null;
  selectedRowIndex = [];
  update = false;
  workSelected = null;

  constructor(props) {
    super(props);

    this.state = {
      workers: null,
      hideConfirmDialog: false
    };

    this._handleOnClickSave = this._handleOnClickSave.bind(this);
    this.dialogClose = this.dialogClose.bind(this);
    this.actionFailure = this.actionFailure.bind(this);
    this.onDataBound = this.onDataBound.bind(this);
    this.onRowDataBound = this.onRowDataBound.bind(this);
    this.rowSelected = this.rowSelected.bind(this);
    this.onClickWorkersInWork = this.onClickWorkersInWork.bind(this);

    this.selectionSettings = {
      checkboxMode: "ResetOnRowClick",
      persistSelection: true,
      type: "Multiple",
      mode: "Row"
    };
    this.pageSettings = { pageCount: 10, pageSize: 10 };
    this.toolbarOptions = ["Search"];
    // this.editSettings = {
    //   allowEditing: true
    // };

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
    this.professionIdRules = { required: true };
  }

  _handleOnClickSave() {
    this.setState({ hideConfirmDialog: true });
  }

  dialogClose() {
    this.setState({
      hideConfirmDialog: false
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
    // console.log("this.update ->", this.update);
    // console.log("this.selectedRowIndex ->", this.selectedRowIndex);
    // console.log("workSelected ->", this.props.workSelected);
    // if (!this.update) {
    //   this.grid.selectRows(this.selectedRowIndex);
    //   this.update = true;
    // }
  }

  onRowDataBound(args) {
    // if (args.data.state === 0) {
    //   const rowIndex = this.grid.getRowIndexByPrimaryKey(args.data.id);
    //   //this.props.addSelection(rowIndex);
    //   if (!this.selectedRowIndex.includes(rowIndex)) {
    //     this.selectedRowIndex.push(rowIndex);
    //   }
    // }
  }

  rowSelected() {
    this.selectedRowIndex = this.grid.getSelectedRowIndexes();
  }

  onClickWorkersInWork() {
    const data = this.grid.getCurrentViewRecords();
    data.forEach(row => {
      if (row.state === 0) {
        const rowIndex = this.grid.getRowIndexByPrimaryKey(row.id);
        this.selectedRowIndex.push(rowIndex);
      }
      this.grid.selectRows(this.selectedRowIndex);
    });
  }

  headerCellInfo(args) {
    args.node.getElementsByClassName("e-checkbox-wrapper")[0] &&
      args.node.getElementsByClassName("e-checkbox-wrapper")[0].remove();
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
          className={"modal-lg modal-primary"}
        >
          <ModalHeader toggle={this.props.toggle}>{title}</ModalHeader>
          <ModalBody>
            <Row>
              <Col>
                <Button
                  color="danger"
                  style={{ marginLeft: "10px" }}
                  onClick={this.onClickWorkersInWork}
                >
                  Seleccionar Trabajadores en Obra
                </Button>
              </Col>
            </Row>
            <div
              style={{
                marginLeft: "18px",
                marginTop: "10px",
                marginBottom: "-10px"
              }}
            >
              <Legend
                elements={[
                  { color: "dot-green", text: "Activo" },
                  { color: "dot-red", text: "En esta obra" },
                  { color: "dot-orange", text: "En otra obra" }
                ]}
              />
            </div>

            <Row>
              <GridComponent
                dataSource={this.workers}
                locale="es-US"
                // allowPaging={true}
                // pageSettings={this.pageSettings}
                actionFailure={this.actionFailure}
                toolbar={this.toolbarOptions}
                style={{
                  marginLeft: 30,
                  marginRight: 30,
                  marginTop: 20,
                  marginBottom: 20
                }}
                height={500}
                ref={g => (this.grid = g)}
                selectionSettings={this.selectionSettings}
                query={query}
                dataBound={this.onDataBound}
                rowDataBound={this.onRowDataBound}
                rowSelected={this.rowSelected}
                headerCellInfo={this.headerCellInfo}
                editSettings={this.editSettings}
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
                  <ColumnDirective
                    field="professionId"
                    headerText="Profesión"
                    width="100"
                    editType="dropdownedit"
                    foreignKeyValue="name"
                    foreignKeyField="id"
                    validationRules={this.professionIdRules}
                    dataSource={this.professions}
                  />
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
                <Inject services={[ForeignKey, Page, Toolbar]} />
              </GridComponent>
            </Row>
          </ModalBody>
          <ModalFooter>
            <Button color="primary" onClick={this._handleOnClickSave}>
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
