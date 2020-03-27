import React, { Component, Fragment } from "react";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import {
  TabComponent,
  TabItemDirective,
  TabItemsDirective
} from "@syncfusion/ej2-react-navigations";
import BasicData from "./basic-data";
import Trainings from "./trainings";
import Documents from "./documents";
import DailySignings from "./daily-signings";
import ChangePassword from "./change-password";

class DetailEmployee extends Component {
  constructor(props) {
    super(props);

    this.headerText = null;

    if (this.props.history.location.state.user.roleId === 3) {
      this.headerText = [
        { text: "Datos Básicos" },
        { text: "Documentos" },
        { text: "Fichajes" },
        { text: "Cambiar Contraseña" }
      ];
    } else {
      this.headerText = [
        { text: "Datos Básicos" },
        { text: "Documentos" },
        { text: "Cambiar Contraseña" }
        //{ text: "Fichajes" }
      ];
    }

    this.contentTemplateBasicDate = this.contentTemplateBasicDate.bind(this);
    this.contentTemplateTrainings = this.contentTemplateTrainings.bind(this);
    this.contentTemplateDocuments = this.contentTemplateDocuments.bind(this);
    this.contentTemplateDailySignings = this.contentTemplateDailySignings.bind(
      this
    );
    this.contentTemplatChangePassword = this.contentTemplatChangePassword.bind(this);
    this.renderTemplateDailySignings = this.renderTemplateDailySignings.bind(
      this
    );
  }

  contentTemplateBasicDate() {
    return (
      <BasicData
        user={this.props.history.location.state.user}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateTrainings() {
    return (
      <Trainings
        user={this.props.history.location.state.user}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateDocuments() {
    return (
      <Documents
        user={this.props.history.location.state.user}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateDailySignings() {
    return (
      <DailySignings
        user={this.props.history.location.state.user}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplatChangePassword() {
    return (
      <ChangePassword
        user={this.props.history.location.state.user}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  renderTemplateDailySignings() {
    if (this.props.history.location.state.user.roleId === 3) {
      return (
        <TabItemDirective
          header={this.headerText[2]}
          content={this.contentTemplateDailySignings}
        />
      );
    } else {
      return null;
    }

  }

  render() {
    let title = "";
    if (
      this.props.history.location.state.user !== null &&
      this.props.history.location.state.user !== undefined
    ) {
      title = ` Detalle Trabajador [${this.props.history.location.state.user.fullname}]`;
    }

    return (
      <Fragment>
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
                  marginBottom: 20
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

                  {this.renderTemplateDailySignings()}

                  <TabItemDirective
                    header={this.headerText[this.headerText.length - 1]}
                    content={this.contentTemplatChangePassword}
                  />
                </TabItemsDirective>
              </TabComponent>
            </div>
          </div>
        </div>
      </Fragment>
    );
  }
}

DetailEmployee.propTypes = {};

const mapStateToProps = state => {
  return {
    errorApplication: state.applicationReducer.error
  };
};

const mapDispatchToProps = dispatch => ({
  showMessage: message => dispatch(ACTION_APPLICATION.showMessage(message))
});

export default connect(mapStateToProps, mapDispatchToProps)(DetailEmployee);
