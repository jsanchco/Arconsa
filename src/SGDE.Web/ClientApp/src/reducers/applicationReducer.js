import { INITIAL_STATE } from "./index";
import ACTIONS from "../actions/applicationAction";

const applicationReducer = (state = INITIAL_STATE.application, action) => {
  switch (action.type) {
    case ACTIONS.Types.SHOW_MESSAGE: {
      return {
        ...state,
        message: action.payload
      };
    }

    case ACTIONS.Types.SET_CURRENTPAGE_EMPLOYEES: {
      return {
        ...state,
        currentPageEmployees: action.payload
      };
    }

    case ACTIONS.Types.SET_CURRENTSEARCH_EMPLOYEES: {
      return {
        ...state,
        currentSearchEmployees: action.payload
      };
    }

    case ACTIONS.Types.SET_CURRENTPAGE_WORKS: {
      return {
        ...state,
        currentPageWorks: action.payload
      };
    }

    case ACTIONS.Types.SET_CURRENTSEARCH_WORKS: {
      return {
        ...state,
        currentSearchWorks: action.payload
      };
    }

    case ACTIONS.Types.SET_CURRENTPAGE_CLIENTS: {
      return {
        ...state,
        currentPageClients: action.payload
      };
    }

    case ACTIONS.Types.SET_CURRENTSEARCH_CLIENTS: {
      return {
        ...state,
        currentSearchClients: action.payload
      };
    }

    default:
      return state;
  }
};

export default applicationReducer;
