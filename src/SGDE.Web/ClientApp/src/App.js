import React, { Component } from "react";
import { HashRouter, Route, Switch } from "react-router-dom";
// import { renderRoutes } from 'react-router-config';
import "./App.scss";
import PrivateRoute from "./routes/PrivateRoute";
import Login from "./views/Pages/Login";
import DefaultLayout from "./containers/DefaultLayout/DefaultLayout";
import { Provider } from "react-redux";
import store from "./store/store";
import data from "./locales/locale.json";

import { loadCldr, L10n, setCulture, setCurrencyCode } from "@syncfusion/ej2-base";

import gregorian from 'cldr-data/main/es/ca-gregorian.json';
import numbers from 'cldr-data/main/es/numbers.json';
import currencyData from 'cldr-data/main/es/currencies.json';
import timeZoneNames from 'cldr-data/main/es/timeZoneNames.json';
import numberingSystems from 'cldr-data/supplemental/numberingSystems.json';
import weekData from 'cldr-data/supplemental/weekData.json';

loadCldr(numberingSystems, currencyData, gregorian, numbers, timeZoneNames, weekData);
setCulture("es");
setCurrencyCode("EUR");
L10n.load(data);

loadCldr(
  require("cldr-data/supplemental/numberingSystems.json"),
  require("cldr-data/main/es-US/ca-gregorian.json"),
  require("cldr-data/main/es-US/numbers.json"),
  require("cldr-data/main/es-US/timeZoneNames.json"),
  require("cldr-data/supplemental/weekData.json")
);

const loading = () => (
  <div className="animated fadeIn pt-3 text-center">Cargando...</div>
);


// Containers
// const DefaultLayout = React.lazy(() => import('./containers/DefaultLayout'));

// Pages
// const Login = React.lazy(() => import('./views/Pages/Login'));
const Register = React.lazy(() => import("./views/Pages/Register"));
const Page404 = React.lazy(() => import("./views/Pages/Page404"));
const Page500 = React.lazy(() => import("./views/Pages/Page500"));

class App extends Component {
  render() {
    return (
      <Provider store={store}>
        <HashRouter>
          <React.Suspense fallback={loading()}>
            <Switch>
              <Route
                exact
                path="/login"
                name="Login Page"
                render={props => <Login {...props} />}
              />
              <Route
                exact
                path="/register"
                name="Register Page"
                render={props => <Register {...props} />}
              />
              <Route
                exact
                path="/404"
                name="Page 404"
                render={props => <Page404 {...props} />}
              />
              <Route
                exact
                path="/500"
                name="Page 500"
                render={props => <Page500 {...props} />}
              />
              <PrivateRoute path="/" name="Home" component={DefaultLayout} />
            </Switch>
          </React.Suspense>
        </HashRouter>
      </Provider>
    );
  }
}

export default App;
