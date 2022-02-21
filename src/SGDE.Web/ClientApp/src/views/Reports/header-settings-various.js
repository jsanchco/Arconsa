import React, { Component } from "react";
import PropTypes from "prop-types";
import { Form, FormGroup, Label, Button, Row, Col } from "reactstrap";
import { AppSwitch } from "@coreui/react";
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

    this.state = {
      showCeros: true,
    };

    this.element = null;

    this._handleClickSelection = this._handleClickSelection.bind(this);
    this._handleOnClick = this._handleOnClick.bind(this);
    this.handleInputChange = this.handleInputChange.bind(this);
  }

  componentDidMount() {
    this.ddl.dataSource = ["Trabajadores", "Obras", "Clientes"];
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
          textDdl,
          this.state.showCeros
        );
      }
    }
  }

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;

    if (name === "show_ceros") {
      this.setState({ showCeros: !target.checked });
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
      <Form style={{ marginLeft: "20px" }}>
        <Row>
          <Col xs="3">
            <FormGroup>
              <Label for="startDate">Fecha Inicio</Label>
              <DatePickerComponent
                id="startDate"
                ref={(g) => (this.dtpStartDate = g)}
                format="dd/MM/yyyy"
              />
            </FormGroup>
          </Col>
          <Col xs="3">
            <FormGroup style={{ marginLeft: "20px" }}>
              <Label for="endDate">Fecha Fin</Label>
              <DatePickerComponent
                id="endDate"
                ref={(g) => (this.dtpEndDate = g)}
                format="dd/MM/yyyy"
              />
            </FormGroup>
          </Col>
          <Col xs="3">
            <FormGroup style={{ marginLeft: "20px" }}>
              <Label for="lists">Listados</Label>
              <DropDownListComponent
                id="lists"
                dataSource={null}
                placeholder={`Selecciona listado`}
                fields={this.fields}
                ref={(g) => (this.ddl = g)}
                change={this._handleClickSelection}
              />
            </FormGroup>
          </Col>
          <Col xs="3">
            <FormGroup style={{ marginTop: "30px" }}>
              <Label htmlFor="show_ceros" style={{ verticalAlign: "bottom" }}>
                Mostrar registos con ceros&nbsp;
              </Label>
              <AppSwitch
                // className={"mx-1 mt-4"}
                variant={"pill"}
                color={"primary"}
                label
                id="show_ceros"
                name="show_ceros"
                placeholder="mostrar registos con ceros"
                onChange={this.handleInputChange}
                dataOn="No"
                dataOff="Si"
                checked={!this.state.showCeros}
              />
            </FormGroup>
          </Col>
        </Row>
        <Row>
          <Col xs="10"></Col>
          <Col xs="2">
            <Button
              color="primary"
              style={{ marginLeft: "30px", textAlign: "left" }}
              onClick={this._handleOnClick}
            >
              Consultar
            </Button>
          </Col>
        </Row>
      </Form>
    );
  }
}

HeaderSettingsVarious.propTypes = {
  showMessage: PropTypes.func.isRequired,
  updateReport: PropTypes.func.isRequired,
};

export default HeaderSettingsVarious;
