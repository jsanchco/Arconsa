import React, { Component } from 'react';
import PropTypes from 'prop-types';

const propTypes = {
  children: PropTypes.node,
};

const defaultProps = {};

class DefaultFooter extends Component {
  render() {

    const enterprise = JSON.parse(localStorage.getItem("enterprise"));
    // eslint-disable-next-line
    const { children, ...attributes } = this.props;
    if (enterprise == null)
      return;

    const nameEnterprise = enterprise.alias;
    return (
      <React.Fragment>
        {/* <span><a href="https://coreui.io">Arconsa</a> &copy; 2019 jsanchco.</span> */}
        <span><a href="https://coreui.io">{nameEnterprise}</a></span>
        <span className="ml-auto">Creado por <a href="https://coreui.io/react">Jesús Sánchez</a></span>
      </React.Fragment>
    );
  }
}

DefaultFooter.propTypes = propTypes;
DefaultFooter.defaultProps = defaultProps;

export default DefaultFooter;
