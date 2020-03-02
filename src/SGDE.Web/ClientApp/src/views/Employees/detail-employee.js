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

class DetailEmployee extends Component {
  constructor(props) {
    super(props);

    this.headerText = [
      { text: "Datos Básicos" },
      { text: "Cursos" },
      { text: "WhatsApp" }
    ];

    this.contentTemplateBasicDate = this.contentTemplateBasicDate.bind(this);
    this.contentTemplateTrainings = this.contentTemplateTrainings.bind(this);
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

  render() {
    return (
      <Fragment>
        <div className="animated fadeIn">
          <div className="card">
            <div className="card-header">
              <i className="icon-book-open"></i> Detalle Trabajador
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
                    content={this.contentTemplateTrainings}
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
