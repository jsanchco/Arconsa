import React, { Component, Fragment } from "react";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import { getWork } from "../../services";
import {
  TabComponent,
  TabItemDirective,
  TabItemsDirective,
} from "@syncfusion/ej2-react-navigations";
import { Breadcrumb, BreadcrumbItem, Container } from "reactstrap";
import AuthorizeCancelWorkers from "./authorize-cancel-workers";
import BasicDataWork from "./basic-data-work";
import InvoicesWork from "./invoices-work";
import WorkCosts from "./work-cost";
import WorkBudgets from "./work-budget";
import WorkBudgets1 from "./work-budget1";

class DetailWork extends Component {
  constructor(props) {
    super(props);

    this.state = {};

    this.headerText = [
      { text: "Datos Básicos" },
      { text: "Presupuestos" },
      { text: "Presupuestos1" },
      { text: "Gastos Proveedores" },
      { text: "Gastos Trabajadores" },
      { text: "Facturas" },
    ];

    this.contentTemplateWorkBudgets =
      this.contentTemplateWorkBudgets.bind(this);
    this.contentTemplateWorkBudgets1 =
      this.contentTemplateWorkBudgets1.bind(this);
    this.contentTemplateAuthorizeCancelWorkers =
      this.contentTemplateAuthorizeCancelWorkers.bind(this);
    this.contentTemplateWorkCosts = this.contentTemplateWorkCosts.bind(this);
    this.contentTemplateBasicDataWork =
      this.contentTemplateBasicDataWork.bind(this);
    this.contentTemplateInvoicesWork =
      this.contentTemplateInvoicesWork.bind(this);
  }

  componentDidMount() {
    getWork(this.props.match.params.id).then((result) => {
      this.setState({
        work: {
          name: result.name,
        },
      });
    });
  }

  contentTemplateWorkBudgets() {
    let workName = "";
    if (this.state.work !== null && this.state.work !== undefined) {
      workName = this.state.work.name;
    }

    return (
      <WorkBudgets
        workId={this.props.match.params.id}
        workName={workName}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateWorkBudgets1() {
    let workName = "";
    if (this.state.work !== null && this.state.work !== undefined) {
      workName = this.state.work.name;
    }

    return (
      <WorkBudgets1
        workId={this.props.match.params.id}
        workName={workName}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateBasicDataWork() {
    return (
      <BasicDataWork
        workId={this.props.match.params.id}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateWorkCosts() {
    return (
      <WorkCosts
        workId={this.props.match.params.id}
        workName={this.state.work.name}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateAuthorizeCancelWorkers() {
    return (
      <AuthorizeCancelWorkers
        workId={this.props.match.params.id}
        workName={this.state.work.name}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateInvoicesWork() {
    return (
      <InvoicesWork
        workId={this.props.match.params.id}
        workName={this.state.work.name}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  render() {
    const user = JSON.parse(localStorage.getItem("user"));

    let title = "";
    if (this.state.work !== null && this.state.work !== undefined) {
      title = ` Detalle Obra [${this.state.work.name}]`;
    }

    return (
      <Fragment>
        <Breadcrumb>
          {/*eslint-disable-next-line*/}
          <BreadcrumbItem>
            <a href="/#">Inicio</a>
          </BreadcrumbItem>
          {/* eslint-disable-next-line*/}
          <BreadcrumbItem>
            <a href="#/works/works">Obras</a>
          </BreadcrumbItem>
          <BreadcrumbItem active>Detalles</BreadcrumbItem>
        </Breadcrumb>

        <Container fluid>
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
                      content={this.contentTemplateWorkBudgets}
                    />
                    <TabItemDirective
                      header={this.headerText[2]}
                      content={this.contentTemplateWorkBudgets1}
                    />
                    <TabItemDirective
                      header={this.headerText[3]}
                      content={this.contentTemplateWorkCosts}
                    />
                    <TabItemDirective
                      header={this.headerText[4]}
                      content={this.contentTemplateAuthorizeCancelWorkers}
                    />
                    {user.roleId === 1 ? (
                      <TabItemDirective
                        header={this.headerText[4]}
                        content={this.contentTemplateInvoicesWork}
                      />
                    ) : null}
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
