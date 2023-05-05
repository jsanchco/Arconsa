import React, { Component } from "react";
import {
  Form,
  Col,
  FormGroup,
  Input,
  Label,
  Row,
  Button,
} from "reactstrap";
import {
  createSpinner,
  showSpinner,
  hideSpinner,
} from "@syncfusion/ej2-popups";
import { getEnterprise, updateEnterprise } from "../../services";

class BasicDataCompany extends Component {
  constructor(props) {
    super(props);

    this.state = {
      id: JSON.parse(localStorage.getItem("enterprise")).id,
      name: "",
      alias: "",
      cif: "",
      address: "",
      phoneNumber: "",
    };

    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.getCompanyData = this.getCompanyData.bind(this);
  }

  componentDidMount() {
    const element = document.getElementById("container");

    createSpinner({
      target: element,
    });
    showSpinner(element);

    const enterpriseId = JSON.parse(localStorage.getItem("enterprise")).id;
    getEnterprise(enterpriseId)
      .then((result) => {
        this.setState({
          companyName: result.name,
          alias: result.alias,
          cif: result.cif,
          address: result.address,
          phoneNumber: result.phoneNumber,
        });
        hideSpinner(element);
      })
      .catch((error) => {
        this.props.showMessage({
          statusText: error,
          responseText: error,
          type: "danger",
        });
        hideSpinner(element);
      });

    // getSettings(COMPANY_DATA)
    //   .then((result) => {
    //     const data = JSON.parse(result.data);
    //     this.setState({
    //       companyName: data.companyName,
    //       cif: data.cif,
    //       address: data.address,
    //       phoneNumber: data.phoneNumber,
    //     });
    //     hideSpinner(element);
    //   })
    //   .catch((error) => {
    //     this.props.showMessage({
    //       statusText: error,
    //       responseText: error,
    //       type: "danger",
    //     });
    //     hideSpinner(element);
    //   });
  }

  getCompanyData() {
    return {
      id: this.state.id,
      name: this.state.companyName,
      alias: this.state.alias,
      cif: this.state.cif,
      address: this.state.address,
      phoneNumber: this.state.phoneNumber,
    };
  }

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;

    this.setState({
      [name]: target.value,
    });
  }

  handleSubmit() {
    const element = document.getElementById("container");

    createSpinner({
      target: element,
    });
    showSpinner(element);
    updateEnterprise(this.getCompanyData())
      .then(() => {
        hideSpinner(element);
      })
      .catch((error) => {
        this.props.showMessage({
          statusText: error,
          responseText: error,
          type: "danger",
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
          marginBottom: 20,
        }}
        id="container"
      >
        <Form>
          <Row>
            <Col xs="3">
              <FormGroup>
                <Label htmlFor="name">Nombre de la Empresa</Label>
                <Input
                  type="text"
                  id="name"
                  name="name"
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
            <Col xs="3">
              <FormGroup>
                <Label htmlFor="alias">Nombre corto</Label>
                <Input
                  type="text"
                  id="alias"
                  name="alias"
                  placeholder="nombre corto"
                  required
                  value={this.state.alias}
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
      </div>
    );
  }
}

BasicDataCompany.propTypes = {};

export default BasicDataCompany;
