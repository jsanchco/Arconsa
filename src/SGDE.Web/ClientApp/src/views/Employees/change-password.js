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
import { DialogComponent } from "@syncfusion/ej2-react-popups";
import { restorePassword } from "../../services";

class ChangePassword extends Component {
  constructor(props) {
    super(props);

    this.state = {
      password: "",
      newPassword: "",
      repeatPassword: "",
      hideConfirmDialog: false
    };

    this.confirmButton = [
      {
        click: () => {
          this.setState({ hideConfirmDialog: false });
          restorePassword(this.props.user.id);
        },
        buttonModel: { content: "Si", isPrimary: true }
      },
      {
        click: () => {
          this.setState({ hideConfirmDialog: false });
        },
        buttonModel: { content: "No" }
      }
    ];

    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleRestorePassword = this.handleRestorePassword.bind(this);
    this.dialogClose = this.dialogClose.bind(this);
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

  handleRestorePassword() {
    this.setState({ hideConfirmDialog: true });
  }

  dialogClose() {
    this.setState({
      hideConfirmDialog: false
    });
  }

  render() {
    return (
      <Fragment>
        <DialogComponent
          id="confirmDialog"
          header="Restablcer Contraseña"
          visible={this.state.hideConfirmDialog}
          showCloseIcon={true}
          animationSettings={this.animationSettings}
          width="500px"
          content="¿Estás seguro de deseas restablecer la contraseña a sus valores originales?"
          ref={dialog => (this.confirmDialogInstance = dialog)}
          target="#target-change-password"
          buttons={this.confirmButton}
          close={this.dialogClose}
        />

        <Container style={{ marginTop: "100px", marginBottom: "100px" }} id="target-change-password">
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
                            onClick={() => this.handleRestorePassword()}
                          >
                            Restablecer
                          </Button>
                        </Col>
                        <Col xs="6" className="text-right">
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
