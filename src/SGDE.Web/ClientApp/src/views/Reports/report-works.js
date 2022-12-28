import React, { Component, Fragment } from "react";
import {
  Breadcrumb,
  BreadcrumbItem,
  Container,
  Row,
  Form,
  FormGroup,
  Label,
  Input,
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
import { config, REPORT_WORKS, COMPANY_DATA } from "../../constants";
import { TOKEN_KEY, getSettings } from "../../services";
import ACTION_APPLICATION from "../../actions/applicationAction";
import Legend from "../../components/legend";

class ReportWorks extends Component {
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
    this.printComplete = this.printComplete.bind(this);

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
    const valueFilter = document.getElementById("filter").value;

    // const element = document.getElementById("container-report-works");

    // createSpinner({
    //   target: element,
    // });
    // showSpinner(element);

    this.grid.dataSource = new DataManager({
      adaptor: new WebApiAdaptor(),
      url: `${config.URL_API}/${REPORT_WORKS}`,
      headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
    });
    this.grid.query = new Query()
      .addParams("startDate", valueDtpStartDate)
      .addParams("endDate", valueDtpEndDate)
      .addParams("filter", valueFilter);

    this.grid.refresh();
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
    let title = "INFORME de OBRAS Abiertas";
    let type = "OBRAS";
    let fileName = "Inf_Obras_Abiertas.xlsx";
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
    div.innerHTML =
      "OBRAS Abiertas desde el " + valueDtpStartDate + " al " + valueDtpEndDate;
    div.style.textAlign = "center";
    div.style.color = "red";
    div.style.padding = "10px 0";
    div.style.fontSize = "25px";
    args.element.insertBefore(div, args.element.childNodes[0]);
  }

  exportQueryCellInfo(args) {
    if (args.name === "excelQueryCellInfo") {
      if (args.column.headerText === "Pagado") {
        if (args.value === false) {
          args.value = "Pendiente";
          args.style = { backColor: "#ff704d" };
        }
        if (args.value === true) {
          args.value = "Cobrado";
        }
      }
    }
  }

  clientTemplate(args) {
    return (
      <div>
        <a rel="nofollow" href={"/#/clients/detailclient/" + args.clientId}>
          {args.clientName}
        </a>
      </div>
    );
  }

  workTemplate(args) {
    return (
      <div>
        <a rel="nofollow" href={"/#/works/detailwork/" + args.workId}>
          {args.workName}
        </a>
      </div>
    );
  }

  workBudgetTemplate(args) {
    return (
      <div>
        <a rel="nofollow" href={"/#/works/detailwork/" + args.workId + "/1"}>
          {args.workBudgetsName}
        </a>
      </div>
    );
  }

  statusTemplate(args) {
    switch (args.status) {
      case "Abierta":
        return (
          <div>
            <span className="dot-green"></span>
          </div>
        );

      case "Cerrada":
        return (
          <div>
            <span className="dot-red"></span>
          </div>
        );

      case "Juridico":
        return (
          <div>
            <span className="dot-orange"></span>
          </div>
        );

      default:
        return (
          <div>
            <span className="dot-green"></span>
          </div>
        );
    }
  }

  printComplete(args) {
    if (this.grid) {
      const cols = this.grid.getColumns();
      for (const col of cols) {
        if (col.field === "status" && col.template != null) {
          col.visible = false;
        }
        if (col.field === "status" && col.template == null) {
          col.visible = true;
        }
      }
    }
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
          <BreadcrumbItem active>
            Informe de Obras Abiertas entre dos Fechas
          </BreadcrumbItem>
        </Breadcrumb>

        <Container fluid id="container-report-works">
          <div className="animated fadeIn" id="selection-report">
            <div className="card">
              <div className="card-header">
                <i className="icon-list"></i> Informe de Obras Abiertas entre
                dos Fechas
              </div>
              <div className="card-body"></div>
              <div>
                <Form style={{ marginLeft: "20px" }}>
                  <Row>
                    <Col xs="3">
                      <FormGroup>
                        <Label for="startDate">Fecha Inicio</Label>
                        <DatePickerComponent
                          id="startDate"
                          ref={(g) => (this.dtpStartDate = g)}
                          format="dd/MM/yyyy"
                        />
                      </FormGroup>
                    </Col>
                    <Col xs="3">
                      <FormGroup style={{ marginLeft: "20px" }}>
                        <Label for="endDate">Fecha Fin</Label>
                        <DatePickerComponent
                          id="endDate"
                          ref={(g) => (this.dtpEndDate = g)}
                          format="dd/MM/yyyy"
                        />
                      </FormGroup>
                    </Col>
                    <Col xs="3">
                      <FormGroup style={{ marginLeft: "20px" }}>
                        <Label for="filter">Filtrar</Label>
                        <Input
                          type="text"
                          id="filter"
                          name="filter"
                          placeholder="Por favor seleccione filtro"
                        />
                      </FormGroup>
                    </Col>
                    <Col
                      xs="3"
                      style={{
                        textAlign: "right",
                        marginLeft: "-10px",
                        marginTop: "30px",
                      }}
                    >
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
                <Col>&nbsp;</Col>
              </Row>
              <Row>
                <Col xs="3"></Col>
                <Col xs="9">
                  <Legend
                    elements={[
                      { color: "dot-green", text: "Abierta" },
                      { color: "dot-orange", text: "Juridico" },
                      { color: "dot-red", text: "Cerrada" },
                    ]}
                  />
                </Col>
              </Row>
              <Row>
                <GridComponent
                  id="gridWorksReports"
                  name="gridWorksReports"
                  locale="es-US"
                  toolbar={this.toolbarOptions}
                  toolbarClick={this.clickHandler}
                  style={{
                    marginLeft: 30,
                    marginRight: 30,
                    marginTop: 10,
                    marginBottom: 20,
                    overflow: "auto",
                  }}
                  allowGrouping={true}
                  allowExcelExport={true}
                  ref={(g) => (this.grid = g)}
                  allowTextWrap={true}
                  textWrapSettings={this.wrapSettings}
                  allowSorting={true}
                  beforePrint={this.beforePrint}
                  excelQueryCellInfo={this.exportQueryCellInfo}
                  printComplete={this.printComplete}
                >
                  <ColumnsDirective>
                    <ColumnDirective
                      field="workName"
                      headerText="Obra"
                      width="100"
                      template={this.workTemplate}
                    />
                    <ColumnDirective
                      field="clientName"
                      headerText="Cliente"
                      width="100"
                      template={this.clientTemplate}
                    />
                    <ColumnDirective
                      field="workBudgetName"
                      headerText="Presupuesto(s)"
                      width="140"
                    />
                    <ColumnDirective
                      field="workType"
                      headerText="Tipo"
                      width="80"
                    />
                    <ColumnDirective
                      field="dateOpenWork"
                      headerText="Apertura"
                      format={this.format}
                      width="110"
                    />
                    <ColumnDirective
                      field="dateCloseWork"
                      headerText="Cierre"
                      format={this.format}
                      width="110"
                    />
                    <ColumnDirective
                      field="workBudgetTotalContract"
                      headerText="Total Presupuesto(s)"
                      width="160"
                    />
                    <ColumnDirective
                      field="invoicePaidSum"
                      headerText="Total Pagadas"
                      width="130"
                      format="N2"
                    />
                    <ColumnDirective
                      field="invoiceSum"
                      headerText="Total"
                      width="100"
                      format="N2"
                    />
                    <ColumnDirective
                      field="status"
                      headerText="Estado"
                      width="100"
                      template={this.statusTemplate}
                      textAlign="Center"
                    />
                    <ColumnDirective
                      field="status"
                      headerText="Estado"
                      width="100"
                      visible={false}
                    />
                  </ColumnsDirective>

                  <AggregatesDirective>
                    <AggregateDirective>
                      <AggregateColumnsDirective>
                        <AggregateColumnDirective
                          field="workBudgetTotalContract"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                          groupCaptionTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="invoicePaidSum"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                          groupCaptionTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="invoiceSum"
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

ReportWorks.propTypes = {};

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
});

export default connect(mapStateToProps, mapDispatchToProps)(ReportWorks);
