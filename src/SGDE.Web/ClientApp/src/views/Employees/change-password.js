import React, { Component, Fragment } from "react";
import {
  Button,
  Card,
  CardBody,
  CardGroup,
  Col,
  Container,
  Form,
  Input,
  InputGroup,
  InputGroupAddon,
  InputGroupText,
  Row
} from "reactstrap";
import { updatePassword } from "../../services";
import {
  createSpinner,
  showSpinner,
  hideSpinner
} from "@syncfusion/ej2-popups";

class ChangePassword extends Component {
  constructor(props) {
    super(props);

    this.state = {
      password: "",
      newPassword: "",
      repeatPassword: ""
    };

    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;

    this.setState({
      [name]: target.value
    });
  }

  handleSubmit() {
    if (this.state.newPassword !== this.state.repeatPassword) {
      this.props.showMessage({
        statusText: "Deben coincidir la nueva contraseña con la repetida",
        responseText: "Deben coincidir la nueva contraseña con la repetida",
        type: "danger"
      });
    } else {
      const element = document.getElementById("container");

      createSpinner({
        target: element
      });
      showSpinner(element);
      updatePassword({
        id: this.props.user.id,
        password: this.state.password,
        newPassword: this.state.newPassword
      })
        .then(() => {
          hideSpinner(element);
        })
        .catch(() => {
          hideSpinner(element);
        });
    }
  }

  render() {
    return (
      <Fragment>
        {/* <div
          className="app flex-row align-items-center"
          style={{ marginTop: "-200px", marginBottom: "-150px" }}
        > */}
          <Container  style={{ marginTop: "100px", marginBottom: "100px" }}>
            <Row className="justify-content-center">
              <Col md="5">
                <CardGroup>
                  <Card className="p-4">
                    <CardBody>
                      <Form>
                        <h1>Cambiar Contraseña</h1>
                        <p className="text-muted"></p>
                        <InputGroup className="mb-3">
                          <InputGroupAddon addonType="prepend">
                            <InputGroupText>
                              <i className="icon-lock"></i>
                            </InputGroupText>
                          </InputGroupAddon>
                          <Input
                            id="password"
                            name="password"
                            type="password"
                            placeholder="Contraseña anterior"
                            autoComplete="password"
                            value={this.state.password}
                            onChange={this.handleInputChange}
                          />
                        </InputGroup>
                        <InputGroup className="mb-4">
                          <InputGroupAddon addonType="prepend">
                            <InputGroupText>
                              <i className="icon-lock"></i>
                            </InputGroupText>
                          </InputGroupAddon>
                          <Input
                            id="newPassword"
                            name="newPassword"
                            type="password"
                            placeholder="Contraseña nueva"
                            autoComplete="newPassword"
                            value={this.state.newPassword}
                            onChange={this.handleInputChange}
                          />
                        </InputGroup>
                        <InputGroup className="mb-4">
                          <InputGroupAddon addonType="prepend">
                            <InputGroupText>
                              <i className="icon-lock"></i>
                            </InputGroupText>
                          </InputGroupAddon>
                          <Input
                            id="repeatPassword"
                            name="repeatPassword"
                            type="password"
                            placeholder="Repetir contraseña"
                            autoComplete="repeatPassword"
                            value={this.state.repeatPassword}
                            onChange={this.handleInputChange}
                          />
                        </InputGroup>
                        <Row>
                          <Col xs="6">
                            <Button
                              color="primary"
                              className="px-4"
                              onClick={() => this.handleSubmit()}
                            >
                              Cambiar
                            </Button>
                          </Col>
                        </Row>
                      </Form>
                    </CardBody>
                  </Card>
                </CardGroup>
              </Col>
            </Row>
          </Container>
        {/* </div> */}
      </Fragment>
    );
  }
}

ChangePassword.propTypes = {};

export default ChangePassword;
