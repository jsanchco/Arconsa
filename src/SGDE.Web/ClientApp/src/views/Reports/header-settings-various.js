import React, { Component } from "react";
import PropTypes from "prop-types";
import { Form, FormGroup, Label, Button } from "reactstrap";
import { DatePickerComponent } from "@syncfusion/ej2-react-calendars";
import { DropDownListComponent } from "@syncfusion/ej2-react-dropdowns";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";

L10n.load(data);

class HeaderSettingsVarious extends Component {
  dtpStartDate = null;
  dtpEndDate = null;
  ddl = null;

  fields = { text: "name", value: "id" };

  constructor(props) {
    super(props);

    this.element = null;

    this._handleClickSelection = this._handleClickSelection.bind(this);
    this._handleOnClick = this._handleOnClick.bind(this);
  }

  componentDidMount() {
    this.ddl.dataSource = [
      "Trabajadores",
      "Obras",
      "Clientes"
    ];
  }

  _handleClickSelection(event) {
    // switch (event.value) {
    //   case "Trabajadores":
    //   case "Obras":
    //     this.dtpStartDate.cssClass = "e-disabled";
    //     this.dtpEndDate.cssClass = "e-disabled";
    //     break;

    //   default:
    //     this.dtpStartDate.cssClass = null;
    //     this.dtpEndDate.cssClass = null;
    //     break;
    // }
  }

  _handleOnClick() {
    const valueDtpStartDate = this.formatDate(this.dtpStartDate.value);
    const valueDtpEndDate = this.formatDate(this.dtpEndDate.value);
    const textDdl = this.ddl.text;

    if (textDdl === null) {
      this.props.showMessage({
        statusText: "Consulta mal configurada",
        responseText: "Consulta mal configurada",
        type: "danger",
      });
    } else {
      if (this.dtpStartDate.value > this.dtpEndDate.value) {
        this.props.showMessage({
          statusText: "Consulta mal configurada",
          responseText: "Consulta mal configurada",
          type: "danger",
        });
      } else {
        this.props.updateReport(
          valueDtpStartDate,
          valueDtpEndDate,
          textDdl
        );
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

  render() {
    return (
      <Form inline style={{ marginLeft: "20px" }}>
        <FormGroup className="mb-2 mr-sm-2 mb-sm-0">
          <Label for="startDate" className="mr-sm-2">
            Fecha Inicio
          </Label>
          <DatePickerComponent
            id="startDate"
            ref={(g) => (this.dtpStartDate = g)}
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
            ref={(g) => (this.dtpEndDate = g)}
            format="dd/MM/yyyy"
          />
        </FormGroup>
        <FormGroup
          className="mb-2 mr-sm-2 mb-sm-0"
          style={{ marginLeft: "20px" }}
        >
          <Label for="lists" className="mr-sm-2">
            Listados
          </Label>
          <DropDownListComponent
            id="lists"
            dataSource={null}
            placeholder={`Selecciona listado`}
            fields={this.fields}
            ref={(g) => (this.ddl = g)}
            change={this._handleClickSelection}
          />
        </FormGroup>
        <Button
          color="primary"
          style={{ marginLeft: "20px" }}
          onClick={this._handleOnClick}
        >
          Consultar
        </Button>
      </Form>
    );
  }
}

HeaderSettingsVarious.propTypes = {
  showMessage: PropTypes.func.isRequired,
  updateReport: PropTypes.func.isRequired,
};

export default HeaderSettingsVarious;
