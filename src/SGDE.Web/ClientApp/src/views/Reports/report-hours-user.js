import React, { Component, Fragment } from "react";
import { Row } from "reactstrap";
import { DataManager, WebApiAdaptor } from "@syncfusion/ej2-data";
import { config, REPORTS, ROLES } from "../../constants";
import { connect } from "react-redux";
import ACTION_APPLICATION from "../../actions/applicationAction";
import { TOKEN_KEY, } from "../../services";
import HeaderSettings from "./header-settings";
import GridSelection from "./grid-selection";

class ReportHoursUser extends Component {
  dataSource = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${ROLES}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  constructor(props) {
    super(props);

    this.updateReport = this.updateReport.bind(this);
  }

  updateReport(type, start, end, selection) {
    console.log("type ->", type);
    console.log("start ->", start);
    console.log("end ->", end);
    console.log("selection ->", selection);
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
