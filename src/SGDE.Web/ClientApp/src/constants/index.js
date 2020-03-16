export const PROFESSIONS = "api/Professions";
export const ROLES = "api/Roles";
export const USERS = "api/Users";
export const CLIENTS = "api/Clients";
export const WORKS = "api/Works";
export const TYPES_DOCUMENT = "api/TypesDocument";
export const TRAININGS = "api/Trainings";
export const DOCUMENTS = "api/UserDocuments";
export const UPLOADBOX_SAVE = "api/UploadBox";
export const WORKERSHIRING = "api/WorkersHiring";
export const USERSHIRING = "api/UsersHiring";
export const ASSIGNWORKERS = "api/UsersHiring/assignworkers";
export const DAILYSIGNINGS = "api/dailysignings";
export const REPORTS = "api/Reports";
export const AUTHENTICATE = "api/Users/authenticate";

const dev = {
  URL_API: "http://localhost:51667"
  //URL_API: "https://arconsa-api.azurewebsites.net"
};

const prod = {
  URL_API: "https://arconsa-api.azurewebsites.net"
};

export const config = process.env.NODE_ENV === "development" ? dev : prod;
