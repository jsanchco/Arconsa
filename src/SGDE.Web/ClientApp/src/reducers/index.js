import { combineReducers } from "redux";
import applicationReducer from "./applicationReducer";
import authenticationReducer from "./authenticationReducer";

export const INITIAL_STATE = {
  application: {
    message: null,
    currentPageEmployees: 1,
    currentSearchEmployees: "",
    currentPageWorks: 1,
    currentSearchWorks: "",
    currentPageClients: 1,
    currentSearchClients: "",
    currentPageInvoices: 1,
    currentSearchInvoices: "",  
  },
  authentication: {
    user: null, 
    token: null,
    isAuthenticated: false
  }
};

export default combineReducers({
  applicationReducer,
  authenticationReducer
});
