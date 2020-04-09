import React, { Component } from "react";
import PropTypes from "prop-types";
import {
  getDetailInvoiceByHoursWoker,
  importPreviousInvoice,
} from "../services";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Inject,
  ForeignKey,
  Sort,
  Aggregate,
  AggregateColumnsDirective,
  AggregateColumnDirective,
  AggregateDirective,
  AggregatesDirective,
} from "@syncfusion/ej2-react-grids";

class GridDetailInvoice extends Component {
  gridDetailInvoice = null;

  detailInvoice = [];

  numericParams = {
    params: {
      decimals: 2,
      format: "N",
      validateDecimalOnType: true,
    },
  };

  sortingOptions = {
    columns: [{ field: "id", direction: "Ascending" }],
  };

  constructor(props) {
    super(props);

    this.toolbarOptions = [
      "Add",
      "Edit",
      "Delete",
      "Update",
      "Cancel",
      {
        text: "Detalle por Horas",
        tooltipText: "detalle por horas",
        prefixIcon: "e-custom-icons e-details",
        id: "DetailByHours",
      },
      {
        text: "Limpiar",
        tooltipText: "limpiar",
        prefixIcon: "e-custom-icons e-empty",
        id: "EmptyDetail",
      },
      {
        text: "Importar Factura Anterior",
        tooltipText: "importar factura anterior",
        prefixIcon: "e-custom-icons e-file-workers",
        id: "PreviousInvoice",
      },
      {
        text: "Importar Datos desde Excel",
        tooltipText: "importar datos desde excel",
        prefixIcon: "e-custom-icons e-import-from-excel",
        id: "ImportFromExcel",
      },
    ];

    this.editSettings = {
      showDeleteConfirmDialog: true,
      allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      newRowPosition: "Bottom",
    };

    this.selectedRow = null;

    this.actionComplete = this.actionComplete.bind(this);
    this.clickHandler = this.clickHandler.bind(this);
    this.footerSumEuros = this.footerSumEuros.bind(this);
    this.templateTotal = this.templateTotal.bind(this);
    this.rowSelected = this.rowSelected.bind(this);
  }

  componentDidUpdate(prevProps) {
    if (
      prevProps.dataSourceDetailInvoce !== this.props.dataSourceDetailInvoce
    ) {
      this.gridDetailInvoice.dataSource = this.props.dataSourceDetailInvoce;
      this.props.updateDataSourceDetailInvoce(
        this.props.dataSourceDetailInvoce
      );
    }
  }

  templateTotal(args) {
    const sum = (args.unitsTotal * args.priceUnity).toFixed(2);
    return <div>{sum}</div>;
  }

  actionComplete(args) {
    if (args.requestType === "save") {
      const dataSource = this.gridDetailInvoice.dataSource;
      if (!args.data.id) {
        const listId = dataSource.map((x) => {
          return x.id ? x.id : 0;
        });
        args.data.id = Math.max(...listId) + 1;
      }

      const sumUnits = Number(
        (args.data.units + args.data.unitsAccumulated).toFixed(2)
      );
      this.gridDetailInvoice.dataSource.find(
        (x) => x.id === args.data.id
      ).unitsTotal = sumUnits;

      const total = Number(
        (
          (args.data.units + args.data.unitsAccumulated) *
          args.data.priceUnity
        ).toFixed(2)
      );
      this.gridDetailInvoice.dataSource.find(
        (x) => x.id === args.data.id
      ).total = total;

      this.gridDetailInvoice.sortColumn("id", "Ascending");
      this.props.updateDataSourceDetailInvoce(dataSource);
    }
    if (args.requestType === "delete") {
      const dataSource = this.gridDetailInvoice.dataSource;
      this.props.updateDataSourceDetailInvoce(dataSource);
      this.gridDetailInvoice.sortColumn("id", "Ascending");
    }
  }

  clickHandler(args) {
    if (args.item.id === "DetailByHours") {
      const data = this.props.invoiceQuery;
      getDetailInvoiceByHoursWoker(data).then((result) => {
        let dataSource = [];
        for (let cont = 1; cont <= result.length; cont++) {
          dataSource.push({
            id: cont,
            servicesPerformed: result[cont - 1].servicesPerformed,
            units: result[cont - 1].units,
            unitsAccumulated: 0,
            unitsTotal: result[cont - 1].units,
            nameUnit: result[cont - 1].nameUnit,
            priceUnity: result[cont - 1].priceUnity,
            total: Number(
              (result[cont - 1].units * result[cont - 1].priceUnity).toFixed(2)
            ),
          });
        }
        this.gridDetailInvoice.dataSource = dataSource;
        this.props.updateDataSourceDetailInvoce(dataSource);
      });
    }

    if (args.item.id === "EmptyDetail") {
      this.detailInvoice = [];
      this.gridDetailInvoice.dataSource = [];
    }

    if (args.item.id === "PreviousInvoice") {
      importPreviousInvoice({
        workId: this.props.workId,
        startDate: this.props.startDate,
      }).then((result) => {
        this.gridDetailInvoice.dataSource = result.detailInvoice;
        this.props.updateDataSourceDetailInvoce(result.detailInvoice);
      });
    }

    if (args.item.id === "ImportFromExcel") {
      navigator.clipboard.readText().then((text) => {
        let lines = text.split("\n");
        let dataSource = [];
        for (let i = 0; i < lines.length; i++) {
          let fields = lines[i].split("\t");
          if (fields.length !== 6) {
            continue;
          }

          dataSource.push({
            id: i + 1,
            servicesPerformed: fields[0],
            units: fields[1],
            unitsAccumulated: fields[2],
            unitsTotal: fields[3],
            nameUnit: fields[4],
            priceUnity: fields[5],
            total: Number((fields[1] * fields[5]).toFixed(2)),
          });
        }
        this.detailInvoice = dataSource;
        this.gridDetailInvoice.dataSource = dataSource;
      });
    }
  }

  footerSumUnits(args) {
    return <span>Total: {args.Sum.toFixed(2)}</span>;
  }

  footerSumEuros(args) {
    return <span>Total: {args.Sum.toFixed(2)}€</span>;
  }

  footerTaxBase(args) {
    return <span>B. Imponible: {args.Custom}€</span>;
  }

  dataBound() {
    if (!this.gridDetailInvoice) {
      return;
    }

    const dataSource = this.gridDetailInvoice.dataSource;
    this.gridDetailInvoice.sortColumn("id", "Ascending");
    this.props.updateDataSourceDetailInvoce(dataSource);
  }

  rowSelected() {
    if (this.grid) {
      this.selectedRow = this.grid.getSelectedRecords()[0];
    }
  }

  customAggregateFn(args) {
    let total = 0;
    for (let cont = 0; cont < args.result.length; cont++) {
      total += Number((args.result[cont].units * args.result[cont].priceUnity));
    }

    return total.toFixed(2);
  }

  render() {
    return (
      <div>
        <GridComponent
          dataSource={this.detailInvoice}
          locale="es-US"
          toolbar={this.toolbarOptions}
          toolbarClick={this.clickHandler}
          style={{
            marginLeft: 30,
            marginRight: 30,
            marginTop: 20,
            marginBottom: 20,
          }}
          ref={(g) => (this.gridDetailInvoice = g)}
          editSettings={this.editSettings}
          actionComplete={this.actionComplete}
          dataBound={this.dataBound}
          sortSettings={this.sortingOptions}
          allowSorting={true}
          rowSelected={this.rowSelected}
          allowTextWrap={true}
          textWrapSettings={this.wrapSettings}
        >
          <ColumnsDirective>
            <ColumnDirective
              field="id"
              headerText="Id"
              width="40"
              isPrimaryKey={true}
              isIdentity={true}
              visible={false}
            />
            <ColumnDirective
              field="servicesPerformed"
              headerText="Servicios Prestados"
              width="100"
              textAlign="left"
            />
            <ColumnDirective
              headerText="Unidades"
              textAlign="Center"
              columns={[
                {
                  field: "unitsAccumulated",
                  headerText: "Acumuladas",
                  width: "100",
                  fotmat: "N2",
                  textAlign: "left",
                  editType: "numericedit",
                  edit: this.numericParams,
                  allowEditing: false,
                  defaultValue: 0,
                },
                {
                  field: "units",
                  headerText: "Añadidas",
                  width: "100",
                  fotmat: "N2",
                  textAlign: "left",
                  editType: "numericedit",
                  edit: this.numericParams,
                },
                {
                  field: "unitsTotal",
                  headerText: "Total uds.",
                  width: "100",
                  fotmat: "N2",
                  textAlign: "left",
                  editType: "numericedit",
                  edit: this.numericParams,
                  allowEditing: false,
                },
              ]}
            />
            <ColumnDirective
              field="nameUnit"
              headerText="Nombre Unidades"
              width="100"
              textAlign="center"
            />
            <ColumnDirective
              field="priceUnity"
              headerText="Precio Unidad"
              width="100"
              fotmat="N2"
              textAlign="right"
              editType="numericedit"
              edit={this.numericParams}
            />
            <ColumnDirective
              field="total"
              headerText="Total"
              width="100"
              fotmat="N2"
              allowEditing={false}
              editType="numericedit"
              edit={this.numericParams}
              template={this.templateTotal}
              textAlign="right"
            />
          </ColumnsDirective>

          <AggregatesDirective>
            <AggregateDirective>
              <AggregateColumnsDirective>
                <AggregateColumnDirective
                  field="units"
                  type="Sum"
                  format="N2"
                  footerTemplate={this.footerSumUnits}
                >
                  {" "}
                </AggregateColumnDirective>

                <AggregateColumnDirective
                  field="unitsAccumulated"
                  type="Sum"
                  format="N2"
                  footerTemplate={this.footerSumUnits}
                >
                  {" "}
                </AggregateColumnDirective>

                <AggregateColumnDirective
                  field="unitsTotal"
                  type="Sum"
                  format="N2"
                  footerTemplate={this.footerSumUnits}
                >
                  {" "}
                </AggregateColumnDirective>

                <AggregateColumnDirective
                  field="total"
                  type="Sum"
                  format="N2"
                  footerTemplate={this.footerSumEuros}
                >
                  {" "}
                </AggregateColumnDirective>
              </AggregateColumnsDirective>
            </AggregateDirective>

            <AggregateDirective>
              <AggregateColumnsDirective>
                <AggregateColumnDirective
                  field="total"
                  footerTemplate={this.footerTaxBase}
                  type="Custom"
                  customAggregate={this.customAggregateFn}
                />
              </AggregateColumnsDirective>
            </AggregateDirective>
          </AggregatesDirective>

          <Inject services={[ForeignKey, Sort, Aggregate]} />
        </GridComponent>
      </div>
    );
  }
}

GridDetailInvoice.propTypes = {
  invoiceQuery: PropTypes.object.isRequired,
  showMessage: PropTypes.func.isRequired,
  updateDataSourceDetailInvoce: PropTypes.func.isRequired,
  dataSourceDetailInvoce: PropTypes.array,
  workId: PropTypes.number,
  startDate: PropTypes.string,
};

export default GridDetailInvoice;
