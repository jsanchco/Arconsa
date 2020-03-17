import React, { Component, Fragment } from "react";
import { Form, Col, FormGroup, Input, Label, Row, Button } from "reactstrap";
import { updateUser } from "../../services";
import ModalSelectImage from "../Modals/modal-select-image";
import {
  createSpinner,
  showSpinner,
  hideSpinner
} from "@syncfusion/ej2-popups";

class BasicData extends Component {
  constructor(props) {
    super(props);

    this.state = {
      id: props.user.id,
      name: props.user.name,
      surname: props.user.surname,
      dni: props.user.dni,
      username: props.user.username,
      address: props.user.address,
      phoneNumber: props.user.phoneNumber,
      priceHour: props.user.priceHour,
      priceHourSale: props.user.priceHourSale,
      observations: props.user.observations,
      photo: props.user.photo,
      roleId: props.user.roleId,
      professionId: props.user.professionId,
      workId: props.user.workId,
      clientId: props.user.clientId,
      modal: false
    };

    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleCancel = this.handleCancel.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
    this.updatePhoto = this.updatePhoto.bind(this);
    this.getUser = this.getUser.bind(this);
  }

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;

    this.setState({
      [name]: target.value
    });
  }

  getUser() {
    return {
      id: this.state.id,
      name: this.state.name,
      surname: this.state.surname,
      dni: this.state.dni,
      username: this.state.username,
      address: this.state.address,
      phoneNumber: this.state.phoneNumber,
      priceHour: this.state.priceHour,
      priceHourSale: this.state.priceHourSale,
      observations: this.state.observations,
      photo: this.state.photo,
      roleId: this.state.roleId,
      professionId: this.state.professionId,
      workId: this.state.workId,
      clientId: this.state.clientId
    };
  }

  handleSubmit() {
    const element = document.getElementById("container");

    createSpinner({
      target: element
    });
    showSpinner(element);
    updateUser(this.getUser())
      .then(() => {
        hideSpinner(element);
      })
      .catch(() => {
        hideSpinner(element);
      });
  }

  handleCancel() {
    if (this.props.user.roleId === 3) {
      this.props.history.push("/employees/employees");
    } else {
      this.props.history.push("/settings/usersnoworker");
    }
  }

  updatePhoto(newPhoto) {
    let remove = newPhoto.indexOf("base64,") + 7;
    newPhoto = newPhoto.substring(remove);
    this.setState({ photo: newPhoto });
  }

  toggleModal() {
    this.setState({
      modal: !this.state.modal
    });
  }

  render() {
    let src = "";
    if (this.state.photo !== null && this.state.photo !== "") {
      src = "data:image/png;base64," + this.state.photo;
    } else {
      src = "assets/img/avatars/user_no_photo.png";
    }

    return (
      <Fragment>
        <ModalSelectImage
          isOpen={this.state.modal}
          toggle={this.toggleModal}
          updatePhoto={this.updatePhoto}
          userId={this.props.user.id}
          type="image"
        />
        <div
          style={{
            marginLeft: 10,
            marginRight: 60,
            marginTop: 20,
            marginBottom: 20
          }}
          id="container"
        >
          <Form>
            <Row>
              <Col xs="2">
                <img
                  src={src}
                  alt={this.state.username}
                  width="230"
                  height="210"
                  onDoubleClick={this.toggleModal}
                  style={{ cursor: "pointer" }}
                />
              </Col>
              <Col xs="10">
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
                      <Label htmlFor="surname">Apellidos</Label>
                      <Input
                        type="text"
                        id="surname"
                        name="surname"
                        placeholder="apellidos"
                        required
                        value={this.state.surname || ""}
                        onChange={this.handleInputChange}
                      />
                    </FormGroup>
                  </Col>
                  <Col xs="4">
                    <FormGroup>
                      <Label htmlFor="dni">DNI</Label>
                      <Input
                        type="text"
                        id="dni"
                        name="dni"
                        placeholder="dni"
                        value={this.state.dni || ""}
                        onChange={this.handleInputChange}
                      />
                    </FormGroup>
                  </Col>
                </Row>
                <Row>
                  <Col xs="4">
                    <FormGroup>
                      <Label htmlFor="username">Códgio Acceso</Label>
                      <Input
                        type="text"
                        id="username"
                        name="username"
                        placeholder="código acceso"
                        required
                        value={this.state.username}
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
                        value={this.state.address || ""}
                        onChange={this.handleInputChange}
                      />
                    </FormGroup>
                  </Col>
                  <Col xs="4">
                    <FormGroup>
                      <Label htmlFor="phoneNumber">Teléfono</Label>
                      <Input
                        type="text"
                        id="phoneNumber"
                        name="phoneNumber"
                        placeholder="teléfono"
                        value={this.state.phoneNumber || ""}
                        onChange={this.handleInputChange}
                      />
                    </FormGroup>
                  </Col>
                </Row>
                <Row>
                  <Col xs="4">
                    <FormGroup>
                      <Label htmlFor="priceHour">Precio Hora</Label>
                      <Input
                        type="number"
                        id="priceHour"
                        name="priceHour"
                        placeholder="precio hora"
                        value={this.state.priceHour}
                        onChange={this.handleInputChange}
                      />
                    </FormGroup>
                  </Col>
                  <Col xs="4">
                    <FormGroup>
                      <Label htmlFor="priceHourSale">Precio Venta Hora</Label>
                      <Input
                        type="number"
                        id="priceHourSale"
                        name="priceHourSale"
                        placeholder="precio venta hora"
                        value={this.state.priceHourSale}
                        onChange={this.handleInputChange}
                      />
                    </FormGroup>
                  </Col>
                  <Col xs="4">
                    <FormGroup>
                      <Label htmlFor="observations">Observaciones</Label>
                      <Input
                        type="text"
                        id="observations"
                        name="observations"
                        placeholder="observaciones"
                        value={this.state.observations || ""}
                        onChange={this.handleInputChange}
                      />
                    </FormGroup>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col
                xs="12"
                style={{
                  marginTop: -20,
                  marginLeft: 40,
                  fontSize: 10,
                  fontWeight: "bold"
                }}
              >
                Doble click para cambiar la imagen
              </Col>
            </Row>
            <Row>
              <Col xs="12" style={{ marginTop: "20px", textAlign: "right" }}>
                <div className="form-actions">
                  <Button color="primary" onClick={this.handleSubmit}>
                    Guardar
                  </Button>
                  <Button
                    color="secondary"
                    style={{ marginLeft: "10px" }}
                    onClick={this.handleCancel}
                  >
                    Cancelar
                  </Button>
                </div>
              </Col>
            </Row>
          </Form>
        </div>
      </Fragment>
    );
  }
}

BasicData.propTypes = {};

export default BasicData;
