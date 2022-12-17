using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;
using System;
using System.Collections.Generic;

namespace SGDE.Domain.Repositories
{
    public interface IIndirectCostRepository
    {
        QueryResult<IndirectCost> GetAll(int skip = 0, int take = 0, string filter = null);
        List<IndirectCost> GetBetweeenDates(DateTime startDate, DateTime endDate);
        IndirectCost GetById(int id);
        IndirectCost Add(IndirectCost newIndirectCost);
        bool Update(IndirectCost indirectCost);
        bool Delete(int id);
        List<IndirectCost> GetAllInDate(DateTime date);
        List<IndirectCost> GetAllBetweenDates(DateTime start, DateTime end);
    }
}
