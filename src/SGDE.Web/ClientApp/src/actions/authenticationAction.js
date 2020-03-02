// types of action
const Types = {
  LOGIN: "LOGIN",
  LOGOUT: "LOGOUT"
};

// actions
const logIn = (user, token) => ({
  type: Types.LOGIN,
  payload: { user, token }
});

const logOut = () => ({
  type: Types.LOGOUT
});

export default {
  logIn,
  logOut,
  Types
};
