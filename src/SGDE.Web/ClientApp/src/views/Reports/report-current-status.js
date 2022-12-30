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
  Page,
} from "@syncfusion/ej2-react-grids";
import { connect } from "react-redux";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, COMPANY_DATA, REPORT_CURRENT_STATUS } from "../../constants";
import { TOKEN_KEY, getSettings } from "../../services";
import ACTION_APPLICATION from "../../actions/applicationAction";

class ReportCurrentStatus extends Component {
  currentStatus = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${REPORT_CURRENT_STATUS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  dtpStartDate = null;
  dtpEndDate = null;
  grid = null;

  wrapSettings = { wrapMode: "Content" };

  constructor(props) {
    super(props);

    this.toolbarOptions = ["Print", "ExcelExport", "Search"];

    this.pageSettings = {
      pageCount: 10,
      pageSize: 10,
    };

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
          url: `${config.URL_API}/${REPORT_CURRENT_STATUS}`,
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
    let title = "INFORME del ESTADO ACTUAL de OBRAS";
    let type = "ESTADO ACTUAL";
    let fileName = "Inf_Seg.xlsx";
    const date = this.formatDate(new Date());
    const valueDtpStartDate = this.formatDate(new Date(2021, 0, 1));
    const valueDtpEndDate = this.formatDate(new Date());

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
                value: "Fecha Inicio",
                style: { fontColor: "#C67878", bold: true },
              },
              {
                index: 6,
                value: "Fecha Fin",
                width: 150,
                style: { fontColor: "#C67878", bold: true },
              },
            ],
          },
          {
            index: 6,
            cells: [
              { index: 5, value: valueDtpStartDate },
              { index: 6, value: valueDtpEndDate, width: 150 },
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
    const valueDtpStartDate = this.formatDate(new Date(2021, 0, 1));
    const valueDtpEndDate = this.formatDate(new Date());

    var div = document.createElement("Div");
    div.innerHTML =
      "ESTADO ACTUAL de OBRAS del " + valueDtpStartDate + " al " + valueDtpEndDate;
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

  dataBound(args) {
    console.log("Count Items");
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
            Informe de Resultados
          </BreadcrumbItem>
        </Breadcrumb>

        <Container fluid>
          <div className="animated fadeIn" id="selection-report">
            <div className="card">
              <div className="card-header">
                <i className="icon-list"></i> Informe de Resultados
              </div>
              <div className="card-body"></div>
              {/* <div>
                <Form style={{ marginLeft: "20px" }}>
                  <Row>
                    <Col xs="5">
                      <FormGroup>
                        <Label for="startDate">Fecha Inicio</Label>
                        <DatePickerComponent
                          id="startDate"
                          ref={(g) => (this.dtpStartDate = g)}
                          format="dd/MM/yyyy"
                          value={
                            new Date(
                              new Date().getFullYear(),
                              new Date().getMonth(),
                              1
                            )
                          }
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
                          value={new Date()}
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
              </div> */}

              <Row>
                <GridComponent
                  dataSource={this.currentStatus}
                  id="GridCurrentStatus"
                  locale="es-US"
                  allowPaging={true}
                  pageSettings={this.pageSettings}
                  toolbar={this.toolbarOptions}
                  toolbarClick={this.clickHandler}
                  style={{
                    marginLeft: 30,
                    marginRight: 30,
                    marginTop: -20,
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
                  dataBound={this.dataBound}
                >
                  <ColumnsDirective>
                    <ColumnDirective
                      field="workId"
                      isPrimaryKey={true}
                      isIdentity={true}
                      visible={false}
                    />
                    <ColumnDirective
                      field="workName"
                      headerText="Obra"
                      width="110"
                      template={this.workTemplate}
                    />
                    <ColumnDirective
                      field="clientName"
                      headerText="Cliente"
                      width="100"
                      template={this.clientTemplate}
                    />
                    <ColumnDirective
                      field="workBudgetsName"
                      headerText="Presupuesto(s)"
                      width="150"
                      template={this.workBudgetTemplate}
                    />
                    <ColumnDirective
                      field="openDate"
                      headerText="Apert. Obra"
                      // format={this.format}
                      width="150"
                    />
                    <ColumnDirective
                      field="closeDate"
                      headerText="Cierre Obra"
                      // format={this.format}
                      width="150"
                    />
                    <ColumnDirective
                      field="workBudgetsSum"
                      headerText="Total Presupuesto(s)"
                      width="180"
                      textAlign="right"
                    />
                    <ColumnDirective
                      field="invoicesSum"
                      headerText="Facturación"
                      width="180"
                      textAlign="right"
                    />
                    <ColumnDirective
                      field="workCostsSum"
                      headerText="Gastos Prov."
                      width="180"
                      textAlign="right"
                    />
                    <ColumnDirective
                      field="authorizeCancelWorkersCostsSum"
                      headerText="Gastos Trab."
                      width="180"
                      textAlign="right"
                    />
                    <ColumnDirective
                      field="indirectCostsSum"
                      headerText="Gastos Indirect."
                      width="180"
                      textAlign="right"
                    />
                    <ColumnDirective
                      field="total"
                      headerText="Total"
                      width="180"
                      textAlign="right"
                    />
                  </ColumnsDirective>

                  <AggregatesDirective>
                    <AggregateDirective>
                      <AggregateColumnsDirective>
                        <AggregateColumnDirective
                          field="workBudgetsSum"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                          groupCaptionTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="invoicesSum"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                          groupCaptionTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="workCostsSum"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                          groupCaptionTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="authorizeCancelWorkersCostsSum"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                          groupCaptionTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="indirectCostsSum"
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
                    services={[
                      Page,
                      Group,
                      ExcelExport,
                      Toolbar,
                      Sort,
                      Aggregate,
                    ]}
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

ReportCurrentStatus.propTypes = {};

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ReportCurrentStatus);
