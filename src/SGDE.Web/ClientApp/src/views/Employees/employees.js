import React, { Component, Fragment } from "react";
import { Breadcrumb, BreadcrumbItem, Container, Row } from "reactstrap";
// import { getValue } from "@syncfusion/ej2-base";
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
  Sort,
  Resize,
} from "@syncfusion/ej2-react-grids";
// import { Tooltip } from "@syncfusion/ej2-popups";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, USERS, PROFESSIONS, ROLES } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import { TOKEN_KEY } from "../../services";

L10n.load(data);

class Employees extends Component {
  users = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${USERS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  // professions = new DataManager({
  //   adaptor: new WebApiAdaptor(),
  //   url: `${config.URL_API}/${PROFESSIONS}`,
  //   headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  // });

  roles = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${ROLES}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  professionIdRules = { required: false };
  roleIdRules = { required: true };
  grid = null;
  wrapSettings = { wrapMode: "Content" };

  constructor(props) {
    super(props);

    this.state = {
      users: null,
      professions: null,
      roles: null,
      rowSelected: null,
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
        id: "Details",
      },
      "Print",
      "Search",
    ];
    this.editSettings = {
      showDeleteConfirmDialog: true,
      allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      newRowPosition: "Top",
    };
    this.pageSettings = { 
      pageCount: 10, 
      pageSize: 10, 
      currentPage: props.currentPageEmployees 
    };
    this.searchSettings = { 
      key: props.currentSearchEmployees
    };
    this.actionFailure = this.actionFailure.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
    this.actionBegin = this.actionBegin.bind(this);
    this.clickHandler = this.clickHandler.bind(this);
    this.rowSelected = this.rowSelected.bind(this);
    this.formatDate = this.formatDate.bind(this);
    this.template = this.gridTemplate.bind(this);
    // this.tooltip = this.tooltip.bind(this);
    this.dataBound = this.dataBound.bind(this);

    this.format = { type: "dateTime", format: "dd/MM/yyyy" };

    this.query = new Query().addParams("roles", [3]);
  }

  dataBound() {
    this.props.setCurrentPageEmployees(this.grid.pageSettings.currentPage);
    this.props.setCurrentSearchEmployees(this.grid.searchSettings.key);
  }

  gridTemplate(args) {
    // let color = "";
    // switch (args.state) {
    //   case 0:
    //     color = "dot-small-green";
    //     break;
    //   case 1:
    //     color = "dot-small-red";
    //     break;

    //   default:
    //     color = "dot-small-green";
    //     break;
    // }

    if (args.photo !== null && args.photo !== "") {
      const src = "data:image/png;base64," + args.photo;
      return (
        <div className="image">
          {/* <span id={"user-" + args.id} className={color}></span> */}
          <img src={src} alt={args.name} width="100px" height="100px" />
        </div>
      );
    } else {
      return (
        <div className="image">
          {/* <span id={"user-" + args.id} className={color}></span> */}
          <img
            src={"assets/img/avatars/user_no_photo.png"}
            alt={args.name}
            width="100px"
            height="100px"
          />
        </div>
      );
    }
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
      if (
        args.data.birthDate !== null &&
        args.data.birthDate !== "" &&
        args.data.birthDate !== undefined
      ) {
        let date = this.formatDate(args.data.birthDate);
        args.data.birthDate = date;
      }
    }
  }

  clickHandler(args) {
    if (args.item.id === "Details") {
      const { rowSelected } = this.state;
      if (rowSelected !== null) {
        this.props.history.push({
          pathname: "/employees/detailemployee/" + rowSelected.id,
          state: {
            user: rowSelected,
          },
        });
      } else {
        this.props.showMessage({
          statusText: "Debes seleccionar un usuario",
          responseText: "Debes seleccionar un usuario",
          type: "danger",
        });
      }
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

  rowSelected() {
    const selectedRecords = this.grid.getSelectedRecords();
    this.setState({ rowSelected: selectedRecords[0] });
  }

  // tooltip(args) {
  //   if (args.column.field === "stateDescription" && args.data.roleId === 3) {
  //     const tooltip = new Tooltip({
  //       content: getValue(args.column.field, args.data).toString(),
  //     });
  //     tooltip.appendTo(args.cell);
  //   }
  // }

  render() {
    return (
      <Fragment>
        <Breadcrumb class>
          {/*eslint-disable-next-line*/}
          <BreadcrumbItem><a href="#">Inicio</a></BreadcrumbItem>
          {/* eslint-disable-next-line*/}
          <BreadcrumbItem active>Trabajadores</BreadcrumbItem>
        </Breadcrumb>

        <Container fluid>
          <div className="animated fadeIn">
            <div className="card">
              <div className="card-header">
                <i className="icon-list"></i> Trabajadores
              </div>
              <div className="card-body"></div>

              <Row>
                <GridComponent
                  dataSource={this.users}
                  id="Employees"
                  locale="es"
                  allowPaging={true}
                  pageSettings={this.pageSettings}
                  searchSettings={this.searchSettings} 
                  toolbar={this.toolbarOptions}
                  toolbarClick={this.clickHandler}
                  editSettings={this.editSettings}
                  style={{
                    marginLeft: 30,
                    marginRight: 30,
                    marginTop: -20,
                    marginBottom: 20,
                    overflow: "auto",
                  }}
                  actionFailure={this.actionFailure}
                  actionComplete={this.actionComplete}
                  actionBegin={this.actionBegin}
                  allowGrouping={true}
                  rowSelected={this.rowSelected}
                  ref={(g) => (this.grid = g)}
                  query={this.query}
                  allowTextWrap={true}
                  textWrapSettings={this.wrapSettings}
                  allowSorting={true}
                  queryCellInfo={this.tooltip}
                  allowResizing={true}
                  // enablePersistence={true}
                  dataBound={this.dataBound}
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
                      field="stateDescription"
                      headerText="Foto"
                      width="100"
                      template={this.template}
                      textAlign="Center"
                      allowEditing={false}
                    />
                    <ColumnDirective
                      field="name"
                      headerText="Nombre"
                      width="60"
                    />
                    <ColumnDirective
                      field="surname"
                      headerText="Apellidos"
                      width="70"
                    />
                    <ColumnDirective
                      field="workName"
                      headerText="Obra"
                      width="70"
                    />
                    <ColumnDirective
                      field="birthDate"
                      headerText="F. Nacim."
                      width="70"
                      type="date"
                      format={this.format}
                      editType="datepickeredit"
                    />
                    <ColumnDirective field="dni" headerText="DNI" width="70" />
                    <ColumnDirective
                      field="securitySocialNumber"
                      headerText="S. Social"
                      width="70"
                    />
                    <ColumnDirective
                      field="phoneNumber"
                      headerText="Teléfono"
                      width="70"
                    />
                    {/* <ColumnDirective
                      field="professionId"
                      headerText="Profesión"
                      width="100"
                      editType="dropdownedit"
                      foreignKeyValue="name"
                      foreignKeyField="id"
                      validationRules={this.professionIdRules}
                      dataSource={this.professions}
                    /> */}
                    <ColumnDirective
                      field="professions"
                      headerText="Puestos"
                      width="100"
                    />
                    <ColumnDirective
                      field="roleId"
                      headerText="Role"
                      width="100"
                      visible={false}
                      defaultValue={3}
                    />
                  </ColumnsDirective>
                  <Inject
                    services={[
                      ForeignKey,
                      Group,
                      Page,
                      Toolbar,
                      Edit,
                      Sort,
                      Resize,
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

Employees.propTypes = {};

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
    currentPageEmployees: state.applicationReducer.currentPageEmployees,
    currentSearchEmployees: state.applicationReducer.currentSearchEmployees
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
  setCurrentPageEmployees: (currentPageEmployees) => dispatch(ACTION_APPLICATION.setCurrentPageEmployees(currentPageEmployees)),
  setCurrentSearchEmployees: (currentSearchEmployees) => dispatch(ACTION_APPLICATION.setCurrentSearchEmployees(currentSearchEmployees))
});

export default connect(mapStateToProps, mapDispatchToProps)(Employees);
