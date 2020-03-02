import React, { Component } from "react";
import { Row } from "reactstrap";
import {
  ScheduleComponent,
  Day,
  Week,
  WorkWeek,
  Month,
  Agenda,
  Inject,
  ViewsDirective,
  ViewDirective
} from "@syncfusion/ej2-react-schedule";
import { extend } from "@syncfusion/ej2-base";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../../locales/locale.json";
import * as dataSource from "./datasource.json";

L10n.load(data);

class WorkOrders extends Component {
  constructor(props) {
    super(props);

    this.data = extend([], dataSource.zooEventsData, null, true);
  }

  onEventRendered(args) {
    let categoryColor = args.data.CategoryColor;
    if (!args.element || !categoryColor) {
      return;
    }
    if (this.scheduleObj.currentView === "Agenda") {
      args.element.firstChild.style.borderLeftColor = categoryColor;
    } else {
      args.element.style.backgroundColor = categoryColor;
    }
  }

  render() {
    return (
      <div className="animated fadeIn">
        <div className="card">
          <div className="card-header">
            <i className="icon-calendar"></i> Mi Agenda
          </div>
          <div className="card-body"></div>
          <Row>
            <ScheduleComponent
              locale="es-US"
              style={{
                marginLeft: 30,
                marginRight: 30,
                marginTop: -20,
                marginBottom: 20
              }}
              selectedDate={new Date(2018, 1, 15)}
              ref={t => (this.scheduleObj = t)}
              eventSettings={{ dataSource: this.data }}
              eventRendered={this.onEventRendered.bind(this)}
            >
              <ViewsDirective>
                <ViewDirective option="Day" />
                <ViewDirective option="Week" />
                <ViewDirective option="WorkWeek" />
                <ViewDirective option="Month" />
              </ViewsDirective>
              <Inject services={[Day, Week, WorkWeek, Month, Agenda]} />
            </ScheduleComponent>
          </Row>
        </div>
      </div>
    );
  }
}

WorkOrders.propTypes = {};

export default WorkOrders;
