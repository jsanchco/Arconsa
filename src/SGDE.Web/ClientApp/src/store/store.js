import rootReducer from "../reducers";
import { createStore, applyMiddleware } from "redux";
import logger from "redux-logger";

const store = createStore(rootReducer, applyMiddleware(logger), window.REDUX_INITIAL_DATA);

export default store;