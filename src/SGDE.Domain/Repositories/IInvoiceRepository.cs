﻿namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using SGDE.Domain.Helpers;
    using System;
    using System.Collections.Generic;

    #endregion

    public interface IInvoiceRepository
    {
        QueryResult<Invoice> GetAll(int skip = 0, int take = 0, int enterpriseId = 0, string filter = null, int workId = 0, int clientId = 0);
        Invoice GetById(int id);
        Invoice Add(Invoice newInvoice);
        Invoice AddInvoiceFromQuery(Invoice invoice);
        bool Update(Invoice invoice);
        bool Delete(int id);
        int CountInvoices();
        int CountInvoicesInYear(int? enterpriseId, int year);
        int? CheckInvoice(Invoice newInvoice);
        List<Invoice> GetAllBetweenDates(int enterpriseId, DateTime startDate, DateTime endDate, int workId = 0, int clientId = 0);
        List<Invoice> GetAllLite();
    }
}
