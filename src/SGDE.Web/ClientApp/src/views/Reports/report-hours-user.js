import React, { Component, Fragment } from "react";
import { Row } from "reactstrap";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import HeaderSettings from "./header-settings";
import GridSelection from "./grid-selection";

class ReportHoursUser extends Component {
  constructor(props) {
    super(props);

    this.state = {
      settings: null
    };

    this.updateReport = this.updateReport.bind(this);
  }

  updateReport(type, start, end, selection) {
    this.setState({ settings: { type, start, end, selection } });
  }

  render() {
    return (
      <Fragment>
        <div className="animated fadeIn">
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
                dataSource={this.dataSource}
                settings={this.state.settings}
              />
            </Row>
          </div>
        </div>
      </Fragment>
    );
  }
}

ReportHoursUser.propTypes = {};

const mapStateToProps = state => {
  return {
    errorApplication: state.applicationReducer.error
  };
};

const mapDispatchToProps = dispatch => ({
  showMessage: message => dispatch(ACTION_APPLICATION.showMessage(message))
});

export default connect(mapStateToProps, mapDispatchToProps)(ReportHoursUser);
