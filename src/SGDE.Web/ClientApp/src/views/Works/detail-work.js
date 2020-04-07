import React, { Component, Fragment } from "react";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import {
  TabComponent,
  TabItemDirective,
  TabItemsDirective,
} from "@syncfusion/ej2-react-navigations";
import AuthorizeCancelWorkers from "./authorize-cancel-workers";
import BasicDataWork from "./basic-data-work";
import InvoicesWork from "./invoices-work";

class DetailWork extends Component {
  constructor(props) {
    super(props);

    this.headerText = [
      { text: "Datos Básicos" },
      { text: "Altas/Bajas de Trabajadores" },
      { text: "Facturas" }
    ];

    this.contentTemplateAuthorizeCancelWorkers = this.contentTemplateAuthorizeCancelWorkers.bind(
      this
    );
    this.contentTemplateBasicDataWork = this.contentTemplateBasicDataWork.bind(
      this
    );
    this.contentTemplateInvoicesWork = this.contentTemplateInvoicesWork.bind(
      this
    );
  }

  contentTemplateBasicDataWork() {
    return (
      <BasicDataWork
        work={this.props.history.location.state.work}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateAuthorizeCancelWorkers() {
    return (
      <AuthorizeCancelWorkers
        work={this.props.history.location.state.work}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateInvoicesWork() {
    return (
      <InvoicesWork
        work={this.props.history.location.state.work}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  render() {
    let title = "";
    if (
      this.props.history.location.state.work !== null &&
      this.props.history.location.state.work !== undefined
    ) {
      title = ` Detalle Obra [${this.props.history.location.state.work.name}]`;
    }

    return (
      <Fragment>
        <div className="animated fadeIn">
          <div className="card">
            <div className="card-header">
              <i className="icon-book-open"></i>
              {title}
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
                    content={this.contentTemplateBasicDataWork}
                  />
                  <TabItemDirective
                    header={this.headerText[1]}
                    content={this.contentTemplateAuthorizeCancelWorkers}
                  />
                  <TabItemDirective
                    header={this.headerText[2]}
                    content={this.contentTemplateInvoicesWork}
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

DetailWork.propTypes = {};

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
});

export default connect(mapStateToProps, mapDispatchToProps)(DetailWork);
