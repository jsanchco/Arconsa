export default {
  items: [
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
      url: "/settings",
      icon: "icon-settings",
      children: [
        {
          name: "Datos de la Empresa",
          url: "/settings/companydata",
          icon: "icon-note"
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
          name: "Agregar Usuarios",
          url: "/settings/usersnoworker",
          icon: "icon-people"
        }
      ]
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
      name: "Trabajadores",
      url: "/employees/employees",
      icon: "icon-people"
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
        }
      ]
    },
    {
      name: "Facturas",
      url: "/invoices/Invoices",
      icon: "cui-file"
    }
  ]
};
