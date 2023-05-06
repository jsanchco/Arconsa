namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Converters;
    using Entities;
    using SGDE.Domain.Helpers;
    using ViewModels;

    #endregion

    public partial class Supervisor
    {
        public (BarItemViewModel costsAndIncomes, BarItemViewModel worksOpenedAndClosed) GetDashboard(int enterpriseId)
        {
            return (costsAndIncomes: GetCostsAndIncomes(), worksOpenedAndClosed: GetWorksOpenedAndClosed(enterpriseId));
        }

        public BarItemViewModel GetCostsAndIncomes_old()
        {
            DateTime startDate;
            DateTime endDate;
            var now = DateTime.Now;
            var actualYear = now.Year;
            var previousYear = actualYear - 1;

            var result = new BarItemViewModel
            {
                name = "Gastos/Ingresos por Trimestre",
                labels = new string[] {
                    $"1º Trimestre",
                    $"2º Trimestre",
                    $"3º Trimestre",
                    $"4º Trimestre"
                },
                datasets = new List<Dataset>()
            };

            var costsWorkers = GetHistoryBetweenDates(new DateTime(actualYear - 1, 1, 1), new DateTime(actualYear, 12, 31));

            // año Pasado
            var dataCostsPrevious = new List<double>();
            // 1º Trimestre
            startDate = new DateTime(actualYear - 1, 1, 1);
            endDate = new DateTime(actualYear - 1, 3, 31);
            //var sumCostsWorkers = 0.0;
            var sumCostsWorkers = costsWorkers.Where(x => x.dtStartDate >= startDate && x.dtStartDate <= endDate).Sum(x => x.priceTotal);
            var sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            var sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            var sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsPrevious.Add(Math.Round(sumCosts, 2));

            var dataInvoicesPrevious = new List<double>();
            var invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            var sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesPrevious.Add(Math.Round(sumInvoices, 2));


            // 2º Trimestre
            startDate = new DateTime(actualYear - 1, 4, 1);
            endDate = new DateTime(actualYear - 1, 6, 30);
            sumCostsWorkers = costsWorkers.Where(x => x.dtStartDate >= startDate && x.dtStartDate <= endDate).Sum(x => x.priceTotal);
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsPrevious.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesPrevious.Add(Math.Round(sumInvoices, 2));


            // 3º Trimestre
            startDate = new DateTime(actualYear - 1, 7, 1);
            endDate = new DateTime(actualYear - 1, 9, 30);
            sumCostsWorkers = costsWorkers.Where(x => x.dtStartDate >= startDate && x.dtStartDate <= endDate).Sum(x => x.priceTotal);
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsPrevious.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesPrevious.Add(Math.Round(sumInvoices, 2));

            // 4º Trimestre
            startDate = new DateTime(actualYear - 1, 10, 1);
            endDate = new DateTime(actualYear - 1, 12, 31);
            sumCostsWorkers = costsWorkers.Where(x => x.dtStartDate >= startDate && x.dtStartDate <= endDate).Sum(x => x.priceTotal);
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsPrevious.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesPrevious.Add(Math.Round(sumInvoices, 2));


            var dataSetCostsPrevious = new Dataset
            {
                label = $"Gastos {previousYear}",
                backgroundColor = "rgba(255,99,132,0.2)",
                borderColor = "rgba(255,99,132,1)",
                data = dataCostsPrevious
            };
            result.datasets.Add(dataSetCostsPrevious);

            var dataSetInvoicesPrevious = new Dataset
            {
                label = $"Ingresos {previousYear}",
                backgroundColor = "rgba(255, 206, 86,0.2)",
                borderColor = "rgba(255, 206, 86,1)",
                data = dataInvoicesPrevious
            };
            result.datasets.Add(dataSetInvoicesPrevious);


            // año actual
            var dataCostsActual = new List<double>();
            // 1º Trimestre
            startDate = new DateTime(actualYear, 1, 1);
            endDate = new DateTime(actualYear, 3, 31);
            sumCostsWorkers = costsWorkers.Where(x => x.dtStartDate >= startDate && x.dtStartDate <= endDate).Sum(x => x.priceTotal);
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsActual.Add(Math.Round(sumCosts, 2));

            var dataInvoicesActual = new List<double>();
            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesActual.Add(Math.Round(sumInvoices, 2));

            // 2º Trimestre
            startDate = new DateTime(actualYear, 4, 1);
            endDate = new DateTime(actualYear, 6, 30);
            sumCostsWorkers = costsWorkers.Where(x => x.dtStartDate >= startDate && x.dtStartDate <= endDate).Sum(x => x.priceTotal);
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsActual.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesActual.Add(Math.Round(sumInvoices, 2));

            // 3º Trimestre
            startDate = new DateTime(actualYear, 7, 1);
            endDate = new DateTime(actualYear, 9, 30);
            sumCostsWorkers = costsWorkers.Where(x => x.dtStartDate >= startDate && x.dtStartDate <= endDate).Sum(x => x.priceTotal);
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsActual.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesActual.Add(Math.Round(sumInvoices, 2));

            // 4º Trimestre
            startDate = new DateTime(actualYear, 10, 1);
            endDate = new DateTime(actualYear, 12, 31);
            sumCostsWorkers = costsWorkers.Where(x => x.dtStartDate >= startDate && x.dtStartDate <= endDate).Sum(x => x.priceTotal);
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsActual.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesActual.Add(Math.Round(sumInvoices, 2));

            var dataSetCostsActual = new Dataset
            {
                label = $"Gastos {actualYear}",
                backgroundColor = "rgba(179,181,198,0.2)",
                borderColor = "rgba(179,181,198,1)",
                data = dataCostsActual
            };
            result.datasets.Add(dataSetCostsActual);

            var dataSetInvoicesActual = new Dataset
            {
                label = $"Ingresos {actualYear}",
                backgroundColor = "rgb(54, 162, 235, 0.2)",
                borderColor = "rgba(54, 162, 235,1)",
                data = dataInvoicesActual
            };
            result.datasets.Add(dataSetInvoicesActual);

            return result;
        }

        public BarItemViewModel GetCostsAndIncomes()
        {
            DateTime startDate;
            DateTime endDate;
            var now = DateTime.Now;
            var actualYear = now.Year;
            var previousYear = actualYear - 1;

            var result = new BarItemViewModel
            {
                name = "Gastos/Ingresos por Trimestre",
                labels = new string[] {
                    $"1º Trimestre",
                    $"2º Trimestre",
                    $"3º Trimestre",
                    $"4º Trimestre"
                },
                datasets = new List<Dataset>()
            };

            var costsWorkers = GetHistoryBetweenDates(new DateTime(actualYear - 1, 1, 1), new DateTime(actualYear, 12, 31));

            // año Pasado
            var dataCostsPrevious = new List<double>();
            // 1º Trimestre
            startDate = new DateTime(actualYear - 1, 1, 1);
            endDate = new DateTime(actualYear - 1, 3, 31, 23, 59, 59);
            var sumCostsWorkers = 0.0;
            //var sumCostsWorkers = costsWorkers.Where(x => x.dtStartDate >= startDate && x.dtStartDate <= endDate).Sum(x => x.priceTotal);
            var sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            var sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            var sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsPrevious.Add(Math.Round(sumCosts, 2));

            var dataInvoicesPrevious = new List<double>();
            var invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            var sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesPrevious.Add(Math.Round(sumInvoices, 2));


            // 2º Trimestre
            startDate = new DateTime(actualYear - 1, 4, 1);
            endDate = new DateTime(actualYear - 1, 6, 30, 23, 59, 59);
            sumCostsWorkers = 0;
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsPrevious.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesPrevious.Add(Math.Round(sumInvoices, 2));


            // 3º Trimestre
            startDate = new DateTime(actualYear - 1, 7, 1);
            endDate = new DateTime(actualYear - 1, 9, 30, 23, 59, 59);
            sumCostsWorkers = 0;
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsPrevious.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesPrevious.Add(Math.Round(sumInvoices, 2));

            // 4º Trimestre
            startDate = new DateTime(actualYear - 1, 10, 1);
            endDate = new DateTime(actualYear - 1, 12, 31, 23, 59, 59);
            sumCostsWorkers = 0;
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsPrevious.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesPrevious.Add(Math.Round(sumInvoices, 2));


            var dataSetCostsPrevious = new Dataset
            {
                label = $"Gastos {previousYear}",
                backgroundColor = "rgba(255,99,132,0.2)",
                borderColor = "rgba(255,99,132,1)",
                data = dataCostsPrevious
            };
            result.datasets.Add(dataSetCostsPrevious);

            var dataSetInvoicesPrevious = new Dataset
            {
                label = $"Ingresos {previousYear}",
                backgroundColor = "rgba(255, 206, 86,0.2)",
                borderColor = "rgba(255, 206, 86,1)",
                data = dataInvoicesPrevious
            };
            result.datasets.Add(dataSetInvoicesPrevious);


            // año actual
            var dataCostsActual = new List<double>();
            // 1º Trimestre
            startDate = new DateTime(actualYear, 1, 1);
            endDate = new DateTime(actualYear, 3, 31, 23, 59, 59);
            sumCostsWorkers = 0;
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsActual.Add(Math.Round(sumCosts, 2));

            var dataInvoicesActual = new List<double>();
            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesActual.Add(Math.Round(sumInvoices, 2));

            // 2º Trimestre
            startDate = new DateTime(actualYear, 4, 1);
            endDate = new DateTime(actualYear, 6, 30, 23, 59, 59);
            sumCostsWorkers = 0;
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsActual.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesActual.Add(Math.Round(sumInvoices, 2));

            // 3º Trimestre
            startDate = new DateTime(actualYear, 7, 1);
            endDate = new DateTime(actualYear, 9, 30, 23, 59, 59);
            sumCostsWorkers = 0;
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsActual.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesActual.Add(Math.Round(sumInvoices, 2));

            // 4º Trimestre
            startDate = new DateTime(actualYear, 10, 1);
            endDate = new DateTime(actualYear, 12, 31, 23, 59, 59);
            sumCostsWorkers = 0;
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsActual.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesActual.Add(Math.Round(sumInvoices, 2));

            var dataSetCostsActual = new Dataset
            {
                label = $"Gastos {actualYear}",
                backgroundColor = "rgba(255,99,132,0.2)",
                borderColor = "rgba(255,99,132,1)",
                data = dataCostsActual
            };
            result.datasets.Add(dataSetCostsActual);

            var dataSetInvoicesActual = new Dataset
            {
                label = $"Ingresos {actualYear}",
                backgroundColor = "rgba(255, 206, 86,0.2)",
                borderColor = "rgba(255, 206, 86,1)",
                data = dataInvoicesActual
            };
            result.datasets.Add(dataSetInvoicesActual);

            return result;
        }

        public BarItemViewModel GetCostsAndIncomes_good()
        {
            DateTime startDate;
            DateTime endDate;
            var now = DateTime.Now;
            var actualYear = now.Year;
            var previousYear = actualYear - 1;

            var result = new BarItemViewModel
            {
                name = "Gastos/Ingresos por Trimestre",
                labels = new string[] {
                    $"1º Trimestre",
                    $"2º Trimestre",
                    $"3º Trimestre",
                    $"4º Trimestre"
                },
                datasets = new List<Dataset>()
            };

            // año Pasado
            var dataCostsPrevious = new List<double>();
            // 1º Trimestre
            startDate = new DateTime(actualYear - 1, 1, 1);
            endDate = new DateTime(actualYear - 1, 3, 31);
            var sumCostsWorkers = GetHistoryBetweenDates(startDate, endDate).Sum(x => x.priceTotal);
            var sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            var sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            var sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsPrevious.Add(Math.Round(sumCosts, 2));

            var dataInvoicesPrevious = new List<double>();
            var invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            var sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesPrevious.Add(Math.Round(sumInvoices, 2));


            // 2º Trimestre
            startDate = new DateTime(actualYear - 1, 4, 1);
            endDate = new DateTime(actualYear - 1, 6, 30);
            sumCostsWorkers = GetHistoryBetweenDates(startDate, endDate).Sum(x => x.priceTotal);
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsPrevious.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesPrevious.Add(Math.Round(sumInvoices, 2));


            // 3º Trimestre
            startDate = new DateTime(actualYear - 1, 7, 1);
            endDate = new DateTime(actualYear - 1, 9, 30);
            sumCostsWorkers = GetHistoryBetweenDates(startDate, endDate).Sum(x => x.priceTotal);
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsPrevious.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesPrevious.Add(Math.Round(sumInvoices, 2));

            // 4º Trimestre
            startDate = new DateTime(actualYear - 1, 10, 1);
            endDate = new DateTime(actualYear - 1, 12, 31);
            sumCostsWorkers = GetHistoryBetweenDates(startDate, endDate).Sum(x => x.priceTotal);
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsPrevious.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesPrevious.Add(Math.Round(sumInvoices, 2));


            var dataSetCostsPrevious = new Dataset
            {
                label = $"Gastos {previousYear}",
                backgroundColor = "rgba(255,99,132,0.2)",
                borderColor = "rgba(255,99,132,1)",
                data = dataCostsPrevious
            };
            result.datasets.Add(dataSetCostsPrevious);

            var dataSetInvoicesPrevious = new Dataset
            {
                label = $"Ingresos {previousYear}",
                backgroundColor = "rgba(255, 206, 86,0.2)",
                borderColor = "rgba(255, 206, 86,1)",
                data = dataInvoicesPrevious
            };
            result.datasets.Add(dataSetInvoicesPrevious);


            // año actual
            var dataCostsActual = new List<double>();
            // 1º Trimestre
            startDate = new DateTime(actualYear, 1, 1);
            endDate = new DateTime(actualYear, 3, 31);
            sumCostsWorkers = GetHistoryBetweenDates(startDate, endDate).Sum(x => x.priceTotal);
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsActual.Add(Math.Round(sumCosts, 2));

            var dataInvoicesActual = new List<double>();
            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesActual.Add(Math.Round(sumInvoices, 2));

            // 2º Trimestre
            startDate = new DateTime(actualYear, 4, 1);
            endDate = new DateTime(actualYear, 6, 30);
            sumCostsWorkers = GetHistoryBetweenDates(startDate, endDate).Sum(x => x.priceTotal);
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsActual.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesActual.Add(Math.Round(sumInvoices, 2));

            // 3º Trimestre
            startDate = new DateTime(actualYear, 7, 1);
            endDate = new DateTime(actualYear, 9, 30);
            sumCostsWorkers = GetHistoryBetweenDates(startDate, endDate).Sum(x => x.priceTotal);
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsActual.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesActual.Add(Math.Round(sumInvoices, 2));

            // 4º Trimestre
            startDate = new DateTime(actualYear, 10, 1);
            endDate = new DateTime(actualYear, 12, 31);
            sumCostsWorkers = GetHistoryBetweenDates(startDate, endDate).Sum(x => x.priceTotal);
            sumCostsProviders = _workCostRepository.GetBetweenDates(startDate, endDate).Sum(x => x.TaxBase);
            sumCostsIndirects = _indirectCostRepository.GetBetweeenDates(startDate, endDate).Sum(x => x.Amount);
            sumCosts = sumCostsWorkers + sumCostsProviders + sumCostsIndirects;
            dataCostsActual.Add(Math.Round(sumCosts, 2));

            invoices = _invoiceRepository.GetAllBetweenDates(startDate, endDate);
            sumInvoices = invoices.Sum(x => x.TaxBase);
            dataInvoicesActual.Add(Math.Round(sumInvoices, 2));

            var dataSetCostsActual = new Dataset
            {
                label = $"Gastos {actualYear}",
                backgroundColor = "rgba(179,181,198,0.2)",
                borderColor = "rgba(179,181,198,1)",
                data = dataCostsActual
            };
            result.datasets.Add(dataSetCostsActual);

            var dataSetInvoicesActual = new Dataset
            {
                label = $"Ingresos {actualYear}",
                backgroundColor = "rgb(54, 162, 235, 0.2)",
                borderColor = "rgba(54, 162, 235,1)",
                data = dataInvoicesActual
            };
            result.datasets.Add(dataSetInvoicesActual);

            return result;
        }

        public BarItemViewModel GetWorksOpenedAndClosed(int enterpriseId)
        {
            DateTime startDate;
            DateTime endDate;
            var now = DateTime.Now;
            var actualYear = now.Year;
            var previousYear = actualYear - 1;

            var result = new BarItemViewModel
            {
                name = "Obras Abiertas/Cerradas por Trimestre",
                labels = new string[] {
                    $"1º Trimestre",
                    $"2º Trimestre",
                    $"3º Trimestre",
                    $"4º Trimestre"
                },
                datasets = new List<Dataset>()
            };

            var worksStatusHistory = _workStatusHistoryRepository.GetAll();

            var dataOpenPrevious = new List<double>();
            var dataJuridicPrevious = new List<double>();
            var dataClosePrevious = new List<double>();

            var dataOpenActual = new List<double>();
            var dataJuridicActual = new List<double>();
            var dataCloseActual = new List<double>();

            // 1º Trimestre año anterior
            startDate = new DateTime(actualYear - 1, 1, 1);
            endDate = new DateTime(actualYear - 1, 3, 31, 23, 59, 59);
            var sumOpen = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Abierta").Count();
            dataOpenPrevious.Add(sumOpen);
            var sumJuridic = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Juridico").Count();
            dataJuridicPrevious.Add(sumJuridic);
            var sumClose = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Cerrada").Count();
            dataClosePrevious.Add(sumClose);

            // 2º Trimestre año anterior
            startDate = new DateTime(actualYear - 1, 4, 1);
            endDate = new DateTime(actualYear - 1, 6, 30, 23, 59, 59);
            sumOpen = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Abierta").Count();
            dataOpenPrevious.Add(sumOpen);
            sumJuridic = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Juridico").Count();
            dataJuridicPrevious.Add(sumJuridic);
            sumClose = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Cerrada").Count();
            dataClosePrevious.Add(sumClose);

            // 3º Trimestre año anterior
            startDate = new DateTime(actualYear - 1, 7, 1);
            endDate = new DateTime(actualYear - 1, 9, 30, 23, 59, 59);
            sumOpen = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Abierta").Count();
            dataOpenPrevious.Add(sumOpen);
            sumJuridic = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Juridico").Count();
            dataJuridicPrevious.Add(sumJuridic);
            sumClose = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Cerrada").Count();
            dataClosePrevious.Add(sumClose);

            // 4º Trimestre año anterior
            startDate = new DateTime(actualYear - 1, 10, 1);
            endDate = new DateTime(actualYear - 1, 12, 31, 23, 59, 59);
            sumOpen = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Abierta").Count();
            dataOpenPrevious.Add(sumOpen);
            sumJuridic = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Juridico").Count();
            dataJuridicPrevious.Add(sumJuridic);
            sumClose = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Cerrada").Count();
            dataClosePrevious.Add(sumClose);

            // 1º Trimestre año actual
            startDate = new DateTime(actualYear, 1, 1);
            endDate = new DateTime(actualYear, 3, 31, 23, 59, 59);
            sumOpen = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Abierta").Count();
            dataOpenActual.Add(sumOpen);
            sumJuridic = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Juridico").Count();
            dataJuridicActual.Add(sumJuridic);
            sumClose = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Cerrada").Count();
            dataCloseActual.Add(sumClose);

            // 2º Trimestre año actual
            startDate = new DateTime(actualYear, 4, 1);
            endDate = new DateTime(actualYear, 6, 30, 23, 59, 59);
            sumOpen = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Abierta").Count();
            dataOpenActual.Add(sumOpen);
            sumJuridic = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Juridico").Count();
            dataJuridicActual.Add(sumJuridic);
            sumClose = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Cerrada").Count();
            dataCloseActual.Add(sumClose);

            // 3º Trimestre año actual
            startDate = new DateTime(actualYear, 7, 1);
            endDate = new DateTime(actualYear, 9, 30, 23, 59, 59);
            sumOpen = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Abierta").Count();
            dataOpenActual.Add(sumOpen);
            sumJuridic = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Juridico").Count();
            dataJuridicActual.Add(sumJuridic);
            sumClose = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Cerrada").Count();
            dataCloseActual.Add(sumClose);

            // 4º Trimestre año actual
            startDate = new DateTime(actualYear, 10, 1);
            endDate = new DateTime(actualYear, 12, 31, 23, 59, 59);
            sumOpen = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Abierta").Count();
            dataOpenActual.Add(sumOpen);
            sumJuridic = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Juridico").Count();
            dataJuridicActual.Add(sumJuridic);
            sumClose = worksStatusHistory.Where(x => x.DateChange >= startDate && x.DateChange <= endDate && x.Value == "Cerrada").Count();
            dataCloseActual.Add(sumClose);

            var dataSetOpenPrevious = new Dataset
            {
                label = $"Obras Abiertas {previousYear}",
                backgroundColor = "rgba(22, 160, 133, 0.2)",
                borderColor = "rgba(22, 160, 133, 1)",
                data = dataOpenPrevious
            };
            result.datasets.Add(dataSetOpenPrevious);

            var dataSetOpenActual = new Dataset
            {
                label = $"Obras Abiertas {actualYear}",
                backgroundColor = "rgba(22, 160, 133, 0.2)",
                borderColor = "rgba(22, 160, 133, 1)",
                data = dataOpenActual
            };
            result.datasets.Add(dataSetOpenActual);

            var dataSetJuridicPrevious = new Dataset
            {
                label = $"Obas Juridico {previousYear}",
                backgroundColor = "rgba(255, 128, 0, 0.2)",
                borderColor = "rgba(255, 128, 0, 1)",
                data = dataJuridicPrevious
            };
            result.datasets.Add(dataSetJuridicPrevious);

            var dataSetJuridicActual = new Dataset
            {
                label = $"Obas Juridico {actualYear}",
                backgroundColor = "rgba(255, 128, 0, 0.2)",
                borderColor = "rgba(255, 128, 0, 1)",
                data = dataJuridicActual
            };
            result.datasets.Add(dataSetJuridicActual);

            var dataSetClosePrevious = new Dataset
            {
                label = $"Obas Cerradas {previousYear}",
                backgroundColor = "rgba(231, 76, 60, 0.2)",
                borderColor = "rgba(231, 76, 60, 1)",
                data = dataClosePrevious
            };
            result.datasets.Add(dataSetClosePrevious);

            var dataSetCloseActual = new Dataset
            {
                label = $"Obas Cerradas {actualYear}",
                backgroundColor = "rgba(231, 76, 60, 0.2)",
                borderColor = "rgba(231, 76, 60, 1)",
                data = dataCloseActual
            };
            result.datasets.Add(dataSetCloseActual);

            return result;
        }

        public BarItemViewModel GetWorksOpenedAndClosed_old()
        {
            DateTime startDate;
            DateTime endDate;
            var now = DateTime.Now;
            var actualYear = now.Year;
            var previousYear = actualYear - 1;

            var result = new BarItemViewModel
            {
                name = "Obras Abiertas/Cerradas por Trimestre",
                labels = new string[] {
                    $"1º Trimestre",
                    $"2º Trimestre",
                    $"3º Trimestre",
                    $"4º Trimestre"
                },
                datasets = new List<Dataset>()
            };

            var works = _workRepository.GetAll().Data;

            // año Pasado
            var dataOpenPrevious = new List<double>();
            var dataClosePrevious = new List<double>();
            // 1º Trimestre
            startDate = new DateTime(actualYear - 1, 1, 1);
            endDate = new DateTime(actualYear - 1, 3, 31);
            var sumOpen = works.Where(x => x.OpenDate >= startDate && x.OpenDate <= endDate).Count();
            dataOpenPrevious.Add(sumOpen);
            var sumClose = works.Where(x => x.CloseDate >= startDate && x.CloseDate <= endDate).Count();
            dataClosePrevious.Add(sumClose);

            // 2º Trimestre
            startDate = new DateTime(actualYear - 1, 4, 1);
            endDate = new DateTime(actualYear - 1, 6, 30);
            sumOpen = works.Where(x => x.OpenDate >= startDate && x.OpenDate <= endDate).Count();
            dataOpenPrevious.Add(sumOpen);
            sumClose = works.Where(x => x.CloseDate >= startDate && x.CloseDate <= endDate).Count();
            dataClosePrevious.Add(sumClose);

            // 3º Trimestre
            startDate = new DateTime(actualYear - 1, 7, 1);
            endDate = new DateTime(actualYear - 1, 9, 30);
            sumOpen = works.Where(x => x.OpenDate >= startDate && x.OpenDate <= endDate).Count();
            dataOpenPrevious.Add(sumOpen);
            sumClose = works.Where(x => x.CloseDate >= startDate && x.CloseDate <= endDate).Count();
            dataClosePrevious.Add(sumClose);

            // 4º Trimestre
            startDate = new DateTime(actualYear - 1, 10, 1);
            endDate = new DateTime(actualYear - 1, 12, 31);
            sumOpen = works.Where(x => x.OpenDate >= startDate && x.OpenDate <= endDate).Count();
            dataOpenPrevious.Add(sumOpen);
            sumClose = works.Where(x => x.CloseDate >= startDate && x.CloseDate <= endDate).Count();
            dataClosePrevious.Add(sumClose);

            var dataSetOpenPrevious = new Dataset
            {
                label = $"Obras Abiertas {previousYear}",
                backgroundColor = "rgba(255,99,132,0.2)",
                borderColor = "rgba(255,99,132,1)",
                data = dataOpenPrevious
            };
            result.datasets.Add(dataSetOpenPrevious);

            var dataSetClosePrevious = new Dataset
            {
                label = $"Obas Cerradas {previousYear}",
                backgroundColor = "rgba(255, 206, 86,0.2)",
                borderColor = "rgba(255, 206, 86,1)",
                data = dataClosePrevious
            };
            result.datasets.Add(dataSetClosePrevious);


            // año actual
            var dataOpenActual = new List<double>();
            var dataCloseActual = new List<double>();

            // 1º Trimestre
            startDate = new DateTime(actualYear, 1, 1);
            endDate = new DateTime(actualYear, 3, 31);
            sumOpen = works.Where(x => x.OpenDate >= startDate && x.OpenDate <= endDate).Count();
            dataOpenActual.Add(sumOpen);
            sumClose = works.Where(x => x.CloseDate >= startDate && x.CloseDate <= endDate).Count();
            dataCloseActual.Add(sumClose);

            // 2º Trimestre
            startDate = new DateTime(actualYear, 4, 1);
            endDate = new DateTime(actualYear, 6, 30);
            sumOpen = works.Where(x => x.OpenDate >= startDate && x.OpenDate <= endDate).Count();
            dataOpenActual.Add(sumOpen);
            sumClose = works.Where(x => x.CloseDate >= startDate && x.CloseDate <= endDate).Count();
            dataCloseActual.Add(sumClose);

            // 3º Trimestre
            startDate = new DateTime(actualYear, 7, 1);
            endDate = new DateTime(actualYear, 9, 30);
            sumOpen = works.Where(x => x.OpenDate >= startDate && x.OpenDate <= endDate).Count();
            dataOpenActual.Add(sumOpen);
            sumClose = works.Where(x => x.CloseDate >= startDate && x.CloseDate <= endDate).Count();
            dataCloseActual.Add(sumClose);

            // 4º Trimestre
            startDate = new DateTime(actualYear, 10, 1);
            endDate = new DateTime(actualYear, 12, 31);
            sumOpen = works.Where(x => x.OpenDate >= startDate && x.OpenDate <= endDate).Count();
            dataOpenActual.Add(sumOpen);
            sumClose = works.Where(x => x.CloseDate >= startDate && x.CloseDate <= endDate).Count();
            dataCloseActual.Add(sumClose);

            var dataSetOpenActual = new Dataset
            {
                label = $"Obras Abiertas {actualYear}",
                backgroundColor = "rgba(179,181,198,0.2)",
                borderColor = "rgba(179,181,198,1)",
                data = dataOpenActual
            };
            result.datasets.Add(dataSetOpenActual);

            var dataSetCloseActual = new Dataset
            {
                label = $"Obas Cerradas {actualYear}",
                backgroundColor = "rgb(54, 162, 235, 0.2)",
                borderColor = "rgba(54, 162, 235,1)",
                data = dataCloseActual
            };
            result.datasets.Add(dataSetCloseActual);

            return result;
        }
    }
}
