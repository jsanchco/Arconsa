import React, { Component, Fragment } from "react";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import {
  TabComponent,
  TabItemDirective,
  TabItemsDirective,
} from "@syncfusion/ej2-react-navigations";
import BasicDataClient from "./basic-data-client";
import WorksByClient from "./works-by-client";
import ProfessionInClient from "./profession-in-client";
import InvoicesClient from "./invoices-client";

class DetailClient extends Component {
  constructor(props) {
    super(props);

    this.headerText = [
      { text: "Datos Básicos" },
      { text: "Obras" },
      { text: "Precios por Profesión" },
      { text: "Facturas" },
    ];

    this.contentTemplateWorks = this.contentTemplateWorks.bind(this);
    this.contentTemplateBasicDataClient = this.contentTemplateBasicDataClient.bind(
      this
    );
    this.contentTemplateProfessionInClient = this.contentTemplateProfessionInClient.bind(
      this
    );
    this.contentTemplateInvoicesInClient = this.contentTemplateInvoicesInClient.bind(
      this
    );
  }

  contentTemplateBasicDataClient() {
    return (
      <BasicDataClient
        client={this.props.history.location.state.client}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
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

  contentTemplateProfessionInClient() {
    return (
      <ProfessionInClient
        client={this.props.history.location.state.client}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateInvoicesInClient() {
    return (
      <InvoicesClient
        client={this.props.history.location.state.client}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  render() {
    const user = JSON.parse(localStorage.getItem("user"));

    let title = "";
    if (
      this.props.history.location.state.client !== null &&
      this.props.history.location.state.client !== undefined
    ) {
      title = ` Detalle Cliente [${this.props.history.location.state.client.name}]`;
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
                    content={this.contentTemplateBasicDataClient}
                  />

                  <TabItemDirective
                    header={this.headerText[1]}
                    content={this.contentTemplateWorks}
                  />

                  <TabItemDirective
                    header={this.headerText[2]}
                    content={this.contentTemplateProfessionInClient}
                  />
                  {user.roleId === 1 ? (
                    <TabItemDirective
                      header={this.headerText[3]}
                      content={this.contentTemplateInvoicesInClient}
                    />
                  ) : null}
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

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
});

export default connect(mapStateToProps, mapDispatchToProps)(DetailClient);
