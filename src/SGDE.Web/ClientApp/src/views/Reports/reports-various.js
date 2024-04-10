import React, { Component, Fragment } from "react";
import PropTypes from "prop-types";
import {
  Breadcrumb,
  BreadcrumbItem,
  Container,
  Form,
  FormGroup,
  Label,
  Button,
  Row,
  Col,
} from "reactstrap";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import { AppSwitch } from "@coreui/react";
import { DatePickerComponent } from "@syncfusion/ej2-react-calendars";
import { DropDownListComponent } from "@syncfusion/ej2-react-dropdowns";
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
  Resize,
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, REPORTS_ALL } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY, getSettings } from "../../services";
import { COMPANY_DATA } from "../../constants";

L10n.load(data);

class ReportsVarious extends Component {
  dtpStartDate = null;
  dtpEndDate = null;
  ddl = null;
  grid = null;
  element = null;
  title = null;

  wrapSettings = { wrapMode: "Content" };
  fields = { text: "name", value: "id" };

  constructor(props) {
    super(props);

    this.state = {
      companyName: "",
      cif: "",
      address: "",
      phoneNumber: "",
    };

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

    this.clickHandler = this.clickHandler.bind(this);
    this.getExcelExportProperties = this.getExcelExportProperties.bind(this);
    this.renderColumn = this.renderColumn.bind(this);
    this.footerCount = this.footerCount.bind(this);
    this.beforePrint = this.beforePrint.bind(this);
    this.renderColumnsEmbargos = this.renderColumnsEmbargos.bind(this);
    this.renderColumnsAdvances = this.renderColumnsAdvances.bind(this);
    this.handleClickSelection = this.handleClickSelection.bind(this);
    this.handleOnClick = this.handleOnClick.bind(this);
    this.handleInputChange = this.handleInputChange.bind(this);

    this.format = { type: "dateTime", format: "dd/MM/yyyy" };
  }

  componentDidMount() {
    this.ddl.dataSource = ["Trabajadores", "Obras", "Clientes"];
    const enterprise = JSON.parse(localStorage.getItem("enterprise"))

    this.setState({
      companyName: enterprise.companyName,
      cif: enterprise.cif,
      address: enterprise.address,
      phoneNumber: enterprise.phoneNumber,
    });  

    // getSettings(COMPANY_DATA)
    //   .then((result) => {
    //     const data = JSON.parse(result.data);
    //     this.setState({
    //       companyName: data.companyName,
    //       cif: data.cif,
    //       address: data.address,
    //       phoneNumber: data.phoneNumber,
    //     });
    //   })
    //   .catch((error) => {
    //     this.props.showMessage({
    //       statusText: error,
    //       responseText: error,
    //       type: "danger",
    //     });
    //   });
  }

  handleOnClick() {
    const valueDtpStartDate = this.formatDate(this.dtpStartDate.value);
    const valueDtpEndDate = this.formatDate(this.dtpEndDate.value);
    const textDdl = this.ddl.text;

    if (textDdl === null) {
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
        this.grid.dataSource = new DataManager({
          adaptor: new WebApiAdaptor(),
          url: `${config.URL_API}/${REPORTS_ALL}`,
          headers: [
            { Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) },
          ],
        });

        switch (textDdl) {
          case "Trabajadores":
            this.grid.query = new Query()
              .addParams("enterpriseId", JSON.parse(localStorage.getItem("enterprise")).id)
              .addParams("workers", true)
              .addParams("startDate", valueDtpStartDate)
              .addParams("endDate", valueDtpEndDate)
              .addParams("showCeros", this.state.showCeros);

            break;
          case "Obras":
            this.grid.query = new Query()
              .addParams("enterpriseId", JSON.parse(localStorage.getItem("enterprise")).id)
              .addParams("works", true)
              .addParams("startDate", valueDtpStartDate)
              .addParams("endDate", valueDtpEndDate)
              .addParams("showCeros", this.state.showCeros);

            break;
          case "Clientes":
            this.grid.query = new Query()
              .addParams("enterpriseId", JSON.parse(localStorage.getItem("enterprise")).id)
              .addParams("clients", true)
              .addParams("startDate", valueDtpStartDate)
              .addParams("endDate", valueDtpEndDate)
              .addParams("showCeros", this.state.showCeros);

            break;

          default:
            break;
        }
      }
    }
  }

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;

    if (name === "show_ceros") {
      this.setState({ showCeros: !target.checked });
    }
  }

  handleClickSelection(event) {
    switch (event.value) {
      case "Trabajadores":
        this.title = "TRABAJADORES";
        this.grid.columnModel[0].field = "workerName";
        this.grid.columnModel[0].headerText = "Trabajador";
        this.grid.getColumnByField("totalWorkers").visible = false;
        // this.grid.getColumnByField("hasEmbargosPendings").visible = true;
        // this.grid.getColumnByField("hasAdvancesPendings").visible = true;
        this.grid.getColumnByField("totalEmbargos").visible = true;
        this.grid.getColumnByField("totalAdvances").visible = true;
        this.grid.refreshHeader();

        this.grid.dataSource = null;

        break;
      case "Obras":
        this.title = "OBRAS";
        this.grid.columnModel[0].field = "workName";
        this.grid.columnModel[0].headerText = "Obra";
        this.grid.getColumnByField("totalWorkers").visible = true;
        // this.grid.getColumnByField("hasEmbargosPendings").visible = false;
        // this.grid.getColumnByField("hasAdvancesPendings").visible = false;
        this.grid.getColumnByField("totalEmbargos").visible = false;
        this.grid.getColumnByField("totalAdvances").visible = false;
        this.grid.refreshHeader();

        this.grid.dataSource = null;

        break;
      case "Clientes":
        this.title = "CLIENTES";
        this.grid.columnModel[0].field = "clientName";
        this.grid.columnModel[0].headerText = "Cliente";
        this.grid.getColumnByField("totalWorkers").visible = true;
        // this.grid.getColumnByField("hasEmbargosPendings").visible = false;
        // this.grid.getColumnByField("hasAdvancesPendings").visible = false;
        this.grid.getColumnByField("totalEmbargos").visible = false;
        this.grid.getColumnByField("totalAdvances").visible = false;
        this.grid.refreshHeader();

        this.grid.dataSource = null;

        break;

      default:
        break;
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

  footerCount(args) {
    return (
      <span>
        Total: {args.Count} {this.state.titleFooter}
      </span>
    );
  }

  footerSum(args) {
    let amount = args.Sum.toString().replace("$", "").replace(".00", "");

    return <span>Total: {amount}</span>;
  }

  footerSumEuros(args) {
    let amount = Number(args.Sum);
    amount = Math.round((amount + Number.EPSILON) * 100) / 100;

    if (isNaN(amount)) {
      amount = args.Sum.replace(",", "").replace("$", "");
      amount = Number(amount);
      amount = Math.round((amount + Number.EPSILON) * 100) / 100;
    }

    return <span>Total: {amount}€</span>;
  }

  getExcelExportProperties() {
    let title = `INFORME de ${this.ddl.text.toUpperCase()}`;
    let type = this.ddl.text;
    let fileName = `Inf_${this.ddl.text}.xlsx`;
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
              { index: 5, value: this.formatDate(this.dtpStartDate.value) },
              {
                index: 6,
                value: this.formatDate(this.dtpEndDate.value),
                width: 150,
              },
            ],
          },
        ],
      },
      fileName: fileName,
    };
  }

  beforePrint(args) {
    var div = document.createElement("Div");
    div.innerHTML = this.ddl.text;
    div.style.textAlign = "center";
    div.style.color = "red";
    div.style.padding = "10px 0";
    div.style.fontSize = "25px";
    args.element.insertBefore(div, args.element.childNodes[0]);
  }

  templateHasEmbargosPendings(args) {
    if (args.hasEmbargosPendings) {
      return <span>Si</span>;
    } else {
      return <span>No</span>;
    }
  }

  templateHasAdvancesPendings(args) {
    if (args.hasAdvancesPendings) {
      return <span>Si</span>;
    } else {
      return <span>No</span>;
    }
  }

  renderColumnsEmbargos() {
    if (this.state.titleColumn === "") {
      return null;
    }

    switch (this.state.titleColumn) {
      case "Trabajador":
        return (
          <ColumnDirective
            field="hasEmbargosPendings"
            headerText="Embargos"
            template={this.templateHasEmbargosPendings}
            width="70"
          />
        );

      default:
        return null;
    }
  }

  renderColumnsAdvances() {
    if (this.state.titleColumn === "") {
      return null;
    }

    switch (this.state.titleColumn) {
      case "Trabajador":
        return (
          <ColumnDirective
            field="hasAdvancesPendings"
            headerText="Adelantos"
            template={this.templateHasAdvancesPendings}
            width="70"
          />
        );

      default:
        return null;
    }
  }

  renderColumn() {
    if (this.state.titleColumn === "") {
      return null;
    }

    switch (this.state.titleColumn) {
      case "Obra":
        return (
          <ColumnDirective
            field="totalWorkers"
            headerText="Nº Trab."
            width="70"
          />
        );

      default:
        return null;
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
          <BreadcrumbItem active>Informes Varios</BreadcrumbItem>
        </Breadcrumb>

        <Container fluid>
          <div className="animated fadeIn" id="selection-report">
            <div className="card">
              <div className="card-header">
                <i className="icon-list"></i> Informes Varios
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
                        <Label for="lists">Listados</Label>
                        <DropDownListComponent
                          id="lists"
                          dataSource={null}
                          placeholder={`Selecciona listado`}
                          fields={this.fields}
                          ref={(g) => (this.ddl = g)}
                          change={this.handleClickSelection}
                        />
                      </FormGroup>
                    </Col>
                    <Col xs="3">
                      <FormGroup style={{ marginTop: "30px" }}>
                        <Label
                          htmlFor="show_ceros"
                          style={{ verticalAlign: "bottom" }}
                        >
                          Mostrar registos con ceros&nbsp;
                        </Label>
                        <AppSwitch
                          variant={"pill"}
                          color={"primary"}
                          label
                          id="show_ceros"
                          name="show_ceros"
                          placeholder="mostrar registos con ceros"
                          onChange={this.handleInputChange}
                          dataOn="No"
                          dataOff="Si"
                          // checked={!this.state.showCeros}
                        />
                      </FormGroup>
                    </Col>
                  </Row>
                  <Row>
                    <Col xs="10"></Col>
                    <Col xs="2">
                      <Button
                        id="consultar"
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

              <Row style={{ marginTop: "10px" }} id="row-grid">
                <GridComponent
                  id="gridReportsVarious"
                  locale="es"
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
                  allowResizing={true}
                  beforePrint={this.beforePrint}
                >
                  {/* 
                  title: "TRABAJADORES",
                  titleColumn: "Trabajador",
                  titleFooter: "Trabajadores",
                  field: "workerName",
                */}
                  <ColumnsDirective>
                    <ColumnDirective width="120" />
                    <ColumnDirective
                      field="totalWorkers"
                      headerText="Nº Trab."
                      width="110"
                      visible={false}
                    />
                    <ColumnDirective
                      field="totalHoursOrdinary"
                      headerText="Ordinarias"
                      width="120"
                    />
                    <ColumnDirective
                      field="priceTotalHoursOrdinary"
                      headerText="Precio Ord."
                      width="120"
                    />
                    <ColumnDirective
                      field="priceTotalHoursSaleOrdinary"
                      headerText="Venta Ord."
                      width="120"
                    />
                    <ColumnDirective
                      field="totalHoursExtraordinary"
                      headerText="Extra"
                      width="100"
                    />
                    <ColumnDirective
                      field="priceTotalHoursExtraordinary"
                      headerText="Precio Extra"
                      width="130"
                    />
                    <ColumnDirective
                      field="priceTotalHoursSaleExtraordinary"
                      headerText="Venta Extra"
                      width="120"
                    />
                    <ColumnDirective
                      field="totalHoursFestive"
                      headerText="Festivo"
                      width="100"
                    />
                    <ColumnDirective
                      field="priceTotalHoursFestive"
                      headerText="Precio Festivo"
                      width="135"
                    />
                    <ColumnDirective
                      field="priceTotalHoursSaleFestive"
                      headerText="Venta Festivo"
                      width="130"
                    />
                    <ColumnDirective
                      field="totalHoursNocturnal"
                      headerText="Nocturna"
                      width="110"
                    />
                    <ColumnDirective
                      field="priceTotalHoursNocturnal"
                      headerText="Precio Nocturna"
                      width="145"
                    />
                    <ColumnDirective
                      field="priceTotalHoursSaleNocturnal"
                      headerText="Venta Nocturna"
                      width="145"
                    />
                    <ColumnDirective
                      field="priceDiary"
                      headerText="Precio Diario"
                      width="130"
                    />
                    <ColumnDirective
                      field="priceSaleDiary"
                      headerText="Venta Diario"
                      width="130"
                    />
                    <ColumnDirective
                      field="totalEmbargos"
                      headerText="Embargos"
                      // template={this.templateHasEmbargosPendings}
                      width="120"
                      visible={false}
                    />
                    <ColumnDirective
                      field="totalAdvances"
                      headerText="Adelantos"
                      // template={this.templateHasAdvancesPendings}
                      width="120"
                      visible={false}
                    />
                    {/* <ColumnDirective
                      field="hasEmbargosPendings"
                      headerText="Embargos"
                      template={this.templateHasEmbargosPendings}
                      width="70"
                      visible={false}
                    />
                    <ColumnDirective
                      field="hasAdvancesPendings"
                      headerText="Adelantos"
                      template={this.templateHasAdvancesPendings}
                      width="70"
                      visible={false}
                    /> */}
                  </ColumnsDirective>

                  <AggregatesDirective>
                    <AggregateDirective>
                      <AggregateColumnsDirective>
                        <AggregateColumnDirective
                          field="totalWorkers"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSum}
                        >
                          {" "}
                        </AggregateColumnDirective>
                        <AggregateColumnDirective
                          field="totalHoursOrdinary"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSum}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="priceTotalHoursOrdinary"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="priceTotalHoursSaleOrdinary"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="totalHoursExtraordinary"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSum}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="priceTotalHoursExtraordinary"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="priceTotalHoursSaleExtraordinary"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>
                        <AggregateColumnDirective
                          field="totalHoursFestive"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSum}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="priceTotalHoursFestive"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="priceTotalHoursSaleFestive"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>
                        <AggregateColumnDirective
                          field="totalHoursNocturnal"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSum}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="priceTotalHoursNocturnal"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="priceTotalHoursSaleNocturnal"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="priceDiary"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>

                        <AggregateColumnDirective
                          field="priceSaleDiary"
                          type="Sum"
                          format="N2"
                          footerTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>
                      </AggregateColumnsDirective>
                    </AggregateDirective>
                  </AggregatesDirective>

                  <Inject
                    services={[
                      Group,
                      ExcelExport,
                      Toolbar,
                      Sort,
                      Aggregate,
                      Resize,
                    ]}
                  />
                </GridComponent>{" "}
              </Row>
            </div>
          </div>
        </Container>
      </Fragment>
    );
  }
}

ReportsVarious.propTypes = { showMessage: PropTypes.func.isRequired };

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
});

export default connect(mapStateToProps, mapDispatchToProps)(ReportsVarious);
