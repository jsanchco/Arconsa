import React, { Component } from "react";
import PropTypes from "prop-types";
import { Button, Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import "./modal-select.css";

class ModalSelectImage extends Component {
  constructor(props) {
    super(props);

    this.state = { file: "", imagePreviewUrl: "" };

    this._handleOnClick = this._handleOnClick.bind(this);
  }

  _handleImageChange(e) {
    e.preventDefault();

    let reader = new FileReader();
    let file = e.target.files[0];

    reader.onloadend = () => {
      this.setState({
        file: file,
        imagePreviewUrl: reader.result
      });
    };

    reader.readAsDataURL(file);
  }

  _handleOnClick() {
    this.props.toggle();

    this.props.updatePhoto(this.state.imagePreviewUrl);
  }

  render() {
    let { imagePreviewUrl } = this.state;
    let $imagePreview = null;
    if (imagePreviewUrl) {
      $imagePreview = <img src={imagePreviewUrl} alt="test" />;
    } else {
      $imagePreview = (
        <div className="previewText">
          Por favor, selecciona una imagen para previsualizarla
        </div>
      );
    }

    return (
      <Modal isOpen={this.props.isOpen} toggle={this.props.toggle}>
        <ModalHeader toggle={this.props.toggle}>Selecciona archivo</ModalHeader>
        <ModalBody>
          <label htmlFor="file-upload" className="custom-file-upload">
            <i className="fa fa-cloud-upload"></i> Seleccionar imagen
          </label>
          <input
            id="file-upload"
            accept="image/*"
            type="file"
            onChange={e => this._handleImageChange(e)}
          />
          {/* <input
            accept="image/*"
            type="file"
            onChange={e => this._handleImageChange(e)}
          /> */}
          <div className="imgPreview">{$imagePreview}</div>
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

ModalSelectImage.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  toggle: PropTypes.func.isRequired,
  updatePhoto: PropTypes.func,
  userId: PropTypes.number.isRequired,
  type: PropTypes.string.isRequired
};

export default ModalSelectImage;
