import React, { Component, Fragment } from "react";
import {
  Container,
  Row,
  Col,
  Card,
  CardBody,
  CardColumns,
  CardHeader,
} from "reactstrap";
import { Bar } from "react-chartjs-2";
import {
  createSpinner,
  showSpinner,
  hideSpinner,
} from "@syncfusion/ej2-popups";
import { connect } from "react-redux";
import { getDashboard } from "../../services";
import ACTION_APPLICATION from "../../actions/applicationAction";
import { CustomTooltips } from "@coreui/coreui-plugin-chartjs-custom-tooltips";
import { select } from "@syncfusion/ej2-react-schedule";

const options = {
  tooltips: {
    enabled: false,
    custom: CustomTooltips,
  },
  maintainAspectRatio: false,
};

class Dashboard extends Component {
  constructor(props) {
    super(props);

    this.state = null;

    this.getDashboard = this.getDashboard.bind(this);
  }

  loading = () => (
    <div className="animated fadeIn pt-1 text-center">Loading...</div>
  );

  componentDidMount() {
    this.getDashboard();
  }

  getDashboard() {
    const element = document.getElementById("container-bar");
    if (element != null) {
      createSpinner({
        target: element,
      });
      showSpinner(element);
    }

    getDashboard()
      .then((result) => {
        this.setState({
          charts: result.data,
        });
        hideSpinner(element);
      })
      .catch((error) => {
        this.props.showMessage({
          statusText: error,
          responseText: error,
          type: "danger",
        });
        hideSpinner(element);
      });
  }

  render() {
    const element = document.getElementById("container-bar");
    if (element == null) {
      return (
        <Fragment>
          <Container id="container-bar" />
        </Fragment>
      );
    }

    return (
      <Fragment>
        <Container id="container-bar">
          <Row className="justify-content-center">
            <Col md="6">
              <div style={{ cursor: "pointer" }} className="clearfix">
                <h1
                  className="display-3"
                  style={{
                    textAlign: "center",
                    fontWeight: "500",
                    color: "dimgray",
                    fontStyle: "italic",
                  }}
                >
                  ADECUA
                </h1>
              </div>
            </Col>
          </Row>
        </Container>

        <hr></hr>
        <br></br>

        <Container fluid>
          <div className="animated fadeIn">
            {/* <CardColumns className="cols-1"> */}
            <Card>
              <CardHeader>
                <strong>{this.state.charts.Item1.name}</strong>
              </CardHeader>
              <CardBody>
                <div className="chart-wrapper">
                  <Bar data={this.state.charts.Item1} options={options} height={300} />
                </div>
              </CardBody>
            </Card>

            <Card>
              <CardHeader>
                <strong>{this.state.charts.Item2.name}</strong>
              </CardHeader>
              <CardBody>
                <div className="chart-wrapper">
                  <Bar data={this.state.charts.Item2} options={options} height={300} />
                </div>
              </CardBody>
            </Card>

            {/* </CardColumns> */}
          </div>
        </Container>
      </Fragment>
    );
  }
}

const mapStateToProps = (state) => {
  return {
    errorApplication: state.applicationReducer.error,
  };
};

const mapDispatchToProps = (dispatch) => ({
  showMessage: (message) => dispatch(ACTION_APPLICATION.showMessage(message)),
});

export default connect(mapStateToProps, mapDispatchToProps)(Dashboard);
