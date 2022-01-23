// types of action
const Types = {
  SHOW_MESSAGE: "SHOW_MESSAGE",
  SET_CURRENTPAGE_EMPLOYEES: "SET_CURRENTPAGE_EMPLOYEES",
  SET_CURRENTSEARCH_EMPLOYEES: "SET_CURRENTSEARCH_EMPLOYEES",
  SET_CURRENTPAGE_WORKS: "SET_CURRENTPAGE_WORKS",
  SET_CURRENTSEARCH_WORKS: "SET_CURRENTSEARCH_WORKS",
  SET_CURRENTPAGE_CLIENTS: "SET_CURRENTPAGE_CLIENTS",
  SET_CURRENTSEARCH_CLIENTS: "SET_CURRENTSEARCH_CLIENTS",
  SET_CURRENTPAGE_INVOICES: "SET_CURRENTPAGE_INVOICES",
  SET_CURRENTSEARCH_INVOICES: "SET_CURRENTSEARCH_INVOICES"
};

// actions
const showMessage = message => ({
  type: Types.SHOW_MESSAGE,
  payload: message
});

const setCurrentPageEmployees = currentPageEmployees => ({
  type: Types.SET_CURRENTPAGE_EMPLOYEES,
  payload: currentPageEmployees
});

const setCurrentSearchEmployees = currentSearchEmployees => ({
  type: Types.SET_CURRENTSEARCH_EMPLOYEES,
  payload: currentSearchEmployees
});

const setCurrentPageWorks = currentPageWorks => ({
  type: Types.SET_CURRENTPAGE_WORKS,
  payload: currentPageWorks
});

const setCurrentSearchWorks = currentSearchWorks => ({
  type: Types.SET_CURRENTSEARCH_WORKS,
  payload: currentSearchWorks
});

const setCurrentPageClients = currentPageClients => ({
  type: Types.SET_CURRENTPAGE_CLIENTS,
  payload: currentPageClients
});

const setCurrentSearchClients = currentSearchClients => ({
  type: Types.SET_CURRENTSEARCH_CLIENTS,
  payload: currentSearchClients
});

const setCurrentPageInvoices = currentPageInvoices => ({
  type: Types.SET_CURRENTPAGE_INVOICES,
  payload: currentPageInvoices
});

const setCurrentSearchInvoices = currentSearchInvoices => ({
  type: Types.SET_CURRENTSEARCH_INVOICES,
  payload: currentSearchInvoices
});

export default {
  showMessage,
  setCurrentPageEmployees,
  setCurrentSearchEmployees,
  setCurrentPageWorks,
  setCurrentSearchWorks,
  setCurrentPageClients,
  setCurrentSearchClients,
  setCurrentPageInvoices,
  setCurrentSearchInvoices,
  Types
};
