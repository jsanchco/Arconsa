import React, { Component, Fragment } from "react";
import { Row } from "reactstrap";
import GridInvoice from "../../components/grid-invoices";

class InvoicesWork extends Component {
  render() {
    let title = "";
    if (this.props.work !== null && this.props.work !== undefined) {
      title = ` Facturas [${this.props.work.name}]`;
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
            <GridInvoice
              clientId={null}
              workId={this.props.work.id}
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
