import React, { Component, Fragment } from "react";
import { Row } from "reactstrap";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../../locales/locale.json";

L10n.load(data);

class TestBlank extends Component {

  constructor(props) {
    super(props);
  }

  render() {
    return (
      <Fragment>
        <div className="animated fadeIn">
          <div
            className="card"
            style={{
              marginRight: "60px",
              marginTop: "20px",
              marginLeft: "60px",
            }}
          >
            <div className="card-header">
              <i className="icon-layers"></i> Test Blank
            </div>
            <div className="card-body"></div>
            <Row>
            </Row>
          </div>
        </div>
      </Fragment>
    );
  }
}

TestBlank.propTypes = {};

export default TestBlank;
