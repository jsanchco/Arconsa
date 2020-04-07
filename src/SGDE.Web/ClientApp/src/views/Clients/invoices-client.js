import React, { Component, Fragment } from "react";
import { Row } from "reactstrap";
import GridInvoice from "../../components/grid-invoices";

class InvoicesClient extends Component {
  render() {
    let title = "";
    if (this.props.client !== null && this.props.client !== undefined) {
      title = ` Facturas [${this.props.client.name}]`;
    }

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
            <Row>
              <GridInvoice
                clientId={this.props.client.id}
                workId={null}
                showMessage={this.props.showMessage}
              />
            </Row>
          </div>
        </div>
      </Fragment>
    );
  }
}

InvoicesClient.propTypes = {};

export default InvoicesClient;
