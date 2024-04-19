import React, { Component, Fragment } from "react";
import { Form, Col, FormGroup, Input, Label, Row, Button } from "reactstrap";
import { DatePickerComponent } from "@syncfusion/ej2-react-calendars";
import { MaskedTextBoxComponent } from "@syncfusion/ej2-react-inputs";
import { MultiSelectComponent } from "@syncfusion/ej2-react-dropdowns";
import { updateUser, getUser } from "../../services";
import ModalSelectImage from "../Modals/modal-select-image";
import {
  createSpinner,
  showSpinner,
  hideSpinner,
} from "@syncfusion/ej2-popups";
import { getProfessions } from "../../services";

class BasicData extends Component {
  fields = { text: "name", value: "id" };
  dtpBirthDate = null;

  constructor(props) {
    super(props);

    this.state = {
      modal: false,
      photo: null,
      msdDataSource: null,
    };

    getUser(props.userId).then((result) => {
      this.setState({
        id: result.id,
        name: result.name,
        surname: result.surname,
        dni: result.dni,
        securitySocialNumber: result.securitySocialNumber,
        birthDate: this.parseDate(result.birthDate),
        address: result.address,
        phoneNumber: result.phoneNumber,
        priceHour: result.priceHour,
        priceHourSale: result.priceHourSale,
        accountNumber: result.accountNumber,
        observations: result.observations,
        professions: result.professions,
        photo: result.photo,
        roleId: result.roleId,
        professionId: result.professionId,
        userProfessions: result.userProfessions,
        workId: result.workId,
        clientId: result.clientId,
        enterpriseId: result.enterpriseId,
        modal: false,
      });
    });

    getProfessions().then((result) => {
      this.setState({ msdDataSource: result.Items });
    });

    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleChangeProfessions = this.handleChangeProfessions.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleCancel = this.handleCancel.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
    this.updatePhoto = this.updatePhoto.bind(this);
    this.getUser = this.getUser.bind(this);
    this.parseDate = this.parseDate.bind(this);
  }

  parseDate(args) {
    if (args === null || args === undefined || args === "") {
      return "";
    }

    return `${args.substring(3, 5)}/${args.substring(0, 2)}/${args.substring(
      6,
      10
    )}`;
  }

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;

    this.setState({
      [name]: target.value,
    });
  }

  handleChangeProfessions(event) {
    this.setState({
      userProfessions: event.value,
    });
  }

  getUser() {
    return {
      id: this.state.id,
      name: this.state.name,
      surname: this.state.surname,
      dni: this.state.dni,
      securitySocialNumber: this.state.securitySocialNumber,
      birthDate: this.dtpBirthDate.value != null ? new Date(
        Date.UTC(
          this.dtpBirthDate.value.getFullYear(),
          this.dtpBirthDate.value.getMonth(),
          this.dtpBirthDate.value.getDate())
        ) : null,
      username: this.state.username,
      address: this.state.address,
      phoneNumber: this.state.phoneNumber,
      priceHour: this.state.priceHour,
      priceHourSale: this.state.priceHourSale,
      accountNumber: this.state.accountNumber,
      observations: this.state.observations,
      professions: this.state.professions,
      photo: this.state.photo,
      roleId: this.state.roleId,
      professionId: this.state.professionId,
      userProfessions: this.state.userProfessions,
      workId: this.state.workId,
      clientId: this.state.clientId,
      enterpriseId: this.state.enterpriseId
    };
  }

  handleSubmit() {
    const element = document.getElementById("container");

    createSpinner({
      target: element,
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
      modal: !this.state.modal,
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
          userId={this.props.userId}
          type="image"
        />
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
              <Col xs="2">
                <div>
                  <img
                    src={src}
                    alt={this.state.username}
                    onDoubleClick={this.toggleModal}
                    style={{ cursor: "pointer", width: "100%" }}
                  />
                  <div style={{ textAlign: "center" }}>
                    <span
                      style={{
                        fontSize: 10,
                        fontWeight: "bold",
                      }}
                    >
                      Doble click para cambiar la imagen
                    </span>
                  </div>
                </div>
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
                      <Label htmlFor="securitySocialNumber">
                        Nº Seguridad Social
                      </Label>
                      <Input
                        type="text"
                        id="securitySocialNumber"
                        name="securitySocialNumber"
                        placeholder="número de la seguridad social"
                        value={this.state.securitySocialNumber || ""}
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
                      <Label htmlFor="username">Códgio Acceso</Label>
                      <Input
                        type="text"
                        id="username"
                        name="username"
                        placeholder="código acceso"
                        required
                        value={this.state.username || ""}
                        onChange={this.handleInputChange}
                      />
                    </FormGroup>
                  </Col>
                  <Col xs="4">
                    <FormGroup>
                      <Label htmlFor="accountNumber">Nº de Cuenta</Label>
                      <MaskedTextBoxComponent
                        mask={"LL00-0000-0000-0000-0000-0000"}
                        id="accountNumber"
                        name="accountNumber"
                        placeholder="nº de cuenta"
                        value={this.state.accountNumber || ""}
                        onChange={this.handleInputChange}
                      />
                    </FormGroup>
                  </Col>
                  {/* <Col xs="4">
                    <FormGroup>
                      <Label htmlFor="priceHour">Precio Coste Hora</Label>
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
                  </Col> */}
                  <Col xs="4">
                    <FormGroup>
                      <Label htmlFor="birthDate">Fecha de Nacimiento</Label>
                      <DatePickerComponent
                        id="birthDate"
                        locale="es"
                        name="birthDate"
                        required
                        format="dd/MM/yyyy"
                        value={this.state.birthDate || ""}
                        onChange={this.handleInputChange}
                        ref={(g) => (this.dtpBirthDate = g)}
                      />
                    </FormGroup>
                  </Col>
                </Row>
                <Row>
                  <Col xs="4">
                    <FormGroup>
                      <Label htmlFor="professions">Puestos de Trabajo</Label>
                      <MultiSelectComponent
                        id="professions"
                        name="professions"
                        dataSource={this.state.msdDataSource}
                        fields={this.fields}
                        value={this.state.userProfessions}
                        placeholder="Selecciona puesto/s"
                        change={this.handleChangeProfessions}
                      />
                    </FormGroup>
                  </Col>
                  <Col xs="8">
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
      </Fragment>
    );
  }
}

BasicData.propTypes = {};

export default BasicData;
