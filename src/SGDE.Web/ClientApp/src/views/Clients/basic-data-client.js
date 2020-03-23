import React, { Component, Fragment } from "react";
import { Form, Col, FormGroup, Input, Label, Row, Button } from "reactstrap";
import { updateClient } from "../../services";
import {
  createSpinner,
  showSpinner,
  hideSpinner
} from "@syncfusion/ej2-popups";

class BasicDataClient extends Component {
  constructor(props) {
    super(props);

    this.state = {
      id: props.client.id,
      name: props.client.name,
      cif: props.client.cif,
      phoneNumber: props.client.phoneNumber,
      address: props.client.address,
      wayToPay: props.client.wayToPay,
      accountNumber: props.client.accountNumber
    };

    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.getClient = this.getClient.bind(this);
  }

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;

    this.setState({
      [name]: target.value
    });
  }

  getClient() {
    return {
      id: this.state.id,
      name: this.state.name,
      cif: this.state.cif,
      phoneNumber: this.state.phoneNumber,
      address: this.state.address,
      wayToPay: this.state.wayToPay,
      accountNumber: this.state.accountNumber
    };
  }

  handleSubmit() {
    const element = document.getElementById("container");

    createSpinner({
      target: element
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
          marginBottom: 20
        }}
        id="container"
      >
        <Fragment>
          <Form>
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
              <Col xs="4">
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
              <Col xs="4">
                <FormGroup>
                  <Label htmlFor="wayToPay">Modo de Pago</Label>
                  <Input
                    type="text"
                    id="wayToPay"
                    name="wayToPay"
                    placeholder="modo de ago"
                    value={this.state.wayToPay || ""}
                    onChange={this.handleInputChange}
                  />
                </FormGroup>
              </Col>
              <Col xs="4">
                <FormGroup>
                  <Label htmlFor="accountNumber">Nº de Cuenta</Label>
                  <Input
                    type="text"
                    id="accountNumber"
                    name="accountNumber"
                    placeholder="número de cuenta"
                    required
                    value={this.state.accountNumber || ""}
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
