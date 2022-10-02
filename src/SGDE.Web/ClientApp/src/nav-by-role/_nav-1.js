export default {
  items: [
    // {
    //   name: "Blank Page",
    //   url: "/pages/blank/blank",
    //   icon: "icon-speedometer",
    //   badge: {
    //     variant: "info"
    //   }
    // },
    {
      name: "Inicio",
      url: "/dashboard",
      icon: "icon-speedometer",
      badge: {
        variant: "info"
      }
    },
    {
      name: "Configuración",
      icon: "icon-settings",
      children: [
        {
          name: "Adecua",
          children: [
            {
              name: "Datos de la Empresa",
              url: "/settings/company/companydata",
              icon: "icon-note"
            },
            {
              name: "Gastos Indirectos",
              url: "/settings/company/indirect-costs",
              icon: "icon-note"
            }         
          ]
        },
        {
          name: "Roles",
          url: "/settings/roles",
          icon: "icon-people"
        },
        {
          name: "Puestos de Trabajo",
          url: "/settings/professions",
          icon: "icon-people"
        },
        {
          name: "Tipos de Documento",
          url: "/settings/typesdocument",
          icon: "icon-layers"
        },
        {
          name: "Gestión de Usuarios",
          url: "/settings/usersnoworker",
          icon: "icon-people"
        },
        {
          name: "Biblioteca",
          url: "/settings/libraries",
          icon: "icon-layers"
        }        
      ]
    },
    {
      name: "Trabajadores",
      url: "/employees/employees",
      icon: "icon-people"
    },
    {
      name: "Clientes",
      url: "/clients/clients",
      icon: "icon-chart"
    },
    {
      name: "Obras",
      url: "/works/works",
      icon: "icon-globe"
    },
    {
      name: "Fichajes",
      url: "/signings/signings",
      icon: "icon-book-open"
    },
    {
      name: "Facturas",
      url: "/invoices/InvoicesCustom",
      icon: "cui-file"
    },
    {
      name: "Facturas New",
      url: "/invoices/Invoices",
      icon: "cui-file"
    },
    {
      name: "Informes",
      url: "/reports",
      icon: "icon-info",
      children: [
        {
          name: "Trabajador",
          url: "/reports/ReportHoursUser",
          icon: "icon-list"
        },
        {
          name: "Obra",
          url: "/reports/ReportHoursWork",
          icon: "icon-list"
        },
        {
          name: "Cliente",
          url: "/reports/ReportHoursClient",
          icon: "icon-list"
        },
        {
          name: "Facturas",
          url: "/reports/ReportInvoices",
          icon: "icon-list"
        },
        // {
        //   name: "Resultados Obras",
        //   url: "/reports/ReportResults",
        //   icon: "icon-list"
        // },
        {
          name: "Varios",
          url: "/reports/ReportsVarious",
          icon: "icon-list"
        }
      ]
    },
    // {
    //   name: "Test",
    //   url: "/Pages/Tests/TestBlank",
    //   icon: "cui-file"
    // },
    // {
    //   name: "Test Hierachical",
    //   url: "/Pages/Tests/TestHierachical",
    //   icon: "cui-file"
    // }
  ]
};
