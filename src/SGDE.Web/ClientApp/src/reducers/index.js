import { combineReducers } from "redux";
import applicationReducer from "./applicationReducer";
import authenticationReducer from "./authenticationReducer";

export const INITIAL_STATE = {
  application: {
    message: null,
    selectedIndexWorkers: []
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
