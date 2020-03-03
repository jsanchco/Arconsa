import { config, AUTHENTICATE, USERS, PROFESSIONS, WORKS, DOCUMENTS } from "../constants";
import store from "../store/store";
import ACTION_AUTHENTICATION from "../actions/authenticationAction";
import ACTION_APPLICATION from "../actions/applicationAction";

export const TOKEN_KEY = "jwt";

export const login = (username, password, history) => {
  const url = `${config.URL_API}/${AUTHENTICATE}`;
  fetch(url, {
    headers: {
      Accept: "text/plain",
      "Content-Type": "application/json"
    },
    method: "POST",
    body: JSON.stringify({ username, password })
  })
    .then(data => data.json())
    .then(result => {
      if (result.user != null && result.token != null) {
        localStorage.setItem("user", JSON.stringify(result.user));
        localStorage.setItem(TOKEN_KEY, result.token);
        store.dispatch(ACTION_AUTHENTICATION.logIn(result.user, result.token));
        history.push("/dashboard");
      } else {
        if (result.Message) {
          console.log("error ->", result.Message);
          localStorage.removeItem("user");
          localStorage.removeItem(TOKEN_KEY);
          store.dispatch(ACTION_AUTHENTICATION.logOut());
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger"
            })
          );
        }
      }
    })
    .catch(error => {
      console.log("error ->", error);
      store.dispatch(
        ACTION_APPLICATION.showMessage({
          statusText: error,
          responseText: error,
          type: "danger"
        })
      );
    });
};

export const logout = () => {
  localStorage.removeItem(TOKEN_KEY);
};

export const isLogin = () => {
  if (localStorage.getItem(TOKEN_KEY)) {
    return true;
  }

  return false;
};

export const getUsers = () => {
  const url = `${config.URL_API}/${USERS}`;
  fetch(url, {
    method: "GET"
  })
    .then(data => data.json())
    .then(result => {
      console.log("result ->", result);
      return result;
    })
    .catch(error => {
      console.log("error ->", error);
    });
};

export const getProfessions = () => {
  const url = `${config.URL_API}/${PROFESSIONS}`;
  fetch(url, {
    method: "GET"
  })
    .then(data => data.json())
    .then(result => {
      console.log("result ->", result);
      return result;
    })
    .catch(error => {
      console.log("error ->", error);
    });
};

export const updateUser = user => {
  return new Promise((resolve, reject) => {
  const url = `${config.URL_API}/${USERS}`;
  fetch(url, {
    headers: {
      Accept: "text/plain",
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`
    },
    method: "PUT",
    body: JSON.stringify(user)
  })
    .then(data => data.json())
    .then(result => {
      if (result.Message) {
        console.log("error ->", result.Message);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: result.Message,
            responseText: result.Message,
            type: "danger"
          })
        );
        reject();
      } else {
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: "200",
            responseText: "Operación realizada con éxito",
            type: "success"
          })
        );
        resolve();
      }
    })
    .catch(error => {
      console.log("error ->", error);
      store.dispatch(
        ACTION_APPLICATION.showMessage({
          statusText: error,
          responseText: error,
          type: "danger"
        })
      );
      reject();
    });
  });
};

export const updateWork = work => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${WORKS}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`
      },
      method: "PUT",
      body: JSON.stringify(work)
    })
      .then(data => data.json())
      .then(result => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger"
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success"
            })
          );
          resolve();
        }
      })
      .catch(error => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger"
          })
        );
        reject();
      });
  });
};

export const updateDocument = document => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${DOCUMENTS}`;
    const u = JSON.stringify(document);
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`
      },
      method: "PUT",
      body: JSON.stringify(document)
    })
      .then(data => data.json())
      .then(result => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger"
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success"
            })
          );
          resolve();
        }
      })
      .catch(error => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger"
          })
        );
        reject();
      });
  });
};
