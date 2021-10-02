import React, { Component, Fragment } from "react";
import { Breadcrumb, BreadcrumbItem, Container, Row } from "reactstrap";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import HeaderSettings from "./header-settings";
import GridSelection from "./grid-selection";

class ReportHoursUser extends Component {
  constructor(props) {
    super(props);

    this.state = {
      settings: null,
    };

    this.updateReport = this.updateReport.bind(this);
  }

  updateReport(type, start, end, selection, textSelection) {
    this.setState({ settings: { type, start, end, selection, textSelection } });
  }

  render() {
    return (
      <Fragment>
        <Breadcrumb class>
          {/*eslint-disable-next-line*/}
          <BreadcrumbItem><a href="#">Inicio</a></BreadcrumbItem>
          {/* eslint-disable-next-line*/}
          <BreadcrumbItem active>Informe de Horas por Tabajador</BreadcrumbItem>
        </Breadcrumb>

        <Container fluid>
          <div className="animated fadeIn" id="selection-report">
            <div className="card">
              <div className="card-header">
                <i className="icon-list"></i> Informe de Horas por Tabajador
              </div>
              <div className="card-body"></div>
              <div>
                <HeaderSettings
                  type="workers"
                  showMessage={this.props.showMessage}
                  updateReport={this.updateReport}
                />
              </div>
              <Row>
                <GridSelection
                  showMessage={this.props.showMessage}
                  settings={this.state.settings}
                />
              </Row>
            </div>
          </div>
        </Container>
      </Fragment>
    );
  }
}

ReportHoursUser.propTypes = {};

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
});

export default connect(mapStateToProps, mapDispatchToProps)(ReportHoursUser);
