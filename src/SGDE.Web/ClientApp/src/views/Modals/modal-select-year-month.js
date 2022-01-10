import React, { Component } from "react";
import PropTypes from "prop-types";
import {
  Form,
  FormGroup,
  Row,
  Col,
  Label,
  Input,
  Button,
  Modal,
  ModalBody,
  ModalFooter,
  ModalHeader,
} from "reactstrap";
import "./modal-select.css";
import { NumericTextBoxComponent } from "@syncfusion/ej2-react-inputs";
import { DropDownListComponent } from "@syncfusion/ej2-react-dropdowns";
import { DataManager } from "@syncfusion/ej2-data";

class ModalSelectYearMonth extends Component {
  constructor(props) {
    super(props);

    this.state = {
      modal: false,
      yearOld: 2021,
      yearNew: 2021,
    };

    this.months = [
      { id: 1, value: "Enero" },
      { id: 2, value: "Febrero" },
      { id: 3, value: "Marzo" },
      { id: 4, value: "Abril" },
      { id: 5, value: "Mayo" },
      { id: 6, value: "Junio" },
      { id: 7, value: "Julio" },
      { id: 8, value: "Agosto" },
      { id: 9, value: "Septiembre" },
      { id: 10, value: "Octubre" },
      { id: 11, value: "Noviembre" },
      { id: 12, value: "Diciembre" },
    ];
    this.editMonths = {
      params: {
        popupWidth: "auto",
        sortOrder: "None",
      },
    };
    this.fieldsDdlMonths = { text: "value", value: "id" };

    this.ntbYearOld = null;
    this.ddlMonthOld = null;
    this.ntbYearNew = null;
    this.ddlMonthNew = null;

    this.handleOnClick = this.handleOnClick.bind(this);
  }

  handleOnClick() {
    if (
      this.ntbYearOld.value == null ||
      this.ddlMonthOld.value == null ||
      this.ntbYearNew.value == null ||
      this.ddlMonthNew.value == null) {
        this.props.showMessage({
          statusText: "Revisa los datos rellenos",
          responseText: "Revisa los datos rellenos",
          type: "danger",
        });

        return;
    }

    this.props.updateInidrectCosts(
      this.ntbYearOld.value,
      this.ddlMonthOld.value,
      this.ntbYearNew.value,
      this.ddlMonthNew.value
    );

    this.props.toggle();
  }

  handleChangeYearOld(args) {
    this.setState({ yearOld: args.value });
  }

  render() {
    return (
      <Modal
        isOpen={this.props.isOpen}
        toggle={this.props.toggle}
        className={"modal-primary"}
      >
        <ModalHeader toggle={this.props.toggle}>
          Selecciona año y mes
        </ModalHeader>
        <ModalBody>
          <Form>
            <Row>
              <Col xs="12" style={{ textAlign: "Center" }}>
                <h3>Fecha Origen</h3>
              </Col>
            </Row>
            <Row>
              <Col xs="6">
                <FormGroup>
                  <Label htmlFor="yearOld">Año</Label>
                  <NumericTextBoxComponent
                    format="N"
                    decimals={0}
                    min={2021}
                    validateDecimalOnType={true}
                    id="yearOld"
                    name="yearOld"
                    value={this.state.yearOld}
                    placeholder="Año origen"
                    ref={(g) => (this.ntbYearOld = g)}
                  />
                </FormGroup>
              </Col>
              <Col xs="6">
                <FormGroup>
                  <Label htmlFor="monthOld">Mes</Label>
                  <DropDownListComponent
                    id="monthOld"
                    name="monthOld"
                    placeholder={"Selecciona mes origen"}
                    fields={this.fieldsDdlMonths}
                    dataSource={new DataManager(this.months)}
                    ref={(g) => (this.ddlMonthOld = g)}
                  />
                </FormGroup>
              </Col>
            </Row>

            <Row>
              <Col xs="12">
                <hr></hr>
              </Col>
            </Row>

            <Row>
              <Col xs="12" style={{ textAlign: "Center" }}>
                <h3>Fecha Destino</h3>
              </Col>
            </Row>
            <Row>
              <Col xs="6">
                <FormGroup>
                  <Label htmlFor="yearNew">Año</Label>
                  <NumericTextBoxComponent
                    format="N"
                    decimals={0}
                    min={2021}
                    validateDecimalOnType={true}
                    id="yearNew"
                    name="yearNew"
                    value={this.state.yearNew}
                    placeholder="Año destino"
                    ref={(g) => (this.ntbYearNew = g)}
                  />
                </FormGroup>
              </Col>
              <Col xs="6">
                <FormGroup>
                  <Label htmlFor="name">Mes</Label>
                  <DropDownListComponent
                    id="monthNew"
                    name="monthNew"
                    placeholder={"Selecciona mes destino"}
                    fields={this.fieldsDdlMonths}
                    dataSource={new DataManager(this.months)}
                    ref={(g) => (this.ddlMonthNew = g)}
                  />
                </FormGroup>
              </Col>
            </Row>
          </Form>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={this.handleOnClick}>
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
    );
  }
}

ModalSelectYearMonth.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  toggle: PropTypes.func.isRequired,
  updateInidrectCosts: PropTypes.func,
  showMessage: PropTypes.func
};

export default ModalSelectYearMonth;
