import React, { Component } from "react";
import PropTypes from "prop-types";
import { Form, FormGroup, Label, Button } from "reactstrap";
import { DatePickerComponent } from "@syncfusion/ej2-react-calendars";
import { DropDownListComponent } from "@syncfusion/ej2-react-dropdowns";
import { L10n, loadCldr } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { getWorkers, getWorks, getClients } from "../../services";

import * as gregorian from "cldr-data/main/es-US/ca-gregorian.json";
import * as numbers from "cldr-data/main/es-US/numbers.json";
import * as timeZoneNames from "cldr-data/main/es-US/timeZoneNames.json";
import * as numberingSystems from "cldr-data/supplemental/numberingSystems.json";
import * as weekData from "cldr-data/supplemental/weekData.json";

loadCldr(numberingSystems, gregorian, numbers, timeZoneNames, weekData);

L10n.load(data);

class HeaderSettings extends Component {
  dtpStartDate = null;
  dtpEndDate = null;
  ddl = null;

  fields = { text: "name", value: "id" };

  constructor(props) {
    super(props);

    this._handleOnClick = this._handleOnClick.bind(this);
  }

  componentDidMount() {
    if (this.props.type === "workers") {
      getWorkers().then(items => {
        this.ddl.dataSource = items;
      });
    }

    if (this.props.type === "works") {
      getWorks().then(items => {
        this.ddl.dataSource = items;
      });
    }

    if (this.props.type === "clients") {
      getClients().then(items => {
        this.ddl.dataSource = items;
      });
    }
  }

  _handleOnClick() {
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
        this.props.updateReport(
          this.props.type,
          valueDtpStartDate,
          valueDtpEndDate,
          valueDdl
        );
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
    let title = null;

    switch (this.props.type) {
      case "workers":
        title = "Trabajador";
        break;

      case "works":
        title = "Obra";
        break;

      case "clients":
        title = "Cliente";
        break;

      default:
        title = "";
    }

    return (
      <Form inline style={{ marginLeft: "20px" }}>
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
          <Label for={title} className="mr-sm-2">
            {title}
          </Label>
          <DropDownListComponent
            id={title}
            dataSource={null}
            placeholder={`Selecciona ${title}`}
            fields={this.fields}
            ref={g => (this.ddl = g)}
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

HeaderSettings.propTypes = {
  type: PropTypes.string.isRequired,
  showMessage: PropTypes.func.isRequired,
  updateReport: PropTypes.func.isRequired
};

export default HeaderSettings;