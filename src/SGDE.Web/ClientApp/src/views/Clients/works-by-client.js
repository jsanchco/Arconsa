import React, { Component } from "react";
import { Row } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Inject,
  Page,
  ForeignKey,
  Group,
  Resize
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, WORKS, CLIENTS } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY } from "../../services";
import Legend from "../../components/legend";

L10n.load(data);

class WorksByClient extends Component {
  works = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${WORKS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  clients = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${CLIENTS}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }]
  });

  grid = null;
  clientIdRules = { required: true };
  numericParams = {
    params: {
      decimals: 0,
      format: "N",
      validateDecimalOnType: true,
      showSpinButton: false
    }
  };

  constructor(props) {
    super(props);

    this.state = {
      works: null,
      clients: null,
      rowSelected: null,
      rowSelectedindex: null,
      modal: false
    };

    this.pageSettings = { pageCount: 10, pageSize: 10 };
    this.rowSelected = this.rowSelected.bind(this);
    this.openTemplate = this.openTemplate.bind(this);
    this.dateTemplate = this.dateTemplate.bind(this);
    this.workTemplate = this.workTemplate.bind(this);

    this.query = new Query()
      .addParams("clientId", props.clientId)
      .addParams("showCloseWorks", true);
  }

  openTemplate(args) {
    if (args.open === true) {
      return (
        <div>
          <span className="dot-green"></span>
        </div>
      );
    } else {
      return (
        <div>
          <span className="dot-red"></span>
        </div>
      );
    }
  }

  formatDate(args) {
    if (args === null || args === "") {
      return "";
    }

    let day = args.getDate();
    if (day < 10)
      day = "0" + day;
      
    const month = args.getMonth() + 1;
    const year = args.getFullYear();

    if (month < 10) {
      return `${day}/0${month}/${year}`;
    } else {
      return `${day}/${month}/${year}`;
    }
  }

  dateTemplate(args) {
    // const titleOpen = `${this.formatDate(args.openDate)}`;
    // const titleClose = `${this.formatDate(args.closeDate)}`;
    return (
      <div>
        <div style={{ display: "flex" }}>
          <div style={{ textAlign: "left", width: "50%" }}>Apertura:</div>
          <div style={{ textAlign: "left", width: "50%" }}>{args.openDate}</div>
        </div>
        <div style={{ display: "flex" }}>
          <div style={{ textAlign: "left", width: "50%" }}>Cierre:</div>
          <div style={{ textAlign: "left", width: "50%" }}>
            {args.closeDate}
          </div>
        </div>
      </div>
    );
  }

  workTemplate(args) {
    return ( 
      <div> 
        <a rel='nofollow' href={"/#/works/detailwork/" + args.id}>{args.name}</a> 
      </div> 
    ); 
  }     

  rowSelected() {
    const selectedRecords = this.grid.getSelectedRecords();
    const selectedRowIndex = this.grid.getSelectedRowIndexes();
    this.setState({
      rowSelected: selectedRecords[0],
      rowSelectedindex: selectedRowIndex[0]
    });
  }

  render() {
    let title = ` Obras`;

    return (
      <div className="animated fadeIn" id="target-works">
        <div className="card" style={{ marginRight: "60px", marginTop: "20px" }}>
          <div className="card-header">
            <i className="icon-globe"></i>{title} 
          </div>
          <div className="card-body"></div>

          <div
            style={{
              marginLeft: "35px",
              marginTop: "-20px",
              marginBottom: "30px"
            }}
          >
            <Legend
              elements={[
                { color: "dot-green", text: "Obra Abierta" },
                { color: "dot-red", text: "Obra Cerrada" }
              ]}
            />
          </div>

          <Row>
            <GridComponent
              dataSource={this.works}
              locale="es-US"
              allowPaging={true}
              pageSettings={this.pageSettings}
              toolbar={this.toolbarOptions}
              toolbarClick={this.clickHandler}
              editSettings={this.editSettings}
              style={{
                marginLeft: 30,
                marginRight: 30,
                marginTop: -20,
                marginBottom: 20
              }}
              allowGrouping={true}
              rowSelected={this.rowSelected}
              ref={g => (this.grid = g)}
              selectionSettings={this.selectionSettings}
              query={this.query}
              allowResizing={true}
            >
              <ColumnsDirective>
                <ColumnDirective 
                  field="name" 
                  headerText="Nombre" 
                  width="100" 
                  template={this.workTemplate}
                />
                <ColumnDirective
                  field="address"
                  headerText="Dirección"
                  width="100"
                />
                {/* <ColumnDirective
                  field="estimatedDuration"
                  headerText="Duración Estimada"
                  width="100"
                /> */}
                <ColumnDirective
                  field="worksToRealize"
                  headerText="Trabajos a Realizar"
                  width="100"
                />
                <ColumnDirective
                  field="numberPersonsRequested"
                  headerText="Trabajadores"
                  width="70"
                  fotmat="N0"
                  textAlign="right"
                  editType="numericedit"
                  edit={this.numericParams}
                />
                {/* <ColumnDirective
                  field="clientId"
                  headerText="Cliente"
                  width="100"
                  editType="dropdownedit"
                  foreignKeyValue="name"
                  foreignKeyField="id"
                  validationRules={this.clientIdRules}
                  dataSource={this.clients}
                /> */}
                <ColumnDirective
                  field="closeDate"
                  headerText="Fechas"
                  width="120"
                  template={this.dateTemplate}
                  textAlign="Center"
                  allowEditing={false}
                />
                <ColumnDirective
                  field="open"
                  headerText="Ab./Cer."
                  width="70"
                  template={this.openTemplate}
                  textAlign="Center"
                  allowEditing={false}
                  defaultValue={true}
                />
                <ColumnDirective field="openDate" visible={false} />
                <ColumnDirective field="closeDate" visible={false} />
              </ColumnsDirective>
              <Inject
                services={[Group, ForeignKey, Page, Resize ]}
              />
            </GridComponent>
          </Row>
        </div>
      </div>
    );
  }
}

WorksByClient.propTypes = {};

export default WorksByClient;