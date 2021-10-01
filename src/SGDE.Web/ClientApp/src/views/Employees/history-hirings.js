import React, { Component, Fragment } from "react";
import { Row } from "reactstrap";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
  Inject,
  Page,
} from "@syncfusion/ej2-react-grids";
import { DataManager, WebApiAdaptor, Query } from "@syncfusion/ej2-data";
import { config, USERSHIRING } from "../../constants";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../locales/locale.json";
import { TOKEN_KEY } from "../../services";

L10n.load(data);

class HistoryHirings extends Component {
  userHirings = new DataManager({
    adaptor: new WebApiAdaptor(),
    url: `${config.URL_API}/${USERSHIRING}`,
    headers: [{ Authorization: "Bearer " + localStorage.getItem(TOKEN_KEY) }],
  });

  grid = null;

  wrapSettings = { wrapMode: "Content" };

  constructor(props) {
    super(props);

    this.state = {
      trainings: null,
    };

    this.format = { type: "dateTime", format: "dd/MM/yyyy" };

    this.pageSettings = { pageCount: 10, pageSize: 10 };
    this.query = new Query().addParams("userId", props.userId);
  }

  render() {
    return (
      <Fragment>
        <div className="animated fadeIn">
          <div
            className="card"
            style={{ marginRight: "60px", marginTop: "20px" }}
          >
            <div className="card-header">
              <i className="icon-layers"></i> Historial de Contratación
            </div>
            <div className="card-body"></div>
            <Row>
              <GridComponent
                dataSource={this.userHirings}
                locale="es-US"
                allowPaging={true}
                pageSettings={this.pageSettings}
                style={{
                  marginLeft: 30,
                  marginRight: 30,
                  marginTop: -20,
                  marginBottom: 20,
                }}
                allowGrouping={false}
                ref={(g) => (this.grid = g)}
                query={this.query}
                allowTextWrap={true}
                textWrapSettings={this.wrapSettings}
              >
                <ColumnsDirective>
                  <ColumnDirective
                    field="clientName"
                    headerText="Cliente"
                    width="100"
                  />
                  <ColumnDirective
                    field="name"
                    headerText="Obra (dia contrat.)"
                    width="100"
                  />
                  <ColumnDirective
                    field="startDate"
                    headerText="Fecha Inicio"
                    width="100"
                    type="date"
                    format={this.format}
                    editType="datepickeredit"
                  />
                  <ColumnDirective
                    field="endDate"
                    headerText="Fecha Fin"
                    width="100"
                    type="date"
                    format={this.format}
                    editType="datepickeredit"
                  />
                  <ColumnDirective
                    field="professionName"
                    headerText="Profesión"
                    width="100"
                  />
                  />
                </ColumnsDirective>
                <Inject services={[Page]} />
              </GridComponent>
            </Row>
          </div>
        </div>
      </Fragment>
    );
  }
}

HistoryHirings.propTypes = {};

export default HistoryHirings;
