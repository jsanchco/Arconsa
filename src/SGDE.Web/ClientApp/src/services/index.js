import {
  config,
  AUTHENTICATE,
  USERS,
  PROFESSIONS,
  WORKS,
  DOCUMENTS,
  ASSIGNWORKERS
} from "../constants";
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

export const updateWorkersInWork = (workers, idWork) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${ASSIGNWORKERS}`;
    let workersId = [];

    workers.forEach(worker => {
      workersId.push(worker.id);
    });

    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`
      },
      method: "POST",
      body: JSON.stringify({listIdUsers: workersId, idWork: idWork})
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

export const base64ToArrayBuffer = base64 => {
  var binaryString = window.atob(base64);
  var binaryLen = binaryString.length;
  var bytes = new Uint8Array(binaryLen);
  for (var i = 0; i < binaryLen; i++) {
    var ascii = binaryString.charCodeAt(i);
    bytes[i] = ascii;
  }
  return bytes;
};

export const saveByteArray = (reportName, byte, type) => {
  var blob = new Blob([byte], { type: type });
  var link = document.createElement("a");
  link.href = window.URL.createObjectURL(blob);
  var fileName = reportName;
  link.download = fileName;
  link.click();
};
