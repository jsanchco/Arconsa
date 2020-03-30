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
  Label
} from "reactstrap";
import { getWorksByUserId } from "../../services";
import { DatePickerComponent } from "@syncfusion/ej2-react-calendars";
import { DropDownListComponent } from "@syncfusion/ej2-react-dropdowns";
import { DialogComponent } from "@syncfusion/ej2-react-popups";
import { sendMassiveSigning } from "../../services";
import "./modal-select.css";
import "./modal-worker.css";

class ModalMassiveSigning extends Component {
  fields = { text: "name", value: "id" };
  ddl = null;

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

    this.animationSettings = { effect: "None" };
    this.handleOnClickSave = this.handleOnClickSave.bind(this);
    this.sendMassiveSigning = this.sendMassiveSigning.bind(this);
    this.formatDate = this.formatDate.bind(this);
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

    const day = args.getDate();
    const month = args.getMonth() + 1;
    const year = args.getFullYear();

    if (month < 10) {
      return `${day}/0${month}/${year}`;
    } else {
      return `${day}/${month}/${year}`;
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
  updateDailySignings:  PropTypes.func.isRequired
};

export default ModalMassiveSigning;
