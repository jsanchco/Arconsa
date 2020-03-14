import React, { Component, Fragment } from "react";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import {
  TabComponent,
  TabItemDirective,
  TabItemsDirective
} from "@syncfusion/ej2-react-navigations";
import WorksByClient from "./works-by-client";

class DetailClient extends Component {
  constructor(props) {
    super(props);

    this.headerText = [
      { text: "Obras" }
    ];

    this.contentTemplateWorks = this.contentTemplateWorks.bind(this);
  }

  contentTemplateWorks() {
    return (
      <WorksByClient
        client={this.props.history.location.state.client}
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
              <i className="icon-book-open"></i> Detalle Cliente
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
                    content={this.contentTemplateWorks}
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

DetailClient.propTypes = {};

const mapStateToProps = state => {
  return {
    errorApplication: state.applicationReducer.error
  };
};

const mapDispatchToProps = dispatch => ({
  showMessage: message => dispatch(ACTION_APPLICATION.showMessage(message))
});

export default connect(mapStateToProps, mapDispatchToProps)(DetailClient);
