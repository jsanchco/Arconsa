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
  Row,
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
  ForeignKey,
} from "@syncfusion/ej2-react-grids";
import { AppSwitch } from "@coreui/react";
import "./modal-select.css";
import "./modal-worker.css";

class ModalMassiveSigning extends Component {
  fields = { text: "name", value: "id" };
  ddl = null;
  grid = null;

  hoursRules = {
    regex: ["^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", "Hora mal formateada"],
    required: true,
  };
  hourTypeIdRules = { required: false };

  baseHours = [
    { id: 1, startHour: "08:00", endHour: "17:00", hourTypeId: 1, total: 9 },
  ];
  hourTypes = [
    { id: 1, name: "Hora Ordinaria" },
    { id: 2, name: "Hora Extra" },
    { id: 3, name: "Hora Festivo" },
  ];

  constructor(props) {
    super(props);

    this.state = {
      hideConfirmDialog: false,
      includeSaturdays: false,
      includeSundays: false,
    };

    this.confirmButton = [
      {
        click: () => {
          this.setState({ hideConfirmDialog: false });
          this.sendMassiveSigning();
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

    this.toolbarOptions = ["Add", "Edit", "Delete", "Update", "Cancel"];
    this.editSettings = {
      showDeleteConfirmDialog: true,
      allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      newRowPosition: "Bottom",
    };
    this.hourTypeIdRules = { required: true };

    this.animationSettings = { effect: "None" };
    this.handleOnClickSave = this.handleOnClickSave.bind(this);
    this.sendMassiveSigning = this.sendMassiveSigning.bind(this);
    this.formatDate = this.formatDate.bind(this);
    this.sumHours = this.sumHours.bind(this);
    this.templateSumHours = this.templateSumHours.bind(this);
    this.handleInputChange = this.handleInputChange.bind(this);
  }

  componentDidUpdate(prevProps, prevState) {
    if (this.props.userId && this.props.isOpen === true) {
      getWorksByUserId(this.props.userId).then((items) => {
        if (this.ddl) {
          this.ddl.dataSource = items;
        }
      });
    }
  }

  handleOnClickSave() {
    this.setState({ hideConfirmDialog: true });
  }

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;

    if (name === "includeSaturdays") {
      this.setState({
        includeSaturdays: target.checked,
      });
    }

    if (name === "includeSundays") {
      this.setState({
        includeSundays: target.checked,
      });
    }
  }

  dialogClose() {
    this.setState({
      hideConfirmDialog: false,
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
        type: "danger",
      });

      return;
    } 
    sendMassiveSigning({
      userHiringId: valueDdl,
      startSigning: valueDtpStartDate,
      endSigning: valueDtpEndDate,
      data: this.grid.getCurrentViewRecords(),
      includeSaturdays: this.state.includeSaturdays,
      includeSundays: this.state.includeSundays,
    }).then(() => {
      this.props.updateDailySignings();
    });
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

  templateSumHours(args) {
    return <div>{this.sumHours(args.startHour, args.endHour)}</div>;
  }

  sumHours(startHour, endHour) {
    let hours = 0;
    if (startHour <= endHour) {
      const now = new Date();
      const dateStart = new Date(
        now.getFullYear(),
        now.getMonth(),
        now.getDate(),
        startHour.substring(0, 2),
        startHour.substring(3, 5)
      );
      const dateEnd = new Date(
        now.getFullYear(),
        now.getMonth(),
        now.getDate(),
        endHour.substring(0, 2),
        endHour.substring(3, 5)
      );
      const milliseconds = Math.abs(dateEnd - dateStart);
      hours = milliseconds / 36e5;
    } else {
      const now = new Date();
      const dateStart = new Date(
        now.getFullYear(),
        now.getMonth(),
        now.getDate(),
        startHour.substring(0, 2),
        startHour.substring(3, 5)
      );
      let dateEnd = new Date(
        now.getFullYear(),
        now.getMonth(),
        now.getDate(),
        endHour.substring(0, 2),
        endHour.substring(3, 5)
      );
      dateEnd.setDate(dateEnd.getDate() + 1);
      const milliseconds = Math.abs(dateEnd - dateStart);
      hours = milliseconds / 36e5;
    }

    return hours;
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
          ref={(dialog) => (this.confirmDialogInstance = dialog)}
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
              <Form inline style={{ marginLeft: "10px", marginBottom: "20px" }}>
                <FormGroup className="col-2">
                  <Label for="startDate">Fecha Inicio</Label>
                  <DatePickerComponent
                    id="startDate"
                    ref={(g) => (this.dtpStartDate = g)}
                    format="dd/MM/yyyy"
                  />
                </FormGroup>
                <FormGroup className="col-2" style={{ marginLeft: "10px" }}>
                  <Label for="endDate">Fecha Fin</Label>
                  <DatePickerComponent
                    id="endDate"
                    ref={(g) => (this.dtpEndDate = g)}
                    format="dd/MM/yyyy"
                  />
                </FormGroup>
                <FormGroup className="col-7" style={{ marginLeft: "10px" }}>
                  <Label for="workId">Obra</Label>
                  <DropDownListComponent
                    id="workId"
                    dataSource={null}
                    placeholder={`Selecciona Obra`}
                    fields={this.fields}
                    ref={(g) => (this.ddl = g)}
                  />
                </FormGroup>
              </Form>
            </Row>
            <Row style={{ marginTop: "-10px" }}>
              <FormGroup className="col-4" style={{ marginLeft: "10px" }}>
                <Label
                  htmlFor="includeSaturdays"
                  style={{ verticalAlign: "bottom" }}
                >
                  Incluir Sábados&nbsp;
                </Label>
                <AppSwitch
                  className={"mx-1 mt-4"}
                  variant={"pill"}
                  color={"primary"}
                  label
                  checked={this.state.includeSaturdays}
                  id="includeSaturdays"
                  name="includeSaturdays"
                  placeholder="incluir sábados"
                  onChange={this.handleInputChange}
                  dataOn="Si"
                  dataOff="No"
                />
              </FormGroup>
              <FormGroup className="col-4">
                <Label
                  htmlFor="includeSundays"
                  style={{ verticalAlign: "bottom" }}
                >
                  Incluir Domingos&nbsp;
                </Label>
                <AppSwitch
                  className={"mx-1 mt-4"}
                  variant={"pill"}
                  color={"primary"}
                  label
                  checked={this.state.includeSundays}
                  id="includeSundays"
                  name="includeSundays"
                  placeholder="incluir domingos"
                  onChange={this.handleInputChange}
                  dataOn="Si"
                  dataOff="No"
                />
              </FormGroup>
            </Row>
            <Row>
              <GridComponent
                dataSource={this.baseHours}
                locale="es-US"
                toolbar={this.toolbarOptions}
                style={{
                  marginLeft: 30,
                  marginRight: 30,
                  marginTop: 20,
                  marginBottom: 20,
                }}
                ref={(g) => (this.grid = g)}
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
                  <ColumnDirective
                    field="startHour"
                    headerText="Hora Inicio"
                    width="100"
                    validationRules={this.hoursRules}
                  />
                  <ColumnDirective
                    field="endHour"
                    headerText="Hora Fin"
                    width="100"
                    validationRules={this.hoursRules}
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
                    allowEditing={false}
                    template={this.templateSumHours}
                  />
                </ColumnsDirective>
                <Inject services={[ForeignKey]} />
              </GridComponent>
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
  // userId: PropTypes.number.isRequired,
  showMessage: PropTypes.func.isRequired,
  updateDailySignings: PropTypes.func.isRequired,
};

export default ModalMassiveSigning;
