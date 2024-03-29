import React, { Component, Fragment } from "react";
import { Breadcrumb, BreadcrumbItem, Container, Row } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Edit,
  Inject,
  Toolbar,
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor } from "@syncfusion/ej2-data";
import { config, ROLES } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import { TOKEN_KEY } from "../../services";

L10n.load(data);

class Roles extends Component {
  roles = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${ROLES}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  grid = null;

  constructor(props) {
    super(props);

    this.state = {
      roles: null,
    };

    this.toolbarOptions = ["Add", "Edit", "Delete", "Update", "Cancel"];
    this.editSettings = {
      showDeleteConfirmDialog: true,
      allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      newRowPosition: "Top",
    };
    this.actionFailure = this.actionFailure.bind(this);
    this.actionComplete = this.actionComplete.bind(this);
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
    }
    if (args.requestType === "delete") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });
    }
  }

  render() {
    return (
      <Fragment>
        <Breadcrumb>
          {/*eslint-disable-next-line*/}
          <BreadcrumbItem><a href="#">Inicio</a></BreadcrumbItem>
          {/* eslint-disable-next-line*/}
          <BreadcrumbItem active>Roles de Usuarios</BreadcrumbItem>
        </Breadcrumb>

        <Container fluid>
          <div className="animated fadeIn">
            <div className="card">
              <div className="card-header">
                <i className="icon-people"></i> Roles de Usuarios
              </div>
              <div className="card-body"></div>
              <Row>
                <GridComponent
                  dataSource={this.roles}
                  locale="es-US"
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
                  rowSelected={this.rowSelected}
                  ref={(g) => (this.grid = g)}
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
                      field="name"
                      headerText="Nombre"
                      width="100"
                    />
                  </ColumnsDirective>
                  <Inject services={[Toolbar, Edit]} />
                </GridComponent>
              </Row>
            </div>
          </div>
        </Container>
      </Fragment>
    );
  }
}

Roles.propTypes = {};

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
});

export default connect(mapStateToProps, mapDispatchToProps)(Roles);
