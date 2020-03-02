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
import { login, logout } from "../../../services";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../../actions/applicationAction";
import ACTION_AUTHENTICATION from "../../../actions/authenticationAction";
import ReactNotification, { store } from "react-notifications-component";
import "react-notifications-component/dist/theme.css";

class Login extends Component {
  constructor(props) {
    super(props);

    this.state = {
      username: "",
      password: ""
    };

    this.handleUsernameChange = this.handleUsernameChange.bind(this);
    this.handlePasswordChange = this.handlePasswordChange.bind(this);
  }

  handleUsernameChange(e) {
    this.setState({ username: e.target.value });
  }

  handlePasswordChange(e) {
    this.setState({ password: e.target.value });
  }

  handleLogin() {
    login(this.state.username, this.state.password, this.props.history);
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
                        <h1>Login</h1>
                        <p className="text-muted">Inicia sesión en tu cuenta</p>
                        <InputGroup className="mb-3">
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
                        <InputGroup className="mb-4">
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
                        <Row>
                          <Col xs="6">
                            <Button
                              color="primary"
                              className="px-4"
                              onClick={() => this.handleLogin()}
                            >
                              Entrar
                            </Button>
                          </Col>
                          {/* <Col xs="6" className="text-right">
                            <Button color="link" className="px-0">
                              Forgot password?
                            </Button>
                          </Col> */}
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
