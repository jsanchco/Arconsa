export const PROFESSIONS = "api/Professions";
export const ROLES = "api/Roles";
export const USERS = "api/Users";
export const CLIENTS = "api/Clients";
export const CLIENTSWITHOUTFILTER = "api/Clients/getclientswithoutilter";
export const WORKS = "api/Works";
export const UPDATEDATESWORK = "api/Works/updatedateswork";
export const TYPES_DOCUMENT = "api/TypesDocument";
export const TRAININGS = "api/Trainings";
export const COSTWORKERS = "api/CostWorkers";
export const DOCUMENTS = "api/UserDocuments";
export const UPLOADBOX_SAVE = "api/UploadBox";
export const WORKERSHIRING = "api/WorkersHiring";
export const USERSHIRING = "api/UsersHiring";
export const ASSIGNWORKERS = "api/UsersHiring/assignworkers";
export const DAILYSIGNINGS = "api/dailysignings";
export const REPORTS = "api/Reports";
export const REPORTS_ALL= "api/Reports/GetReportAll";
export const AUTHENTICATE = "api/Users/authenticate";
export const UPDATEPASSWORD = "api/Users/updatepassword";
export const INVOICERESPONSES = "api/InvoiceResponses";
export const DETAILINVOICEBYHOURSWORKER = "api/InvoiceResponses/getdetailinvoicebyhoursworker";
export const SETTINGS = "api/Settings";
export const HOURTYPES = "api/HourTypes";
export const PROFESSIONINCLIENTS = "api/ProfessionInClients";
export const RESTOREPASSWORD = "api/Users/restorepassword";
export const MASSIVESIGNING = "api/dailysignings/massivesigning";
export const REMOVEALLDAILYSIGNING = "api/dailysignings/removeall";
export const INVOICES = "api/Invoices";
export const PRINTINVOICE = "api/Invoices/print";
export const BILLPAYMENT = "api/Invoices/billpayment";
export const IMPORTPREVIOUSINVOICE = "api/Invoices/importpreviousinvoice";
export const COMPANY_DATA = "COMPANY_DATA";


const dev = {
  URL_API: "http://localhost:51667"
  //URL_API: "https://arconsa-api.azurewebsites.net"
};

const prod = {
  URL_API: "https://arconsa1-api.azurewebsites.net/"
};

export const config = process.env.NODE_ENV === "development" ? dev : prod;
