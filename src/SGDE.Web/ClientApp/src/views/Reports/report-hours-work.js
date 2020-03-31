import React, { Component, Fragment } from "react";
import { Row } from "reactstrap";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import HeaderSettings from "./header-settings";
import GridSelection from "./grid-selection";

class ReportHoursWork extends Component {
  constructor(props) {
    super(props);

    this.state = {
      settings: null
    };

    this.updateReport = this.updateReport.bind(this);
  }

  updateReport(type, start, end, selection, textSelection) {
    this.setState({ settings: { type, start, end, selection, textSelection } });
  }

  render() {
    return (
      <Fragment>
        <div className="animated fadeIn">
          <div className="card">
            <div className="card-header">
              <i className="icon-list"></i> Informe de Horas por Obra
            </div>
            <div className="card-body"></div>
            <div>
              <HeaderSettings
                type="works"
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
      </Fragment>
    );
  }
}

ReportHoursWork.propTypes = {};

const mapStateToProps = state => {
  return {
    errorApplication: state.applicationReducer.error
  };
};

const mapDispatchToProps = dispatch => ({
  showMessage: message => dispatch(ACTION_APPLICATION.showMessage(message))
});

export default connect(mapStateToProps, mapDispatchToProps)(ReportHoursWork);
