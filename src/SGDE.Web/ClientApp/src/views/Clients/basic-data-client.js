import React, { Component, Fragment } from "react";
import { Form, Col, FormGroup, Input, Label, Row, Button } from "reactstrap";
import { AppSwitch } from "@coreui/react";
import {
  MaskedTextBoxComponent,
  NumericTextBoxComponent,
} from "@syncfusion/ej2-react-inputs";
import { updateClient, getClient } from "../../services";
import {
  createSpinner,
  showSpinner,
  hideSpinner,
} from "@syncfusion/ej2-popups";

class BasicDataClient extends Component {
  constructor(props) {
    super(props);

    this.ntbPercentageRetention = null;

    this.state = {};
    getClient(this.props.clientId).then((result) => {
      this.setState({
        id: result.id,
        name: result.name,
        cif: result.cif,
        phoneNumber: result.phoneNumber,
        address: result.address,
        wayToPay: result.wayToPay,
        expirationDays: result.expirationDays,
        accountNumber: result.accountNumber,
        email: result.email,
        emailInvoice: result.emailInvoice,
        active: result.active,
      });
    });

    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleChangeExpirationDays =
      this.handleChangeExpirationDays.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.getClient = this.getClient.bind(this);
  }

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;

    if (name === "active") {
      this.setState({
        [name]: target.checked,
      });
    } else {
      this.setState({
        [name]: target.value,
      });
    }
  }

  handleChangeExpirationDays(args) {
    this.setState({ expirationDays: args.value });
  }

  getClient() {
    return {
      id: this.state.id,
      name: this.state.name,
      cif: this.state.cif,
      phoneNumber: this.state.phoneNumber,
      address: this.state.address,
      wayToPay: this.state.wayToPay,
      expirationDays: this.state.expirationDays,
      accountNumber: this.state.accountNumber,
      email: this.state.email,
      emailInvoice: this.state.emailInvoice,
      active: this.state.active,
    };
  }

  handleSubmit() {
    const element = document.getElementById("container");

    createSpinner({
      target: element,
    });
    showSpinner(element);
    updateClient(this.getClient())
      .then(() => {
        hideSpinner(element);
      })
      .catch(() => {
        hideSpinner(element);
      });
  }

  render() {
    return (
      <div
        style={{
          marginLeft: 10,
          marginRight: 60,
          marginTop: 20,
          marginBottom: 20,
        }}
        id="container"
      >
        <Fragment>
          <Form>
            <Row style={{ textAlign: "right" }}>
              <Col xs="12">
                <FormGroup>
                  <Label htmlFor="active" style={{ verticalAlign: "bottom" }}>
                    Activo&nbsp;
                  </Label>
                  <AppSwitch
                    className={"mx-1 mt-4"}
                    variant={"pill"}
                    color={"primary"}
                    label
                    checked={this.state.active}
                    id="active"
                    name="active"
                    placeholder="Activo"
                    onChange={this.handleInputChange}
                    dataOn="Si"
                    dataOff="No"
                  />
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col xs="4">
                <FormGroup>
                  <Label htmlFor="name">Nombre</Label>
                  <Input
                    type="text"
                    id="name"
                    name="name"
                    placeholder="nombre"
                    required
                    value={this.state.name}
                    onChange={this.handleInputChange}
                  />
                </FormGroup>
              </Col>
              <Col xs="4">
                <FormGroup>
                  <Label htmlFor="cif">CIF</Label>
                  <Input
                    type="text"
                    id="cif"
                    name="cif"
                    placeholder="cif"
                    value={this.state.cif || ""}
                    onChange={this.handleInputChange}
                  />
                </FormGroup>
              </Col>
              <Col xs="4">
                <FormGroup>
                  <Label htmlFor="address">Dirección</Label>
                  <Input
                    type="text"
                    id="address"
                    name="address"
                    placeholder="dirección"
                    required
                    value={this.state.address || ""}
                    onChange={this.handleInputChange}
                  />
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col xs="3">
                <FormGroup>
                  <Label htmlFor="name">Teléfono</Label>
                  <Input
                    type="text"
                    id="phoneNumber"
                    name="phoneNumber"
                    placeholder="teléfono"
                    required
                    value={this.state.phoneNumber}
                    onChange={this.handleInputChange}
                  />
                </FormGroup>
              </Col>
              <Col xs="3">
                <FormGroup>
                  <Label htmlFor="wayToPay">Modo de Pago</Label>
                  <Input
                    type="text"
                    id="wayToPay"
                    name="wayToPay"
                    placeholder="modo de pago"
                    value={this.state.wayToPay || ""}
                    onChange={this.handleInputChange}
                  />
                </FormGroup>
              </Col>
              <Col xs="3">
                <FormGroup>
                  <Label htmlFor="expirationDays">Vencimiento</Label>
                  <NumericTextBoxComponent
                    format="N0"
                    id="expirationDays"
                    name="expirationDays"
                    value={this.state.expirationDays}
                    min={0}
                    showSpinButton={false}
                    placeholder="vencimiento"
                    change={this.handleChangeExpirationDays}
                    ref={(g) => (this.ntbPercentageRetention = g)}
                  />
                </FormGroup>
              </Col>
              <Col xs="3">
                <FormGroup>
                  <Label htmlFor="accountNumber">Nº de Cuenta</Label>
                  <MaskedTextBoxComponent
                    mask={"LL00-0000-0000-0000-0000-0000"}
                    id="accountNumber"
                    name="accountNumber"
                    placeholder="nº de cuenta"
                    value={this.state.accountNumber || ""}
                    onChange={this.handleInputChange}
                  />
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col xs="6">
                <FormGroup>
                  <Label htmlFor="email">Email</Label>
                  <Input
                    type="text"
                    id="email"
                    name="email"
                    placeholder="email cliente"
                    value={this.state.email || ""}
                    onChange={this.handleInputChange}
                  />
                </FormGroup>
              </Col>
              <Col xs="6">
                <FormGroup>
                  <Label htmlFor="emailInvoice">Email Facturación</Label>
                  <Input
                    type="text"
                    id="emailInvoice"
                    name="emailInvoice"
                    placeholder="email facturación"
                    value={this.state.emailInvoice || ""}
                    onChange={this.handleInputChange}
                  />
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col xs="12" style={{ marginTop: "20px", textAlign: "right" }}>
                <div className="form-actions">
                  <Button color="primary" onClick={this.handleSubmit}>
                    Guardar
                  </Button>
                </div>
              </Col>
            </Row>
          </Form>
        </Fragment>
      </div>
    );
  }
}

BasicDataClient.propTypes = {};

export default BasicDataClient;
