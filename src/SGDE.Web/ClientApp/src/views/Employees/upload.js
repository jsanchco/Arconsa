import React, { Component } from "react";
import { Col, FormGroup, Input, Label, Row } from "reactstrap";
import { UploaderComponent } from "@syncfusion/ej2-react-inputs";
import { config, UPLOADBOX_SAVE } from "../../constants";
import { TOKEN_KEY } from "../../services";

class Upload extends Component {
  constructor(props) {
    super(props);

    this.dropContainerEle = null;
    this.dropContainerRef = element => {
      this.dropContainerEle = element;
    };
    this.asyncSettings = {
      saveUrl: `${config.URL_API}/${UPLOADBOX_SAVE}`,
      removeUrl:
        "https://aspnetmvc.syncfusion.com/services/api/uploadbox/Remove"
    };
    this.allowedExtensions= '.jpg, .gif, .png'
    // this.onFileUpload = this.onFileUpload.bind(this);
  }

  onRemoveFile(args) {
    args.postRawFile = false;
  }

  onFileUpload(args) {
    args.currentRequest.setRequestHeader("Authorization", "Bearer " + localStorage.getItem(TOKEN_KEY)) 
    args.customFormData = [{"data": JSON.stringify({userId: this.props.user.id, type: "image"})}];
  }

  render() {
    return (
      <div
        style={{
          marginLeft: 10,
          marginRight: 60,
          marginTop: 20,
          marginBottom: 20
        }}
      >
        <UploaderComponent
          id="fileUpload"
          type="file"
          ref={scope => {
            this.uploadObj = scope;
          }}
          asyncSettings={this.asyncSettings}
          removing={this.onRemoveFile.bind(this)}
          uploading={this.onFileUpload = this.onFileUpload.bind(this)}
          allowedExtensions= {this.allowedExtensions}    
        ></UploaderComponent>
      </div>
    );
  }
}

Upload.propTypes = {};

export default Upload;
