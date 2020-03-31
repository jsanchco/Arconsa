import React, { Component } from "react";
import PropTypes from "prop-types";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Edit,
  Inject,
  Toolbar,
  Group,
  ExcelExport,
  Aggregate,
  AggregateColumnsDirective,
  AggregateColumnDirective,
  AggregateDirective,
  AggregatesDirective
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, REPORTS } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY, getSettings } from "../../services";
import { COMPANY_DATA } from "../../constants";

L10n.load(data);

class GridSelection extends Component {
  grid = null;

  constructor(props) {
    super(props);

    this.state = {
      companyName: "",
      cif: "",
      address: "",
      phoneNumber: ""
    };

    this.toolbarOptions = [
      "Print",
      "ExcelExport",
      {
        text: "Colapsar",
        tooltipText: "Colapsar todas las Filas",
        prefixIcon: "e-custom-icons e-file-upload",
        id: "CollapseAll"
      }
    ];

    this.renderWorker = this.renderWorker.bind(this);
    this.renderWork = this.renderWork.bind(this);
    this.renderClient = this.renderClient.bind(this);
    this.clickHandler = this.clickHandler.bind(this);
    this.getExcelExportProperties = this.getExcelExportProperties.bind(this);
  }

  componentDidMount() {
    getSettings(COMPANY_DATA)
      .then(result => {
        const data = JSON.parse(result.data);
        this.setState({
          companyName: data.companyName,
          cif: data.cif,
          address: data.address,
          phoneNumber: data.phoneNumber
        });
      })
      .catch(error => {
        this.props.showMessage({
          statusText: error,
          responseText: error,
          type: "danger"
        });
      });
  }

  componentDidUpdate(prevProps) {
    if (prevProps.settings !== this.props.settings) {
      const { settings } = this.props;
      switch (settings.type) {
        case "workers":
          this.grid.dataSource = new DataManager({
            adaptor: new WebApiAdaptor(),
            url: `${config.URL_API}/${REPORTS}`,
            headers: [
              { Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }
            ]
          });
          this.grid.query = new Query()
            .addParams("workerId", settings.selection)
            .addParams("startDate", settings.start)
            .addParams("endDate", settings.end);

          this.grid.refresh();
          break;

        case "works":
          this.grid.dataSource = new DataManager({
            adaptor: new WebApiAdaptor(),
            url: `${config.URL_API}/${REPORTS}`,
            headers: [
              { Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }
            ]
          });
          this.grid.query = new Query()
            .addParams("workId", settings.selection)
            .addParams("startDate", settings.start)
            .addParams("endDate", settings.end);

          this.grid.refresh();
          break;

        case "clients":
          this.grid.dataSource = new DataManager({
            adaptor: new WebApiAdaptor(),
            url: `${config.URL_API}/${REPORTS}`,
            headers: [
              { Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }
            ]
          });
          this.grid.query = new Query()
            .addParams("clientId", settings.selection)
            .addParams("startDate", settings.start)
            .addParams("endDate", settings.end);

          this.grid.refresh();
          break;

        default:
          break;
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
    return <span>Total: {args.Sum} horas</span>;
  }

  footerSumEuros(args) {
    return <span>Total: {args.Sum}€</span>;
  }

  renderWorker() {
    if (
      this.props.settings !== null &&
      this.props.settings !== undefined &&
      this.props.settings.type === "workers"
    ) {
      return null;
    } else {
      return (
        <ColumnDirective field="userName" headerText="Trabajador" width="100" />
      );
    }
  }

  renderWork() {
    if (
      this.props.settings !== null &&
      this.props.settings !== undefined &&
      this.props.settings.type === "works"
    ) {
      return null;
    } else {
      return <ColumnDirective field="workName" headerText="Obra" width="100" />;
    }
  }

  renderClient() {
    if (
      this.props.settings !== null &&
      this.props.settings !== undefined &&
      this.props.settings.type === "clients"
    ) {
      return null;
    } else {
      return (
        <ColumnDirective field="clientName" headerText="Cliente" width="100" />
      );
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
    let title = "INFORME por ";
    const date = this.formatDate(new Date());
    let type = "";
    const { settings } = this.props;

    console.log("settings ->", settings);

    switch (settings.type) {
      case "workers":
        title = title + "TRABAJADOR";
        type = "TRABAJADOR";
        break;
      case "works":
        title = title + "OBRA";
        type = "OBRA";
        break;
      case "clients":
        title = title + "CLIENTE";
        type = "CLIENTE";
        break;

      default:
        break;
    }

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
                  bold: true
                }
              }
            ]
          },
          {
            index: 3,
            cells: [
              {
                index: 1,
                colSpan: 3,
                value: this.state.companyName,
                style: { fontColor: "#C67878", fontSize: 15, bold: true }
              },
              {
                index: 5,
                colSpan: 2,
                value: type,
                style: { fontColor: "#C67878", bold: true }
              },
              {
                index: 7,
                value: "FECHA",
                style: { fontColor: "#C67878", bold: true },
                width: 150
              }
            ]
          },
          {
            index: 4,
            cells: [
              { index: 1, colSpan: 3, value: this.state.address },
              { index: 5, colSpan: 2, value: settings.textSelection },
              {
                index: 7,
                value: date,
                width: 150
              }
            ]
          },
          {
            index: 5,
            cells: [
              {
                index: 1,
                colSpan: 3,
                value: this.state.phoneNumber
              },
              {
                index: 5,
                value: "Fecha Inicio",
                style: { fontColor: "#C67878", bold: true }
              },
              {
                index: 6,
                value: "Fecha Fin",
                width: 150,
                style: { fontColor: "#C67878", bold: true }
              }
            ]
          },
          {
            index: 6,
            cells: [
              { index: 5, value: settings.start },
              { index: 6, value: settings.end, width: 150 }
            ]
          }
        ]
      },

      footer: {
        footerRows: 5,
        rows: [
          {
            cells: [
              {
                colSpan: 6,
                value: "Thank you for your business!",
                style: { fontColor: "#C67878", hAlign: "Center", bold: true }
              }
            ]
          },
          {
            cells: [
              {
                colSpan: 6,
                value: "!Visit Again!",
                style: { fontColor: "#C67878", hAlign: "Center", bold: true }
              }
            ]
          }
        ]
      },
      fileName: "exceldocument.xlsx"
    };
  }

  render() {
    return (
      <div className="control-pane">
        <div className="control-section">
          <div>
            <GridComponent
              id="Grid"
              //dataSource={null}
              locale="es-US"
              toolbar={this.toolbarOptions}
              toolbarClick={this.clickHandler}
              style={{
                marginLeft: 30,
                marginRight: 30,
                marginTop: 10,
                marginBottom: 20
              }}
              allowGrouping={true}
              allowExcelExport={true}
              rowSelected={this.rowSelected}
              ref={g => (this.grid = g)}
            >
              <ColumnsDirective>
                {this.renderClient()}

                {this.renderWork()}

                {this.renderWorker()}

                <ColumnDirective
                  field="dateHour"
                  headerText="Fecha"
                  width="100"
                />
                <ColumnDirective
                  field="hours"
                  headerText="Horas"
                  width="100"
                  fotmat="N1"
                  textAlign="right"
                  editType="numericedit"
                />
                <ColumnDirective
                  field="hourTypeName"
                  headerText="Tipo"
                  width="100"
                />
                <ColumnDirective
                  field="priceHour"
                  headerText="Precio Coste"
                  width="100"
                  fotmat="C1"
                  textAlign="right"
                  editType="numericedit"
                />
                <ColumnDirective
                  field="priceHourSale"
                  headerText="Precio Venta"
                  width="100"
                  fotmat="C1"
                  textAlign="right"
                  editType="numericedit"
                />
              </ColumnsDirective>

              <AggregatesDirective>
                <AggregateDirective>
                  <AggregateColumnsDirective>
                    <AggregateColumnDirective
                      field="hours"
                      type="Sum"
                      format="C2"
                      footerTemplate={this.footerSum}
                    >
                      {" "}
                    </AggregateColumnDirective>

                    <AggregateColumnDirective
                      field="priceHour"
                      type="Sum"
                      format="C2"
                      footerTemplate={this.footerSumEuros}
                    >
                      {" "}
                    </AggregateColumnDirective>

                    <AggregateColumnDirective
                      field="priceHourSale"
                      type="Sum"
                      format="C2"
                      footerTemplate={this.footerSumEuros}
                    >
                      {" "}
                    </AggregateColumnDirective>
                  </AggregateColumnsDirective>
                </AggregateDirective>

                <AggregateDirective>
                  <AggregateColumnsDirective>
                    <AggregateColumnDirective
                      field="hours"
                      type="Sum"
                      groupCaptionTemplate={this.footerSum}
                    >
                      {" "}
                    </AggregateColumnDirective>
                  </AggregateColumnsDirective>
                </AggregateDirective>

                <AggregateDirective>
                  <AggregateColumnsDirective>
                    <AggregateColumnDirective
                      field="priceHour"
                      type="Sum"
                      groupCaptionTemplate={this.footerSumEuros}
                    >
                      {" "}
                    </AggregateColumnDirective>
                  </AggregateColumnsDirective>
                </AggregateDirective>

                <AggregateDirective>
                  <AggregateColumnsDirective>
                    <AggregateColumnDirective
                      field="priceHourSale"
                      type="Sum"
                      groupCaptionTemplate={this.footerSumEuros}
                    >
                      {" "}
                    </AggregateColumnDirective>
                  </AggregateColumnsDirective>
                </AggregateDirective>
              </AggregatesDirective>

              <Inject
                services={[Group, ExcelExport, Toolbar, Edit, Aggregate]}
              />
            </GridComponent>
          </div>
        </div>
      </div>
    );
  }
}

GridSelection.propTypes = {
  //settings: PropTypes.object.isRequired,
  showMessage: PropTypes.func.isRequired
};

export default GridSelection;
