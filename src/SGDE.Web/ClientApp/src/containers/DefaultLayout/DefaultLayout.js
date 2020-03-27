import React, { Component, Suspense, Fragment } from "react";
import { Redirect, Route, Switch } from "react-router-dom";
import * as router from "react-router-dom";
import { Container } from "reactstrap";
import { connect } from "react-redux";

import {
  AppAside,
  AppFooter,
  AppHeader,
  AppSidebar,
  AppSidebarFooter,
  AppSidebarForm,
  AppSidebarHeader,
  AppSidebarMinimizer,
  AppBreadcrumb2 as AppBreadcrumb,
  AppSidebarNav2 as AppSidebarNav
} from "@coreui/react";
// sidebar nav config
import navigation from "../../_nav";
import navigation1 from "../../nav-by-role/_nav-1";
import navigation2 from "../../nav-by-role/_nav-2";
import navigation3 from "../../nav-by-role/_nav-3";
// routes config
import routes from "../../routes";
import ReactNotification, { store } from "react-notifications-component";
import "react-notifications-component/dist/theme.css";

const DefaultAside = React.lazy(() => import("./DefaultAside"));
const DefaultFooter = React.lazy(() => import("./DefaultFooter"));
const DefaultHeader = React.lazy(() => import("./DefaultHeader"));

class DefaultLayout extends Component {
  componentDidUpdate(prevProps) {
    if (
      this.props.messageApplication != null &&
      prevProps.messageApplication !== this.props.messageApplication
    ) {
      this.showMessage();
    }
  }

  loading = () => (
    <div className="animated fadeIn pt-1 text-center">Cargando...</div>
  );

  signOut(e) {
    e.preventDefault();
    this.props.history.push("/login");
  }

  showMessage() {
    let message;
    if (this.props.messageApplication.type === "danger") {
      if (this.props.messageApplication.responseText !== undefined) {
        message = this.props.messageApplication.responseText.toString();
      } else {
        message = this.props.messageApplication.toString();
      }
    } else {
      message = this.props.messageApplication.statusText;
    }
    if (message === "" || message === undefined) {
      message = "ERROR en la petición";
    }

    store.addNotification({
      message: message,
      type: this.props.messageApplication.type,
      container: "bottom-center",
      animationIn: ["animated", "fadeIn"],
      animationOut: ["animated", "fadeOut"],
      dismiss: {
        duration: 5000,
        showIcon: true
      },
      width: 800
    });
  }

  renderAppSidebarNav() {
    const user = JSON.parse(localStorage.getItem("user"));
    
    switch (user.roleId) {
      case 1:
        return (
          <AppSidebarNav
            navConfig={navigation1}
            {...this.props}
            router={router}
          />
        );
      case 2:
        return (
          <AppSidebarNav
            navConfig={navigation2}
            {...this.props}
            router={router}
          />
        );
      case 3:
        return (
          <AppSidebarNav
            navConfig={navigation3}
            {...this.props}
            router={router}
          />
        );

      default:
        return (
          <AppSidebarNav
            navConfig={navigation}
            {...this.props}
            router={router}
          />
        );
    }
  }

  render() {
    return (
      <Fragment>
        <ReactNotification />
        <div className="app">
          <AppHeader fixed>
            <Suspense fallback={this.loading()}>
              <DefaultHeader onLogout={e => this.signOut(e)} history={this.props.history} />
            </Suspense>
          </AppHeader>
          <div className="app-body">
            <AppSidebar fixed display="lg">
              <AppSidebarHeader />
              <AppSidebarForm />
              <Suspense>

                {this.renderAppSidebarNav()}

                {/* <AppSidebarNav
                  navConfig={navigation}
                  {...this.props}
                  router={router}
                /> */}
              </Suspense>
              <AppSidebarFooter />
              <AppSidebarMinimizer />
            </AppSidebar>
            <main className="main">
              <AppBreadcrumb appRoutes={routes} router={router} />
              <Container fluid>
                <Suspense fallback={this.loading()}>
                  <Switch>
                    {routes.map((route, idx) => {
                      return route.component ? (
                        <Route
                          key={idx}
                          path={route.path}
                          exact={route.exact}
                          name={route.name}
                          render={props => <route.component {...props} />}
                        />
                      ) : null;
                    })}
                    <Redirect from="/" to="/dashboard" />
                  </Switch>
                </Suspense>
              </Container>
            </main>
            <AppAside fixed>
              <Suspense fallback={this.loading()}>
                <DefaultAside />
              </Suspense>
            </AppAside>
          </div>
          <AppFooter>
            <Suspense fallback={this.loading()}>
              <DefaultFooter />
            </Suspense>
          </AppFooter>
        </div>
      </Fragment>
    );
  }
}

const mapStateToProps = state => {
  return {
    messageApplication: state.applicationReducer.message
  };
};

export default connect(mapStateToProps)(DefaultLayout);
