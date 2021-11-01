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
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, USERS, PROFESSIONS, ROLES } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import { TOKEN_KEY } from "../../services";

L10n.load(data);

class UsersNoWorker extends Component {
  users = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${USERS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  professions = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${PROFESSIONS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  roles = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${ROLES}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  professionIdRules = { required: false };
  roleIdRules = { required: true };
  grid = null;

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
      "Search",
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
    this.clickHandler = this.clickHandler.bind(this);
    this.rowSelected = this.rowSelected.bind(this);

    this.template = this.gridTemplate;

    this.query = new Query().addParams("roles", [1, 2]);
  }

  gridTemplate(args) {
    if (args.photo !== null && args.photo !== "") {
      const src = "data:image/png;base64," + args.photo;
      return (
        <div className="image">
          <img src={src} alt={args.name} width="100px" height="100px" />
        </div>
      );
    } else {
      return (
        <div className="image">
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

  clickHandler(args) {
    if (args.item.id === "Details") {
      const { rowSelected } = this.state;
      if (rowSelected !== null) {
        this.props.history.push({
          pathname: "/employees/detailemployee",
          state: {
            user: rowSelected,
          },
        });
      }
    }
  }

  rowSelected() {
    const selectedRecords = this.grid.getSelectedRecords();
    this.setState({ rowSelected: selectedRecords[0] });
  }

  renderColumnRole() {
    const user = JSON.parse(localStorage.getItem("user"));

    switch (user.roleId) {
      case 1:
        return (
          <ColumnDirective
            field="roleId"
            headerText="Role"
            width="100"
            editType="dropdownedit"
            foreignKeyValue="name"
            foreignKeyField="id"
            validationRules={this.roleIdRules}
            dataSource={this.roles}
          />
        );

      case 2:
        return (
          <ColumnDirective
            field="roleId"
            headerText="Role"
            width="100"
            visible={false}
            defaultValue={2}
          />
        );

      default:
        return null;
    }
  }

  render() {
    return (
      <Fragment>
        <Breadcrumb class>
          {/*eslint-disable-next-line*/}
          <BreadcrumbItem><a href="#">Inicio</a></BreadcrumbItem>
          {/* eslint-disable-next-line*/}
          <BreadcrumbItem active>Gestión de Usuarios</BreadcrumbItem>
        </Breadcrumb>

        <Container fluid>
          <div className="animated fadeIn">
            <div className="card">
              <div className="card-header">
                <i className="icon-list"></i> Gestión de Usuarios
              </div>
              <div className="card-body"></div>
              <Row>
                <GridComponent
                  dataSource={this.users}
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
                  allowGrouping={true}
                  rowSelected={this.rowSelected}
                  ref={(g) => (this.grid = g)}
                  query={this.query}
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
                      headerText="Foto"
                      width="100"
                      template={this.template}
                      textAlign="Center"
                      allowEditing={false}
                    />
                    <ColumnDirective field="dni" headerText="DNI" width="100" />
                    <ColumnDirective
                      field="name"
                      headerText="Nombre"
                      width="100"
                    />
                    <ColumnDirective
                      field="surname"
                      headerText="Apellidos"
                      width="100"
                    />
                    <ColumnDirective
                      field="username"
                      headerText="Código Acceso"
                      width="100"
                      required={true}
                    />
                    <ColumnDirective
                      field="phoneNumber"
                      headerText="Teléfono"
                      width="100"
                    />
                    <ColumnDirective
                      field="professionId"
                      headerText="Puesto"
                      width="100"
                      editType="dropdownedit"
                      foreignKeyValue="name"
                      foreignKeyField="id"
                      validationRules={this.professionIdRules}
                      dataSource={this.professions}
                    />

                    {this.renderColumnRole()}
                  </ColumnsDirective>
                  <Inject services={[ForeignKey, Group, Page, Toolbar, Edit]} />
                </GridComponent>
              </Row>
            </div>
          </div>
        </Container>
      </Fragment>
    );
  }
}

UsersNoWorker.propTypes = {};

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
});

export default connect(mapStateToProps, mapDispatchToProps)(UsersNoWorker);
