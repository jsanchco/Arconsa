import React, { Component, Fragment } from "react";
import { Row } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Edit,
  Inject,
  Toolbar,
  Page,
} from "@syncfusion/ej2-react-grids";
import { DateTimePickerComponent } from "@syncfusion/ej2-react-calendars";
import { getValue } from "@syncfusion/ej2-base";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import {
  config,
  DAILYSIGNINGS,
  USERSHIRING,
  HOURTYPES,
  PROFESSIONSBYUSER,
} from "../../constants";
import { loadCldr, L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY, getUser } from "../../services";
import ModalMassiveSigning from "../Modals/modal-massive-signing";
import { DialogComponent } from "@syncfusion/ej2-react-popups";
import { removeAllDailySigning } from "../../services";

import * as numberingSystems from "cldr-data/supplemental/numberingSystems.json";
import * as gregorian from "cldr-data/main/es-US/ca-gregorian.json";
import * as numbers from "cldr-data/main/es-US/numbers.json";
import * as timeZoneNames from "cldr-data/main/es-US/timeZoneNames.json";

loadCldr(numberingSystems, gregorian, numbers, timeZoneNames);

L10n.load(data);

class DailySignings extends Component {
  dailySignings = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${DAILYSIGNINGS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  userHirings = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${USERSHIRING}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  hourTypes = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${HOURTYPES}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  professions = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${PROFESSIONSBYUSER}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  userHiringIdRules = { required: true };
  hourTypeIdRules = { required: true };
  grid = null;

  wrapSettings = { wrapMode: "Content" };

  constructor(props) {
    super(props);

    this.state = {
      dailySignings: null,
      userHirings: null,
      professions: null,
      rowSelected: null,
      modal: false,
      hideConfirmDialog: false,
      user: {
        fullname: ""
      }      
    };

    this.toolbarOptions = [
      "Add",
      "Edit",
      // "Delete",
      "Update",
      "Cancel",
      // {
      //   text: "Plantilla Automática",
      //   tooltipText: "Plantilla para a generación de Fichajes Automática",
      //   prefixIcon: "e-custom-icons e-details",
      //   id: "Template",
      // },
      {
        text: "Borrar Seleccionados",
        tooltipText: "Borrar registros seleccionados",
        prefixIcon: "e-custom-icons e-remove",
        id: "RemoveAll",
      },
      "Print",
    ];
    this.editSettings = {
      showDeleteConfirmDialog: true,
      allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      newRowPosition: "Top",
    };
    this.pageSettings = { pageCount: 10, pageSize: 10 };
    this.actionFailure = this.actionFailure.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
    this.actionBegin = this.actionBegin.bind(this);
    this.rowSelected = this.rowSelected.bind(this);
    this.startHourTemplate = this.startHourTemplate.bind(this);
    this.formatDate = this.formatDate.bind(this);
    this.clickHandler = this.clickHandler.bind(this);
    this.rowDataBound = this.rowDataBound.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
    this.updateDailySignings = this.updateDailySignings.bind(this);
    this.beforePrint = this.beforePrint.bind(this);
    this.dateTemplateStartHour = this.dateTemplateStartHour.bind(this);
    this.dateTemplateEndHour = this.dateTemplateEndHour.bind(this);
    this.dateTemplateTotalHours = this.dateTemplateTotalHours.bind(this);

    this.template = this.gridTemplate;
    this.format = { type: "dateTime", format: "dd/MM/yyyy HH:mm" };    

    this.queryDailySignings = new Query().addParams("userId", props.userId);
    this.editProfessions = {
      params: {
        query: new Query().addParams("userId", props.userId),
        idth: "auto"
      }
    };
    this.editWorks = {
      params: {
        idth: "auto"
      }
    };

    this.animationSettings = { effect: "None" };
    this.confirmButton = [
      {
        click: () => {
          const selectedRecords = this.grid.getSelectedRecords();
          if (Array.isArray(selectedRecords) && selectedRecords.length > 0) {
            let result = selectedRecords.map((a) => a.id);
            removeAllDailySigning(result)
              .then(() => {
                this.setState({ hideConfirmDialog: false });
                this.updateDailySignings();
              })
              .catch(() => {
                this.setState({ hideConfirmDialog: false });
                this.updateDailySignings();
              });
          }
        },
        buttonModel: { content: "Si", isPrimary: true },
      },
      {
        click: () => {
          this.setState({ hideConfirmDialog: false });
        },
        buttonModel: { content: "No" },
      },
    ];
  }

  componentDidMount() {
    getUser(this.props.userId).then((result) => {
      this.setState({
        user: {
          fullname: result.fullname,
        },
      });
    });
  }

  gridTemplate(args) {
    if (args.file !== null && args.file !== "") {
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

  dialogClose() {
    this.setState({
      hideConfirmDialog: false,
    });
  }

  formatDate(args) {
    var date = new Date(args);
    var month = ("0" + (date.getMonth() + 1)).slice(-2);
    var day = ("0" + date.getDate()).slice(-2);
    var hours = ("0" + date.getHours()).slice(-2);
    var minutes = ("0" + date.getMinutes()).slice(-2);

    return `${[day, month, date.getFullYear()].join("/")} ${hours}:${minutes}`;
  }

  formatDateWithOutTime(args) {
    var date = new Date(args);
    var month = ("0" + (date.getMonth() + 1)).slice(-2);
    var day = ("0" + date.getDate()).slice(-2);
    
    return `${[day, month, date.getFullYear()].join("/")}`;
  }

  actionBegin(args) {
    if (args.requestType === "add" || args.requestType === "beginEdit") {
      this.grid.columns[1].edit.params.query.params = [];
      this.grid.columns[1].edit.params.query.addParams(
        "userId",
        this.props.userId
      );
    }

    // if (args.requestType === "save") {
    //   let date = this.formatDate(args.data.startHour);
    //   args.data.startHour = date;

    //   if (
    //     args.data.endHour !== null &&
    //     args.data.endHour !== "" &&
    //     args.data.endHour !== undefined
    //   ) {
    //     date = this.formatDate(args.data.endHour);
    //     args.data.endHour = date;
    //   }
    // }
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
      this.grid.refresh();
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });
    }
    if (args.requestType === "delete") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });
    }
  }

  clickHandler(args) {
    if (args.item.id === "Template") {
      this.toggleModal();
    }
    if (args.item.id === "RemoveAll") {
      this.setState({ hideConfirmDialog: true });
    }
  }

  toggleModal() {
    this.setState({
      modal: !this.state.modal,
    });
  }

  startHourTemplate(args) {
    return (
      <DateTimePickerComponent
        value={getValue("startHour", args)}
        id="startHour"
        placeholder="Hora Inicio"
        floatLabelType="Never"
        format="dd/MM/yyyy HH:mm"
      />
    );
  }

  endHourTemplate(args) {
    return (
      <DateTimePickerComponent
        value={getValue("endHour", args)}
        id="endHour"
        placeholder="Hora Fin"
        floatLabelType="Never"
        format="dd/MM/yyyy HH:mm"
      />
    );
  }

  rowSelected() {
    const selectedRecords = this.grid.getSelectedRecords();
    this.setState({ rowSelected: selectedRecords[0] });
  }

  updateDailySignings() {
    this.grid.refresh();
  }

  rowDataBound(args) {
    if (args.row) {
      var dataStartHour = getValue("startHour", args.data);
      var startDate = new Date(dataStartHour);
      switch (startDate.getDay()) {
        case 0:
        case 6:
          args.row.classList.add("color-green");
          break;

        default:
          break;
      }
    }
  }

  beforePrint(args) {
    var div = document.createElement("Div");
    div.innerHTML = this.state.user.fullname;
    div.style.textAlign = "center";
    div.style.color = "red";
    div.style.padding = "10px 0";
    div.style.fontSize = "25px";
    args.element.insertBefore(div, args.element.childNodes[0]);
  }

  dateTemplateStartHour(args) {
    let value = args.startHour;
    if (args.hourTypeId === 5) {      
      value = this.formatDateWithOutTime(value);
    }
    else {
      value = this.formatDate(value);
    }
    
    return <div>{value}</div>;
  }

  dateTemplateEndHour(args) {
    let value = args.endHour;
    value = this.formatDate(value);
    if (args.hourTypeId === 5) {
      value = "";
    }

    return <div>{value}</div>;
  }

  dateTemplateTotalHours(args) {
    let value = args.totalHours;
    if (args.hourTypeId === 5) {
      value = "";
    }

    return <div>{value}</div>;
  }

  render() {
    return (
      <Fragment>
        <DialogComponent
          id="confirmDialogRemoveAll"
          header="Eliminar Todos"
          visible={this.state.hideConfirmDialog}
          showCloseIcon={true}
          animationSettings={this.animationSettings}
          width="500px"
          content="¿Estás seguro de eliminar estos fichajes?"
          ref={(dialog) => (this.confirmDialogInstance = dialog)}
          target="#target-daily-signing"
          buttons={this.confirmButton}
          close={this.dialogClose.bind(this)}
        ></DialogComponent>

        <ModalMassiveSigning
          isOpen={this.state.modal}
          toggle={this.toggleModal}
          userId={this.props.userId}
          showMessage={this.props.showMessage}
          updateDailySignings={this.updateDailySignings}
        />

        <div className="animated fadeIn" id="target-daily-signing">
          <div
            className="card"
            style={{ marginRight: "60px", marginTop: "20px" }}
          >
            <div className="card-header">
              <i className="icon-layers"></i> Fichajes
            </div>
            <div className="card-body"></div>
            <Row>
              <GridComponent
                dataSource={this.dailySignings}
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
                  marginBottom: 20,
                }}
                actionFailure={this.actionFailure}
                actionComplete={this.actionComplete}
                actionBegin={this.actionBegin}
                allowGrouping={false}
                rowSelected={this.rowSelected}
                rowDataBound={this.rowDataBound}
                ref={(g) => (this.grid = g)}
                query={this.queryDailySignings}
                allowTextWrap={true}
                textWrapSettings={this.wrapSettings}
                beforePrint={this.beforePrint}
              >
                <ColumnsDirective>
                  <ColumnDirective type="checkbox" width="50"></ColumnDirective>
                  <ColumnDirective
                    field="userHiringId"
                    headerText="Obra"
                    width="100"
                    editType="dropdownedit"
                    validationRules={this.userHiringIdRules}
                    textAlign="Left"
                    dataSource={this.userHirings}
                    foreignKeyValue="name"
                    foreignKeyField="id"
                    edit={this.editWorks}
                  />
                  <ColumnDirective
                    field="professionId"
                    headerText="Puesto"
                    width="100"
                    editType="dropdownedit"
                    foreignKeyValue="name"
                    foreignKeyField="id"
                    dataSource={this.professions}
                    edit={this.editProfessions}
                    allowFiltering={true}
                  />
                  <ColumnDirective
                    field="id"
                    headerText="Id"
                    width="40"
                    isPrimaryKey={true}
                    isIdentity={true}
                    visible={false}
                  />
                  <ColumnDirective
                    field="hourTypeId"
                    headerText="Tipo de Hora"
                    width="100"
                    editType="dropdownedit"
                    validationRules={this.hourTypeIdRules}
                    textAlign="Center"
                    dataSource={this.hourTypes}
                    foreignKeyValue="name"
                    foreignKeyField="id"
                    defaultValue={1}
                  />                  
                  <ColumnDirective
                    field="startHour"
                    headerText="Hora Inicio"
                    width="100"
                    editType="datetimepickeredit"
                    type="date"
                    format={this.format}
                    textAlign="Center"
                    template={this.dateTemplateStartHour}
                  />
                  <ColumnDirective
                    field="endHour"
                    headerText="Hora Fin"
                    width="100"
                    editType="datetimepickeredit"
                    type="date"
                    format={this.format}
                    textAlign="Center"
                    template={this.dateTemplateEndHour}
                  />
                  <ColumnDirective
                    field="totalHours"
                    headerText="Horas Totales"
                    width="100"
                    allowEditing={false}
                    textAlign="Right"
                    template={this.dateTemplateTotalHours}
                  />
                </ColumnsDirective>
                <Inject services={[Page, Toolbar, Edit]} />
              </GridComponent>
            </Row>
          </div>
        </div>
      </Fragment>
    );
  }
}

DailySignings.propTypes = {};

export default DailySignings;
