import React, { Component, Fragment } from "react";
import PropTypes from "prop-types";
import { Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import { Row, Col } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Inject,
  Toolbar,
  ForeignKey,
  Aggregate,
  AggregateColumnsDirective,
  AggregateColumnDirective,
  AggregateDirective,
  AggregatesDirective,
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, INVOICEPAYMENTSHISTORY } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY } from "../../services";
import "./modal-worker.css";

L10n.load(data);

class ModalInvoicePaymentsHistory extends Component {
  workers = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${INVOICEPAYMENTSHISTORY}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  grid = null;

  constructor(props) {
    super(props);

    this.state = {};

    this.dialogClose = this.dialogClose.bind(this);
    this.actionFailure = this.actionFailure.bind(this);
    this.actionComplete = this.actionComplete.bind(this);

    this.toolbarOptions = ["Add", "Edit", "Delete", "Update", "Cancel"];
    this.editSettings = {
      allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      newRowPosition: "Top",
    };

    this.animationSettings = { effect: "None" };
    this.professionIdRules = { required: true };

    this.wrapSettings = { wrapMode: "Content" };
    this.formatDate = { type: "dateTime", format: "dd/MM/yyyy" };
    this.numericParams = {
      params: {
        decimals: 2,
        format: "N",
        validateDecimalOnType: true,
        showSpinButton: false,
      },
    };
  }

  dialogClose() {
    this.setState({
      hideConfirmDialog: false,
    });
  }

  actionFailure(args) {
    let error = Array.isArray(args) ? args[0].error : args.error;
    if (Array.isArray(error)) {
      error = error[0].error;
    }
    this.props.showMessage({
      statusText: error.statusText,
      responseText: error.responseText,
      type: "danger",
    });
  }

  actionComplete(args) {
    if (args.requestType === "save") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });
    }
    if (args.requestType === "delete") {
      this.props.showMessage({
        statusText: "200",
        responseText: "Operación realizada con éxito",
        type: "success",
      });
    }
  }

  footerSumEuros(args) {
    if (typeof args.Sum === "string" || args.Sum instanceof String) {
      const total = args.Sum.replace(",", "");
      //const total = args.Sum;
      return <span>Total: {total}€</span>;
    } else {
      const total = Math.round((args.Sum + Number.EPSILON) * 100) / 100;
      return <span>Total: {total}€</span>;
    }
  }

  render() {
    let title = "Factura [ ??? ]";
    if (this.props.invoice != null) {
      title = "Factura [" + this.props.invoice.name + "]";

      this.query = new Query().addParams("invoiceId", this.props.invoice.id);
    }

    return (
      <Fragment>
        <Modal
          isOpen={this.props.isOpen}
          toggle={this.props.toggle}
          className={"modal-lg modal-primary"}
        >
          <ModalHeader toggle={this.props.toggle}>{title}</ModalHeader>
          <ModalBody>
            <Row>
              <GridComponent
                dataSource={this.workers}
                locale="es-US"
                actionFailure={this.actionFailure}
                actionComplete={this.actionComplete}
                toolbar={this.toolbarOptions}
                style={{
                  marginLeft: 30,
                  marginRight: 30,
                  marginTop: 20,
                  marginBottom: 20,
                }}
                height={500}
                ref={(g) => (this.grid = g)}
                allowTextWrap={true}
                textWrapSettings={this.wrapSettings}
                allowResizing={true}
                query={this.query}
                editSettings={this.editSettings}
              >
                <ColumnsDirective>
                  <ColumnDirective
                    field="id"
                    headerText="Id"
                    width="40"
                    isPrimaryKey={true}
                    isIdentity={true}
                    visible={false}
                  />
                  <ColumnDirective
                    field="datePayment"
                    headerText="Fecha"
                    width="100"
                    editType="datepickeredit"
                    type="date"
                    format={this.formatDate}
                    defaultValue={new Date()}
                  />
                  <ColumnDirective
                    field="amount"
                    headerText="Cantidad Abonada"
                    width="120"
                    fotmat="N2"
                    textAlign="Right"
                    headerTextAlign="Left"
                    editType="numericedit"
                    edit={this.numericParams}
                    defaultValue={0}
                  />
                  <ColumnDirective
                    field="observations"
                    headerText="Observaciones"
                    width="100"
                  />
                  <ColumnDirective
                    field="invoiceId"
                    defaultValue={this.props.invoice?.id}
                    visible={false}
                  />
                </ColumnsDirective>

                <AggregatesDirective>
                  <AggregateDirective>
                    <AggregateColumnsDirective>
                      <AggregateColumnDirective
                        field="amount"
                        type="Sum"
                        format="N2"
                        footerTemplate={this.footerSumEuros}
                        groupCaptionTemplate={this.footerSumEuros}
                      >
                        {" "}
                      </AggregateColumnDirective>
                    </AggregateColumnsDirective>
                  </AggregateDirective>
                </AggregatesDirective>

                <Inject services={[ForeignKey, Toolbar, Aggregate]} />
              </GridComponent>
            </Row>
          </ModalBody>
          <ModalFooter></ModalFooter>
        </Modal>
      </Fragment>
    );
  }
}

ModalInvoicePaymentsHistory.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  toggle: PropTypes.func.isRequired,
  invoiceSelected: PropTypes.object,
  showMessage: PropTypes.func.isRequired,
};

export default ModalInvoicePaymentsHistory;
