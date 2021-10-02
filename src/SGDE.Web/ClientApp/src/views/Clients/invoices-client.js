import React, { Component, Fragment } from "react";
import GridInvoice from "../../components/grid-invoices";

class InvoicesClient extends Component {
  render() {
    let title = ` Facturas [${this.props.clientName}]`;

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
              clientId={this.props.clientId}
              workId={null}
              showMessage={this.props.showMessage}
            />
          </div>
        </div>
      </Fragment>
    );
  }
}

InvoicesClient.propTypes = {};

export default InvoicesClient;
