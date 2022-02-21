import React, { Component, Fragment } from "react";
import { Breadcrumb, BreadcrumbItem, Container, Row } from "reactstrap";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import HeaderSettingsVarious from "./header-settings-various";
import GridReportVarious from "./grid-report-various";

class ReportsVarious extends Component {
  constructor(props) {
    super(props);

    this.state = {
      settings: null,
    };

    this.updateReport = this.updateReport.bind(this);
  }

  updateReport(start, end, textSelection, showCeros) {
    this.setState({ settings: { start, end, textSelection, showCeros } });
  }

  render() {
    return (
      <Fragment>
        <Breadcrumb>
          {/*eslint-disable-next-line*/}
          <BreadcrumbItem><a href="#">Inicio</a></BreadcrumbItem>
          {/* eslint-disable-next-line*/}
          <BreadcrumbItem active>Informes Varios</BreadcrumbItem>
        </Breadcrumb>

        <Container fluid>
          <div className="animated fadeIn" id="selection-report">
            <div className="card">
              <div className="card-header">
                <i className="icon-list"></i> Informes Varios
              </div>
              <div className="card-body"></div>
              <div>
                <HeaderSettingsVarious
                  showMessage={this.props.showMessage}
                  updateReport={this.updateReport}
                />
              </div>

              <Row style={{ marginTop: "10px" }} id="row-grid">
                <GridReportVarious
                  showMessage={this.props.showMessage}
                  settings={this.state.settings}
                />

                {/* {
                !settings ?
                  <Col id="select-list" xs="12" style={{ textAlign: "center", margin: "50px" }}><h2>Selecciona Listado</h2></Col> :
                  <GridReportVarious
                    showMessage={this.props.showMessage}
                    settings={this.state.settings}
                  />
              } */}
              </Row>
            </div>
          </div>
        </Container>
      </Fragment>
    );
  }
}

ReportsVarious.propTypes = {};

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
});

export default connect(mapStateToProps, mapDispatchToProps)(ReportsVarious);
