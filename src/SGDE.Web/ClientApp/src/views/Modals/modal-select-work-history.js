import React, { Component } from "react";
import PropTypes from "prop-types";
import { Button, Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import "./modal-select.css";

class ModalSelectWorkHistory extends Component {
  constructor(props) {
    super(props);

    this.state = { file: "", fileName: "", fileUrl: "" };

    this._handleOnClick = this._handleOnClick.bind(this);
  }

  _handleFileChange(e) {
    e.preventDefault();

    let reader = new FileReader();
    let file = e.target.files[0];
    let filename = file.name;
    let extension =
      filename.substring(filename.lastIndexOf(".") + 1, filename.length) ||
      filename;

    reader.onloadend = () => {
      let newFileName = this.props.rowSelected.reference
        .split(" ")
        .join("");
      newFileName = newFileName.split(".").join("");
      newFileName = newFileName.split("/").join("");
      newFileName = newFileName.split("\\").join("");
      newFileName = newFileName.split(":").join("");
      newFileName = newFileName.split("*").join("");
      newFileName = newFileName.split("?").join("");
      newFileName = newFileName.split('"').join("");
      newFileName = newFileName.split("<").join("");
      newFileName = newFileName.split(">").join("");
      newFileName = newFileName.split("|").join("");
      newFileName = newFileName.toLowerCase();
      newFileName = `${newFileName}.${extension}`;
      this.setState({
        file: file,
        fileName: newFileName,
        fileUrl: reader.result
      });
    };

    reader.readAsDataURL(file);
  }

  _handleOnClick() {
    this.props.toggle();

    this.props.updateDocument(this.state);
  }

  render() {
    let title = "";
    if (
      this.props.rowSelected !== null &&
      this.props.rowSelected !== undefined
    ) {
      title = (this.props.rowSelected.typeDocumentName !== null &&  this.props.rowSelected.typeDocumentName !== undefined) ?
        `Selecciona ${this.props.rowSelected.typeDocumentName}` :
        `Selecciona archivo`;
    }

    return (
      <Modal
        isOpen={this.props.isOpen}
        toggle={this.props.toggle}
        className={"modal-primary"}
      >
        <ModalHeader toggle={this.props.toggle}>{title}</ModalHeader>
        <ModalBody>
          <label htmlFor="file-upload" className="custom-file-upload">
            <i className="fa fa-cloud-upload"></i> Seleccionar archivo
          </label>
          <input
            id="file-upload"
            type="file"
            onChange={e => this._handleFileChange(e)}
          />
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

ModalSelectWorkHistory.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  toggle: PropTypes.func.isRequired,
  updateDocument: PropTypes.func,
  type: PropTypes.string.isRequired,
  rowSelected: PropTypes.object
};

export default ModalSelectWorkHistory;
