import React, { Component } from "react";
import { Row } from "reactstrap";
import { L10n } from "@syncfusion/ej2-base";
import data from "../../../locales/locale.json";
import {
  Day,
  WorkWeek,
  Month,
  ScheduleComponent,
  ResourcesDirective,
  ResourceDirective,
  ViewsDirective,
  ViewDirective,
  Inject,
  TimelineViews,
  Resize,
  DragAndDrop
} from "@syncfusion/ej2-react-schedule";
import "./group-editing.css";
import { extend } from "@syncfusion/ej2-base";
import * as dataSource from "./datasource.json";

L10n.load(data);

class SharedScheduler extends Component {
  constructor(props) {
    super(props);

    this.data = extend([], dataSource.resourceConferenceData, null, true);
    this.resourceData = [
      { Text: "Margaret", Id: 1, Color: "#1aaa55" },
      { Text: "Robert", Id: 2, Color: "#357cd2" },
      { Text: "Laura", Id: 3, Color: "#7fa900" }
    ];
  }

  getEmployeeName(value) {
    return value.resourceData
      ? value.resourceData[value.resource.textField]
      : value.resourceName;
  }

  getEmployeeImage(value) {
    let resourceName = this.getEmployeeName(value);
    return resourceName.replace(" ", "-").toLowerCase();
  }

  getEmployeeDesignation(value) {
    let resourceName = this.getEmployeeName(value);
    return resourceName === "Margaret"
      ? "Sales Representative"
      : resourceName === "Robert"
      ? "Vice President, Sales"
      : "Inside Sales Coordinator";
  }

  monthEventTemplate(props) {
    return <div className="subject">{props.Subject}</div>;
  }
  
  resourceHeaderTemplate(props) {
    return (
      <div className="template-wrap">
        <div className={"resource-image " + this.getEmployeeImage(props)}></div>
        <div className="resource-details">
          <div className="resource-name">{this.getEmployeeName(props)}</div>
          <div className="resource-designation">
            {this.getEmployeeDesignation(props)}
          </div>
        </div>
      </div>
    );
  }

  render() {
    return (
      <div className="animated fadeIn">
        <div className="card">
          <div className="card-header">
            <i className="icon-calendar"></i> Compartir Agenda
          </div>
          <div className="card-body"></div>
          <Row>
            <div className="schedule-control-section">
              <div className="col-lg-12 control-section">
                <div className="control-wrapper">
                  <ScheduleComponent
                    cssClass="group-editing"
                    selectedDate={new Date(2018, 5, 5)}
                    currentView="WorkWeek"
                    resourceHeaderTemplate={this.resourceHeaderTemplate.bind(
                      this
                    )}
                    locale="es-US"
                    style={{
                      marginLeft: 30,
                      marginRight: 30,
                      marginTop: -20,
                      marginBottom: 20
                    }}
                    eventSettings={{
                      dataSource: this.data,
                      fields: {
                        subject: { title: "Conference Name", name: "Subject" },
                        description: { title: "Summary", name: "Description" },
                        startTime: { title: "From", name: "StartTime" },
                        endTime: { title: "To", name: "EndTime" }
                      }
                    }}
                    group={{ allowGroupEdit: true, resources: ["Conferences"] }}
                  >
                    <ResourcesDirective>
                      <ResourceDirective
                        field="ConferenceId"
                        title="Attendees"
                        name="Conferences"
                        allowMultiple={true}
                        dataSource={this.resourceData}
                        textField="Text"
                        idField="Id"
                        colorField="Color"
                      ></ResourceDirective>
                    </ResourcesDirective>
                    <ViewsDirective>
                      <ViewDirective option="Day" />
                      <ViewDirective option="WorkWeek" />
                      <ViewDirective
                        option="Month"
                        eventTemplate={this.monthEventTemplate.bind(this)}
                      />
                      <ViewDirective option="TimelineWeek" />
                    </ViewsDirective>
                    <Inject
                      services={[
                        Day,
                        WorkWeek,
                        Month,
                        TimelineViews,
                        Resize,
                        DragAndDrop
                      ]}
                    />
                  </ScheduleComponent>
                </div>
              </div>
            </div>
          </Row>
        </div>
      </div>
    );
  }
}

SharedScheduler.propTypes = {};

export default SharedScheduler;
