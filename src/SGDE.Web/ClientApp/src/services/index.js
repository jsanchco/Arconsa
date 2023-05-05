import {
  config,
  AUTHENTICATE,
  USERS,
  PROFESSIONS,
  WORKS,
  DOCUMENTS,
  ASSIGNWORKERS,
  WORKERSHIRING,
  CLIENTS,
  UPDATEPASSWORD,
  UPDATEDATESWORK,
  INVOICERESPONSES,
  COMPANY_DATA,
  SETTINGS,
  RESTOREPASSWORD,
  USERSHIRING,
  VIEWMASSIVESIGNING,
  SENDMASSIVESIGNING,
  REMOVEALLDAILYSIGNING,
  PRINTINVOICE,
  REPORT_CURRENT_STATUS,
  DETAILINVOICEBYHOURSWORKER,
  IMPORTPREVIOUSINVOICE,
  HISTORYHIRINGUPDATEINWORK,
  PROFESSIONSBYUSER,
  WORKCOSTS,
  WORKCOSTBYID,
  WORKBUDGETDATAS,
  WORKBUDGETS,
  REMOVEALLWORKCOSTS,
  COMPANY_ADD_INDIRECTCOSTS,
  EMBARGOS,
  DETAILSEMBARGO,
  COMPANY_LIBRARIES,
  COMPANY_DOCUMENTS,
  WORKHISTORIES,
  WORKCLOSEPAGE,
  BILLPAYMENTWITHAMOUNT,
  ENTERPRISES,
  ENTERPRISEBYUSERID,
  DASHBOARD,
  DASHBOARD_COSTANDINCOMES,
  DASHBOARD_WORKSOPENEDANDCLOSED
} from "../constants";
import store from "../store/store";
import ACTION_AUTHENTICATION from "../actions/authenticationAction";
import ACTION_APPLICATION from "../actions/applicationAction";
import { INVOICES } from "./../constants/index";

export const TOKEN_KEY = "jwt";

export const login = (username, password, history) => {
  return new Promise((resolve, reject) => {
  const url = `${config.URL_API}/${AUTHENTICATE}`;
  fetch(url, {
    headers: {
      Accept: "text/plain",
      "Content-Type": "application/json",
    },
    method: "POST",
    body: JSON.stringify({ username, password }),
  })
    .then((data) => data.json())
    .then((result) => {
      if (result.user != null && result.token != null) {
        localStorage.setItem("user", JSON.stringify(result.user));
        localStorage.setItem(TOKEN_KEY, result.token);
        store.dispatch(ACTION_AUTHENTICATION.logIn(result.user, result.token));
        // history.push("/dashboard");
        resolve({ status: "LOGIN_OK", result });
      } else {
        if (result.message) {
          console.log("error ->", result.message);
          localStorage.removeItem("user");
          localStorage.removeItem(TOKEN_KEY);
          store.dispatch(ACTION_AUTHENTICATION.logOut());
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.message,
              responseText: result.message,
              type: "danger",
            })
          );
        }
        resolve({ status: "LOGIN_KO", result });
      }
    })
    .catch((error) => {
      console.log("error ->", error);
      store.dispatch(
        ACTION_APPLICATION.showMessage({
          statusText: error,
          responseText: error,
          type: "danger",
        })        
      );
      reject({ status: "LOGIN_OK" });
    });
  });
};

export const login_old = (username, password, history) => {
  const url = `${config.URL_API}/${AUTHENTICATE}`;
  fetch(url, {
    headers: {
      Accept: "text/plain",
      "Content-Type": "application/json",
    },
    method: "POST",
    body: JSON.stringify({ username, password }),
  })
    .then((data) => data.json())
    .then((result) => {
      if (result.user != null && result.token != null) {
        localStorage.setItem("user", JSON.stringify(result.user));
        localStorage.setItem(TOKEN_KEY, result.token);
        store.dispatch(ACTION_AUTHENTICATION.logIn(result.user, result.token));
        history.push("/dashboard");
      } else {
        if (result.message) {
          console.log("error ->", result.message);
          localStorage.removeItem("user");
          localStorage.removeItem("enterprise");
          localStorage.removeItem(TOKEN_KEY);
          store.dispatch(ACTION_AUTHENTICATION.logOut());
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.message,
              responseText: result.message,
              type: "danger",
            })
          );
        }
      }
    })
    .catch((error) => {
      console.log("error ->", error);
      store.dispatch(
        ACTION_APPLICATION.showMessage({
          statusText: error,
          responseText: error,
          type: "danger",
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

export const getEnterprisesByUserId = (id) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${ENTERPRISEBYUSERID}/${id}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        resolve(result);
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
}

export const getUsers = () => {
  const url = `${config.URL_API}/${USERS}`;
  fetch(url, {
    method: "GET",
  })
    .then((data) => data.json())
    .then((result) => {
      return result;
    })
    .catch((error) => {
      console.log("error ->", error);
    });
};

export const getUser = (id) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${USERS}/${id}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        resolve(result);
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const getEmbargo = (id) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${EMBARGOS}/${id}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        resolve(result);
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
}

export const getDetailsEmbargo = (embargoId) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${DETAILSEMBARGO}?embargoId=${embargoId}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        resolve(result);
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
}

export const getEnterprise = (id) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${ENTERPRISES}/${id}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        resolve(result);
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const getProfessions = () => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${PROFESSIONS}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET"
    })
      .then((data) => data.json())
      .then((result) => {
        resolve(result);
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const addUser = (user) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${USERS}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "POST",
      body: JSON.stringify(user),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success",
            })
          );
          resolve(result);
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const updateUser = (user) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${USERS}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "PUT",
      body: JSON.stringify(user),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success",
            })
          );
          resolve();
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const updatePassword = (user) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${UPDATEPASSWORD}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "PUT",
      body: JSON.stringify(user),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success",
            })
          );
          resolve();
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const updateClient = (client) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${CLIENTS}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "PUT",
      body: JSON.stringify(client),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success",
            })
          );
          resolve();
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const updateWork = (work) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${WORKS}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "PUT",
      body: JSON.stringify(work),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success",
            })
          );
          resolve();
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const updateDatesWork = (work) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${UPDATEDATESWORK}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "PUT",
      body: JSON.stringify(work),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success",
            })
          );
          resolve();
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const updateDocument = (document) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${DOCUMENTS}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "PUT",
      body: JSON.stringify(document),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success",
            })
          );
          resolve();
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const updateCompanyLibrary = (document) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${COMPANY_LIBRARIES}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "PUT",
      body: JSON.stringify(document),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success",
            })
          );
          resolve();
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const updateCompanyDocument = (document) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${COMPANY_DOCUMENTS}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "PUT",
      body: JSON.stringify(document),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success",
            })
          );
          resolve();
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const updateEnterprise = (enterprise) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${ENTERPRISES}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "PUT",
      body: JSON.stringify(enterprise),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success",
            })
          );
          resolve();
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const updateWorkHistory = (document) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${WORKHISTORIES}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "PUT",
      body: JSON.stringify(document),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success",
            })
          );
          resolve();
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const updateDocumentInWorkCost = (document) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${WORKCOSTS}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "PUT",
      body: JSON.stringify(document),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success",
            })
          );
          resolve();
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const updateDocumentInWorkBudget = (document) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${WORKBUDGETS}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "PUT",
      body: JSON.stringify(document),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success",
            })
          );
          resolve();
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const updateWorkersInWork = (workers, workId) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${ASSIGNWORKERS}`;
    let workersId = [];

    workers.forEach((worker) => {
      workersId.push(worker.id);
    });

    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "POST",
      body: JSON.stringify({ listUserId: workersId, workId: workId }),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success",
            })
          );
          resolve();
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const restorePassword = (userId) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${RESTOREPASSWORD}/${userId}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "PUT",
    })
      .then((data) => data.json())
      .then((result) => {
        if (result !== true) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "Ha ocurrido un error al ralizar la operación",
              responseText: "Ha ocurrido un error al ralizar la operación",
              type: "danger",
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "Operación realizada con éxito",
              responseText: "Operación realizada con éxito",
              type: "success",
            })
          );
          resolve();
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const getWorkers = () => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${WORKERSHIRING}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        resolve(result.Items);
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const getWorks = () => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${WORKS}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        resolve(result.Items);
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const getAllWorks = () => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${WORKS}?showCloseWorks=true`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        resolve(result.Items);
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const getWork = (id) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${WORKS}/${id}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        resolve(result);
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const getWorkCost = (id) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${WORKCOSTBYID}/${id}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        resolve(result);
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const getWorkClosePage = (id) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${WORKCLOSEPAGE}/${id}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        resolve(result);
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const getWorksByUserId = (userId) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${USERSHIRING}/?userId=${userId}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        resolve(result.Items);
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const getProfessionsByUserId = (userId) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${PROFESSIONSBYUSER}/?userId=${userId}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        resolve(result.Items);
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const sendMassiveSigning = (data) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${SENDMASSIVESIGNING}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "POST",
      body: JSON.stringify(data),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          if (result === true) {
            store.dispatch(
              ACTION_APPLICATION.showMessage({
                statusText: "Fichajes generados correctamente",
                responseText: "Fichajes generados correctamente",
                type: "success",
              })
            );
            resolve(result);
          } else {
            store.dispatch(
              ACTION_APPLICATION.showMessage({
                statusText: "Algunos de las fichajes no se han podido ejecutar",
                responseText:
                  "Algunos de las fichajes no se han podido ejecutar",
                type: "danger",
              })
            );
            resolve(result);
          }
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const viewMassiveSigning = (data) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${VIEWMASSIVESIGNING}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "POST",
      body: JSON.stringify(data),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          if (result.Item2 === false) {
            resolve(result.Item1);
          } else {
            resolve(result.Item2);
          }
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const removeAllDailySigning = (data) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${REMOVEALLDAILYSIGNING}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "DELETE",
      body: JSON.stringify(data),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          if (result === true) {
            store.dispatch(
              ACTION_APPLICATION.showMessage({
                statusText: "Fichajes borrados correctamente",
                responseText: "Fichajes borrados correctamente",
                type: "success",
              })
            );
            resolve(result);
          } else {
            store.dispatch(
              ACTION_APPLICATION.showMessage({
                statusText: "Ha habido algún error al borrar los fichajes",
                responseText: "Ha habido algún error al borrar los fichajes",
                type: "danger",
              })
            );
            resolve(result);
          }
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const removeAllWorkCosts = (data) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${REMOVEALLWORKCOSTS}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "DELETE",
      body: JSON.stringify(data),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          if (result === true) {
            store.dispatch(
              ACTION_APPLICATION.showMessage({
                statusText: "Gastos borrados correctamente",
                responseText: "Gastos borrados correctamente",
                type: "success",
              })
            );
            resolve(result);
          } else {
            store.dispatch(
              ACTION_APPLICATION.showMessage({
                statusText: "Ha habido algún error al borrar los gastos",
                responseText: "Ha habido algún error al borrar los gastos",
                type: "danger",
              })
            );
            resolve(result);
          }
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const getClients = () => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${CLIENTS}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        resolve(result.Items);
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const getClient = (id) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${CLIENTS}/${id}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        resolve(result);
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const getInvoiceResponse = (invoice) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${INVOICERESPONSES}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "POST",
      body: JSON.stringify(invoice),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.title) {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.title,
              responseText: result.title,
              type: "danger",
            })
          );
          reject();
        } else {
          if (result.Message) {
            store.dispatch(
              ACTION_APPLICATION.showMessage({
                statusText: result.Message,
                responseText: result.Message,
                type: "danger",
              })
            );
            reject();
          } else {
            store.dispatch(
              ACTION_APPLICATION.showMessage({
                statusText: "Facturada generada correctamente",
                responseText: "Facturada generada correctamente",
                type: "success",
              })
            );
            resolve(result.Items);
          }
        }
      })
      .catch((error) => {
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error.message,
            responseText: error.message,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const getSettings = (name) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${SETTINGS}/${name}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          resolve(result);
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const updateSettings = (setting) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${SETTINGS}`;
    const newSetting = JSON.stringify({
      name: COMPANY_DATA,
      data: JSON.stringify(setting),
    });
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "POST",
      body: newSetting,
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success",
            })
          );
          resolve();
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const printInvoice = (invoiceId) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${PRINTINVOICE}/${invoiceId}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          resolve(result.items);
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

// export const billPayment = (invoiceId) => {
//   return new Promise((resolve, reject) => {
//     const url = `${config.URL_API}/${BILLPAYMENT}/${invoiceId}`;
//     fetch(url, {
//       headers: {
//         Accept: "text/plain",
//         "Content-Type": "application/json",
//         Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
//       },
//       method: "GET",
//     })
//       .then((data) => data.json())
//       .then((result) => {
//         if (result.Message) {
//           console.log("error ->", result.Message);
//           store.dispatch(
//             ACTION_APPLICATION.showMessage({
//               statusText: result.Message,
//               responseText: result.Message,
//               type: "danger",
//             })
//           );
//           reject();
//         } else {
//           resolve(result.items);
//         }
//       })
//       .catch((error) => {
//         console.log("error ->", error);
//         store.dispatch(
//           ACTION_APPLICATION.showMessage({
//             statusText: error,
//             responseText: error,
//             type: "danger",
//           })
//         );
//         reject();
//       });
//   });
// };

export const billPaymentWithAmount = (cancelInvoiceWithAmount) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${BILLPAYMENTWITHAMOUNT}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "POST",
      body: JSON.stringify(cancelInvoiceWithAmount),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          resolve(result.items);
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const getInvoice = (invoiceId) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${INVOICES}/${invoiceId}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          resolve(result);
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const getDetailInvoiceByHoursWoker = (invoiceQuery) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${DETAILINVOICEBYHOURSWORKER}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "POST",
      body: JSON.stringify(invoiceQuery),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.title) {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.title,
              responseText: result.title,
              type: "danger",
            })
          );
          reject();
        } else {
          if (result.Message) {
            store.dispatch(
              ACTION_APPLICATION.showMessage({
                statusText: result.Message,
                responseText: result.Message,
                type: "danger",
              })
            );
            reject();
          } else {
            resolve(result.Items);
          }
        }
      })
      .catch((error) => {
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error.message,
            responseText: error.message,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const importPreviousInvoice = (data) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${IMPORTPREVIOUSINVOICE}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "POST",
      body: JSON.stringify(data),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.title) {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.title,
              responseText: result.title,
              type: "danger",
            })
          );
          reject();
        } else {
          if (result.Message) {
            store.dispatch(
              ACTION_APPLICATION.showMessage({
                statusText: result.Message,
                responseText: result.Message,
                type: "danger",
              })
            );
            reject();
          } else {
            resolve(result);
          }
        }
      })
      .catch((error) => {
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error.message,
            responseText: error.message,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const updateUserHiringInWorkByUser = (historyHiring) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${HISTORYHIRINGUPDATEINWORK}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "POST",
      body: JSON.stringify(historyHiring),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: "200",
              responseText: "Operación realizada con éxito",
              type: "success",
            })
          );
          resolve();
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const getProfessionsByUser = (userId) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${PROFESSIONSBYUSER}?userId=${userId}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        resolve(result.Items);
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const addIndirectCosts = (data) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${COMPANY_ADD_INDIRECTCOSTS}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "POST",
      body: JSON.stringify(data),
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          if (result === true) {
            store.dispatch(
              ACTION_APPLICATION.showMessage({
                statusText: "Costos indirectos guardados correctamente",
                responseText: "Costos indirectos guardados correctamente",
                type: "success",
              })
            );
            resolve(result);
          } else {
            store.dispatch(
              ACTION_APPLICATION.showMessage({
                statusText: "No se han podido generar correctamente los costos indirectos",
                responseText:
                  "No se han podido generar correctamente los costos indirectos",
                type: "danger",
              })
            );
            resolve(result);
          }
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        reject();
      });
  });
};

export const getWorkBudgetData = (workBudgetDataId) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${WORKBUDGETDATAS}/${workBudgetDataId}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          resolve(result);
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const getWorkBudget = (workBudgetId) => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${WORKBUDGETS}/${workBudgetId}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          resolve(result);
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const getCurrentStatus = () => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${REPORT_CURRENT_STATUS}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          resolve(result);
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const getDashboard = () => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${DASHBOARD}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          resolve(result);
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const getCostsAndIncomes = () => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${DASHBOARD_COSTANDINCOMES}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          resolve(result);
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const getWorksOpenedAndClosed = () => {
  return new Promise((resolve, reject) => {
    const url = `${config.URL_API}/${DASHBOARD_WORKSOPENEDANDCLOSED}`;
    fetch(url, {
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY)}`,
      },
      method: "GET",
    })
      .then((data) => data.json())
      .then((result) => {
        if (result.Message) {
          console.log("error ->", result.Message);
          store.dispatch(
            ACTION_APPLICATION.showMessage({
              statusText: result.Message,
              responseText: result.Message,
              type: "danger",
            })
          );
          reject();
        } else {
          resolve(result);
        }
      })
      .catch((error) => {
        console.log("error ->", error);
        store.dispatch(
          ACTION_APPLICATION.showMessage({
            statusText: error,
            responseText: error,
            type: "danger",
          })
        );
        reject();
      });
  });
};

export const base64ToArrayBuffer = (base64) => {
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
