import React, { Component, Fragment } from "react";
import { Form, Col, FormGroup, Input, Label, Row, Button } from "reactstrap";
import { DatePickerComponent } from "@syncfusion/ej2-react-calendars";
import { updateWork } from "../../services";
import {
  createSpinner,
  showSpinner,
  hideSpinner
} from "@syncfusion/ej2-popups";

class BasicDataWork extends Component {
  constructor(props) {
    super(props);

    this.state = {
      id: props.work.id,
      name: props.work.name,
      address: props.work.address,
      worksToRealize: props.work.worksToRealize,
      numberPersonsRequested: props.work.numberPersonsRequested,
      estimatedDuration: props.work.estimatedDuration,
      openDate: props.work.openDate,
      closeDate: props.work.closeDate,
      open: props.work.open,
      clientId: props.work.clientId
    };

    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.getWork = this.getWork.bind(this);
    this.formatDate = this.formatDate.bind(this);
  }

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;

    if (name === "openDate" || name === "closeDate") {
      this.setState({
        [name]: this.formatDate(target.value)
      });
    } else {
      this.setState({
        [name]: target.value
      });
    }
  }

  formatDate(args) {
    if (args === null || args === "") {
      return "";
    }

    let day = args.getDate();
    if (day < 10)
      day = "0" + day;

    const month = args.getMonth() + 1;
    const year = args.getFullYear();

    if (month < 10) {
      return `${day}/0${month}/${year}`;
    } else {
      return `${day}/${month}/${year}`;
    }
  }

  getWork() {
    return {
      id: this.state.id,
      name: this.state.name,
      address: this.state.address,
      worksToRealize: this.state.worksToRealize,
      numberPersonsRequested: this.state.numberPersonsRequested,
      estimatedDuration: this.state.estimatedDuration,
      openDate: this.state.openDate,
      closeDate: this.state.closeDate,
      open: this.state.open,
      clientId: this.state.clientId
    };
  }

  handleSubmit() {
    const element = document.getElementById("container");

    createSpinner({
      target: element
    });
    showSpinner(element);
    updateWork(this.getWork())
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
                  <Label htmlFor="address">Address</Label>
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
              <Col xs="4">
                <FormGroup>
                  <Label htmlFor="worksToRealize">Tipo de Trabajo</Label>
                  <Input
                    type="text"
                    id="worksToRealize"
                    name="worksToRealize"
                    placeholder="tipo de trabajo"
                    value={this.state.worksToRealize || ""}
                    onChange={this.handleInputChange}
                  />
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col xs="2">
                <FormGroup>
                  <Label htmlFor="numberPersonsRequested">Nº Personas</Label>
                  <Input
                    type="number"
                    id="numberPersonsRequested"
                    name="numberPersonsRequested"
                    placeholder="número de personas"
                    value={this.state.numberPersonsRequested}
                    onChange={this.handleInputChange}
                  />
                </FormGroup>
              </Col>
              <Col xs="2">
                <FormGroup>
                  <Label htmlFor="estimatedDuration">T. Estimado</Label>
                  <Input
                    type="text"
                    id="estimatedDuration"
                    name="estimatedDuration"
                    placeholder="tiempo estimado"
                    value={this.state.estimatedDuration}
                    onChange={this.handleInputChange}
                  />
                </FormGroup>
              </Col>              
              <Col xs="4">
                <FormGroup>
                  <Label htmlFor="openDate">Fecha de Apertura de Obra</Label>
                  <DatePickerComponent
                    id="openDate"
                    name="openDate"
                    placeholder="fecha de apertura"
                    required
                    format="dd/MM/yyyy"
                    value={this.state.openDate}
                    onChange={this.handleInputChange}
                  />
                </FormGroup>
              </Col>
              <Col xs="4">
                <FormGroup>
                  <Label htmlFor="address">Fecha de Cierre de Obra</Label>
                  <DatePickerComponent
                    id="closeDate"
                    name="closeDate"
                    placeholder="fecha de cierre"
                    format="dd/MM/yyyy"
                    value={this.state.closeDate}
                    onChange={this.handleInputChange}
                  />
                </FormGroup>
              </Col>
            </Row>
            {/* <Row>
              <Col xs="12">
                <FormGroup>
                  <Label htmlFor="name">Trabajos a Realizar</Label>
                  <Input
                    type="text"
                    id="worksToRealize"
                    name="worksToRealize"
                    placeholder="trabajos a realizar"
                    required
                    value={this.state.worksToRealize}
                    onChange={this.handleInputChange}
                  />
                </FormGroup>
              </Col>
            </Row> */}
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

BasicDataWork.propTypes = {};

export default BasicDataWork;
