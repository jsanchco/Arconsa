import React, { Component, Fragment } from "react";
import { Breadcrumb, BreadcrumbItem, Container, Row } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Edit,
  Inject,
  Toolbar,
  Page,
  ForeignKey,
  Group,
  Aggregate,
  AggregateColumnsDirective,
  AggregateColumnDirective,
  AggregateDirective,
  AggregatesDirective,
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor } from "@syncfusion/ej2-data";
import { config, COMPANY_INDIRECTCOSTS } from "../../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../../locales/locale.json";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../../actions/applicationAction";
import { TOKEN_KEY, addIndirectCosts } from "../../../services";
import ModalSelectYearMonth from "../../Modals/modal-select-year-month";

L10n.load(data);

class IndirectCosts extends Component {
  indirectCosts = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${COMPANY_INDIRECTCOSTS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  grid = null;

  requiredRules = { required: true };

  constructor(props) {
    super(props);

    this.state = {
      rowSelected: null,
      modal: false,
    };

    this.toolbarOptions = [
      "Add",
      "Edit",
      "Delete",
      "Update",
      "Cancel",
      {
        text: "Copiar mes",
        tooltipText: "Copiar mes",
        prefixIcon: "e-custom-icons e-file-workers",
        id: "copyMonth",
      },
    ];
    this.editSettings = {
      showDeleteConfirmDialog: true,
      allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      newRowPosition: "Top",
    };
    this.pageSettings = { pageCount: 10, pageSize: 50 };
    this.actionFailure = this.actionFailure.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
    this.actionBegin = this.actionBegin.bind(this);
    this.rowSelected = this.rowSelected.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
    this.updateInidrectCosts = this.updateInidrectCosts.bind(this);
    this.clickHandler = this.clickHandler.bind(this);

    this.editYear = {
      params: {
        decimals: 0,
        format: "N",
        min: 2020,
        validateDecimalOnType: true,
      },
    };

    this.months = [
      { id: 1, value: "Enero" },
      { id: 2, value: "Febrero" },
      { id: 3, value: "Marzo" },
      { id: 4, value: "Abril" },
      { id: 5, value: "Mayo" },
      { id: 6, value: "Junio" },
      { id: 7, value: "Julio" },
      { id: 8, value: "Agosto" },
      { id: 9, value: "Septiembre" },
      { id: 10, value: "Octubre" },
      { id: 11, value: "Noviembre" },
      { id: 12, value: "Diciembre" },
    ];
    this.editMonths = {
      params: {
        popupWidth: "auto",
        sortOrder: "None",
      },
    };

    this.editAmount = {
      params: {
        decimals: 2,
        format: "N",
      },
    };

    this.groupOptions = {
      columns: ["key"],
    };
  }

  actionFailure(args) {
    let error = Array.isArray(args) ? args[0].error : args.error;
    if (Array.isArray(error)) {
      error = error[0].error;
    }
    this.props.showMessage({
      statusText: error.statusText,
      responseText: error.responseText,
      type: "danger",
    });
  }

  actionComplete(args) {
    if (args.requestType === "save") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });
      this.setState({ rowSelected: null });
    }
    if (args.requestType === "delete") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });
      this.setState({ rowSelected: null });
    }
  }

  actionBegin(args) {
    if (args.requestType === "save") {
      var cols = this.grid.columns;
      for (var i = 0; i < cols.length; i++) {
        if (cols[i].type === "date") {
          var date = args.data[cols[i].field];
          if (date == null) {
            return;
          }

          args.data[cols[i].field] = new Date(
            Date.UTC(
              date.getFullYear(),
              date.getMonth(),
              date.getDate(),
              date.getHours(),
              date.getMilliseconds()
            )
          );
        }
      }
    }
  }

  rowSelected() {
    const selectedRecords = this.grid.getSelectedRecords();
    this.setState({ rowSelected: selectedRecords[0] });
  }

  footerSumEuros(args) {
    var title = args.Sum;
    if (typeof title !== "string") {
      title = title.toString();
    }

    title = title.replace(",", ".");
    const index = title.lastIndexOf(".");
    if (index >= 0) {
      title = `${title.substring(0, index)},${title.substring(
        index + 1,
        title.length
      )}`;
    }

    return <span>Total: {title}€</span>;
  }

  clickHandler(args) {
    if (args.item.id === "copyMonth") {
      this.setState({
        modal: !this.state.modal,
      });
    }
  }

  toggleModal() {
    this.setState({
      modal: !this.state.modal,
    });
  }

  updateInidrectCosts(yearOld, monthOld, yearNew, monthNew) {
    addIndirectCosts({ yearOld, monthOld, yearNew, monthNew }).then(() => {
      this.grid.refresh();
    });
  }

  render() {
    return (
      <Fragment>
        <ModalSelectYearMonth
          isOpen={this.state.modal}
          toggle={this.toggleModal}
          updateInidrectCosts={this.updateInidrectCosts}
          showMessage={this.props.showMessage}
        />

        <Breadcrumb>
          {/*eslint-disable-next-line*/}
          <BreadcrumbItem>
            <a href="/#">Inicio</a>
          </BreadcrumbItem>
          {/* eslint-disable-next-line*/}
          <BreadcrumbItem active>Gestión de Usuarios</BreadcrumbItem>
        </Breadcrumb>

        <Container fluid>
          <div className="animated fadeIn">
            <div className="card">
              <div className="card-header">
                <i className="icon-layers"></i> Gastos Indirectos
              </div>
              <div className="card-body"></div>
              <Row>
                <GridComponent
                  dataSource={this.indirectCosts}
                  locale="es-US"
                  allowPaging={true}
                  pageSettings={this.pageSettings}
                  toolbar={this.toolbarOptions}
                  editSettings={this.editSettings}
                  toolbarClick={this.clickHandler}
                  style={{
                    marginLeft: 30,
                    marginRight: 30,
                    marginTop: -20,
                    marginBottom: 20,
                  }}
                  actionFailure={this.actionFailure}
                  actionComplete={this.actionComplete}
                  actionBegin={this.actionBegin}
                  allowGrouping={true}
                  rowSelected={this.rowSelected}
                  ref={(g) => (this.grid = g)}
                  groupSettings={this.groupOptions}
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
                      field="year"
                      headerText="Año"
                      width="70"
                      edit={this.editYear}
                      textAlign="Right"
                      editType="numericedit"
                    />
                    <ColumnDirective
                      field="month"
                      headerText="Mes"
                      width="70"
                      editType="dropdownedit"
                      foreignKeyValue="value"
                      foreignKeyField="id"
                      validationRules={this.requiredRules}
                      dataSource={new DataManager(this.months)}
                      edit={this.editMonths}
                    />
                    <ColumnDirective
                      field="accountNumber"
                      headerText="Nº Cuenta"
                      width="70"
                    />
                    <ColumnDirective
                      field="description"
                      headerText="Descripción"
                      width="150"
                    />
                    <ColumnDirective
                      field="amount"
                      headerText="Cantidad"
                      width="100"
                      edit={this.editAmount}
                      textAlign="Right"
                      editType="numericedit"
                    />
                    <ColumnDirective
                      field="key"
                      headerText="Clave"
                      width="100"
                      allowEditing={false}
                    />
                    <ColumnDirective
                      field="date"
                      headerText="Fecha"
                      width="100"
                      visible={false}
                    />
                  </ColumnsDirective>

                  <AggregatesDirective>
                    <AggregateDirective>
                      <AggregateColumnsDirective>
                        <AggregateColumnDirective
                          field="amount"
                          type="Sum"
                          format="N2"
                          groupCaptionTemplate={this.footerSumEuros}
                        >
                          {" "}
                        </AggregateColumnDirective>
                      </AggregateColumnsDirective>
                    </AggregateDirective>
                  </AggregatesDirective>

                  <Inject
                    services={[
                      ForeignKey,
                      Group,
                      Page,
                      Toolbar,
                      Edit,
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

IndirectCosts.propTypes = {};

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
});

export default connect(mapStateToProps, mapDispatchToProps)(IndirectCosts);
