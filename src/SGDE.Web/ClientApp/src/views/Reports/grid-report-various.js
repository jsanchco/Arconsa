import React, { Component, Fragment } from "react";
import PropTypes from "prop-types";
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
// import PubSub from "pubsub-js";

L10n.load(data);

class GridReportVarious extends Component {
  grid = null;
  wrapSettings = { wrapMode: "Content" };
  // title = null;
  // titleColumn = null;
  // titleFooter = null;
  // field = null;

  constructor(props) {
    super(props);

    this.state = {
      companyName: "",
      cif: "",
      address: "",
      phoneNumber: "",
      title: "",
      titleColumn: "",
      field: "",
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
    this.updateGrid = this.updateGrid.bind(this);
    this.renderColumnsEmbargos = this.renderColumnsEmbargos.bind(this);
    this.renderColumnsAdvances = this.renderColumnsAdvances.bind(this);

    this.format = { type: "dateTime", format: "dd/MM/yyyy" };
  }

  componentDidMount() {
    // PubSub.subscribe("updateGrid", this.updateGrid);
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

  componentDidUpdate(prevProps) {
    if (prevProps.settings == null && this.props.settings == null) return;

    // if (this.hasChanges(prevProps.settings, this.props.settings)) {
    if (prevProps.settings !== this.props.settings) {
      const { settings } = this.props;
      switch (settings.textSelection) {
        case "Trabajadores":
          // this.title = "TRABAJADORES";
          // this.titleColumn = "Trabajador";
          // this.titleFooter = "Trabajadores";
          // this.field = "workerName";

          this.setState({
            title: "TRABAJADORES",
            titleColumn: "Trabajador",
            titleFooter: "Trabajadores",
            field: "workerName",
          });
          this.grid.dataSource = new DataManager({
            adaptor: new WebApiAdaptor(),
            url: `${config.URL_API}/${REPORTS_ALL}`,
            headers: [
              { Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) },
            ],
          });
          this.grid.query = new Query()
            .addParams("enterpriseId", JSON.parse(localStorage.getItem("enterprise")).id)
            .addParams("workers", true)
            .addParams("startDate", settings.start)
            .addParams("endDate", settings.end)
            .addParams("showCeros", settings.showCeros);

          this.grid.refresh();
          break;

        case "Obras":
          // this.title = "OBRAS";
          // this.titleColumn = "Obra";
          // this.titleFooter = "Obras";
          // this.field = "workName";

          this.setState({
            title: "OBRAS",
            titleColumn: "Obra",
            titleFooter: "Obras",
            field: "workName",
          });
          this.grid.dataSource = new DataManager({
            adaptor: new WebApiAdaptor(),
            url: `${config.URL_API}/${REPORTS_ALL}`,
            headers: [
              { Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) },
            ],
          });
          this.grid.query = new Query()
            .addParams("enterpriseId", JSON.parse(localStorage.getItem("enterprise")).id)
            .addParams("works", true)
            .addParams("startDate", settings.start)
            .addParams("endDate", settings.end)
            .addParams("showCeros", settings.showCeros);

          this.grid.refresh();
          break;

        case "Clientes":
          // this.title = "CLIENTES";
          // this.titleColumn = "Cliente";
          // this.titleFooter = "Clientes";
          // this.field = "clientName";

          this.setState({
            title: "CLIENTES",
            titleColumn: "Cliente",
            titleFooter: "Clientes",
            field: "clientName",
          });
          this.grid.dataSource = new DataManager({
            adaptor: new WebApiAdaptor(),
            url: `${config.URL_API}/${REPORTS_ALL}`,
            headers: [
              { Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) },
            ],
          });
          this.grid.query = new Query()
            .addParams("enterpriseId", JSON.parse(localStorage.getItem("enterprise")).id)
            .addParams("clients", true)
            .addParams("startDate", settings.start)
            .addParams("endDate", settings.end)
            .addParams("showCeros", settings.showCeros);

          this.grid.refresh();
          break;

        default:
          break;
      }
    }
  }

  updateGrid(topic, data) {
    if (topic !== "updateGrid") return;

    switch (data.textSelection) {
      case "Trabajadores":
        this.setState({
          title: "TRABAJADORES",
          titleColumn: "Trabajador",
          titleFooter: "Trabajadores",
          field: "workerName",
        });
        this.grid.dataSource = new DataManager({
          adaptor: new WebApiAdaptor(),
          url: `${config.URL_API}/${REPORTS_ALL}`,
          headers: [
            { Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) },
          ],
        });
        this.grid.query = new Query()
          .addParams("enterpriseId", JSON.parse(localStorage.getItem("enterprise")).id)
          .addParams("workers", true)
          .addParams("startDate", data.start)
          .addParams("endDate", data.end)
          .addParams("showCeros", data.showCeros);

        this.grid.refresh();
        break;

      case "Obras":
        this.setState({
          title: "OBRAS",
          titleColumn: "Obra",
          titleFooter: "Obras",
          field: "workName",
        });
        this.grid.dataSource = new DataManager({
          adaptor: new WebApiAdaptor(),
          url: `${config.URL_API}/${REPORTS_ALL}`,
          headers: [
            { Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) },
          ],
        });
        this.grid.query = new Query()
          .addParams("enterpriseId", JSON.parse(localStorage.getItem("enterprise")).id)
          .addParams("works", true)
          .addParams("startDate", data.start)
          .addParams("endDate", data.end)
          .addParams("showCeros", data.showCeros);

        this.grid.refresh();
        break;

      case "Clientes":
        this.setState({
          title: "CLIENTES",
          titleColumn: "Cliente",
          titleFooter: "Clientes",
          field: "clientName",
        });
        this.grid.dataSource = new DataManager({
          adaptor: new WebApiAdaptor(),
          url: `${config.URL_API}/${REPORTS_ALL}`,
          headers: [
            { Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) },
          ],
        });
        this.grid.query = new Query()
          .addParams("enterpriseId", JSON.parse(localStorage.getItem("enterprise")).id)
          .addParams("clients", true)
          .addParams("startDate", data.start)
          .addParams("endDate", data.end)
          .addParams("showCeros", data.showCeros);

        this.grid.refresh();
        break;

      default:
        break;
    }
  }

  hasChanges(prevProps, actualProps) {
    if (prevProps == null && actualProps == null) return false;

    if (prevProps == null && actualProps != null) return true;

    if (
      prevProps.start !== actualProps.start ||
      prevProps.end !== actualProps.end ||
      prevProps.textSelection !== actualProps.textSelection ||
      prevProps.showCeros !== actualProps.showCeros
    ) {
      return true;
    }

    return false;
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
        {/* Total: {args.Count} {this.titleFooter} */}
        Total: {args.Count} {this.state.titleFooter}
      </span>
    );
  }

  footerSum(args) {
    let amount = args.Sum.toString().replace("$", "").replace(".00", "");

    return <span>Total: {amount}</span>;
    //return <span>Total: {args.Sum} horas</span>;
  }

  footerSumEuros(args) {
    // return <span>Total: {args.Sum.toFixed(2)}€</span>;
    let amount = Number(args.Sum);
    amount = Math.round((amount + Number.EPSILON) * 100) / 100;

    if (isNaN(amount)) {
      amount = args.Sum.replace(",", "").replace("$", "");
      amount = Number(amount);
      amount = Math.round((amount + Number.EPSILON) * 100) / 100;
    }

    return <span>Total: {amount}€</span>;
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
    // let title = `INFORME de ${this.title}`;
    // let type = this.title;
    // let fileName = `Inf_${this.title}.xlsx`;

    let title = `INFORME de ${this.state.title}`;
    let type = this.state.title;
    let fileName = `Inf_${this.state.title}.xlsx`;
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
              { index: 5, value: this.props.settings.start },
              { index: 6, value: this.props.settings.end, width: 150 },
            ],
          },
        ],
      },
      fileName: fileName,
    };
  }

  beforePrint(args) {
    var div = document.createElement("Div");
    div.innerHTML = this.props.settings.textSelection;
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
        <ColumnsDirective>
          <ColumnDirective
            field={this.state.field}
            headerText={this.state.titleColumn}
            width="100"
          />

          {this.renderColumn()}

          <ColumnDirective
            field="totalHoursOrdinary"
            headerText="Ordinarias"
            width="70"
          />
          <ColumnDirective
            field="priceTotalHoursOrdinary"
            headerText="Precio Ord."
            width="70"
          />
          <ColumnDirective
            field="priceTotalHoursSaleOrdinary"
            headerText="Venta Ord."
            width="70"
          />
          <ColumnDirective
            field="totalHoursExtraordinary"
            headerText="Extra"
            width="70"
          />
          <ColumnDirective
            field="priceTotalHoursExtraordinary"
            headerText="Precio Extra"
            width="70"
          />
          <ColumnDirective
            field="priceTotalHoursSaleExtraordinary"
            headerText="Venta Extra"
            width="70"
          />
          <ColumnDirective
            field="totalHoursFestive"
            headerText="Festivo"
            width="70"
          />
          <ColumnDirective
            field="priceTotalHoursFestive"
            headerText="Precio Festivo"
            width="70"
          />
          <ColumnDirective
            field="priceTotalHoursSaleFestive"
            headerText="Venta Festivo"
            width="70"
          />
          <ColumnDirective
            field="totalHoursNocturnal"
            headerText="Nocturna"
            width="70"
          />
          <ColumnDirective
            field="priceTotalHoursNocturnal"
            headerText="Precio Nocturna"
            width="70"
          />
          <ColumnDirective
            field="priceTotalHoursSaleNocturnal"
            headerText="Venta Festivo"
            width="70"
          />
          <ColumnDirective
            field="priceDiary"
            headerText="Precio Diario"
            width="70"
          />
          <ColumnDirective
            field="priceSaleDiary"
            headerText="Venta Diario"
            width="70"
          />

          {this.renderColumnsEmbargos()}
          {this.renderColumnsAdvances()}
        </ColumnsDirective>

        <AggregatesDirective>
          <AggregateDirective>
            <AggregateColumnsDirective>
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
          services={[Group, ExcelExport, Toolbar, Sort, Aggregate, Resize]}
        />
      </GridComponent>
    );
  }
}

GridReportVarious.propTypes = {
  showMessage: PropTypes.func.isRequired,
};

export default GridReportVarious;
