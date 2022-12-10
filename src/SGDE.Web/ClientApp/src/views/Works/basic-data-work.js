import React, { Component, Fragment } from "react";
import { Form, Col, FormGroup, Input, Label, Row, Button } from "reactstrap";
import { AppSwitch } from "@coreui/react";
import { DropDownListComponent } from "@syncfusion/ej2-react-dropdowns";
import { DatePickerComponent } from "@syncfusion/ej2-react-calendars";
import { NumericTextBoxComponent } from "@syncfusion/ej2-react-inputs";
import { updateWork, getWork } from "../../services";
import {
  createSpinner,
  showSpinner,
  hideSpinner,
} from "@syncfusion/ej2-popups";

class BasicDataWork extends Component {
  constructor(props) {
    super(props);

    this.state = {};

    getWork(this.props.workId).then((result) => {
      this.setState({
        id: result.id,
        name: result.name,
        address: result.address,
        worksToRealize: result.worksToRealize,
        numberPersonsRequested: result.numberPersonsRequested,
        estimatedDuration: result.estimatedDuration,
        openDate: result.openDate,
        closeDate: result.closeDate,
        passiveSubject: result.passiveSubject,
        invoiceToOrigin: result.invoiceToOrigin,
        totalContract: result.totalContract,
        percentageRetention: result.percentageRetention,
        percentageIVA: result.percentageIVA,
        open: result.open,
        clientId: result.clientId,
        workBudgets: result.workBudgets,
        status: result.status
      });
    });

    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.getWork = this.getWork.bind(this);
    this.formatDate = this.formatDate.bind(this);
    this.handleChangeTotalContract = this.handleChangeTotalContract.bind(this);
    this.handleChangePercentageRetention =
      this.handleChangePercentageRetention.bind(this);
    this.handleChangePercentageIVA = this.handleChangePercentageIVA.bind(this);
    this.updateFromInvoiceToOrigin = this.updateFromInvoiceToOrigin.bind(this);
    this.updateFromPassiveSubject = this.updateFromPassiveSubject.bind(this);
    this.renderTotalContratc = this.renderTotalContratc.bind(this);

    this.ntbTotalContract = null;
    this.ntbPercentageRetention = null;
    this.ntbPercentageIVA = null;

    this.typeWork = [{ id: "HO" }, { id: "PA" }, { id: "MA" }];
    this.ddlTypeWork = null;
  }

  componentDidMount() {
    this.updateFromInvoiceToOrigin();
  }

  componentDidUpdate(prevProps, prevState) {
    if (this.props.refreshStatus !== prevProps.refreshStatus) {
      getWork(this.props.workId).then((result) => {
        this.setState({
          id: result.id,
          name: result.name,
          address: result.address,
          worksToRealize: result.worksToRealize,
          numberPersonsRequested: result.numberPersonsRequested,
          estimatedDuration: result.estimatedDuration,
          openDate: result.openDate,
          closeDate: result.closeDate,
          passiveSubject: result.passiveSubject,
          invoiceToOrigin: result.invoiceToOrigin,
          totalContract: result.totalContract,
          percentageRetention: result.percentageRetention,
          percentageIVA: result.percentageIVA,
          open: result.open,
          clientId: result.clientId,
          workBudgets: result.workBudgets,
          status: result.status
        });
      });
    }

    if (this.state.invoiceToOrigin !== prevState.invoiceToOrigin) {
      this.updateFromInvoiceToOrigin();
    }

    if (this.state.passiveSubject !== prevState.passiveSubject) {
      this.updateFromPassiveSubject();
    }
  }

  updateFromInvoiceToOrigin() {
    if (this.state.invoiceToOrigin) {
      this.ntbPercentageRetention.enabled = true;
      if (this.ntbTotalContract) {
        this.ntbTotalContract.enabled = true;
      }
    } else {
      this.ntbPercentageRetention.enabled = false;
      if (this.ntbTotalContract) {
        this.ntbTotalContract.enabled = false;
      }
    }
  }

  updateFromPassiveSubject() {
    if (this.state.passiveSubject) {
      this.ntbPercentageIVA.enabled = false;
      this.ntbPercentageIVA.value = 0;
    } else {
      this.ntbPercentageIVA.enabled = true;
    }
  }

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;

    if (name === "openDate" || name === "closeDate") {
      this.setState({
        [name]: this.formatDate(target.value),
      });
    } else {
      if (name === "passiveSubject" || name === "invoiceToOrigin") {
        this.setState({
          [name]: target.checked,
        });
      } else {
        this.setState({
          [name]: target.value,
        });
      }
    }

    if (name === "passiveSubject") {
      this.ntbPercentageIVA.enabled = !target.checked;
    }
  }

  handleChangeTotalContract(args) {
    this.setState({ totalContract: args.value });
  }

  handleChangePercentageRetention(args) {
    this.setState({ percentageRetention: args.value });
  }

  handleChangePercentageIVA(args) {
    this.setState({ percentageIVA: args.value });
  }

  formatDate(args) {
    if (args === null || args === "") {
      return "";
    }

    let day = args.getDate();
    if (day < 10) day = "0" + day;

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
      worksToRealize: this.ddlTypeWork.value,
      numberPersonsRequested: this.state.numberPersonsRequested,
      estimatedDuration: this.state.estimatedDuration,
      openDate: this.state.openDate,
      closeDate: this.state.closeDate,
      passiveSubject: this.state.passiveSubject,
      invoiceToOrigin: this.state.invoiceToOrigin,
      totalContract: this.state.totalContract,
      percentageRetention: this.state.percentageRetention,
      percentageIVA: this.state.percentageIVA,
      open: this.state.open,
      clientId: this.state.clientId,
      workBudgets: this.state.workBudgets,
      status:  this.state.status
    };
  }

  handleSubmit() {
    const element = document.getElementById("container");

    createSpinner({
      target: element,
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

  renderTotalContratc() {
    if (this.state.workBudgets == null) {
      return null;
    }

    var budgets = [];
    for (var i = 0; i < this.state.workBudgets.length; i++) {
      const workBubget = this.state.workBudgets[i];
      budgets.push(
        <Fragment>
          <Row>
            <Col xs="5" style={{ marginRight: "-30px" }}>
              <b>{workBubget.Item1}</b>
            </Col>
            <Col xs="2" style={{ marginRight: "-50px" }}>
              <b>...</b>
            </Col>
            <Col xs="4" style={{ marginLeft: "-30px", textAlign: "right" }}>
              <b>{workBubget.Item2}€</b>
            </Col>
          </Row>
        </Fragment>
      );
    }

    return budgets;
  }

  render() {
    let color = '';
    switch (this.state.status) {
      case "Abierta":
        color = "#16a085";
        break;

      case "Cerrada":
        color = "#e74c3c";
        break;
          
      case "Juridico":
        color = "#ff8000";
        break;
        
      default:
        color = "#16a085";
        break;
    }

    return (
      <Fragment>
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
              <Col style={{ background: color }}>&nbsp;</Col>
            </Row>
            <Row>
              <Col>&nbsp;</Col>
            </Row>            
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
                  <Label htmlFor="address">Dirección</Label>
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
                  <DropDownListComponent
                    id="worksToRealize"
                    dataSource={this.typeWork}
                    placeholder={`Selecciona Tipo de Obra`}
                    fields={{ text: "id", value: "id" }}
                    ref={(g) => (this.ddlTypeWork = g)}
                    value={this.state.worksToRealize || ""}
                  />
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col xs="2">
                <FormGroup>
                  <Label
                    htmlFor="passiveSubject"
                    style={{ verticalAlign: "bottom" }}
                  >
                    Sujeto Pasivo&nbsp;
                  </Label>
                  <AppSwitch
                    className={"mx-1 mt-4"}
                    variant={"pill"}
                    color={"primary"}
                    label
                    checked={this.state.passiveSubject}
                    id="passiveSubject"
                    name="passiveSubject"
                    placeholder="sujeto pasivo"
                    onChange={this.handleInputChange}
                    dataOn="Si"
                    dataOff="No"
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
            <Row>
              <Col xs="4">
                <FormGroup>
                  <Label
                    htmlFor="invoiceToOrigin"
                    style={{ verticalAlign: "bottom" }}
                  >
                    Factura a Origen&nbsp;
                  </Label>
                  <AppSwitch
                    className={"mx-1 mt-4"}
                    variant={"pill"}
                    color={"primary"}
                    label
                    checked={this.state.invoiceToOrigin}
                    id="invoiceToOrigin"
                    name="invoiceToOrigin"
                    placeholder="factura a origen"
                    onChange={this.handleInputChange}
                    dataOn="Si"
                    dataOff="No"
                  />
                </FormGroup>
              </Col>
              <Col xs="4">
                <FormGroup>
                  <Label htmlFor="percentageRetention">Retención</Label>
                  <NumericTextBoxComponent
                    format="p2"
                    id="percentageRetention"
                    name="percentageRetention"
                    value={this.state.percentageRetention}
                    min={0}
                    max={1}
                    step={0.01}
                    showSpinButton={false}
                    placeholder="porcentaje retención"
                    change={this.handleChangePercentageRetention}
                    ref={(g) => (this.ntbPercentageRetention = g)}
                  />
                </FormGroup>
              </Col>
              <Col xs="4">
                <FormGroup>
                  <Label htmlFor="percentageIVA">IVA</Label>
                  <NumericTextBoxComponent
                    format="p2"
                    id="percentageIVA"
                    name="percentageIVA"
                    value={this.state.percentageIVA}
                    min={0}
                    max={1}
                    step={0.01}
                    showSpinButton={false}
                    placeholder="porcentaje IVA"
                    change={this.handleChangePercentageIVA}
                    ref={(g) => (this.ntbPercentageIVA = g)}
                  />
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col xs="12">
                <hr></hr>
              </Col>
            </Row>
            <Row>
              <Col xs="3"></Col>
              <Col xs="5">
                <br></br>
                <FormGroup>
                  <Label
                    htmlFor="totalContract"
                    style={{
                      fontSize: "Medium",
                      textAlign: "Center",
                      display: "block",
                      marginRight: "140px",
                    }}
                  >
                    <b>Total Contrato</b>
                  </Label>
                  {this.renderTotalContratc()}
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
      </Fragment>
    );
  }
}

BasicDataWork.propTypes = {};

export default BasicDataWork;
