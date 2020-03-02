// types of action
const Types = {
  SHOW_MESSAGE: "SHOW_MESSAGE"
};

// actions
const showMessage = message => ({
  type: Types.SHOW_MESSAGE,
  payload: message
});

export default {
  showMessage,
  Types
};
