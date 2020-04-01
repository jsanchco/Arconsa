import React, { Component, Fragment } from "react";
import PropTypes from "prop-types";
import {
  Button,
  Modal,
  ModalBody,
  ModalFooter,
  ModalHeader,
  Form,
  FormGroup,
  Label,
  Row
} from "reactstrap";
import { getWorksByUserId } from "../../services";
import { DatePickerComponent } from "@syncfusion/ej2-react-calendars";
import { DropDownListComponent } from "@syncfusion/ej2-react-dropdowns";
import { DialogComponent } from "@syncfusion/ej2-react-popups";
import { sendMassiveSigning } from "../../services";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Inject,
  ForeignKey
} from "@syncfusion/ej2-react-grids";
import { MaskedTextBoxComponent } from "@syncfusion/ej2-react-inputs";
import "./modal-select.css";
import "./modal-worker.css";

class ModalMassiveSigning extends Component {
  fields = { text: "name", value: "id" };
  ddl = null;
  grid = null;

  baseHours = [
    { startHour: "08:00", endHour: "14:00", hourTypeId: 1, total: 6 },
    { startHour: "15:00", endHour: "17:00", hourTypeId: 1, total: 2 }
  ];
  hourTypes = [
    { id: 1, name: "Hora Ordinaria" },
    { id: 2, name: "Hora Extra" },
    { id: 3, name: "Hora Festivo" }
  ];

  constructor(props) {
    super(props);

    this.state = {
      hideConfirmDialog: false
    };

    this.confirmButton = [
      {
        click: () => {
          this.setState({ hideConfirmDialog: false });
          this.sendMassiveSigning();
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

    this.toolbarOptions = ["Add", "Edit", "Delete", "Update", "Cancel"];
    this.editSettings = {
      showDeleteConfirmDialog: true,
      allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      newRowPosition: "Bottom"
    };
    this.hourTypeIdRules = { required: true };

    this.animationSettings = { effect: "None" };
    this.handleOnClickSave = this.handleOnClickSave.bind(this);
    this.sendMassiveSigning = this.sendMassiveSigning.bind(this);
    this.formatDate = this.formatDate.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
    this.checkFormats = this.checkFormats.bind(this);
  }

  componentDidUpdate(prevProps, prevState) {
    if (this.props.userId && this.props.isOpen === true) {
      getWorksByUserId(this.props.userId).then(items => {
        if (this.ddl) {
          this.ddl.dataSource = items;
        }
      });
    }
  }

  handleOnClickSave() {
    this.setState({ hideConfirmDialog: true });
  }

  dialogClose() {
    this.setState({
      hideConfirmDialog: false
    });
  }

  sendMassiveSigning() {
    const valueDtpStartDate = this.formatDate(this.dtpStartDate.value);
    const valueDtpEndDate = this.formatDate(this.dtpEndDate.value);
    const valueDdl = this.ddl.value;

    if (
      valueDtpStartDate === null ||
      valueDtpStartDate === "" ||
      valueDtpEndDate === null ||
      valueDtpEndDate === "" ||
      valueDdl === null
    ) {
      this.props.showMessage({
        statusText: "Consulta mal configurada",
        responseText: "Consulta mal configurada",
        type: "danger"
      });
    } else {
      if (this.dtpStartDate.value > this.dtpEndDate.value) {
        this.props.showMessage({
          statusText: "Consulta mal configurada",
          responseText: "Consulta mal configurada",
          type: "danger"
        });
      } else {
        sendMassiveSigning({
          userHiringId: valueDdl,
          startSigning: valueDtpStartDate,
          endSigning: valueDtpEndDate
        }).then(() => {
          this.props.updateDailySignings();
        });
      }
    }
  }

  formatDate(args) {
    if (args === null || args === "") {
      return "";
    }

    let day = args.getDate();
    if (day < 10) day = "0" + day;

    const month = args.getMonth() + 1;
    const year = args.getFullYear();

    if (month < 10) {
      return `${day}/0${month}/${year}`;
    } else {
      return `${day}/${month}/${year}`;
    }
  }

  checkFormats(args) {
    if (args.length !== 5) {
      return false;
    }
  }

  editTemplate(args) {
    return (
      <MaskedTextBoxComponent
        mask={"00:00"}
        id="startHour"
        name="startHour"
      />
    );
  }

  actionComplete(args) {
    if (args.requestType === "save") {
      if (!this.checkFormats(args.data.startHour)) {
        args.cancel = true;
      }
    }
  }

  render() {
    return (
      <Fragment>
        <DialogComponent
          id="confirmDialog"
          header="Validar Fichajes"
          visible={this.state.hideConfirmDialog}
          showCloseIcon={true}
          animationSettings={this.animationSettings}
          width="500px"
          content="¿Estás seguro de hacer estos fichajes?"
          ref={dialog => (this.confirmDialogInstance = dialog)}
          target="#target-daily-signing"
          buttons={this.confirmButton}
          close={this.dialogClose.bind(this)}
        ></DialogComponent>

        <Modal
          isOpen={this.props.isOpen}
          toggle={this.props.toggle}
          className={"modal-lg modal-primary"}
        >
          <ModalHeader toggle={this.props.toggle}>
            Selecciona Fechas y Obra
          </ModalHeader>
          <ModalBody>
            <Row>
              <Form inline style={{ marginLeft: "20px", marginBottom: "20px" }}>
                <FormGroup className="mb-2 mr-sm-2 mb-sm-0">
                  <Label for="startDate" className="mr-sm-2">
                    Fecha Inicio
                  </Label>
                  <DatePickerComponent
                    id="startDate"
                    ref={g => (this.dtpStartDate = g)}
                    format="dd/MM/yyyy"
                  />
                </FormGroup>
                <FormGroup
                  className="mb-2 mr-sm-2 mb-sm-0"
                  style={{ marginLeft: "20px" }}
                >
                  <Label for="endDate" className="mr-sm-2">
                    Fecha Fin
                  </Label>
                  <DatePickerComponent
                    id="endDate"
                    ref={g => (this.dtpEndDate = g)}
                    format="dd/MM/yyyy"
                  />
                </FormGroup>
                <FormGroup
                  className="mb-2 mr-sm-2 mb-sm-0"
                  style={{ marginLeft: "20px" }}
                >
                  <Label for="workId" className="mr-sm-2">
                    Obra
                  </Label>
                  <DropDownListComponent
                    id="workId"
                    dataSource={null}
                    placeholder={`Selecciona Obra`}
                    fields={this.fields}
                    ref={g => (this.ddl = g)}
                  />
                </FormGroup>
              </Form>
            </Row>
            <Row>
              <Row>
                <GridComponent
                  dataSource={this.baseHours}
                  locale="es-US"
                  actionFailure={this.actionFailure}
                  toolbar={this.toolbarOptions}
                  style={{
                    marginLeft: 30,
                    marginRight: 30,
                    marginTop: 20,
                    marginBottom: 20
                  }}
                  ref={g => (this.grid = g)}
                  editSettings={this.editSettings}
                  actionComplete={this.actionComplete}
                >
                  <ColumnsDirective>
                    <ColumnDirective
                      field="startHour"
                      headerText="Hora Inicio"
                      width="100"
                    />
                    <ColumnDirective
                      field="endHour"
                      headerText="Hora Fin"
                      width="100"
                    />
                    <ColumnDirective
                      field="hourTypeId"
                      headerText="Tipo"
                      width="100"
                      editType="dropdownedit"
                      foreignKeyValue="name"
                      foreignKeyField="id"
                      validationRules={this.hourTypeIdRules}
                      dataSource={this.hourTypes}
                    />
                    <ColumnDirective
                      field="total"
                      headerText="Total Horas"
                      width="100"
                    />
                  </ColumnsDirective>
                  <Inject services={[ForeignKey]} />
                </GridComponent>
              </Row>
            </Row>
          </ModalBody>
          <ModalFooter>
            <Button color="primary" onClick={this.handleOnClickSave}>
              Fichaje Masivo
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

ModalMassiveSigning.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  toggle: PropTypes.func.isRequired,
  userId: PropTypes.number.isRequired,
  showMessage: PropTypes.func.isRequired,
  updateDailySignings: PropTypes.func.isRequired
};

export default ModalMassiveSigning;
