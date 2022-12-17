import React, { Component, Fragment } from "react";
import { Container, Row, Col } from "reactstrap";
import { Bar } from 'react-chartjs-2';
import { DatePickerComponent } from "@syncfusion/ej2-react-calendars";
import { connect } from "react-redux";
import { getDashboard } from "../../services";
import ACTION_APPLICATION from "../../actions/applicationAction";
import { CustomTooltips } from '@coreui/coreui-plugin-chartjs-custom-tooltips';

const bar = {
  labels: ["January", "February", "March", "April", "May", "June", "July"],
  datasets: [
    {
      label: "My First dataset",
      backgroundColor: "rgba(255,99,132,0.2)",
      borderColor: "rgba(255,99,132,1)",
      borderWidth: 1,
      hoverBackgroundColor: "rgba(255,99,132,0.4)",
      hoverBorderColor: "rgba(255,99,132,1)",
      data: [65, 59, 80, 81, 56, 55, 40],
    },
    {
      label: "My Second dataset",
      backgroundColor: "rgba(255,99,132,0.2)",
      borderColor: "rgba(255,99,132,1)",
      borderWidth: 1,
      hoverBackgroundColor: "rgba(255,99,132,0.4)",
      hoverBorderColor: "rgba(255,99,132,1)",
      data: [60, 50, 88, 89, 50, 58, 49],
    },
    {
      label: "My Third dataset",
      backgroundColor: "rgba(255,99,132,0.2)",
      borderColor: "rgba(255,99,132,1)",
      borderWidth: 1,
      hoverBackgroundColor: "rgba(255,99,132,0.4)",
      hoverBorderColor: "rgba(255,99,132,1)",
      data: [60, 50, 88, 89, 50, 58, 49],
    },
  ],
};

const options = {
  tooltips: {
    enabled: false,
    custom: CustomTooltips
  },
  maintainAspectRatio: false
}

class Dashboard_WtihSelectDates extends Component {
  constructor(props) {
    super(props);

    this.dtpStartDate = null;
    this.dtpEndDate = null;

    this.state = {
      start: this.setMonthForCurrentDate(-4, false),
      end: this.setMonthForCurrentDate(0, true),
    };

    this.onChangeCalendarStart = this.onChangeCalendarStart.bind(this);
    this.onChangeCalendarEnd = this.onChangeCalendarEnd.bind(this);
    this.getDashboard = this.getDashboard.bind(this);
  }

  formatDate(args) {
    if (args === null || args === "") {
      return "";
    }

    let day = args.getDate();
    if (day < 10) day = "0" + day;

    const month = args.getMonth() + 1;
    const year = args.getFullYear();

    if (month < 10) {
      return `${day}-0${month}-${year}`;
    } else {
      return `${day}-${month}-${year}`;
    }
  }

  onChangeCalendarStart() {
    let newDate = new Date(
      this.dtpStartDate.value.getFullYear(),
      this.dtpStartDate.value.getMonth(),
      1
    );

    this.setState({
      start: newDate,
    });
  }

  onChangeCalendarEnd() {
    let newDate = new Date(
      this.dtpEndDate.value.getFullYear(),
      this.dtpEndDate.value.getMonth(),
      1
    );
    newDate.setMonth(newDate.getMonth() + 1);
    newDate.setDate(newDate.getDate() - 1);

    this.setState({
      end: newDate,
    });
  }

  setMonthForCurrentDate(numberOfMonths, isMonthFinal) {
    let newDate = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
    newDate.setMonth(newDate.getMonth() + Number(numberOfMonths));

    if (isMonthFinal) {
      newDate.setMonth(newDate.getMonth() + 1);
      newDate.setDate(newDate.getDate() - 1);
    }

    return newDate;
  }

  loading = () => (
    <div className="animated fadeIn pt-1 text-center">Loading...</div>
  );

  componentDidMount() {
    if (this.dtpStartDate != null && this.dtpEndDate != null) {
      this.getDashboard();
    }
  }

  componentDidUpdate(prevProps, prevState, snapshot) {
    if (
      prevState.start !== this.state.start ||
      prevState.end !== this.state.end
    ) {
      this.getDashboard();
    }
  }

  getDashboard() {
    getDashboard(
      this.formatDate(this.dtpStartDate.value),
      this.formatDate(this.dtpEndDate.value)
    )
      .then((result) => {
        const data = JSON.parse(result.data);
        this.setState({
          companyName: data.companyName,
          cif: data.cif,
          address: data.address,
          phoneNumber: data.phoneNumber,
        });
      })
      .catch((error) => {
        this.props.showMessage({
          statusText: error,
          responseText: error,
          type: "danger",
        });
      });
  }

  render() {
    return (
      <Fragment>
        <Container>
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
          <div className="animated fadeIn" id="selection-report">
            <div className="card">
              <div className="card-body">
                <Row>
                  <Col>
                    <DatePickerComponent
                      id="startDate"
                      ref={(g) => (this.dtpStartDate = g)}
                      placeholder="Mes inicio"
                      start="Year"
                      depth="Year"
                      format="MMM/yyyy"
                      change={this.onChangeCalendarStart}
                      value={this.state.start}
                    />
                  </Col>
                  <Col>
                    <DatePickerComponent
                      id="endDate"
                      ref={(g) => (this.dtpEndDate = g)}
                      placeholder="Mes fin"
                      start="Year"
                      depth="Year"
                      format="MMM/yyyy"
                      change={this.onChangeCalendarEnd}
                      value={this.state.end}
                    />
                  </Col>
                </Row>
                <Row>
                  <Col>
                    <div className="chart-wrapper">
                      <Bar data={bar} options={options} />
                    </div>
                  </Col>
                </Row>
              </div>
            </div>
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

export default connect(mapStateToProps, mapDispatchToProps)(Dashboard_WtihSelectDates);
