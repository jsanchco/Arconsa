import React, { Component, Fragment } from "react";
import {
  Breadcrumb,
  BreadcrumbItem,
  Container,
  Row,
  Form,
  FormGroup,
  Label,
  Button,
  Col,
} from "reactstrap";
import { DatePickerComponent } from "@syncfusion/ej2-react-calendars";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Inject,
  Toolbar,
  Group,
  ExcelExport,
  Sort,
  Aggregate,
  AggregateColumnsDirective,
  AggregateColumnDirective,
  AggregateDirective,
  AggregatesDirective,
} from "@syncfusion/ej2-react-grids";
import {
  createSpinner,
  showSpinner,
  hideSpinner,
} from "@syncfusion/ej2-popups";
import { connect } from "react-redux";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, REPORT_INVOICES, COMPANY_DATA } from "../../constants";
import { TOKEN_KEY, getSettings } from "../../services";
import ACTION_APPLICATION from "../../actions/applicationAction";

class ReportInvoices extends Component {
  dtpStartDate = null;
  dtpEndDate = null;
  grid = null;

  wrapSettings = { wrapMode: "Content" };

  constructor(props) {
    super(props);

    this.toolbarOptions = [
      "Print",
      "ExcelExport",
      {
        text: "Colapsar",
        tooltipText: "Colapsar todas las Filas",
        prefixIcon: "e-custom-icons e-file-upload",
        id: "CollapseAll",
      },
    ];

    this.templateIsPaid = this.templateIsPaid.bind(this);
    this.handleOnClick = this.handleOnClick.bind(this);
    this.clickHandler = this.clickHandler.bind(this);
    this.getExcelExportProperties = this.getExcelExportProperties.bind(this);
    this.beforePrint = this.beforePrint.bind(this);

    this.format = { type: "dateTime", format: "dd/MM/yyyy" };
  }

  componentDidMount() {
    getSettings(COMPANY_DATA)
      .then((result) => {
        const data = JSON.parse(result.data);
        this.setState({
          companyName: data.companyName,
          cif: data.cif,
          address: data.address,
          phoneNumber: data.phoneNumber,
        });
      })
      .catch((error) => {
        this.props.showMessage({
          statusText: error,
          responseText: error,
          type: "danger",
        });
      });
  }

  templateIsPaid(args) {
    if (args.isPaid) {
      return <span>Si</span>;
    } else {
      return <span>No</span>;
    }
  }

  handleOnClick() {
    const valueDtpStartDate = this.formatDate(this.dtpStartDate.value);
    const valueDtpEndDate = this.formatDate(this.dtpEndDate.value);

    if (
      valueDtpStartDate === null ||
      valueDtpStartDate === "" ||
      valueDtpEndDate === null ||
      valueDtpEndDate === ""
    ) {
      this.props.showMessage({
        statusText: "Consulta mal configurada",
        responseText: "Consulta mal configurada",
        type: "danger",
      });
    } else {
      if (this.dtpStartDate.value > this.dtpEndDate.value) {
        this.props.showMessage({
          statusText: "Consulta mal configurada",
          responseText: "Consulta mal configurada",
          type: "danger",
        });
      } else {
        // showSpinner(this.element);

        this.grid.dataSource = new DataManager({
          adaptor: new WebApiAdaptor(),
          url: `${config.URL_API}/${REPORT_INVOICES}`,
          headers: [
            { Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) },
          ],
        });
        this.grid.query = new Query()
          .addParams("startDate", valueDtpStartDate)
          .addParams("endDate", valueDtpEndDate);

        this.grid.refresh();
      }
    }
  }

  clickHandler(args) {
    if (args.item.id === "CollapseAll") {
      if (this.grid.groupSettings.columns.length > 0) {
        this.grid.groupModule.collapseAll();
      }
    }
    if (args.item.text === "Exportar a Excel") {
      this.grid.excelExport(this.getExcelExportProperties());
    }
  }

  footerSum(args) {
    const total = Math.round((args.Sum + Number.EPSILON) * 100) / 100;
    return <span>Total: {total} horas</span>;
  }

  footerSumEuros(args) {
    if (typeof args.Sum === "string" || args.Sum instanceof String) {
      const total = args.Sum.replace(",", "");
      //const total = args.Sum;
      return <span>Total: {total}€</span>;
    } else {
      const total = Math.round((args.Sum + Number.EPSILON) * 100) / 100;
      return <span>Total: {total}€</span>;
    }
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

  getExcelExportProperties() {
    let title = "INFORME de FACTURAS";
    let type = "FACTURA";
    let fileName = "Inf_fact.xlsx";
    const date = this.formatDate(new Date());

    return {
      header: {
        headerRows: 7,
        rows: [
          {
            index: 1,
            cells: [
              {
                index: 1,
                colSpan: 7,
                value: title,
                style: {
                  fontColor: "#C25050",
                  fontSize: 25,
                  hAlign: "Center",
                  bold: true,
                },
              },
            ],
          },
          {
            index: 3,
            cells: [
              {
                index: 1,
                colSpan: 3,
                value: this.state.companyName,
                style: { fontColor: "#C67878", fontSize: 15, bold: true },
              },
              {
                index: 5,
                colSpan: 2,
                value: type,
                style: { fontColor: "#C67878", bold: true },
              },
              {
                index: 7,
                value: "FECHA INFORME",
                style: { fontColor: "#C67878", bold: true },
                width: 150,
              },
            ],
          },
          {
            index: 4,
            cells: [
              { index: 1, colSpan: 3, value: this.state.address },
              { index: 5, colSpan: 2, value: "" },
              {
                index: 7,
                value: date,
                width: 150,
              },
            ],
          },
          {
            index: 5,
            cells: [
              {
                index: 1,
                colSpan: 3,
                value: this.state.phoneNumber,
              },
              {
                index: 5,
                value: "",
                style: { fontColor: "#C67878", bold: true },
              },
              {
                index: 6,
                value: "",
                width: 150,
                style: { fontColor: "#C67878", bold: true },
              },
            ],
          },
          {
            index: 6,
            cells: [
              { index: 5, value: "" },
              { index: 6, value: "", width: 150 },
            ],
          },
        ],
      },
      // footer: {
      //   footerRows: 5,
      //   rows: [
      //     {
      //       cells: [
      //         {
      //           colSpan: 6,
      //           value: "Thank you for your business!",
      //           style: { fontColor: "#C67878", hAlign: "Center", bold: true }
      //         }
      //       ]
      //     },
      //     {
      //       cells: [
      //         {
      //           colSpan: 6,
      //           value: "!Visit Again!",
      //           style: { fontColor: "#C67878", hAlign: "Center", bold: true }
      //         }
      //       ]
      //     }
      //   ]
      // },
      fileName: fileName,
    };
  }

  beforePrint(args) {
    const valueDtpStartDate = this.formatDate(this.dtpStartDate.value);
    const valueDtpEndDate = this.formatDate(this.dtpEndDate.value);

    var div = document.createElement("Div");
    div.innerHTML = "FACTURAS desde el " + valueDtpStartDate + " al " + valueDtpEndDate;
    div.style.textAlign = "center";
    div.style.color = "red";
    div.style.padding = "10px 0";
    div.style.fontSize = "25px";
    args.element.insertBefore(div, args.element.childNodes[0]);
  }

  render() {
    return (
      <Fragment>
        <Breadcrumb>
          {/*eslint-disable-next-line*/}
          <BreadcrumbItem>
            <a href="#">Inicio</a>
          </BreadcrumbItem>
          {/* eslint-disable-next-line*/}
          <BreadcrumbItem active>Informe de Horas por Obra</BreadcrumbItem>
        </Breadcrumb>

        <Container fluid>
          <div className="animated fadeIn" id="selection-report">
            <div className="card">
              <div className="card-header">
                <i className="icon-list"></i> Informe de Facturas
              </div>
              <div className="card-body"></div>
              <div>
                <Form style={{ marginLeft: "20px" }}>
                  <Row>
                    <Col xs="5">
                      <FormGroup>
                        <Label for="startDate">Fecha Inicio</Label>
                        <DatePickerComponent
                          id="startDate"
                          ref={(g) => (this.dtpStartDate = g)}
                          format="dd/MM/yyyy"
                        />
                      </FormGroup>
                    </Col>
                    <Col xs="5">
                      <FormGroup style={{ marginLeft: "20px" }}>
                        <Label for="endDate">Fecha Fin</Label>
                        <DatePickerComponent
                          id="endDate"
                          ref={(g) => (this.dtpEndDate = g)}
                          format="dd/MM/yyyy"
                        />
                      </FormGroup>
                    </Col>
                  </Row>
                  <Row>
                    <Col xs="10"></Col>
                    <Col xs="2">
                      <Button
                        color="primary"
                        style={{ marginLeft: "30px", textAlign: "left" }}
                        onClick={this.handleOnClick}
                      >
                        Consultar
                      </Button>
                    </Col>
                  </Row>
                </Form>
              </div>
              <Row>
                <GridComponent
                  id="GridInvoices"
                  locale="es-US"
                  toolbar={this.toolbarOptions}
                  toolbarClick={this.clickHandler}
                  style={{
                    marginLeft: 30,
                    marginRight: 30,
                    marginTop: 10,
                    marginBottom: 20,
                  }}
                  allowGrouping={true}
                  allowExcelExport={true}
                  ref={(g) => (this.grid = g)}
                  allowTextWrap={true}
                  textWrapSettings={this.wrapSettings}
                  allowSorting={true}
                  beforePrint={this.beforePrint}
                >
                  <ColumnsDirective>
                    <ColumnDirective
                      field="name"
                      headerText="Nº Factura"
                      width="100"
                    />
                    <ColumnDirective
                      field="issueDate"
                      headerText="Fecha"
                      format={this.format}
                      width="100"
                    />
                    <ColumnDirective
                      field="clientName"
                      headerText="Cliente"
                      width="100"
                    />
                    <ColumnDirective
                      field="workName"
                      headerText="Obra"
                      width="100"
                    />
                    <ColumnDirective
                      field="taxBase"
                      headerText="Importe"
                      width="100"
                    />
                    <ColumnDirective
                      field="ivaTaxBase"
                      headerText="IVA"
                      width="100"
                    />
                    <ColumnDirective
                      field="total"
                      headerText="Total"
                      width="100"
                    />
                    <ColumnDirective
                      field="payDate"
                      headerText="Fecha Pago"
                      format={this.format}
                      width="100"
                    />
                    <ColumnDirective
                      field="isPaid"
                      headerText="Pagado"
                      width="100"
                      template={this.templateIsPaid}
                    />
                  </ColumnsDirective>

                  <AggregatesDirective>
                    <AggregateDirective>
                      <AggregateColumnsDirective>
                        <AggregateColumnDirective
                          field="taxBase"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                          groupCaptionTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="ivaTaxBase"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                          groupCaptionTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="total"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                          groupCaptionTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>
                      </AggregateColumnsDirective>
                    </AggregateDirective>
                  </AggregatesDirective>

                  <Inject
                    services={[Group, ExcelExport, Toolbar, Sort, Aggregate]}
                  />
                </GridComponent>
              </Row>
            </div>
          </div>
        </Container>
      </Fragment>
    );
  }
}

ReportInvoices.propTypes = {};

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
});

export default connect(mapStateToProps, mapDispatchToProps)(ReportInvoices);
