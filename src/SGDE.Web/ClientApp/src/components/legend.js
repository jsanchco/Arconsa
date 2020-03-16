import React, { Component, Fragment } from "react";
import PropTypes from "prop-types";

class Legend extends Component {
  render() {
    return (
      <div style={{ display: "flex" }}>
        {this.props.elements.map((element, i) => {
          return (
            <Fragment key={element.color}>
              <div style={{ textAlign: "left", width: "5%" }}>
                <span className={element.color}></span>
              </div>
              <div style={{ textAlign: "left", width: "20%" }}>
                {element.text}
              </div>
            </Fragment>
          );
        })}
      </div>
    );
  }
}

Legend.propTypes = {
  elements: PropTypes.array.isRequired
};

export default Legend;
