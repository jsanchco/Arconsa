import React, { Component, Fragment } from "react";
import { Form, Col, FormGroup, Input, Label, Row, Button } from "reactstrap";
import { DatePickerComponent } from "@syncfusion/ej2-react-calendars";
import { DropDownListComponent } from "@syncfusion/ej2-react-dropdowns";
import { getWorks } from "../../services";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import {
  createSpinner,
  showSpinner,
  hideSpinner
} from "@syncfusion/ej2-popups";
import { getInvoice } from "../../services";

class Invoices extends Component {
  constructor(props) {
    super(props);

    this.state = {
      invoiceNumber: "",
      startDate: null,
      endDate: null,
      issueDate: null,
      typeInvoice: 1,
      clientId: null,
      workId: null
    };

    this.ddl = null;
    this.fields = { text: "name", value: "id" };
    this.dataSource = getWorks().then(items => {
      this.ddl.dataSource = items;
    });

    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.formatDate = this.formatDate.bind(this);
    this.getDataInvoice = this.getDataInvoice.bind(this);
  }

  getDataInvoice() {
    return {
      invoiceNumber: this.state.invoiceNumber,
      startDate: this.state.startDate,
      endDate: this.state.endDate,
      issueDate: this.state.issueDate,
      typeInvoice: this.state.typeInvoice,
      clientId: this.state.clientId
    };
  }

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;

    if (name === "startDate" || name === "endDate" || name === "issueDate") {
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

    const day = args.getDate();
    const month = args.getMonth() + 1;
    const year = args.getFullYear();

    if (month < 10) {
      return `${day}/0${month}/${year}`;
    } else {
      return `${day}/${month}/${year}`;
    }
  }

  handleSubmit() {
    let error = false;
    if (this.state.startDate === null || this.state.startDate === undefined) {
      error = true;
    }
    if (this.state.endDate === null || this.state.endDate === undefined) {
      error = true;
    }
    if (this.state.startDate > this.state.endDate) {
      error = true;
    }

    if (error === true) {
      this.props.showMessage({
        statusText: "Fechas mal configuradas",
        responseText: "Fechas mal configuradas",
        type: "danger"
      });
    }

    const element = document.getElementById("container");

    createSpinner({
      target: element
    });
    showSpinner(element);
    getInvoice(this.getDataInvoice())
      .then(() => {
        hideSpinner(element);
      })
      .catch(error => {
        this.props.showMessage({
          statusText: error,
          responseText: error,
          type: "danger"
        });
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
          <div className="animated fadeIn">
            <div className="card">
              <div className="card-header">
                <i className="cui-file"></i>
                Facturas
              </div>
              <div className="card-body">
                <Form>
                  <Row>
                    <Col xs="3">
                      <FormGroup>
                        <Label htmlFor="name">Nº Factura</Label>
                        <Input
                          type="text"
                          id="invoiceNumber"
                          name="invoiceNumber"
                          placeholder="número de factura"
                          required
                          value={this.state.name}
                          onChange={this.handleInputChange}
                        />
                      </FormGroup>
                    </Col>
                    <Col xs="3">
                      <FormGroup>
                        <Label htmlFor="name">Obra</Label>
                        <DropDownListComponent
                          id="workId"
                          name="workId"
                          dataSource={this.dataSource}
                          fields={this.fields}
                          placeholder="selecciona obra"
                          onChange={this.handleInputChange}
                          ref={g => (this.ddl = g)}
                        />
                      </FormGroup>
                    </Col>
                    <Col xs="2">
                      <FormGroup>
                        <Label htmlFor="startDate">Fecha de Inicio</Label>
                        <DatePickerComponent
                          id="startDate"
                          name="startDate"
                          placeholder="fecha de inicio"
                          format="dd/MM/yyyy"
                          value={this.state.startDate}
                          onChange={this.handleInputChange}
                        />
                      </FormGroup>
                    </Col>
                    <Col xs="2">
                      <FormGroup>
                        <Label htmlFor="endDate">Fecha Final</Label>
                        <DatePickerComponent
                          id="endDate"
                          name="endDate"
                          placeholder="fecha final"
                          format="dd/MM/yyyy"
                          value={this.state.endDate}
                          onChange={this.handleInputChange}
                        />
                      </FormGroup>
                    </Col>
                    <Col xs="2">
                      <FormGroup>
                        <Label htmlFor="issueDate">Fecha de Emisión</Label>
                        <DatePickerComponent
                          id="issueDate"
                          name="issueDate"
                          placeholder="fecha de emisión"
                          format="dd/MM/yyyy"
                          value={this.state.issueDate}
                          onChange={this.handleInputChange}
                        />
                      </FormGroup>
                    </Col>
                  </Row>
                  <Row>
                    <Col
                      xs="12"
                      style={{ marginTop: "20px", textAlign: "right" }}
                    >
                      <div className="form-actions">
                        <Button color="primary" onClick={this.handleSubmit}>
                          Generar Factura
                        </Button>
                      </div>
                    </Col>
                  </Row>
                </Form>
              </div>
            </div>
          </div>
        </Fragment>
      </div>
    );
  }
}

Invoices.propTypes = {};

const mapStateToProps = state => {
  return {
    errorApplication: state.applicationReducer.error
  };
};

const mapDispatchToProps = dispatch => ({
  showMessage: message => dispatch(ACTION_APPLICATION.showMessage(message))
});

export default connect(mapStateToProps, mapDispatchToProps)(Invoices);
