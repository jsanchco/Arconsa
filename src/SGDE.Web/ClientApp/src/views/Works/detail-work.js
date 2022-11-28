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
import WorkHistory from "./work-history";
import ClosePage from "./close-page";
import WorkStatusHistory from "./work-status-history";

class DetailWork extends Component {
  tab = null;

  constructor(props) {
    super(props);

    this.state = {};

    this.headerText = [
      { text: "Datos Básicos" },
      { text: "Estado" },
      { text: "Presupuestos" },
      // { text: "Presupuestos" },
      { text: "Gastos Proveedores" },
      { text: "Gastos Trabajadores" },
      { text: "Facturas" },
      { text: "Historia de Obra" },
      { text: "Hoja de Cierre" },
    ];

    this.selectedTab = this.selectedTab.bind(this);
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
    this.contentTemplateWorkHistory =
      this.contentTemplateWorkHistory.bind(this);
    this.contentTemplateClosePage = this.contentTemplateClosePage.bind(this);
    this.contentTemplateWorkStatusHistory =
      this.contentTemplateWorkStatusHistory.bind(this);
  }

  selectedTab(args) {
    this.setState({ tabSelected: args });
    console.log("detail-work -> selectedTab");
  }

  componentDidMount() {
    getWork(this.props.match.params.id).then((result) => {
      this.setState({
        work: {
          name: result.name,
        },
      });
    });

    const selectedTab = this.props.match.params.selectedTab;
    if (selectedTab !== null) {
      this.tab.selectedItem = parseInt(selectedTab);
    }
  }

  contentTemplateWorkStatusHistory() {
    let workName = "";
    if (this.state.work !== null && this.state.work !== undefined) {
      workName = this.state.work.name;
    }

    return (
      <WorkStatusHistory
        workId={this.props.match.params.id}
        workName={workName}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
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
    let workName = "";
    if (this.state.work !== null && this.state.work !== undefined) {
      workName = this.state.work.name;
    }

    return (
      <WorkCosts
        workId={this.props.match.params.id}
        workName={workName}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateAuthorizeCancelWorkers() {
    let workName = "";
    if (this.state.work !== null && this.state.work !== undefined) {
      workName = this.state.work.name;
    }

    return (
      <AuthorizeCancelWorkers
        workId={this.props.match.params.id}
        workName={workName}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateInvoicesWork() {
    let workName = "";
    if (this.state.work !== null && this.state.work !== undefined) {
      workName = this.state.work.name;
    }

    return (
      <InvoicesWork
        workId={this.props.match.params.id}
        workName={workName}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateWorkHistory() {
    let workName = "";
    if (this.state.work !== null && this.state.work !== undefined) {
      workName = this.state.work.name;
    }

    return (
      <WorkHistory
        workId={this.props.match.params.id}
        workName={workName}
        history={this.props.history}
        showMessage={this.props.showMessage}
      />
    );
  }

  contentTemplateClosePage() {
    let workName = "";
    if (this.state.work !== null && this.state.work !== undefined) {
      workName = this.state.work.name;
    }

    return (
      <ClosePage
        workId={this.props.match.params.id}
        workName={workName}
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
                  ref={(g) => (this.tab = g)}
                  selected={this.selectedTab}
                >
                  <TabItemsDirective>
                    <TabItemDirective
                      header={this.headerText[0]}
                      content={this.contentTemplateBasicDataWork}
                    />
                    <TabItemDirective
                      header={this.headerText[1]}
                      content={this.contentTemplateWorkStatusHistory}
                    />                    
                    {/* <TabItemDirective
                      header={this.headerText[2]}
                      content={this.contentTemplateWorkBudgets}
                      visible={false}
                    /> */}
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
                        header={this.headerText[5]}
                        content={this.contentTemplateInvoicesWork}
                      />
                    ) : null}
                    {user.roleId === 1 ? (
                      <TabItemDirective
                        header={this.headerText[6]}
                        content={this.contentTemplateWorkHistory}
                      />
                    ) : null}
                    {user.roleId === 1 ? (
                      <TabItemDirective
                        header={this.headerText[7]}
                        content={this.contentTemplateClosePage}
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
