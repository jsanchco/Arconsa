import React, { Component } from "react";
import { Row } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Edit,
  Inject,
  Toolbar,
  Page,
  ForeignKey,
  ContextMenu,
  Group
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor } from "@syncfusion/ej2-data";
import { config, WORKS, CLIENTSWITHOUTFILTER } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import { TOKEN_KEY, updateDatesWork } from "../../services";
import ModalWorkers from "../Modals/modal-workers";
import Legend from "../../components/legend";

L10n.load(data);

class Works extends Component {
  works = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${WORKS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  clients = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${CLIENTSWITHOUTFILTER}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  grid = null;
  clientIdRules = { required: true };
  contextMenuItems = [
    { text: "Cerrar Obra", target: ".e-content", id: "closeWork" },
    { text: "Abrir Obra", target: ".e-content", id: "openWork" }
  ];
  numericParams = {
    params: {
      decimals: 0,
      format: "N",
      validateDecimalOnType: true
    }
  };
  wrapSettings = { wrapMode: "Content" };

  constructor(props) {
    super(props);

    this.state = {
      works: null,
      clients: null,
      rowSelected: null,
      rowSelectedindex: null,
      modal: false
    };

    this.toolbarOptions = [
      "Add",
      "Edit",
      "Delete",
      "Update",
      "Cancel",
      {
        text: "Detalles",
        tooltipText: "Detalles",
        prefixIcon: "e-custom-icons e-details",
        id: "Details"
      },
      {
        text: "Añadir/Quitar trabajadores",
        tooltipText: "Añadir/Quitar Trabajadores",
        prefixIcon: "e-custom-icons e-file-workers",
        id: "Workers"
      },
      "Search"
    ];
    this.editSettings = {
      showDeleteConfirmDialog: true,
      allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      newRowPosition: "Top"
    };
    this.pageSettings = { pageCount: 10, pageSize: 10 };
    this.actionFailure = this.actionFailure.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
    this.contextMenuClick = this.contextMenuClick.bind(this);
    this.rowSelected = this.rowSelected.bind(this);
    this.contextMenuOpen = this.contextMenuOpen.bind(this);
    this.openTemplate = this.openTemplate.bind(this);
    this.dateTemplate = this.dateTemplate.bind(this);
    this.clickHandler = this.clickHandler.bind(this);
    this.toggleModal = this.toggleModal.bind(this);

    this.selectionSettings = {
      checkboxMode: "ResetOnRowClick",
      type: "Single"
    };
  }

  clickHandler(args) {
    if (args.item.id === "Details") {
      const selectedRecords = this.grid.getSelectedRecords();
      if (Array.isArray(selectedRecords) && selectedRecords.length === 1) {
        this.setState({ rowSelected: selectedRecords[0] });

        this.props.history.push({
          pathname: "/works/detailwork",
          state: {
            work: selectedRecords[0]
          }
        });
      } else {
        this.setState({ rowSelected: null });
        this.props.showMessage({
          statusText: "Debes seleccionar un solo registro",
          responseText: "Debes seleccionar un solo registro",
          type: "danger"
        });
      }
    }

    if (args.item.id === "Workers") {
      const selectedRecords = this.grid.getSelectedRecords();
      if (Array.isArray(selectedRecords) && selectedRecords.length === 1) {
        this.setState({ rowSelected: selectedRecords[0] });
        this.toggleModal();
      } else {
        this.setState({ rowSelected: null });
        this.props.showMessage({
          statusText: "Debes seleccionar un solo registro",
          responseText: "Debes seleccionar un solo registro",
          type: "danger"
        });
      }
    }
  }

  toggleModal() {
    this.setState({
      modal: !this.state.modal
    });
  }

  contextMenuOpen() {
    const { rowSelected } = this.state;
    var contextMenuObj = this.grid.contextMenuModule.contextMenu;
    if (rowSelected && rowSelected.open) {
      // contextMenuObj.enableItems(["Cerrar Obra"], false);
      contextMenuObj.hideItems(["Abrir Obra"]);
      contextMenuObj.showItems(["Cerrar Obra"]);
    } else {
      // contextMenuObj.enableItems(["Abrir Obra"], false);
      contextMenuObj.hideItems(["Cerrar Obra"]);
      contextMenuObj.showItems(["Abrir Obra"]);
    }
  }

  openTemplate(args) {
    if (args.open === true) {
      return (
        <div>
          <span className="dot-green"></span>
        </div>
      );
    } else {
      return (
        <div>
          <span className="dot-red"></span>
        </div>
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

  dateTemplate(args) {
    // const titleOpen = `${this.formatDate(args.openDate)}`;
    // const titleClose = `${this.formatDate(args.closeDate)}`;
    return (
      <div>
        <div style={{ display: "flex" }}>
          <div style={{ textAlign: "left", width: "30%" }}>Aper.:</div>
          <div style={{ textAlign: "left", width: "70%" }}>{args.openDate}</div>
        </div>
        <div style={{ display: "flex" }}>
          <div style={{ textAlign: "left", width: "30%" }}>Cier.:</div>
          <div style={{ textAlign: "left", width: "70%" }}>
            {args.closeDate}
          </div>
        </div>
      </div>
    );
  }

  contextMenuClick(args) {
    const workSelected = this.state.rowSelected;
    if (args.item.id === "openWork") {
      workSelected.open = true;
      updateDatesWork(workSelected).then(() => {
        this.grid.setCellValue(workSelected.id, "closeDate", null);
        this.grid.setCellValue(
          workSelected.id,
          "openDate",
          this.formatDate(new Date())
        );
        this.grid.setCellValue(workSelected.id, "open", true);
      });
    }
    if (args.item.id === "closeWork") {
      workSelected.open = false;
      updateDatesWork(workSelected).then(() => {
        this.grid.setCellValue(
          workSelected.id,
          "closeDate",
          this.formatDate(new Date())
        );
        this.grid.setCellValue(workSelected.id, "open", false);
      });
    }
  }

  rowSelected() {
    const selectedRecords = this.grid.getSelectedRecords();
    const selectedRowIndex = this.grid.getSelectedRowIndexes();
    this.setState({
      rowSelected: selectedRecords[0],
      rowSelectedindex: selectedRowIndex[0]
    });
  }

  actionFailure(args) {
    let error = Array.isArray(args) ? args[0].error : args.error;
    if (Array.isArray(error)) {
      error = error[0].error;
    }
    this.props.showMessage({
      statusText: error.statusText,
      responseText: error.responseText,
      type: "danger"
    });
  }

  actionComplete(args) {
    if (args.requestType === "save") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success"
      });
    }
    if (args.requestType === "delete") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success"
      });
    }
  }

  headerCellInfo(args) {
    args.node.getElementsByClassName("e-checkbox-wrapper")[0] &&
      args.node.getElementsByClassName("e-checkbox-wrapper")[0].remove();
  }

  render() {
    return (
      <div className="animated fadeIn" id="target-works">
        <ModalWorkers
          isOpen={this.state.modal}
          toggle={this.toggleModal}
          workSelected={this.state.rowSelected}
          showMessage={this.props.showMessage}
        />
        <div className="card">
          <div className="card-header">
            <i className="icon-globe"></i> Obras
          </div>
          <div className="card-body"></div>

          <div
            style={{
              marginLeft: "35px",
              marginTop: "-20px",
              marginBottom: "30px"
            }}
          >
            <Legend
              elements={[
                { color: "dot-green", text: "Obra Abierta" },
                { color: "dot-red", text: "Obra Cerrada" }
              ]}
            />
          </div>

          <Row>
            <GridComponent
              dataSource={this.works}
              locale="es-US"
              allowPaging={true}
              pageSettings={this.pageSettings}
              toolbar={this.toolbarOptions}
              toolbarClick={this.clickHandler}
              editSettings={this.editSettings}
              style={{
                marginLeft: 30,
                marginRight: 30,
                marginTop: -20,
                marginBottom: 20
              }}
              actionFailure={this.actionFailure}
              actionComplete={this.actionComplete}
              allowGrouping={true}
              rowSelected={this.rowSelected}
              ref={g => (this.grid = g)}
              contextMenuItems={this.contextMenuItems}
              contextMenuOpen={this.contextMenuOpen}
              contextMenuClick={this.contextMenuClick}
              selectionSettings={this.selectionSettings}
              headerCellInfo={this.headerCellInfo}
              allowTextWrap={true}
              textWrapSettings={this.wrapSettings}
            >
              <ColumnsDirective>
                <ColumnDirective type="checkbox" width="50"></ColumnDirective>
                <ColumnDirective
                  field="id"
                  headerText="Id"
                  width="40"
                  isPrimaryKey={true}
                  isIdentity={true}
                  visible={false}
                />
                <ColumnDirective field="name" headerText="Nombre" width="100" />
                <ColumnDirective
                  field="address"
                  headerText="Dirección"
                  width="200"
                />
                {/* <ColumnDirective
                  field="estimatedDuration"
                  headerText="Duración Estimada"
                  width="100"
                /> */}
                <ColumnDirective
                  field="worksToRealize"
                  headerText="Tipo"
                  width="50"
                />
                <ColumnDirective
                  field="numberPersonsRequested"
                  headerText="Personas"
                  width="50"
                  textAlign="right"
                  allowEditing={false}
                />
                <ColumnDirective
                  field="clientId"
                  headerText="Cliente"
                  width="100"
                  editType="dropdownedit"
                  foreignKeyValue="name"
                  foreignKeyField="id"
                  validationRules={this.clientIdRules}
                  dataSource={this.clients}
                />
                <ColumnDirective
                  field="closeDate"
                  headerText="Fechas"
                  width="100"
                  template={this.dateTemplate}
                  textAlign="Center"
                  allowEditing={false}
                  defaultValue={true}
                />
                <ColumnDirective
                  field="open"
                  headerText="Ab./Cer."
                  width="100"
                  template={this.openTemplate}
                  textAlign="Center"
                  allowEditing={false}
                />
                <ColumnDirective field="openDate" visible={false} />
                <ColumnDirective field="closeDate" visible={false} />
              </ColumnsDirective>
              <Inject
                services={[Group, ContextMenu, ForeignKey, Page, Toolbar, Edit]}
              />
            </GridComponent>
          </Row>
        </div>
      </div>
    );
  }
}

Works.propTypes = {};

const mapStateToProps = state => {
  return {};
};

const mapDispatchToProps = dispatch => ({
  showMessage: message => dispatch(ACTION_APPLICATION.showMessage(message))
});

export default connect(mapStateToProps, mapDispatchToProps)(Works);
