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

    default:
      return state;
  }
};

export default applicationReducer;
