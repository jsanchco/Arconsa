import React, { Component, Fragment } from "react";
import { Form, Col, FormGroup, Input, Label, Row, Button } from "reactstrap";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import {
  createSpinner,
  showSpinner,
  hideSpinner
} from "@syncfusion/ej2-popups";
import { getSettings, updateSettings } from "../../services";
import { COMPANY_DATA } from "../../constants";

class CompanyData extends Component {
  constructor(props) {
    super(props);

    this.state = {
      companyName: "",
      cif: "",
      address: "",
      phoneNumber: ""
    };

    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.getCompanyData = this.getCompanyData.bind(this);
  }

  componentDidMount() {
    const element = document.getElementById("container");

    createSpinner({
      target: element
    });
    showSpinner(element);
    getSettings(COMPANY_DATA)
      .then(result => {
        const data = JSON.parse(result.data);
        this.setState({
          companyName: data.companyName,
          cif: data.cif,
          address: data.address,
          phoneNumber: data.phoneNumber
        });
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

  getCompanyData() {
    return {
      companyName: this.state.companyName,
      cif: this.state.cif,
      address: this.state.address,
      phoneNumber: this.state.phoneNumber
    };
  }

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;

    this.setState({
      [name]: target.value
    });
  }

  handleSubmit() {
    const element = document.getElementById("container");

    createSpinner({
      target: element
    });
    showSpinner(element);
    updateSettings(this.getCompanyData())
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
                        <Label htmlFor="name">Nombre de la Empresa</Label>
                        <Input
                          type="text"
                          id="companyName"
                          name="companyName"
                          placeholder="nombre de la empresa"
                          required
                          value={this.state.companyName}
                          onChange={this.handleInputChange}
                        />
                      </FormGroup>
                    </Col>
                    <Col xs="3">
                      <FormGroup>
                        <Label htmlFor="cif">CIF</Label>
                        <Input
                          type="text"
                          id="cif"
                          name="cif"
                          placeholder="cif"
                          required
                          value={this.state.cif}
                          onChange={this.handleInputChange}
                        />
                      </FormGroup>
                    </Col>
                    <Col xs="3">
                      <FormGroup>
                        <Label htmlFor="address">Dirección</Label>
                        <Input
                          type="text"
                          id="address"
                          name="address"
                          placeholder="dirección"
                          required
                          value={this.state.address}
                          onChange={this.handleInputChange}
                        />
                      </FormGroup>
                    </Col>
                    <Col xs="3">
                      <FormGroup>
                        <Label htmlFor="phoneNumber">Teléfono</Label>
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
                  </Row>
                  <Row>
                    <Col
                      xs="12"
                      style={{ marginTop: "20px", textAlign: "right" }}
                    >
                      <div className="form-actions">
                        <Button color="primary" onClick={this.handleSubmit}>
                          Guardar
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

CompanyData.propTypes = {};

const mapStateToProps = state => {
  return {
    errorApplication: state.applicationReducer.error
  };
};

const mapDispatchToProps = dispatch => ({
  showMessage: message => dispatch(ACTION_APPLICATION.showMessage(message))
});

export default connect(mapStateToProps, mapDispatchToProps)(CompanyData);
