export const PROFESSIONS = "api/Professions";
export const ROLES = "api/Roles";
export const USERS = "api/Users";
export const CLIENTS = "api/Clients";
export const CLIENTSWITHOUTFILTER = "api/Clients/getclientswithoutfilter";
export const CLIENTSLITE = "api/Clients/getallclientslite";
export const WORKS = "api/Works";
export const WORKSLITE = "api/Works/getallworkslite";
export const UPDATEDATESWORK = "api/Works/updatedateswork";
export const WORKCLOSEPAGE = "api/Works/getclosepage";
export const TYPES_DOCUMENT = "api/TypesDocument";
export const TRAININGS = "api/Trainings";
export const COSTWORKERS = "api/CostWorkers";
export const DOCUMENTS = "api/UserDocuments";
export const UPLOADBOX_SAVE = "api/UploadBox";
export const WORKERSHIRING = "api/WorkersHiring";
export const USERSHIRING = "api/UsersHiring";
export const ASSIGNWORKERS = "api/UsersHiring/assignworkers";
export const DAILYSIGNINGS = "api/dailysignings";
export const ADVANCES = "api/Advances";
export const REPORTS = "api/Reports";
export const REPORTS_ALL= "api/Reports/GetReportAll";
export const REPORT_INVOICES = "api/Reports/GetReportInvoices";
export const REPORT_TRACING = "api/Reports/GetReportTracing";
export const REPORT_RESULTS = "api/Reports/GetReportResults";
export const REPORT_WORKS = "api/Reports/GetWorkReportBetweenDates";
export const REPORT_CURRENT_STATUS = "api/Reports/GetReportCurrentStatus";
export const AUTHENTICATE = "api/Users/authenticate";
export const UPDATEPASSWORD = "api/Users/updatepassword";
export const INVOICERESPONSES = "api/InvoiceResponses";
export const DETAILINVOICEBYHOURSWORKER = "api/InvoiceResponses/getdetailinvoicebyhoursworker";
export const SETTINGS = "api/Settings";
export const HOURTYPES = "api/HourTypes";
export const PROFESSIONINCLIENTS = "api/ProfessionInClients";
export const RESTOREPASSWORD = "api/Users/restorepassword";
export const VIEWMASSIVESIGNING = "api/dailysignings/viewmassivesigning";
export const SENDMASSIVESIGNING = "api/dailysignings/sendmassivesigning";
export const REMOVEALLDAILYSIGNING = "api/dailysignings/removeall";
export const INVOICES = "api/Invoices";
export const PRINTINVOICE = "api/Invoices/print";
export const BILLPAYMENTWITHAMOUNT = "api/Invoices/billpaymentwithamount";
export const IMPORTPREVIOUSINVOICE = "api/Invoices/importpreviousinvoice";
export const DETAILSINVOICE = "api/DetailsInvoice";
export const HISTORYHIRINGBYUSERID = "api/HistoryHiring/getbyuserid";
export const HISTORYHIRINGBYWORKID = "api/HistoryHiring";
export const HISTORYHIRINGUPDATEINWORK = "api/HistoryHiring/updateinwork";
export const PROFESSIONSBYUSER = "api/CostWorkers/getprofessionsbyuser";
export const PROFESSIONSBYUSERID = "api/CostWorkers/getprofessionsbyuserid";
export const EMBARGOS = "api/Embargos";
export const DETAILSEMBARGO = "api/DetailsEmbargo";
export const SSHIRINGS = "api/sshirings";
export const WORKCOSTS = "api/workcosts";
export const WORKCOSTBYID = "api/workcosts";
export const WORKHISTORIES = "api/workhistories";
export const WORKSTATUSHISTORIES = "api/workstatushistories";
export const REMOVEALLWORKCOSTS = "api/workcosts/removeall";
export const WORKBUDGETDATAS = "api/workbudgetdatas";
export const WORKBUDGETS = "api/workbudgets";
export const WORKBUDGETSLITE = "api/workbudgets/getallbudgetslite";
export const COMPANY_INDIRECTCOSTS = "api/indirectCosts";
export const COMPANY_ADD_INDIRECTCOSTS = "api/indirectCosts/addindirectcosts";
export const COMPANY_LIBRARIES = "api/libraries";
export const COMPANY_DOCUMENTS = "api/companydatas";
export const DASHBOARD = "api/dashboard";
export const DASHBOARD_COSTANDINCOMES = "api/dashboard/costsandincomes";
export const DASHBOARD_WORKSOPENEDANDCLOSED = "api/dashboard/worksopenedandclosed";
export const INVOICEPAYMENTSHISTORY = "api/InvoicePaymentsHistory";
export const COMPANY_DATA = "COMPANY_DATA";

const dev = {
  URL_API: "http://localhost:44566"
  //URL_API: "https://arconsa1-api.azurewebsites.net"
};

const prod = {
  URL_API: "https://arconsa1-api.azurewebsites.net/"
};

export const config = process.env.NODE_ENV === "development" ? dev : prod;
