export default {
  items: [
    {
      name: "Inicio",
      url: "/dashboard",
      icon: "icon-speedometer",
      badge: {
        variant: "info",
      },
    },
    {
      name: "Datos de Empresa",
      url: "/Enterprise/DetailCompany",
    },
    {
      name: "Configuración",
      icon: "icon-settings",
      children: [
        {
          name: "Roles",
          url: "/settings/roles",
          icon: "icon-people",
        },
        {
          name: "Puestos de Trabajo",
          url: "/settings/professions",
          icon: "icon-people",
        },
        {
          name: "Tipos de Documento",
          url: "/settings/typesdocument",
          icon: "icon-layers",
        },
        {
          name: "Gestión de Usuarios",
          url: "/settings/usersnoworker",
          icon: "icon-people",
        },
        {
          name: "Biblioteca",
          url: "/settings/libraries",
          icon: "icon-layers",
        },
      ],
    },
    {
      name: "Trabajadores",
      url: "/employees/employees",
      icon: "icon-people",
    },
    {
      name: "Clientes",
      url: "/clients/clients",
      icon: "icon-chart",
    },
    {
      name: "Obras",
      url: "/works/works",
      icon: "icon-globe",
    },
    {
      name: "Fichajes",
      url: "/signings/signings",
      icon: "icon-book-open",
    },
    {
      name: "Facturas",
      url: "/invoices/Invoices",
      icon: "cui-file",
    },
    {
      name: "Informes",
      icon: "cui-map",
      children: [
        {
          name: "Resultados",
          url: "/reports/ReportCurrentStatus",
          icon: "cui-file",
        },
        {
          name: "Obras",
          children: [
            {
              name: "Partidas",
              url: "/invoices/Tracing",
              icon: "cui-file",
            },
            {
              name: "Abiertas",
              url: "/reports/ReportWorks",
              icon: "cui-file",
            },
          ]
        },
        {
          name: "Por Horas",
          children: [
            {
              name: "Trabajador",
              url: "/reports/ReportHoursUser",
              icon: "cui-file",
            },
            {
              name: "Obra",
              url: "/reports/ReportHoursWork",
              icon: "cui-file",
            },
            {
              name: "Cliente",
              url: "/reports/ReportHoursClient",
              icon: "cui-file",
            },
            {
              name: "Facturas",
              url: "/reports/ReportInvoices",
              icon: "cui-file",
            },
            {
              name: "Varios",
              url: "/reports/ReportsVarious",
              icon: "cui-file",
            },
          ],
        },
      ],
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
  ],
};
