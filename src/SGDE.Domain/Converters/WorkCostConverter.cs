﻿using SGDE.Domain.Entities;
using SGDE.Domain.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SGDE.Domain.Converters
{
    public class WorkCostConverter
    {
        public static WorkCostViewModel Convert(WorkCost workCost)
        {
            if (workCost == null)
                return null;

            var workCostViewModel = new WorkCostViewModel
            {
                id = workCost.Id,
                addedDate = workCost.AddedDate,
                modifiedDate = workCost.ModifiedDate,
                iPAddress = workCost.IPAddress,

                date = workCost.Date,
                numberInvoice = workCost.NumberInvoice,
                provider = workCost.Provider,
                taxBase = workCost.TaxBase,
                iva = workCost.Iva,
                typeWorkCost = workCost.TypeWorkCost,
                fileName = workCost.FileName,
                description = workCost.Description,
                file = workCost.File,
                typeFile = workCost.TypeFile,
                
                workId = workCost.WorkId,
                workName = workCost.Work.Name
            };

            if (workCost.File != null && workCost.File.Length > 0)
                workCostViewModel.hasFile = true;

            return workCostViewModel;
        }

        public static List<WorkCostViewModel> ConvertList(IEnumerable<WorkCost> workCosts)
        {
            return workCosts?.Select(workCost =>
            {
                var model = new WorkCostViewModel
                {
                    id = workCost.Id,
                    addedDate = workCost.AddedDate,
                    modifiedDate = workCost.ModifiedDate,
                    iPAddress = workCost.IPAddress,

                    date = workCost.Date,
                    numberInvoice = workCost.NumberInvoice,
                    provider = workCost.Provider,
                    taxBase = workCost.TaxBase,
                    iva = workCost.Iva,
                    typeWorkCost = workCost.TypeWorkCost,
                    fileName = workCost.FileName,
                    description = workCost.Description,
                    typeFile = workCost.TypeFile,

                    workId = workCost.WorkId,
                    workName = workCost.Work.Name
                };
                if (workCost.File != null && workCost.File.Length > 0)
                    model.hasFile = true;

                return model;
            })
                .ToList();
        }
    }
}
