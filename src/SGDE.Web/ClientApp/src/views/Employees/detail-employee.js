import React, { Component, Fragment } from "react";
import { Breadcrumb, BreadcrumbItem, Container } from "reactstrap";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import { getUser } from "../../services";
import {
  TabComponent,
  TabItemDirective,
  TabItemsDirective,
} from "@syncfusion/ej2-react-navigations";
import BasicData from "./basic-data";
import Trainings from "./trainings";
import Documents from "./documents";
import DailySignings from "./daily-signings";
import ChangePassword from "./change-password";
import CostWorkers from "./cost-workers";
import HistoryHirings from "./history-hirings";
import Embargos from "./embargos";

class DetailEmployee extends Component {
  constructor(props) {
    super(props);

    this.state = {
      user: {
        fullname: "",
        roleId: 0,
      },
    };

    this.headerText = null;

    this.headerText = [
      { text: "Datos Básicos" },
      { text: "Documentos" },
      { text: "Fichajes" },
      { text: "Precios Coste" },
      { text: "Historial de Contratación" },
      { text: "Embargos" },
      { text: "Cambiar Contraseña" }
    ];

    this.contentTemplateBasicDate = this.contentTemplateBasicDate.bind(this);
    this.contentTemplateTrainings = this.contentTemplateTrainings.bind(this);
    this.contentTemplateDocuments = this.contentTemplateDocuments.bind(this);
    this.contentTemplateDailySignings =
      this.contentTemplateDailySignings.bind(this);
    this.contentTemplatChangePassword =
      this.contentTemplatChangePassword.bind(this);
    this.renderTemplateDailySignings =
      this.renderTemplateDailySignings.bind(this);
    this.contentTemplatCostWorkers = this.contentTemplatCostWorkers.bind(this);
    this.contentTemplatHistoryHirings =
      this.contentTemplatHistoryHirings.bind(this);
    this.contentTemplatEmbargos =
      this.contentTemplatEmbargos.bind(this);
  }

  componentDidMount() {
    getUser(this.props.match.params.id).then((result) => {
      this.setState({
        user: {
          fullname: result.fullname,
          roleId: result.roleId,
        },
      });
    });
  }

  contentTemplateBasicDate() {
    return (
      <BasicData
        userId={this.props.match.params.id}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateTrainings() {
    return (
      <Trainings
        userId={this.props.match.params.id}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateDocuments() {
    return (
      <Documents
        userId={this.props.match.params.id}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateDailySignings() {
    return (
      <DailySignings
        userId={this.props.match.params.id}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplatChangePassword() {
    return (
      <ChangePassword
        userId={this.props.match.params.id}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplatCostWorkers() {
    return (
      <CostWorkers
        userId={this.props.match.params.id}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplatHistoryHirings() {
    return (
      <HistoryHirings
        userId={this.props.match.params.id}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplatEmbargos() {
    return (
      <Embargos
        userId={this.props.match.params.id}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  renderTemplateDailySignings() {
    if (this.user.roleId === 3) {
      return (
        <Fragment>
          <TabItemDirective
            header={this.headerText[2]}
            content={this.contentTemplateDailySignings}
          />
          <TabItemDirective
            header={this.headerText[3]}
            content={this.contentTemplatCostWorkers}
          />
        </Fragment>
      );
    } else {
      return null;
    }
  }

  render() {
    let title = "";
    if (this.state.user !== null && this.state.user !== undefined) {
      this.state.user.roleId === 3
        ? (title = ` Detalle Trabajador [${this.state.user.fullname}]`)
        : (title = ` Perfil [${this.state.user.fullname}]`);
    }

    return (
      <Fragment>
        <Breadcrumb class>
          {/*eslint-disable-next-line*/}
          <BreadcrumbItem>
            <a href="#">Inicio</a>
          </BreadcrumbItem>
          {/* eslint-disable-next-line*/}
          <BreadcrumbItem>
            <a href="#/employees/employees">Trabajadores</a>
          </BreadcrumbItem>
          <BreadcrumbItem active>Detalles</BreadcrumbItem>
        </Breadcrumb>

        <Container fluid>
          <div className="animated fadeIn">
            <div className="card">
              <div className="card-header">
                <i className="icon-book-open"></i> {title}
              </div>
              <div className="card-body">
                <TabComponent
                  style={{
                    marginLeft: 30,
                    marginRight: 30,
                    marginTop: 0,
                    marginBottom: 20,
                  }}
                >
                  <TabItemsDirective>
                    <TabItemDirective
                      header={this.headerText[0]}
                      content={this.contentTemplateBasicDate}
                    />
                    <TabItemDirective
                      header={this.headerText[1]}
                      content={this.contentTemplateDocuments}
                    />

                    {this.state.user.roleId === 3 ? (
                      <TabItemDirective
                        header={this.headerText[2]}
                        content={this.contentTemplateDailySignings}
                      />
                    ) : null}

                    {this.state.user.roleId === 3 ? (
                      <TabItemDirective
                        header={this.headerText[3]}
                        content={this.contentTemplatCostWorkers}
                      />
                    ) : null}

                    {this.state.user.roleId === 3 ? (
                      <TabItemDirective
                        header={this.headerText[4]}
                        content={this.contentTemplatHistoryHirings}
                      />
                    ) : null}

                    {this.state.user.roleId === 3 ? (
                      <TabItemDirective
                        header={this.headerText[5]}
                        content={this.contentTemplatEmbargos}
                      />
                    ) : null}

                    <TabItemDirective
                      header={this.headerText[6]}
                      content={this.contentTemplatChangePassword}
                    />
                  </TabItemsDirective>
                </TabComponent>
              </div>
            </div>
          </div>
        </Container>
      </Fragment>
    );
  }
}

DetailEmployee.propTypes = {};

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
});

export default connect(mapStateToProps, mapDispatchToProps)(DetailEmployee);
