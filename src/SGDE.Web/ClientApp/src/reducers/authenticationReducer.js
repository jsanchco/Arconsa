import { INITIAL_STATE } from "./index";
import ACTIONS from "../actions/authenticationAction";

const aauthenticationReducer = (
  state = INITIAL_STATE.authentication,
  action
) => {
  switch (action.type) {
    case ACTIONS.Types.LOGIN: {
      return {
        ...state,
        user: action.payload.user,
        token: action.payload.token,
        isAuthenticated: true
      };
    }
    case ACTIONS.Types.LOGOUT: {
      return {
        ...state,
        user: null,
        token: null,
        isAuthenticated: false
      };
    }

    default:
      return state;
  }
};

export default aauthenticationReducer;
