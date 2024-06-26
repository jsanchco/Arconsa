import React from 'react';

const Breadcrumbs = React.lazy(() => import('./views/Base/Breadcrumbs'));
const Cards = React.lazy(() => import('./views/Base/Cards'));
const Carousels = React.lazy(() => import('./views/Base/Carousels'));
const Collapses = React.lazy(() => import('./views/Base/Collapses'));
const Dropdowns = React.lazy(() => import('./views/Base/Dropdowns'));
const Forms = React.lazy(() => import('./views/Base/Forms'));
const Jumbotrons = React.lazy(() => import('./views/Base/Jumbotrons'));
const ListGroups = React.lazy(() => import('./views/Base/ListGroups'));
const Navbars = React.lazy(() => import('./views/Base/Navbars'));
const Navs = React.lazy(() => import('./views/Base/Navs'));
const Paginations = React.lazy(() => import('./views/Base/Paginations'));
const Popovers = React.lazy(() => import('./views/Base/Popovers'));
const ProgressBar = React.lazy(() => import('./views/Base/ProgressBar'));
const Switches = React.lazy(() => import('./views/Base/Switches'));
const Tables = React.lazy(() => import('./views/Base/Tables'));
const Tabs = React.lazy(() => import('./views/Base/Tabs'));
const Tooltips = React.lazy(() => import('./views/Base/Tooltips'));
const BrandButtons = React.lazy(() => import('./views/Buttons/BrandButtons'));
const ButtonDropdowns = React.lazy(() => import('./views/Buttons/ButtonDropdowns'));
const ButtonGroups = React.lazy(() => import('./views/Buttons/ButtonGroups'));
const Buttons = React.lazy(() => import('./views/Buttons/Buttons'));
const Charts = React.lazy(() => import('./views/Charts'));
const Dashboard = React.lazy(() => import('./views/Dashboard'));
const CoreUIIcons = React.lazy(() => import('./views/Icons/CoreUIIcons'));
const Flags = React.lazy(() => import('./views/Icons/Flags'));
const FontAwesome = React.lazy(() => import('./views/Icons/FontAwesome'));
const SimpleLineIcons = React.lazy(() => import('./views/Icons/SimpleLineIcons'));
const Alerts = React.lazy(() => import('./views/Notifications/Alerts'));
const Badges = React.lazy(() => import('./views/Notifications/Badges'));
const Modals = React.lazy(() => import('./views/Notifications/Modals'));
const Colors = React.lazy(() => import('./views/Theme/Colors'));
const Typography = React.lazy(() => import('./views/Theme/Typography'));
const Widgets = React.lazy(() => import('./views/Widgets/Widgets'));
const Users = React.lazy(() => import('./views/Users/Users'));
const User = React.lazy(() => import('./views/Users/User'));

const WorkOrders = React.lazy(() => import('./views/Employees/Employee/WorkOrders'));
const SharedScheduler = React.lazy(() => import('./views/Employees/Employee/SharedScheduler'));
const Employees = React.lazy(() => import('./views/Employees/employees'));
const DetailEmployee = React.lazy(() => import('./views/Employees/detail-employee'));
const BasicData = React.lazy(() => import('./views/Employees/basic-data'));
const AddNewEmployee = React.lazy(() => import('./views/Employees/add-new'));

const DetailCompany = React.lazy(() => import('./views/Enterprise/detail-company'));
const BasicDataCompany = React.lazy(() => import('./views/Enterprise/basic-data-company'));
const Roles = React.lazy(() => import('./views/Settings/roles'));
const Professions = React.lazy(() => import('./views/Settings/professions'));
const TypesDocument = React.lazy(() => import('./views/Settings/types-document'));
const UsersNoWorker = React.lazy(() => import('./views/Settings/users-no-worker'));
const Libraries = React.lazy(() => import('./views/Settings/libraries'));

const Clients = React.lazy(() => import('./views/Clients/clients'));
const DetailClient = React.lazy(() => import('./views/Clients/detail-client'));

const Works = React.lazy(() => import('./views/Works/works'));
const DetailWork = React.lazy(() => import('./views/Works/detail-work'));
const BasicDataWork = React.lazy(() => import('./views/Works/basic-data-work'));

const Signings = React.lazy(() => import('./views/Signings/signings'));

const ReportHoursUser = React.lazy(() => import('./views/Reports/report-hours-user'));
const ReportHoursWork = React.lazy(() => import('./views/Reports/report-hours-work'));
const ReportHoursClient = React.lazy(() => import('./views/Reports/report-hours-client'));
const ReportInvoices = React.lazy(() => import('./views/Reports/report-invoices'));
const ReportResults = React.lazy(() => import('./views/Reports/report-results'));
const ReportCurrentStatus = React.lazy(() => import('./views/Reports/report-current-status'));
const ReportsVarious = React.lazy(() => import('./views/Reports/reports-various'));
const Tracing = React.lazy(() => import('./views/Reports/report-tracing'));
const ReportWorks = React.lazy(() => import('./views/Reports/report-works'));

const Invoices = React.lazy(() => import('./views/Invoices/invoices'));
const InvoicesCustom = React.lazy(() => import('./views/Invoices/invoices-custom'));

const Blank = React.lazy(() => import('./views/Pages/Blank/blank'));

const TestBlank = React.lazy(() => import('./views/Pages/Tests/TestBlank'));
const TestHierachical = React.lazy(() => import('./views/Pages/Tests/TestHierachical'));

// https://github.com/ReactTraining/react-router/tree/master/packages/react-router-config
const routes = [
  { path: '/', exact: true, name: 'Inicio' },
  { path: '/pages/blank/blank', name: 'Blank Page', component: Blank },
  { path: '/dashboard', name: 'Inicio', component: Dashboard },
  { path: '/theme', exact: true, name: 'Theme', component: Colors },
  { path: '/theme/colors', name: 'Colors', component: Colors },
  { path: '/theme/typography', name: 'Typography', component: Typography },
  { path: '/base', exact: true, name: 'Base', component: Cards },
  { path: '/base/cards', name: 'Cards', component: Cards },
  { path: '/base/forms', name: 'Forms', component: Forms },
  { path: '/base/switches', name: 'Switches', component: Switches },
  { path: '/base/tables', name: 'Tables', component: Tables },
  { path: '/base/tabs', name: 'Tabs', component: Tabs },
  { path: '/base/breadcrumbs', name: 'Breadcrumbs', component: Breadcrumbs },
  { path: '/base/carousels', name: 'Carousel', component: Carousels },
  { path: '/base/collapses', name: 'Collapse', component: Collapses },
  { path: '/base/dropdowns', name: 'Dropdowns', component: Dropdowns },
  { path: '/base/jumbotrons', name: 'Jumbotrons', component: Jumbotrons },
  { path: '/base/list-groups', name: 'List Groups', component: ListGroups },
  { path: '/base/navbars', name: 'Navbars', component: Navbars },
  { path: '/base/navs', name: 'Navs', component: Navs },
  { path: '/base/paginations', name: 'Paginations', component: Paginations },
  { path: '/base/popovers', name: 'Popovers', component: Popovers },
  { path: '/base/progress-bar', name: 'Progress Bar', component: ProgressBar },
  { path: '/base/tooltips', name: 'Tooltips', component: Tooltips },
  { path: '/buttons', exact: true, name: 'Buttons', component: Buttons },
  { path: '/buttons/buttons', name: 'Buttons', component: Buttons },
  { path: '/buttons/button-dropdowns', name: 'Button Dropdowns', component: ButtonDropdowns },
  { path: '/buttons/button-groups', name: 'Button Groups', component: ButtonGroups },
  { path: '/buttons/brand-buttons', name: 'Brand Buttons', component: BrandButtons },
  { path: '/icons', exact: true, name: 'Icons', component: CoreUIIcons },
  { path: '/icons/coreui-icons', name: 'CoreUI Icons', component: CoreUIIcons },
  { path: '/icons/flags', name: 'Flags', component: Flags },
  { path: '/icons/font-awesome', name: 'Font Awesome', component: FontAwesome },
  { path: '/icons/simple-line-icons', name: 'Simple Line Icons', component: SimpleLineIcons },
  { path: '/notifications', exact: true, name: 'Notifications', component: Alerts },
  { path: '/notifications/alerts', name: 'Alerts', component: Alerts },
  { path: '/notifications/badges', name: 'Badges', component: Badges },
  { path: '/notifications/modals', name: 'Modals', component: Modals },
  { path: '/widgets', name: 'Widgets', component: Widgets },
  { path: '/charts', name: 'Charts', component: Charts },
  { path: '/users', exact: true,  name: 'Users', component: Users },
  { path: '/users/:id', exact: true, name: 'User Details', component: User },
  { path: '/employees/employee/workorders', exact: true, name: 'Partes de Trabajo', component: WorkOrders },
  { path: '/employees/employee/sharedscheduler', exact: true, name: 'Compartir Agenda', component: SharedScheduler },
  { path: '/employees/employees', exact: true, name: 'Trabajadores', component: Employees },
  { path: '/employees/detailemployee/:id', exact: true, name: 'Detalle Trabajador', component: DetailEmployee },
  { path: '/employees/basicdata', exact: true, name: 'Datos Básicos', component: BasicData },
  { path: '/employees/add-new', exact: true, name: 'Datos Básicos', component: AddNewEmployee },
  { path: '/Enterprise/detailcompany', exact: true, name: 'ADECUA', component: DetailCompany},
  { path: '/Enterprise/basicdatacompany', exact: true, name: 'ADECUA', component: BasicDataCompany},
  // { path: '/settings/company/indirect-costs', exact: true, name: 'Gastos Indirectos', component: IndirectCosts },
  { path: '/settings/roles', exact: true, name: 'Roles', component: Roles },
  { path: '/settings/professions', exact: true, name: 'Puestos de Trabajo', component: Professions },
  { path: '/settings/typesdocument', exact: true, name: 'Tipos de Documento', component: TypesDocument },
  { path: '/settings/usersnoworker', exact: true, name: 'Usuarios', component: UsersNoWorker },
  { path: '/settings/libraries', exact: true, name: 'Biblioteca', component: Libraries },
  { path: '/clients/clients', exact: true, name: 'Clientes', component: Clients },
  { path: '/clients/detailclient/:id', exact: true, name: 'Detalle Cliente', component: DetailClient },
  { path: '/works/works', exact: true, name: 'Obras', component: Works },
  { path: '/works/detailwork/:id/:selectedTab?', exact: true, name: 'Detalle Obra', component: DetailWork },
  { path: '/works/basicdatawork', exact: true, name: 'Datos Básicos', component: BasicDataWork },
  { path: '/signings/signings', exact: true, name: 'Fichajes', component: Signings },
  { path: '/reports/reporthoursuser', exact: true, name: 'Informe Horas Trabajador', component: ReportHoursUser },
  { path: '/reports/reporthourswork', exact: true, name: 'Informe Horas Obra', component: ReportHoursWork },
  { path: '/reports/reporthoursclient', exact: true, name: 'Informe Horas Cliente', component: ReportHoursClient },
  { path: '/reports/reportinvoices', exact: true, name: 'Informe Facturas', component: ReportInvoices },
  { path: '/reports/reportcurrentstatus', exact: true, name: 'Resultados Actuales', component: ReportCurrentStatus },
  { path: '/reports/reportresults', exact: true, name: 'Informe Resultados', component: ReportResults },
  { path: '/reports/reportworks', exact: true, name: 'Informe Resultados', component: ReportWorks },
  { path: '/reports/reportsvarious', exact: true, name: 'Informes Varios', component: ReportsVarious },
  { path: '/invoices/invoices', exact: true, name: 'Facturas', component: Invoices },
  { path: '/invoices/tracing', exact: true, name: 'Seguimiento', component: Tracing },  
  { path: '/invoices/invoicescustom', exact: true, name: 'Facturas Personalizadas', component: InvoicesCustom },
  { path: '/pages/Tests/TestBlank', name: 'Test Blank', component: TestBlank },
  { path: '/pages/Tests/TestHierachical', name: 'Test Hierachical', component: TestHierachical }
];

export default routes;
