import React, { Component, Fragment } from "react";
import { Row, Col } from "reactstrap";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import HeaderSettingsVarious from "./header-settings-various";
import GridWorkers from "./grid-workers";
import GridWorks from "./grid-works";

class ReportsVarious extends Component {
  constructor(props) {
    super(props);

    this.state = {
      settings: null
    };

    this.updateReport = this.updateReport.bind(this);
    this.renderGrid = this.renderGrid.bind(this);
  }

  updateReport(start, end, textSelection) {
    this.setState({ settings: { start, end, textSelection } });
  }

  renderGrid() {
    const { settings } = this.state;

    if (!settings) {
      return (
        <Col id="select-list" xs="12" style={{ textAlign: "center", margin: "50px" }}><h2>Selecciona Listado</h2></Col>
      );
    } else {
      const element = document.getElementById("select-list");
      if (element) {
        element.classList.add("hidden");
      }
    }

    switch (settings.textSelection) {
      case "Trabajadores":
        return (
          <GridWorkers
            showMessage={this.props.showMessage}
            settings={this.state.settings}
          />
        );
      case "Obras":
        return (
          <GridWorks
            showMessage={this.props.showMessage}
          />
        );

      default:
        return null;
    }
  }

  render() {

    return (
      <Fragment>
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

              {this.renderGrid()}

            </Row>
          </div>
        </div>
      </Fragment>
    );
  }
}

ReportsVarious.propTypes = {};

const mapStateToProps = state => {
  return {
    errorApplication: state.applicationReducer.error
  };
};

const mapDispatchToProps = dispatch => ({
  showMessage: message => dispatch(ACTION_APPLICATION.showMessage(message))
});

export default connect(mapStateToProps, mapDispatchToProps)(ReportsVarious);
