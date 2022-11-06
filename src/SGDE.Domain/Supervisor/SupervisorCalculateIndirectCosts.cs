using SGDE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.Domain.Supervisor
{
    public partial class Supervisor
    {
        private static readonly string[] MONTHS = { "ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE" };

        public double CalculateIndirectCostsByWork(int workId)
        {
            var invoices = _invoiceRepository.GetAllLite();
            var dates = GetDates(invoices);
            if (!dates.HasValue)
                return 0;

            var columnsDates = GetColumns(dates.Value.minDate, dates.Value.maxDate);
            var indirectCosts = GetAllIndirectCost();

            var matrixResult = new List<ColumnMonthYearWork>();

            foreach (var column in columnsDates)
            {                
                var totalInvoices = invoices
                    .Where(x => x.IssueDate >= column.start && x.IssueDate <= column.end)
                    .Sum(x => x.Total);

                var totalInvoicesByWorkId = invoices
                    .Where(x => x.IssueDate >= column.start && x.IssueDate <= column.end && x.WorkId == workId)
                    .Sum(x => x.Total);

                var percentageInvoices = totalInvoices == 0 ? 0 : Math.Round(totalInvoicesByWorkId / totalInvoices, 2);

                var totalIndirectCosts = _indirectCostRepository.GetAllBetweenDates(column.start, column.end).Sum(x => x.Amount);

                var total = totalIndirectCosts * percentageInvoices;

                matrixResult.Add(
                    new ColumnMonthYearWork 
                    {                       
                        Name = column.name,
                        Start = column.start,
                        End = column.end,
                        WorkId = workId,                         
                        TotalInvoices = totalInvoices,
                        TotalInvoicesByWorkId = totalInvoicesByWorkId,
                        PercentajeInvoices = percentageInvoices,
                        Total = total
                    });
            }

            double result = matrixResult.Sum(x => x.Total);
            return result;
        }

        private (DateTime minDate, DateTime maxDate)? GetDates(List<Invoice> invoices)
        {
            if (invoices?.Count == 0)
                return null;

            var firstInvoice = invoices.First();
            var minDate = new DateTime(firstInvoice.IssueDate.Year, firstInvoice.IssueDate.Month, 1);
            var lastInvoice = invoices.Last();
            var maxDate = new DateTime(lastInvoice.IssueDate.Year, lastInvoice.IssueDate.Month, DateTime.DaysInMonth(lastInvoice.IssueDate.Year, lastInvoice.IssueDate.Month));

            return (minDate, maxDate);
        }

        private List<(string name, DateTime start, DateTime end)> GetColumns(DateTime start, DateTime end)
        {
            var months = ((end.Year - start.Year) * 12) + end.Month - start.Month;
            var result = new List<(string name, DateTime start, DateTime end)>();
            var dateCalculated = start;
            for (var index = 0; index < months; index++)
            {                
                result.Add(($"{MONTHS[dateCalculated.Month - 1]} {dateCalculated.Year}", dateCalculated, dateCalculated.AddMonths(1).AddSeconds(-1)));
                dateCalculated = dateCalculated.AddMonths(1);
            }

            return result;
        }
    }

    public class ColumnMonthYearWork
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; } 
        public int WorkId { get; set; } 
        public double TotalInvoices { get; set; }
        public double TotalInvoicesByWorkId { get; set; }
        public double PercentajeInvoices { get; set; }
        public double Total { get; set; }
    }
}
