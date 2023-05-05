import React, { Component, Fragment } from "react";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import {
  TabComponent,
  TabItemDirective,
  TabItemsDirective,
} from "@syncfusion/ej2-react-navigations";
import { Breadcrumb, BreadcrumbItem, Container } from "reactstrap";
import BasicDataCompany from "./basic-data-company";
import IndirectCosts from "./indirect-costs";
import DocumentsCompany from "./documents-company";

class DetailCompany extends Component {
  constructor(props) {
    super(props);

    this.state = {};

    this.headerText = [
      { text: "Datos Básicos" },
      { text: "Gastos Indirectos" },
      { text: "Documentos de Empresa" },
    ];

    this.contentTemplateBasicDataCompany =
      this.contentTemplateBasicDataCompany.bind(this);
    this.contentTemplateIndirectCosts =
      this.contentTemplateIndirectCosts.bind(this);
    this.contentTemplateDocuments =
      this.contentTemplateDocuments.bind(this);
  }

  componentDidMount() {}

  contentTemplateBasicDataCompany() {
    return (
      <BasicDataCompany
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateIndirectCosts() {
    return (
      <IndirectCosts
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateDocuments() {
    return (
      <DocumentsCompany
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  render() {
    let title = JSON.parse(localStorage.getItem("enterprise")).alias;

    return (
      <Fragment>
        <Breadcrumb>
          {/*eslint-disable-next-line*/}
          <BreadcrumbItem>
            <a href="#">Inicio</a>
          </BreadcrumbItem>
          {/* eslint-disable-next-line*/}
          <BreadcrumbItem active>{title}</BreadcrumbItem>
        </Breadcrumb>

        <Container fluid>
          <div className="animated fadeIn">
            <div className="card">
              <div className="card-header">
                {/* <i className="icon-book-open"></i> */}
                {title}
              </div>
              <div className="card-body">
                <TabComponent
                  style={{
                    marginLeft: 30,
                    marginRight: 30,
                    marginTop: 0,
                    marginBottom: 20,
                    overflow: "auto",
                  }}
                >
                  <TabItemsDirective>
                    <TabItemDirective
                      header={this.headerText[0]}
                      content={this.contentTemplateBasicDataCompany}
                    />
                    <TabItemDirective
                      header={this.headerText[1]}
                      content={this.contentTemplateIndirectCosts}
                    />
                    <TabItemDirective
                      header={this.headerText[2]}
                      content={this.contentTemplateDocuments}
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

DetailCompany.propTypes = {};

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
});

export default connect(mapStateToProps, mapDispatchToProps)(DetailCompany);
