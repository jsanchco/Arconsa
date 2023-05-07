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
import { login, logout, getEnterprisesByUserId } from "../../../services";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../../actions/applicationAction";
import ACTION_AUTHENTICATION from "../../../actions/authenticationAction";
import ReactNotification, { store } from "react-notifications-component";
import { DropDownListComponent } from "@syncfusion/ej2-react-dropdowns";
import "react-notifications-component/dist/theme.css";

class Login extends Component {
  constructor(props) {
    super(props);

    this.ddl = null;
    this.fields = { text: "alias", value: "id" };

    this.state = {
      username: "",
      password: ""
    };

    this.handleUsernameChange = this.handleUsernameChange.bind(this);
    this.handlePasswordChange = this.handlePasswordChange.bind(this);
    this.handleSelectEnterprise = this.handleSelectEnterprise.bind(this);
  }

  handleUsernameChange(e) {
    this.setState({ username: e.target.value });
  }

  handlePasswordChange(e) {
    this.setState({ password: e.target.value });
  }

  handleLogin(e) {
    e.preventDefault();

    login(this.state.username, this.state.password, this.props.history)
      .then((result) => {
        const elementBtnLogin = document.getElementById("btn-login");
        const elementBtnRegresar = document.getElementById("btn-regresar");
        const elementIniciaSesion = document.getElementById("label-inicia-sesion");
        const elementLogin = document.getElementById("label-login");
        const elementEmpresas = document.getElementById("label-empresas");
        const elementUser = document.getElementById("select-user");
        const elementPassword = document.getElementById("select-password");
        const elementEnterprise = document.getElementById("select-enterprise");

        switch (result.status) {
          case "LOGIN_OK":            
            elementBtnLogin.style.display = "none";
            elementBtnRegresar.style.display = "";
            elementIniciaSesion.style.display = "none";
            elementLogin.style.display = "none";
            elementEmpresas.style.display = "";
            elementUser.style.display = "none";
            elementPassword.style.display = "none";
            elementEnterprise.style.display = "";

            getEnterprisesByUserId(result.result.user.id)
            .then((result) => {
              if (result.length === 1) {
                const enterprise = {
                  id: result[0].id,
                  alias: result[0].alias,
                  name: result[0].name
                };
                localStorage.setItem("enterprise", JSON.stringify(enterprise));
                this.props.history.push("/dashboard");
              } else {
                this.ddl.dataSource = result;
              }
            })
            .catch((error) => {
              store.addNotification({
                message: error,
                type: "danger",
                container: "bottom-center",
                animationIn: ["animated", "fadeIn"],
                animationOut: ["animated", "fadeOut"],
                dismiss: {
                  duration: 5000,
                  showIcon: true
                },
                width: 800
              });
            });

            break;

          case "LOGIN_KO":
            elementBtnLogin.style.display = "";
            elementBtnRegresar.style.display = "none";
            elementIniciaSesion.style.display = "";
            elementLogin.style.display = "";
            elementEmpresas.style.display = "none";
            elementUser.style.display = "";
            elementPassword.style.display = "";
            elementEnterprise.style.display = "none";
            break;            

          default:
            elementBtnLogin.style.display = "";
            elementBtnRegresar.style.display = "none";
            elementIniciaSesion.style.display = "";
            elementLogin.style.display = "";
            elementEmpresas.style.display = "none";
            elementUser.style.display = "";
            elementPassword.style.display = "";
            elementEnterprise.style.display = "none";
            break;  
        }
    });  
  }

  handleRegresar(e) {
    const elementBtnLogin = document.getElementById("btn-login");    
    const elementBtnRegresar = document.getElementById("btn-regresar");    
    const elementIniciaSesion = document.getElementById("label-inicia-sesion");
    const elementLogin = document.getElementById("label-login");
    const elementEmpresas = document.getElementById("label-empresas");
    const elementUser = document.getElementById("select-user");
    const elementPassword = document.getElementById("select-password");
    const elementEnterprise = document.getElementById("select-enterprise"); 
    
    elementBtnLogin.style.display = "";
    elementBtnRegresar.style.display = "none";
    elementIniciaSesion.style.display = "";
    elementLogin.style.display = "";
    elementEmpresas.style.display = "none";
    elementUser.style.display = "";
    elementPassword.style.display = "";
    elementEnterprise.style.display = "none";    
  }

  handleSelectEnterprise(e) {
    const enterprise = {
      id: e.itemData.id,
      alias: e.itemData.alias,
      name: e.itemData.name
    };
    localStorage.setItem("enterprise", JSON.stringify(enterprise));
    this.props.history.push("/dashboard");
  }

  componentDidMount() {
    logout();
  }

  componentDidUpdate(prevProps) {
    if (
      this.props.messageApplication != null &&
      prevProps.messageApplication !== this.props.messageApplication
    ) {
      this.showMessage();
    }
  }

  showMessage() {
    let message;
    const { statusText, responseText } = this.props.messageApplication;
    if (this.props.messageApplication.type === "danger") {
      if (typeof statusText === "string" || statusText instanceof String) {
        message = statusText;
        console.log("error ->", statusText);
      } else {
        message = statusText.message;
        console.log("error ->", statusText);
      }
    } else {
      if (typeof responseText === "string" || responseText instanceof String) {
        message = responseText;
        console.log("error ->", responseText);
      } else {
        message = responseText.message;
        console.log("error ->", responseText);
      }
    }

    store.addNotification({
      message: message,
      type: this.props.messageApplication.type,
      container: "bottom-center",
      animationIn: ["animated", "fadeIn"],
      animationOut: ["animated", "fadeOut"],
      dismiss: {
        duration: 5000,
        showIcon: true
      },
      width: 800
    });
  }

  render() {
    return (
      <Fragment>
        <ReactNotification />
        <div className="app flex-row align-items-center">
          <Container>
            <Row className="justify-content-center">
              <Col md="4">
                <CardGroup>
                  <Card className="p-4">
                    <CardBody>
                      <Form>
                        <h1 id="label-login" style={{ display: "" }}>Login</h1>
                        <h1 id="label-empresas" style={{ display: "none" }}>Empresas</h1>
                        <p className="text-muted" id="label-inicia-sesion">Inicia sesión en tu cuenta</p>
                        <InputGroup 
                          className="mb-3"
                          id="select-user">
                          <InputGroupAddon addonType="prepend">
                            <InputGroupText>
                              <i className="icon-user"></i>
                            </InputGroupText>
                          </InputGroupAddon>
                          <Input
                            type="text"
                            placeholder="Usuario"
                            autoComplete="username"
                            value={this.state.username}
                            onChange={this.handleUsernameChange}
                          />
                        </InputGroup>
                        <InputGroup 
                          className="mb-4"
                          id="select-password">
                          <InputGroupAddon addonType="prepend">
                            <InputGroupText>
                              <i className="icon-lock"></i>
                            </InputGroupText>
                          </InputGroupAddon>
                          <Input
                            type="password"
                            placeholder="Contraseña"
                            autoComplete="current-password"
                            value={this.state.password}
                            onChange={this.handlePasswordChange}
                          />
                        </InputGroup>
                        <InputGroup 
                          className="mb-4" 
                          id="select-enterprise"
                          style={{ display: "none" }}>
                          <p>&nbsp;</p>
                          <DropDownListComponent
                            dataSource={null}
                            fields={this.fields}
                            placeholder={`Selecciona empresa`}
                            ref={(g) => (this.ddl = g)}
                            select={this.handleSelectEnterprise.bind(this)}
                            // popupWidth="auto"
                          />                          
                        </InputGroup>                        
                        <Row>
                          <Col xs="6">
                            <Button
                              color="primary"
                              className="px-4"
                              onClick={(e) => this.handleLogin(e)}
                              id="btn-login"
                            >
                              Entrar
                            </Button>
                          </Col>
                          <Col xs="6" className="text-right">
                            <Button 
                              color="link" 
                              className="px-0"
                              id="btn-regresar"
                              onClick={(e) => this.handleRegresar(e)}
                              style={{ display: "none" }}>
                              Regresar
                            </Button>
                          </Col>
                        </Row>
                      </Form>
                    </CardBody>
                  </Card>
                  {/* <Card
                    className="text-white bg-primary py-5 d-md-down-none"
                    style={{ width: "44%" }}
                  >
                    <CardBody className="text-center">
                      <div>
                        <h2>Sign up</h2>
                        <p>
                          Lorem ipsum dolor sit amet, consectetur adipisicing
                          elit, sed do eiusmod tempor incididunt ut labore et
                          dolore magna aliqua.
                        </p>
                        <Link to="/register">
                          <Button
                            color="primary"
                            className="mt-3"
                            active
                            tabIndex={-1}
                          >
                            Register Now!
                          </Button>
                        </Link>
                      </div>
                    </CardBody>
                  </Card> */}
                </CardGroup>
              </Col>
            </Row>
          </Container>
        </div>
      </Fragment>
    );
  }
}

const mapStateToProps = state => {
  return {
    user: state.authenticationReducer.user,
    isAuthenticated: state.authenticationReducer.isAuthenticated,
    messageApplication: state.applicationReducer.message
  };
};

const mapDispatchToProps = dispatch => ({
  showMessage: message => dispatch(ACTION_APPLICATION.showMessage(message)),
  logIn: (username, password) =>
    dispatch(ACTION_AUTHENTICATION.logIn(username, password)),
  logOut: () => dispatch(ACTION_AUTHENTICATION.logOut())
});

export default connect(mapStateToProps, mapDispatchToProps)(Login);
