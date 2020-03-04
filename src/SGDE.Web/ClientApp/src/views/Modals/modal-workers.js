import React, { Component } from "react";
import PropTypes from "prop-types";
import { Button, Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import { Row } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Inject
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor } from "@syncfusion/ej2-data";
import { config, USERS } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY } from "../../services";

L10n.load(data);

class ModalWorkers extends Component {
  workers = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${USERS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  grid = null;

  constructor(props) {
    super(props);

    this.state = { workers: null };

    this._handleOnClick = this._handleOnClick.bind(this);
  }

  _handleOnClick() {
    this.props.toggle();
  }

  render() {
    return (
      <Modal
        isOpen={this.props.isOpen}
        toggle={this.props.toggle}
        className={"modal-lg"}
      >
        <ModalHeader toggle={this.props.toggle}> Trabajadores</ModalHeader>
        <ModalBody>
          <Row>
            <GridComponent
              dataSource={this.workers}
              locale="es-US"
              style={{
                marginLeft: 30,
                marginRight: 30,
                marginTop: 20,
                marginBottom: 20
              }}
              rowSelected={this.rowSelected}
              ref={g => (this.grid = g)}
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
                <ColumnDirective field="name" headerText="Nombre" width="100" />
              </ColumnsDirective>
              <Inject services={[]} />
            </GridComponent>
          </Row>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={this._handleOnClick}>
            Guardar
          </Button>
          <Button
            color="secondary"
            style={{ marginLeft: "10px" }}
            onClick={this.props.toggle}
          >
            Cancelar
          </Button>
        </ModalFooter>
      </Modal>
    );
  }
}

ModalWorkers.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  toggle: PropTypes.func.isRequired,
  rowSelected: PropTypes.object
};

export default ModalWorkers;
