import React, { Component, Fragment } from "react";
import GridInvoice from "../../components/grid-invoices";

class InvoicesWork extends Component {
  render() {
    let title = ` Facturas [${this.props.workName}]`;
    
    return (
      <Fragment>
        <div className="animated fadeIn">
          <div
            className="card"
            style={{ marginRight: "60px", marginTop: "20px" }}
          >
            <div className="card-header">
              <i className="icon-layers"></i>
              {title}
            </div>
            <div className="card-body"></div>
            <GridInvoice
              clientId={null}
              workId={this.props.workId}
              showMessage={this.props.showMessage}
            />
          </div>
        </div>
      </Fragment>
    );
  }
}

InvoicesWork.propTypes = {};

export default InvoicesWork;
