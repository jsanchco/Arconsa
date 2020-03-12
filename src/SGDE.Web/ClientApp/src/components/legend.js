import React, { Component } from "react";
import PropTypes from "prop-types";

class Legend extends Component {
  render() {
    return (
      <div style={{ display: "flex" }}>
        <div style={{ textAlign: "left", width: "5%" }}>
          <span className="dot-red"></span>
        </div>
        <div style={{ textAlign: "left", width: "20%" }}>Obra Cerrada</div>
        <div style={{ textAlign: "left", width: "5%" }}>
          <span className="dot-green"></span>
        </div>
        <div style={{ textAlign: "left", width: "20%" }}>Obra Abierta</div>
      </div>
    );
  }
}

Legend.propTypes = {
  elements: PropTypes.object.isRequired
};

export default Legend;
