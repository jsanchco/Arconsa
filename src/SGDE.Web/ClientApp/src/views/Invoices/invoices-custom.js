import React, { Component, Fragment } from "react";
import { Form, Col, FormGroup, Label, Row, Button, Tooltip } from "reactstrap";
import { DatePickerComponent } from "@syncfusion/ej2-react-calendars";
import { DropDownListComponent } from "@syncfusion/ej2-react-dropdowns";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import {
  createSpinner,
  showSpinner,
  hideSpinner,
} from "@syncfusion/ej2-popups";
import {
  getWorks,
  getInvoice,
  getInvoiceResponse,
  base64ToArrayBuffer,
  saveByteArray,
} from "../../services";
import GridInvoice from "../../components/grid-invoices";
import GridDetailInvoice from "../../components/grid-detail-invoice";

class Invoices extends Component {
  constructor(props) {
    super(props);

    this.state = {
      invoiceNumber: "",
      startDate: null,
      endDate: null,
      issueDate: null,
      typeInvoice: 2,
      clientId: null,
      workId: null,
      updateGrid: null,
      dataSourceDetailInvoce: null,
      tooltipOpen: false,
      selectionHide: true,
    };

    this.toggle = this.toggle.bind(this);

    this.ddl = null;

    this.fields = { text: "name", value: "id" };
    this.dataSource = getWorks().then((items) => {
      this.ddl.dataSource = items;
    });

    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleDropDown = this.handleDropDown.bind(this);
    this.handleDate = this.handleDate.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.formatDate = this.formatDate.bind(this);
    this.updateForm = this.updateForm.bind(this);
    this.updateDataSourceDetailInvoce = this.updateDataSourceDetailInvoce.bind(
      this
    );
    this.handleOnClickHide = this.handleOnClickHide.bind(this);
  }

  getDataInvoiceResponse() {
    return {
      invoiceNumber: this.state.invoiceNumber,
      startDate: this.state.startDate,
      endDate: this.state.endDate,
      issueDate: this.state.issueDate,
      typeInvoice: this.state.typeInvoice,
      clientId: this.state.clientId,
      workId: this.state.workId,
      detailInvoice: this.state.dataSourceDetailInvoce,
    };
  }

  getDataInvoice() {
    return {
      name: this.state.invoiceNumber,
      startDate: this.state.startDate,
      endDate: this.state.endDate,
      issueDate: this.state.issueDate,
      typeInvoice: this.state.typeInvoice,
      clientId: this.state.clientId,
      workId: this.state.workId,
    };
  }

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;

    this.setState({
      [name]: target.value,
    });
  }

  handleDropDown(event) {
    this.setState({
      workId: event.value,
    });
  }

  handleDate(event) {
    const name = event.element.name;
    this.setState({
      [name]: this.formatDate(event.value),
    });
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
        type: "danger",
      });
    }

    const element = document.getElementById("container");

    createSpinner({
      target: element,
    });
    showSpinner(element);

    const data = this.getDataInvoiceResponse();
    getInvoiceResponse(data)
      .then((result) => {
        const fileArr = base64ToArrayBuffer(result.file);
        saveByteArray(result.fileName, fileArr, result.typeFile);
        hideSpinner(element);

        this.setState({ updateGrid: Math.random() });
      })
      .catch((error) => {
        hideSpinner(element);
      });
  }

  updateForm(args) {
    getInvoice(args).then((result) => {
      this.setState({
        startDate: result.startDate,
        endDate: result.endDate,
        issueDate: result.issueDate,
        workId: result.workId,
        dataSourceDetailInvoce: result.detailInvoice,
      });
      this.ddl.value = result.workId;
    });
  }

  updateDataSourceDetailInvoce(args) {
    this.setState({ dataSourceDetailInvoce: args });
  }

  handleOnClickHide() {
    this.setState({
      selectionHide: !this.state.selectionHide,
    });
  }

  toggle() {
    this.setState({
      tooltipOpen: !this.state.tooltipOpen,
    });
  }

  renderFormSelection() {}

  render() {
    const invoiceQuery = this.getDataInvoice();
    const titleToolTip =
      this.state.selectionHide === true
        ? "Ver Generador de Consultas"
        : "Ocultar Generador de Consultas";
    const classSelection =
      this.state.selectionHide === true
        ? "fa fa-eye fa-lg mt-4"
        : "fa fa-eye-slash fa-lg mt-4";
    const classDivSelection = this.state.selectionHide === true ? "hidden" : "";

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
        <Fragment>
          <Tooltip
            placement="bottom"
            isOpen={this.state.tooltipOpen}
            target="toggle-selection"
            toggle={this.toggle}
          >
            {titleToolTip}
          </Tooltip>

          <div className="animated fadeIn">
            <div className="card">
              <div className="card-header">
                <i className="cui-file"></i>
                Facturas
              </div>
              <div className="card-body">
                <Form>
                  <div id="selection" className={classDivSelection}>
                    <Row style={{ marginLeft: "20px", marginRight: "20px" }}>
                      <Col xs="6">
                        <FormGroup>
                          <Label htmlFor="name">Obra</Label>
                          <DropDownListComponent
                            id="workId"
                            name="workId"
                            dataSource={this.dataSource}
                            fields={this.fields}
                            placeholder="selecciona obra"
                            change={this.handleDropDown}
                            ref={(g) => (this.ddl = g)}
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
                            change={this.handleDate}
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
                            change={this.handleDate}
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
                            change={this.handleDate}
                          />
                        </FormGroup>
                      </Col>
                    </Row>
                    <Row>
                      <Col xs="12">
                        <GridDetailInvoice
                          invoiceQuery={invoiceQuery}
                          showMessage={this.props.showMessage}
                          updateDataSourceDetailInvoce={
                            this.updateDataSourceDetailInvoce
                          }
                          dataSourceDetailInvoce={
                            this.state.dataSourceDetailInvoce
                          }
                        />
                      </Col>
                    </Row>
                    <Row style={{ marginLeft: "20px", marginRight: "20px" }}>
                      <Col
                        xs="12"
                        style={{
                          marginTop: "20px",
                          textAlign: "right",
                        }}
                      >
                        <div className="form-actions">
                          <Button color="primary" onClick={this.handleSubmit}>
                            Generar Factura
                          </Button>
                        </div>
                      </Col>
                    </Row>
                  </div>

                  <Row style={{ marginLeft: "20px", marginRight: "20px" }}>
                    <Col xs="12">
                      <div
                        style={{
                          textAlign: "left",
                          marginTop: "-35px",
                          cursor: "pointer",
                        }}
                        onClick={this.handleOnClickHide}
                      >
                        <i id="toggle-selection" class={classSelection}></i>
                      </div>
                    </Col>
                  </Row>

                  <Row>
                    <Col xs="12" style={{}}>
                      <div style={{ textAlign: "center" }}>
                        <h1>Listado de Facturas</h1>
                      </div>
                    </Col>
                  </Row>
                  <Row>
                    <Col xs="12" style={{ marginTop: "40px" }}>
                      <GridInvoice
                        clientId={null}
                        workId={null}
                        update={this.state.updateGrid}
                        showMessage={this.props.showMessage}
                        updateForm={this.updateForm}
                        showViewInvoice={true}
                      />
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

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
});

export default connect(mapStateToProps, mapDispatchToProps)(Invoices);
