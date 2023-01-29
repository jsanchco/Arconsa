import React, { Component } from "react";
import PropTypes from "prop-types";
import {
  Form,
  FormGroup,
  Row,
  Col,
  Label,
  Button,
  Modal,
  ModalBody,
  ModalFooter,
  ModalHeader,
  Input,
} from "reactstrap";
import "./modal-select.css";
import { NumericTextBoxComponent } from "@syncfusion/ej2-react-inputs";
import { DatePickerComponent } from "@syncfusion/ej2-react-calendars";

class ModalCancelInvoice extends Component {
  constructor(props) {
    super(props);

    this.dtpIssueDate = null;
    this.ntbAmount = null;
    this.ntbIva = null;

    this.handleOnClick = this.handleOnClick.bind(this);
  }

  handleOnClick() {
    this.props.toggle();
    this.props.billPaymentWithAmount(
      this.dtpIssueDate.value,
      this.ntbAmount.value,
      this.ntbIva.value,
      document.getElementById("description").value
    );
  }

  render() {
    let title = "Abonar total o parcialmente la Factura: ";
    let taxBase = 0;
    let description = "Abono Fact. ";
    if (this.props.invoice != null) {
      title += this.props.invoice.name;
      taxBase = -this.props.invoice.taxBase;
      description += this.props.invoice.name;
    }

    return (
      <Modal
        isOpen={this.props.isOpen}
        toggle={this.props.toggle}
        className={"modal-primary"}
      >
        <ModalHeader toggle={this.props.toggle}>{title}</ModalHeader>
        <ModalBody>
          <Form>
            <Row>
              <Col xs="12">
                <FormGroup>
                  <Label htmlFor="description">Descripción</Label>
                  <Input
                    type="text"
                    id="description"
                    name="description"
                    placeholder="Descripción"
                    value={description}
                  />
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col xs="4">
                <FormGroup>
                  <Label for="issueDate">Fecha</Label>
                  <DatePickerComponent
                    id="issueDate"
                    ref={(g) => (this.dtpIssueDate = g)}
                    format="dd/MM/yyyy"
                    value={new Date()}
                  />
                </FormGroup>
              </Col>
              <Col xs="4">
                <FormGroup>
                  <Label htmlFor="amount">Cantidad Abono</Label>
                  <NumericTextBoxComponent
                    format="N"
                    decimals={2}
                    max={0}
                    min={taxBase}
                    validateDecimalOnType={true}
                    id="amount"
                    name="amount"
                    value={taxBase}
                    placeholder="Cantidad Abono"
                    showSpinButton={false}
                    ref={(g) => (this.ntbAmount = g)}
                  />
                </FormGroup>
              </Col>
              <Col xs="4">
                <FormGroup>
                  <Label htmlFor="iva">IVA</Label>
                  <NumericTextBoxComponent
                    format="p2"
                    decimals={2}
                    min={0}
                    max={1}
                    step={0.01}
                    showSpinButton={false}
                    validateDecimalOnType={true}
                    id="iva"
                    name="iva"
                    value="0.21"
                    placeholder="iva"
                    ref={(g) => (this.ntbIva = g)}
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

ModalCancelInvoice.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  toggle: PropTypes.func.isRequired,
  billPaymentWithAmount: PropTypes.func,
  showMessage: PropTypes.func,
};

export default ModalCancelInvoice;
