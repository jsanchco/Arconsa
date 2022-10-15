import React, { Component } from "react";
import { Form, Col, Label, Row, Container } from "reactstrap";
import { getWorkClosePage } from "../../services";
import {
  createSpinner,
  showSpinner,
  hideSpinner,
} from "@syncfusion/ej2-popups";

class ClosePage extends Component {
  constructor(props) {
    super(props);

    this.state = {};

    getWorkClosePage(this.props.workId).then((result) => {
      if (this.state.closePageData !== result)
        this.setState({
          closePageData: result,
        });
    });

    this.updateClosePage = this.updateClosePage.bind(this);
  }

  componentDidUpdate(prevProps, prevState) {
    if (
      prevProps.tabSelected?.selectedIndex ===
        this.props.tabSelected?.previousIndex &&
      prevProps.tabSelected?.selectedIndex === 7
    ) {
      this.updateClosePage();
    }
  }

  updateClosePage() {
    const closePageData = this.state.closePageData;
    const element = document.getElementById("container-close-page");

    createSpinner({
      target: element,
    });
    showSpinner(element);

    getWorkClosePage(this.props.workId)
      .then((result) => {
        if (JSON.stringify(closePageData) !== JSON.stringify(result)) {
          console.log("close-page -> updateClosePage");
          this.setState({
            closePageData: result,
          });
        }

        hideSpinner(element);
      })
      .catch(() => {
        hideSpinner(element);
      });
  }

  render() {
    return (
      <div
        style={{
          marginLeft: 10,
          marginRight: 60,
          marginTop: 20,
          marginBottom: 20,
        }}
        id="container-close-page"
      >
        <Form>
          <Container
            style={{
              backgroundColor: "lightskyblue",
            }}
          >
            <Row>
              <Col xs="12">&nbsp;</Col>
            </Row>
            <Row>
              <Col xs="2">
                <Label
                  style={{
                    fontWeight: "bold",
                  }}
                >
                  FECHA APERTURA
                </Label>
              </Col>
              <Col xs="3">
                <Label>{this.state?.closePageData?.openDate}</Label>
              </Col>
              <Col xs="2">
                <Label
                  style={{
                    fontWeight: "bold",
                  }}
                >
                  FECHA CIERRE
                </Label>
              </Col>
              <Col xs="3">
                <Label>{this.state?.closePageData?.closeDate}</Label>
              </Col>
            </Row>
            <Row>
              <Col xs="2">
                <Label
                  style={{
                    fontWeight: "bold",
                  }}
                >
                  CLIENTE
                </Label>
              </Col>
              <Col xs="7">
                <Label>
                  <a
                    rel="nofollow"
                    href={
                      "/#/clients/detailclient/" +
                      this.state?.closePageData?.clientId
                    }
                    style={{ color: "black" }}
                  >
                    {this.state?.closePageData?.clientName}
                  </a>
                </Label>
              </Col>
            </Row>
            <Row>
              <Col xs="2">
                <Label
                  style={{
                    fontWeight: "bold",
                  }}
                >
                  OBRA
                </Label>
              </Col>
              <Col xs="3">
                <Label>{this.state?.closePageData?.workName}</Label>
              </Col>
              <Col xs="2">
                <Label
                  style={{
                    fontWeight: "bold",
                  }}
                >
                  DIRECCIÓN
                </Label>
              </Col>
              <Col xs="2">
                <Label>{this.state?.closePageData?.workAddress}</Label>
              </Col>
            </Row>
            <Row>
              <Col xs="4">&nbsp;</Col>
            </Row>
          </Container>

          <Row>
            <Col xs="12">&nbsp;</Col>
          </Row>

          <Container
            style={{
              backgroundColor: "bisque",
            }}
          >
            <Row>
              <Col xs="12">&nbsp;</Col>
            </Row>
            <Row>
              <Col xs="2">
                <Label
                  style={{
                    fontWeight: "bold",
                  }}
                >
                  PRESUPUESTO(S)
                </Label>
              </Col>
              <Col xs="9">
                <Label>{this.state?.closePageData?.workBudgetsName}</Label>
              </Col>
            </Row>
            <Row>
              <Col xs="2">
                <Label
                  style={{
                    fontWeight: "bold",
                  }}
                >
                  TOTAL
                </Label>
              </Col>
              <Col xs="9">
                <Label>{this.state?.closePageData?.workBudgetsSumFormat}</Label>
              </Col>
            </Row>
            <Row>
              <Col xs="12">&nbsp;</Col>
            </Row>
          </Container>

          <Row>
            <Col xs="12">&nbsp;</Col>
          </Row>

          <Container
            style={{
              backgroundColor: "lightgreen",
            }}
          >
            <Row>
              <Col xs="12">&nbsp;</Col>
            </Row>
            <Row>
              <Col xs="2">
                <Label
                  style={{
                    fontWeight: "bold",
                  }}
                >
                  INGRESOS/FACTURAS
                </Label>
              </Col>
              <Col
                xs="1"
                style={{
                  textAlign: "right",
                }}
              >
                <Label>{this.state?.closePageData?.invoicesSum}€</Label>
              </Col>
            </Row>
            <Row>
              <Col xs="2">
                <Label
                  style={{
                    fontWeight: "bold",
                  }}
                >
                  GASTOS PROVEEDORES
                </Label>
              </Col>
              <Col
                xs="1"
                style={{
                  textAlign: "right",
                }}
              >
                <Label>{this.state?.closePageData?.workCostsSum}€</Label>
              </Col>
            </Row>
            <Row>
              <Col xs="2">
                <Label
                  style={{
                    fontWeight: "bold",
                  }}
                >
                  GASTOS TRABAJADORES
                </Label>
              </Col>
              <Col
                xs="1"
                style={{
                  textAlign: "right",
                }}
              >
                <Label>
                  {this.state?.closePageData?.authorizeCancelWorkersCostsSum}€
                </Label>
              </Col>
            </Row>
            <Row>
              <Col xs="2">
                <Label
                  style={{
                    fontWeight: "bold",
                  }}
                >
                  GASTOS INDIRECTOS
                </Label>
              </Col>
              <Col
                xs="1"
                style={{
                  textAlign: "right",
                }}
              >
                <Label>¿?€</Label>
              </Col>
            </Row>
            <Row>
              <Col xs="2">
                <Label
                  style={{
                    fontWeight: "bold",
                    fontSize: "larger"
                  }}
                >
                  TOTAL
                </Label>
              </Col>
              <Col
                xs="1"
                style={{
                  textAlign: "right",
                }}
              >
                <Label
                  style={{
                    fontSize: "larger",
                  }}
                >
                  {this.state?.closePageData?.total}€
                </Label>
              </Col>
            </Row>
            <Row>
              <Col xs="12">&nbsp;</Col>
            </Row>
          </Container>
        </Form>
      </div>
    );
  }
}

ClosePage.propTypes = {};

export default ClosePage;
