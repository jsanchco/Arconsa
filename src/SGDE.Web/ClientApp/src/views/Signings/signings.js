import React, { Component, Fragment } from "react";
import {
  Breadcrumb,
  BreadcrumbItem,
  Button,
  Container,
  Form,
  FormGroup,
  Label,
  Row,
  Col,
} from "reactstrap";
import {
  sendMassiveSigning,
  viewMassiveSigning,
  getWorkers,
  getWorksByUserId,
  getProfessionsByUserId,
} from "../../services";
import { DatePickerComponent } from "@syncfusion/ej2-react-calendars";
import { DropDownListComponent } from "@syncfusion/ej2-react-dropdowns";
import { DialogComponent } from "@syncfusion/ej2-react-popups";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Inject,
  ForeignKey,
  Aggregate,
  AggregateColumnsDirective,
  AggregateColumnDirective,
  AggregateDirective,
  AggregatesDirective,
  Sort,
} from "@syncfusion/ej2-react-grids";
import {
  createSpinner,
  showSpinner,
  hideSpinner,
} from "@syncfusion/ej2-popups";
import { Query } from "@syncfusion/ej2-data";
import { AppSwitch } from "@coreui/react";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import { L10n, getValue } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";

L10n.load(data);

class Signings extends Component {
  fields = { text: "name", value: "id" };
  ddlEmployees = null;
  ddlProfessions = null;
  ddlWorks = null;
  grid = null;
  dsGridResult = null;

  hoursRules = {
    // regex: ["^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", "Hora mal formateada"],
    required: false,
  };
  hourTypeIdRules = { required: false };

  baseHours = [
    { id: 1, startHour: "08:00", endHour: "17:00", hourTypeId: 1, total: 9 },
  ];
  baseHoursDaily = [
    { id: 1, startHour: null, endHour: null, hourTypeId: 5, total: null },
  ];
  hourTypes = [
    { id: 1, name: "Hora Ordinaria" },
    { id: 2, name: "Hora Extra" },
    { id: 3, name: "Hora Festivo" },
    { id: 4, name: "Hora Nocturna" },
    { id: 5, name: "Diario" },
  ];

  constructor(props) {
    super(props);

    this.element = null;

    this.state = {
      hideConfirmDialog: false,
      includeSaturdays: false,
      includeSundays: false,
      rowSelected: null,
      dsGrid: null,
      dsGridResult: null,
      visibleColumns: true,
      isDaily: false,
    };

    this.confirmButton = [
      {
        click: () => {
          this.setState({ hideConfirmDialog: false });
          this.sendMassiveSigning();
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

    this.toolbarOptions = ["Add", "Edit", "Delete", "Update", "Cancel"];
    this.toolbarOptionsGridResult = [
      "Add",
      "Edit",
      {
        text: "Borrar Seleccionados",
        tooltipText: "Borrar registros seleccionados",
        prefixIcon: "e-custom-icons e-remove",
        id: "RemoveAll",
      },
      // "Delete",
      "Update",
      "Cancel",
    ];

    this.editSettings = {
      showDeleteConfirmDialog: true,
      allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      newRowPosition: "Bottom",
    };
    this.hourTypeIdRules = { required: true };

    this.animationSettings = { effect: "None" };

    this.handleOnClickView = this.handleOnClickView.bind(this);
    this.handleOnClickSave = this.handleOnClickSave.bind(this);
    this.viewMassiveSigning = this.viewMassiveSigning.bind(this);
    this.sendMassiveSigning = this.sendMassiveSigning.bind(this);
    this.formatDate = this.formatDate.bind(this);
    this.sumHours = this.sumHours.bind(this);
    this.templateSumHours = this.templateSumHours.bind(this);
    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleSelectEmployee = this.handleSelectEmployee.bind(this);
    this.rowSelected = this.rowSelected.bind(this);
    this.actionBegin = this.actionBegin.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
    this.footerSumHours = this.footerSumHours.bind(this);
    this.rowDataBoundGridResult = this.rowDataBoundGridResult.bind(this);
    this.clickHandlerGridResult = this.clickHandlerGridResult.bind(this);

    this.changeHourType = {
      params: { change: this.changeHour.bind(this), sortOrder: "none" },
    };
    this.format = { type: "dateTime", format: "dd/MM/yyyy HH:mm" };
  }

  componentDidMount() {
    const enterpriseId = JSON.parse(localStorage.getItem("enterprise")).id;
    getWorkers(enterpriseId).then((items) => {
      this.ddlEmployees.dataSource = items;
      this.searchDataEmployees = items;
    });
    this.setState({ dsGrid: this.baseHours });
  }

  rowSelected() {
    const selectedRecords = this.grid.getSelectedRecords();
    this.setState({
      rowSelected: selectedRecords[0],
    });
  }

  handleFilteringEmployees(e) {
    let query = new Query();
    query =
      e.text !== "" ? query.where("name", "contains", e.text, true) : query;
    e.updateData(this.searchDataEmployees, query);
  }

  handleFilteringWorks(e) {
    let query = new Query();
    query =
      e.text !== "" ? query.where("name", "contains", e.text, true) : query;
    e.updateData(this.searchDataWorks, query);
  }

  handleSelectEmployee(args) {
    const element = document.getElementById("target-signings");
    createSpinner({
      target: element,
    });
    showSpinner(element);

    getWorksByUserId(args.itemData.id)
      .then((items) => {
        this.ddlWorks.value = "";
        this.ddlWorks.text = null;
        this.ddlWorks.dataSource = items;
        this.searchDataWorks = items;

        hideSpinner(element);
      })
      .catch((error) => {
        hideSpinner(element);
      });

    showSpinner(element);

    getProfessionsByUserId(args.itemData.id)
      .then((items) => {
        this.ddlProfessions.value = "";
        this.ddlProfessions.text = null;
        this.ddlProfessions.dataSource = items;

        hideSpinner(element);
      })
      .catch((error) => {
        hideSpinner(element);
      });
  }

  handleOnClickView() {
    const valueDtpStartDate = this.formatDate(this.dtpStartDate.value);
    const valueDtpEndDate = this.formatDate(this.dtpEndDate.value);
    const valueDdlEmployees = this.ddlEmployees.value;
    const valueDdlProfessions = this.ddlProfessions.value;
    const valueDdlWorks = this.ddlWorks.value;

    if (
      valueDtpStartDate === null ||
      valueDtpStartDate === "" ||
      valueDtpEndDate === null ||
      valueDtpEndDate === "" ||
      valueDdlEmployees === null ||
      valueDdlProfessions === null ||
      valueDdlWorks === null
    ) {
      this.props.showMessage({
        statusText: "Consulta mal configurada",
        responseText: "Consulta mal configurada",
        type: "danger",
      });

      return;
    }

    // this.setState({ hideConfirmDialog: true });
    this.viewMassiveSigning();
  }

  handleOnClickSave() {
    this.setState({ hideConfirmDialog: true });
  }

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;

    if (name === "includeSaturdays") {
      this.setState({
        includeSaturdays: target.checked,
      });
    }

    if (name === "includeSundays") {
      this.setState({
        includeSundays: target.checked,
      });
    }
  }

  changeHour(event) {
    if (event.value === 5) {
      this.grid.dataSource = this.baseHoursDaily;
      this.grid.columns[2].visible = false;
      this.grid.columns[3].visible = false;
      this.grid.columns[4].visible = false;
      this.grid.refresh();
    } else {
      if (this.grid.dataSource[0].hourTypeId === 5) {
        this.grid.dataSource = this.baseHours;
        this.grid.columns[2].visible = true;
        this.grid.columns[3].visible = true;
        this.grid.columns[4].visible = true;
        this.grid.refresh();
      }
    }
  }

  dialogClose() {
    this.setState({
      hideConfirmDialog: false,
    });
  }

  dialogCloseDeleteAll() {
    this.setState({
      hideConfirmDialogDeleteAll: false,
    });
  }

  viewMassiveSigning() {
    const element = document.getElementById("target-signings");
    createSpinner({
      target: element,
    });
    showSpinner(element);

    const valueDtpStartDate = this.formatDate(this.dtpStartDate.value);
    const valueDtpEndDate = this.formatDate(this.dtpEndDate.value);
    const valueDdlProfessions = this.ddlProfessions.value;
    const valueDdlWorks = this.ddlWorks.value;

    viewMassiveSigning({
      userHiringId: valueDdlWorks,
      professionId: valueDdlProfessions,
      startSigning: valueDtpStartDate,
      endSigning: valueDtpEndDate,
      data: this.grid.getCurrentViewRecords(),
      includeSaturdays: this.state.includeSaturdays,
      includeSundays: this.state.includeSundays,
    })
      .then((result) => {
        //this.setState( { dsGridResult: result } );
        this.gridResult.dataSource = result;
        this.gridResult.refresh();
        hideSpinner(element);
      })
      .catch((error) => {
        hideSpinner(element);
      });
  }

  sendMassiveSigning() {
    const element = document.getElementById("target-signings");
    createSpinner({
      target: element,
    });
    showSpinner(element);

    const valueDtpStartDate = this.formatDate(this.dtpStartDate.value);
    const valueDtpEndDate = this.formatDate(this.dtpEndDate.value);
    const valueDdlProfessions = this.ddlProfessions.value;
    const valueDdlWorks = this.ddlWorks.value;

    sendMassiveSigning({
      userHiringId: valueDdlWorks,
      professionId: valueDdlProfessions,
      startSigning: valueDtpStartDate,
      endSigning: valueDtpEndDate,
      data: this.transformCurrentViewRecords(
        this.gridResult.getCurrentViewRecords()
      ),
      includeSaturdays: this.state.includeSaturdays,
      includeSundays: this.state.includeSundays,
    })
      .then((result) => {
        hideSpinner(element);
      })
      .catch((error) => {
        hideSpinner(element);
      });
  }

  transformCurrentViewRecords(args) {
    for (var i = 0; i < args.length; i++) {
      let date = new Date(args[i].startHour);
      args[i].startHour = new Date(
        Date.UTC(
          date.getFullYear(),
          date.getMonth(),
          date.getDate(),
          date.getHours(),
          date.getMilliseconds()
        ));

      date = new Date(args[i].endHour);
      args[i].endHour = new Date(
        Date.UTC(
          date.getFullYear(),
          date.getMonth(),
          date.getDate(),
          date.getHours(),
          date.getMilliseconds()
        ));
    }
    return args;
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

  templateSumHours(args) {
    if (args.startHour != null && args.endHour != null) {
      return <div>{this.sumHours(args.startHour, args.endHour)}</div>;
    }
  }

  sumHours(startHour, endHour) {
    let hours = 0;
    if (startHour <= endHour) {
      const now = new Date();
      const dateStart = new Date(
        now.getFullYear(),
        now.getMonth(),
        now.getDate(),
        startHour.substring(0, 2),
        startHour.substring(3, 5)
      );
      const dateEnd = new Date(
        now.getFullYear(),
        now.getMonth(),
        now.getDate(),
        endHour.substring(0, 2),
        endHour.substring(3, 5)
      );
      const milliseconds = Math.abs(dateEnd - dateStart);
      hours = milliseconds / 36e5;
    } else {
      const now = new Date();
      const dateStart = new Date(
        now.getFullYear(),
        now.getMonth(),
        now.getDate(),
        startHour.substring(0, 2),
        startHour.substring(3, 5)
      );
      let dateEnd = new Date(
        now.getFullYear(),
        now.getMonth(),
        now.getDate(),
        endHour.substring(0, 2),
        endHour.substring(3, 5)
      );
      dateEnd.setDate(dateEnd.getDate() + 1);
      const milliseconds = Math.abs(dateEnd - dateStart);
      hours = milliseconds / 36e5;
    }

    return hours;
  }

  dateTemplateTotalHours(args) {
    let value = args.totalHours;
    if (args.hourTypeId === 5) {
      value = "";
    }

    return <div>{value}</div>;
  }

  // formatDateEU(arg) {
  //   if (arg instanceof Date) {
  //     let day = arg.getDate();
  //     if (day < 10) day = "0" + day;

  //     const month = arg.getMonth() + 1;
  //     const year = arg.getFullYear();

  //     if (month < 10) {
  //       return `0${month}/0${day}/${year}`;
  //     } else {
  //       return `${month}/${day}/${year}`;
  //     }
  //   }

  //   return arg;
  // }

  actionBegin(args) {
    if (args.requestType === "add") {
      let values = this.gridResult.dataSource.map((a) => a.id);
      args.data.id = Math.max(...values) + 1;
    }

    // if (args.requestType === "save") {
    //   var cols = this.gridResult.columns;
    //   for (var i = 0; i < cols.length; i++) {
    //     if (cols[i].type === "date") {
    //       var date = args.data[cols[i].field];
    //       if (date !== null) {
    //         args.data[cols[i].field] = new Date(
    //           Date.UTC(
    //             date.getFullYear(),
    //             date.getMonth(),
    //             date.getDate(),
    //             date.getHours(),
    //             date.getMilliseconds()
    //           )
    //         );
    //       }
    //     }
    //   }
    // }

    // if (args.requestType === "save" && args.data.hourTypeId === 5) {
    //   args.data.startHour = new Date(
    //     args.data.startHour.getFullYear(),
    //     args.data.startHour.getMonth(),
    //     args.data.startHour.getDate()
    //   );
    //   args.data.endHour = null;
    //   args.data.totalHours = null;

    //   // this.gridResult.dataSource[args.index].startHour = args.data.startHour;
    //   // this.gridResult.dataSource[args.index].endHour = null;
    //   // this.gridResult.dataSource[args.index].hourTypeId = args.data.hourTypeId;
    // }
    // if (args.requestType === "save" && args.data.hourTypeId !== 5) {
    //   // const milliseconds = Math.abs(args.data.startHour - args.data.endHour);
    //   // const hours = milliseconds / 36e5;
    //   // args.data.totalHours = hours;

    //   this.gridResult.dataSource[args.data.id - 1].startHour = args.data.startHour;
    //   this.gridResult.dataSource[args.data.id - 1].endHour = args.data.endHour;
    //   this.gridResult.dataSource[args.data.id - 1].hourTypeId = args.data.hourTypeId;
    // }
  }

  actionComplete(args) {
    if (args.requestType === "save" && args.data.hourTypeId === 5) {
      args.data.startHour = new Date(
        args.data.startHour.getFullYear(),
        args.data.startHour.getMonth(),
        args.data.startHour.getDate()
      );

      let record = this.gridResult.dataSource.find(
        (x) => x.id === args.data.id
      );
      if (record != null) {
        record.startHour = args.data.startHour;
        record.endHour = null;
      }
    }
    if (args.requestType === "save" && args.data.hourTypeId !== 5) {
      let record = this.gridResult.dataSource.find(
        (x) => x.id === args.data.id
      );
      if (record != null) {
        record.startHour = args.data.startHour;
        record.endHour = args.data.endHour;

        const milliseconds = Math.abs(record.endHour - record.startHour);
        record.totalHours = milliseconds / 36e5;
      }
    }
    if (args.requestType === "save") {
      this.gridResult.sortColumn("startHour", "Ascending");
    }
  }

  footerSumHours(args) {
    return <span>Total: {args.Sum} horas</span>;
  }

  rowDataBoundGridResult(args) {
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

  clickHandlerGridResult(args) {
    if (args.item.id === "RemoveAll") {
      const selectedRecords = this.gridResult.getSelectedRecords();
      if (Array.isArray(selectedRecords) && selectedRecords.length > 0) {
        let values = selectedRecords.map((a) => a.id);
        let newDataSource = [];
        for (
          let index = 0;
          index < this.gridResult.dataSource.length;
          index++
        ) {
          if (!values.includes(this.gridResult.dataSource[index].id)) {
            newDataSource.push(this.gridResult.dataSource[index]);
          }
        }
        this.gridResult.dataSource = newDataSource;
        this.gridResult.refresh();
      } else {
        this.props.showMessage({
          statusText: "Debes seleccionar al menos un registro",
          responseText: "Debes seleccionar al menos un registro",
          type: "danger",
        });
      }
    }
  }

  render() {
    return (
      <Fragment>
        <DialogComponent
          id="confirmDialog"
          header="Validar Fichajes"
          visible={this.state.hideConfirmDialog}
          showCloseIcon={true}
          animationSettings={this.animationSettings}
          width="500px"
          content="¿Estás seguro de hacer estos fichajes?"
          ref={(dialog) => (this.confirmDialogInstance = dialog)}
          target="#target-signings"
          buttons={this.confirmButton}
          close={this.dialogClose.bind(this)}
        ></DialogComponent>

        <Breadcrumb>
          {/*eslint-disable-next-line*/}
          <BreadcrumbItem>
            <a href="/#">Inicio</a>
          </BreadcrumbItem>
          {/* eslint-disable-next-line*/}
          <BreadcrumbItem active>Fichajes</BreadcrumbItem>
        </Breadcrumb>

        <Container fluid>
          <div className="animated fadeIn" id="target-signings">
            <div className="card">
              <div className="card-header">
                <i className="icon-book-open"></i> Plantilla para fichajes
                masivos
              </div>
              <div className="card-body"></div>

              <div
                style={{
                  marginLeft: "35px",
                  marginTop: "-20px",
                  marginBottom: "30px",
                }}
              >
                <Row>
                  <Form
                    inline
                    style={{ marginLeft: "10px", marginBottom: "20px" }}
                  >
                    <FormGroup className="col-2">
                      <Label for="startDate">Fecha Inicio</Label>
                      <DatePickerComponent
                        id="startDate"
                        ref={(g) => (this.dtpStartDate = g)}
                        format="dd/MM/yyyy"
                      />
                    </FormGroup>
                    <FormGroup className="col-2" style={{ marginLeft: "10px" }}>
                      <Label for="endDate">Fecha Fin</Label>
                      <DatePickerComponent
                        id="endDate"
                        ref={(g) => (this.dtpEndDate = g)}
                        format="dd/MM/yyyy"
                      />
                    </FormGroup>
                    <FormGroup className="col-2" style={{ marginLeft: "10px" }}>
                      <Label for="employees">Trabajador</Label>
                      <DropDownListComponent
                        id="employees"
                        dataSource={null}
                        placeholder={`Selecciona Trabajador`}
                        fields={this.fields}
                        ref={(g) => (this.ddlEmployees = g)}
                        filtering={this.handleFilteringEmployees.bind(this)}
                        allowFiltering={true}
                        select={this.handleSelectEmployee.bind(this)}
                        popupWidth="auto"
                      />
                    </FormGroup>
                    <FormGroup className="col-2" style={{ marginLeft: "10px" }}>
                      <Label for="works">Puesto</Label>
                      <DropDownListComponent
                        id="professions"
                        dataSource={null}
                        placeholder={`Selecciona Puesto`}
                        fields={this.fields}
                        ref={(g) => (this.ddlProfessions = g)}
                        popupWidth="auto"
                      />
                    </FormGroup>
                    <FormGroup className="col-3" style={{ marginLeft: "10px" }}>
                      <Label for="works">Obra</Label>
                      <DropDownListComponent
                        id="works"
                        dataSource={null}
                        placeholder={`Selecciona Obra`}
                        fields={this.fields}
                        ref={(g) => (this.ddlWorks = g)}
                        filtering={this.handleFilteringWorks.bind(this)}
                        allowFiltering={true}
                        popupWidth="auto"
                      />
                    </FormGroup>
                  </Form>
                </Row>
                <Row style={{ marginTop: "-10px" }}>
                  <FormGroup className="col-4" style={{ marginLeft: "10px" }}>
                    <Label
                      htmlFor="includeSaturdays"
                      style={{ verticalAlign: "bottom" }}
                    >
                      Incluir Sábados&nbsp;
                    </Label>
                    <AppSwitch
                      className={"mx-1 mt-4"}
                      variant={"pill"}
                      color={"primary"}
                      label
                      checked={this.state.includeSaturdays}
                      id="includeSaturdays"
                      name="includeSaturdays"
                      placeholder="incluir sábados"
                      onChange={this.handleInputChange}
                      dataOn="Si"
                      dataOff="No"
                    />
                  </FormGroup>
                  <FormGroup className="col-4">
                    <Label
                      htmlFor="includeSundays"
                      style={{ verticalAlign: "bottom" }}
                    >
                      Incluir Domingos&nbsp;
                    </Label>
                    <AppSwitch
                      className={"mx-1 mt-4"}
                      variant={"pill"}
                      color={"primary"}
                      label
                      checked={this.state.includeSundays}
                      id="includeSundays"
                      name="includeSundays"
                      placeholder="incluir domingos"
                      onChange={this.handleInputChange}
                      dataOn="Si"
                      dataOff="No"
                    />
                  </FormGroup>
                </Row>
                <Row>
                  <GridComponent
                    dataSource={this.state.dsGrid}
                    locale="es"
                    toolbar={this.toolbarOptions}
                    style={{
                      marginLeft: 20,
                      marginRight: 50,
                      marginTop: 20,
                      marginBottom: 20,
                    }}
                    ref={(g) => (this.grid = g)}
                    editSettings={this.editSettings}
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
                        field="hourTypeId"
                        headerText="Tipo"
                        width="100"
                        editType="dropdownedit"
                        foreignKeyValue="name"
                        foreignKeyField="id"
                        validationRules={this.hourTypeIdRules}
                        dataSource={this.hourTypes}
                        edit={this.changeHourType}
                      />
                      <ColumnDirective
                        field="startHour"
                        headerText="Hora Inicio"
                        width="100"
                        validationRules={this.hoursRules}
                      />
                      <ColumnDirective
                        field="endHour"
                        headerText="Hora Fin"
                        width="100"
                        validationRules={this.hoursRules}
                      />
                      <ColumnDirective
                        field="total"
                        headerText="Total Horas"
                        width="100"
                        allowEditing={false}
                        template={this.templateSumHours}
                      />
                    </ColumnsDirective>
                    <Inject services={[ForeignKey]} />
                  </GridComponent>
                </Row>
                <Row>
                  <Col
                    className="col-11"
                    style={{ textAlign: "right", marginLeft: "70px" }}
                  >
                    <Button color="primary" onClick={this.handleOnClickView}>
                      Ver
                    </Button>
                  </Col>
                </Row>
                <Row>
                  <GridComponent
                    //dataSource={this.state.dsGridResult}
                    dataSource={null}
                    locale="es"
                    allowSorting={true}
                    toolbar={this.toolbarOptionsGridResult}
                    style={{
                      marginLeft: 20,
                      marginRight: 50,
                      marginTop: 20,
                      marginBottom: 20,
                    }}
                    ref={(gResult) => (this.gridResult = gResult)}
                    editSettings={this.editSettings}
                    actionBegin={this.actionBegin}
                    actionComplete={this.actionComplete}
                    rowDataBound={this.rowDataBoundGridResult}
                    toolbarClick={this.clickHandlerGridResult}
                  >
                    <ColumnsDirective>
                      <ColumnDirective
                        type="checkbox"
                        width="50"
                      ></ColumnDirective>
                      <ColumnDirective
                        field="hourTypeId"
                        headerText="Tipo"
                        width="100"
                        editType="dropdownedit"
                        foreignKeyValue="name"
                        foreignKeyField="id"
                        validationRules={this.hourTypeIdRules}
                        dataSource={this.hourTypes}
                        // edit={this.changeHourType}
                      />
                      <ColumnDirective
                        field="startHour"
                        headerText="Fecha Inicio"
                        width="100"
                        validationRules={this.hoursRules}
                        editType="datetimepickeredit"
                        type="date"
                        format={this.format}
                        textAlign="Center"
                      />
                      <ColumnDirective
                        field="endHour"
                        headerText="Fecha Fin"
                        width="100"
                        validationRules={this.hoursRules}
                        editType="datetimepickeredit"
                        type="date"
                        format={this.format}
                        textAlign="Center"
                      />
                      <ColumnDirective
                        field="totalHours"
                        headerText="Total Horas"
                        width="100"
                        allowEditing={false}
                        textAlign="Center"
                        template={this.dateTemplateTotalHours}
                      />
                    </ColumnsDirective>

                    <AggregatesDirective>
                      <AggregateDirective>
                        <AggregateColumnsDirective>
                          <AggregateColumnDirective
                            field="totalHours"
                            type="Sum"
                            format="N2"
                            footerTemplate={this.footerSumHours}
                          >
                            {" "}
                          </AggregateColumnDirective>
                        </AggregateColumnsDirective>
                      </AggregateDirective>
                    </AggregatesDirective>

                    <Inject services={[ForeignKey, Aggregate, Sort]} />
                  </GridComponent>
                </Row>
                <Row>
                  <Col
                    className="col-11"
                    style={{ textAlign: "right", marginLeft: "70px" }}
                  >
                    <Button color="primary" onClick={this.handleOnClickSave}>
                      Fichaje Masivo
                    </Button>
                  </Col>
                </Row>
              </div>
            </div>
          </div>
        </Container>
      </Fragment>
    );
  }
}

Signings.propTypes = {};

const mapStateToProps = (state) => {
  return {};
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
});

export default connect(mapStateToProps, mapDispatchToProps)(Signings);
