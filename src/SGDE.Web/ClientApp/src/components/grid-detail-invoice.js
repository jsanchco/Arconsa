import React, { Component } from "react";
import PropTypes from "prop-types";
import { getDetailInvoiceByHoursWoker } from "../services";
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
import { setValue } from "@syncfusion/ej2-base";

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
    ];

    this.editSettings = {
      showDeleteConfirmDialog: true,
      allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      newRowPosition: "Bottom",
    };

    this.actionComplete = this.actionComplete.bind(this);
    this.clickHandler = this.clickHandler.bind(this);
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
    const sum = (args.units * args.priceUnity).toFixed(2);
    // setValue("total", sum, args.id);    
    
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
            nameUnit: result[cont - 1].nameUnit,
            priceUnity: result[cont - 1].priceUnity,
          });
        }
        this.gridDetailInvoice.dataSource = dataSource;
        this.props.updateDataSourceDetailInvoce(dataSource);
      });
    }
  }

  footerSumEuros(args) {
    return <span>Total: {args.Sum}€</span>;
  }

  dataBound() {
    if (!this.gridDetailInvoice) {
      return;
    }

    const dataSource = this.gridDetailInvoice.dataSource;
    this.gridDetailInvoice.sortColumn("id", "Ascending");
    this.props.updateDataSourceDetailInvoce(dataSource);
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
              field="units"
              headerText="Unidades"
              width="100"
              fotmat="N2"
              textAlign="right"
              editType="numericedit"
              edit={this.numericParams}
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
};

export default GridDetailInvoice;
