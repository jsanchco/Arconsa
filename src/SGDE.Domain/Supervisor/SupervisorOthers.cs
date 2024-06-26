﻿namespace SGDE.Domain.Supervisor
{
    #region Using

    using SGDE.Domain.Entities;
    using SGDE.Domain.ViewModels;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public partial class Supervisor
    {
        public Client GetClient(int clientId)
        {
            return _clientRepository.GetById(clientId);
        }

        public Work GetWork(int workId)
        {
            return _workRepository.GetById(workId);
        }

        public User GetWorker(int userId)
        {
            return _userRepository.GetById(userId);
        }

        public void Update()
        {
            //var costWorkers = _costWorkerRepository.GetAll().Data;
            //foreach (var costWorker in costWorkers)
            //{
            //    if (costWorker.ProfessionId != costWorker.User.ProfessionId)
            //    {
            //        if (costWorker.User.ProfessionId.HasValue)
            //        {
            //            costWorker.ProfessionId = costWorker.User.ProfessionId.Value;
            //            _costWorkerRepository.Update(costWorker);
            //        }
            //    }
            //}
            //var dailySignings = _dailySigningRepository.GetAll().Data;
            //var cont = 1;
            //foreach (var dailySigning in dailySignings)
            //{
            //    if (dailySigning.ProfessionId != dailySigning.UserHiring.User.Profession.Id)
            //    {
            //        dailySigning.ProfessionId = dailySigning.UserHiring.User.Profession.Id;
            //        _dailySigningRepository.Update(dailySigning);
            //    }
            //    System.Diagnostics.Debug.WriteLine($"[{cont}]dailySigning [{dailySigning.Id}] de {dailySignings.Count}");
            //    cont++;
            //}

            //var userHirings = _userHiringRepository.GetAll().Data;
            //var data = userHirings.Where(x => x.ProfessionId == null);
            //foreach (var item in data)
            //{
            //    var profession = _userProfessionRepository.GetAll(item.UserId).FirstOrDefault();
            //    item.ProfessionId = profession.ProfessionId;

            //    _userHiringRepository.Update(item);
            //}

            //foreach (var user in _userRepository.GetAll(0, 0, null, null, new List<int> { 3 }).Data)
            //{
            //    var firstDailySigning = _dailySigningRepository.GetAll(userId: user.Id).Data.OrderBy(x => x.StartHour).FirstOrDefault();
            //    if (firstDailySigning == null)
            //        continue;

            //    _sSHiringRepository.Add(new SSHiring
            //    {
            //        AddedDate = System.DateTime.Now,
            //        StartDate = firstDailySigning.StartHour.Value,
            //        UserId = user.Id
            //    });
            //}
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62100001", Description = "ALQUILERES DE MAQUINARIA", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62100003", Description = "ALQUILER IMPRESORA", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62200000", Description = "REPARACIONES Y CONSERVACIÓN", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62300000", Description = "SERVICIOS PROFESIONALES INDEP.", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62300001", Description = "ASESORAMIENTO LABORAL", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62300002", Description = "HONORARIOS DIRECCIÓN DE OBRA", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62300003", Description = "SERVICIOS PRESTADOS ARQUITECTO", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62500000", Description = "PRIMAS DE SEGUROS", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62600000", Description = "SERVICIOS BANCARIOS Y SIMILARE", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62600001", Description = "COMISIONES BANCARIAS CONFIRMIN", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62800001", Description = "GASTOS DE GASOLINA Y GASOIL", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62900000", Description = "OTROS SERVICIOS", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62900001", Description = "PEAJES Y PARKING", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62900002", Description = "COMIDAS Y DIETAS", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62900003", Description = "DOMINIO DE CORREOS E-MAIL", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62900004", Description = "SUSCRIPCIÓN AZURE 1", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62900005", Description = "GASTOS DE REPRESENTACION", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62900006", Description = "GASTOS IMPRESORA", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62900007", Description = "SUPCRIPCION PLATAFORMAS OBRAS", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "62900008", Description = "GASTOS DE LIMPIEZA", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "63100000", Description = "OTROS TRIBUTOS", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "64000000", Description = "SUELDOS Y SALARIOS", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "64200000", Description = "SEGURIDAD SOCIAL A CARGO DE LA", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "64200000", Description = "SEGURIDAD SOCIAL A CARGO DE LA ARQUINSA", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "66500000", Description = "INT. DTO EFE. ENT. CRÉDITO GR.", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "66900000", Description = "OTROS GASTOS FINANCIEROS", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "67800000", Description = "GASTOS EXCEPCIONALES", Amount = 0, Date = new System.DateTime(2021, 1, 1) });
            //_indirectCostRepository.Add(new IndirectCost { AccountNumber = "67800001", Description = "RECARGOS SEGURIDAD SOCIAL", Amount = 0, Date = new System.DateTime(2021, 1, 1) });

            //var works = _workRepository.GetAll().Data;
            //foreach (var work in works)
            //{
            //    AddWorkBudget(new WorkBudgetViewModel
            //    {
            //        date = work.OpenDate,
            //        reference = $"{work.Name.Split(" ")[0]}",
            //        type = "Version X",
            //        workId = work.Id,
            //        totalContract = (double)work.TotalContract
            //    });
            //    AddWorkBudget(new WorkBudgetViewModel
            //    {
            //        date = work.OpenDate,
            //        reference = $"{work.Name.Split(" ")[0]}",
            //        type = "Definitivo",
            //        workId = work.Id,
            //        totalContract = (double)work.TotalContract
            //    });
            //}

            //var invoices = _invoiceRepository.GetAll().Data;
            //foreach (var invoice in invoices)
            //{
            //    var findInvoice = _invoiceRepository.GetById(invoice.Id);
            //    invoice.WorkBudgetId = findInvoice.Work.WorkBudgets
            //        .Where(x => x.Type == "Definitivo")                    
            //        .FirstOrDefault().Id;

            //    _invoiceRepository.Update(invoice);
            //}

            //var detailinvoices = _detailInvoiceRepository.GetAllWithIncludes();
            //foreach (var detailinvoice in detailinvoices)
            //{
            //    detailinvoice.Iva = detailinvoice.Invoice.Work.PercentageIVA;

            //    _detailInvoiceRepository.Update(detailinvoice);
            //}

            //var workBudgets = _workBudgetRepository.GetAll();
            //workBudgets = workBudgets.Where(x => x.Type == "Definitivo" || x.Type == "Complementario X").ToList();
            //workBudgets = workBudgets.Where(x => x.Invoices != null).ToList();
            //foreach (var workBudget in workBudgets)
            //{
            //    var workBudgetData = _workBudgetDataRepository.Add(new WorkBudgetData
            //    {
            //        WorkId = workBudget.WorkId,
            //        Description = workBudget.Name,
            //    });

            //    workBudget.WorkBudgetDataId = workBudgetData.Id;
            //    _workBudgetRepository.Update(workBudget);
            //}

            //var works = _workRepository.GetAll();
            //foreach (var work in works.Data)
            //{
            //    if (work.CloseDate is null)
            //    {
            //        _workStatusHistoryRepository.Add(new WorkStatusHistory
            //        {
            //            WorkId = work.Id,
            //            DateChange = work.OpenDate,
            //            Value = "Abierta",
            //            Observations = "Apertura de Obra"
            //        });
            //    }
            //    else
            //    {
            //        _workStatusHistoryRepository.Add(new WorkStatusHistory
            //        {
            //            WorkId = work.Id,
            //            DateChange = work.OpenDate,
            //            Value = "Abierta",
            //            Observations = "Apertura de Obra"
            //        });

            //        _workStatusHistoryRepository.Add(new WorkStatusHistory
            //        {
            //            WorkId = work.Id,
            //            DateChange = work.CloseDate.Value,
            //            Value = "Cerrada",
            //            Observations = "Cierre de Obra"
            //        });
            //    }
            //}

            //foreach (var invoice in _invoiceRepository.GetAllLite())
            //{
            //    if (invoice.DetailsInvoice.Count > 0)
            //    {
            //        var detailInvoice = invoice.DetailsInvoice.FirstOrDefault();
            //        _detailInvoiceRepository.Update(detailInvoice);
            //    }
            //}

            //foreach (var invoice in _invoiceRepository.GetAll().Data)
            //{
            //    if (invoice.IsPaid)
            //    {
            //        var invoicePaymentHistory = new InvoicePaymentHistory
            //        {
            //            InvoiceId = invoice.Id,
            //            DatePayment = invoice.PayDate.Value,
            //            Amount = invoice.Total
            //        };
            //        _invoicePaymentHistoryRepository.Add(invoicePaymentHistory);
            //    }
            //}

            //var invoices = _invoiceRepository.GetAll().Data.Where(x => x.ClientId == null);
            //foreach (var invoice in invoices) 
            //{
            //    if (invoice.WorkId.HasValue)
            //    {
            //        var clientId = _workRepository.GetById(invoice.WorkId.Value).ClientId;
            //        invoice.ClientId = clientId;
            //        _invoiceRepository.Update(invoice);
            //    }
            //}

            //_enterpriseRepository.Add(new Enterprise
            //{
            //    Name = "Adecua Project New, S.L.",
            //    Alias = "Adecua",
            //    CIF = "B01769546",
            //    Address = "C/ Abeto nº 10 Nave 28500 Arganda del Rey (Madrid)",
            //    PhoneNumber = "917129482"
            //});
            //_enterpriseRepository.Add(new Enterprise
            //{
            //    Name = "EF7 TERRA CONSULTING PROJECT, SL",
            //    Alias = "EF7 TERRA",
            //    CIF = "B44591659",
            //    Address = "c/ Sierra de Cuerda Larga nº 4 Madrid 28038",
            //    PhoneNumber = "665828838"
            //});

            //_userEnterpriseRepository.Add(new UserEnterprise
            //{
            //    UserId = 1,
            //    EnterpriseId = 1
            //});
            //_userEnterpriseRepository.Add(new UserEnterprise
            //{
            //    UserId = 1,
            //    EnterpriseId = 2
            //});

            //_userEnterpriseRepository.Add(new UserEnterprise
            //{
            //    UserId = 2,
            //    EnterpriseId = 1
            //});
            //_userEnterpriseRepository.Add(new UserEnterprise
            //{
            //    UserId = 2,
            //    EnterpriseId = 2
            //});

            //_userEnterpriseRepository.Add(new UserEnterprise
            //{
            //    UserId = 135,
            //    EnterpriseId = 1
            //});
            //_userEnterpriseRepository.Add(new UserEnterprise
            //{
            //    UserId = 135,
            //    EnterpriseId = 2
            //});

            //var indirectCosts = _indirectCostRepository.GetAll().Data.Where(x => x.EnterpriseId == null);
            //foreach (var indirectCost in indirectCosts)
            //{
            //    if (!indirectCost.EnterpriseId.HasValue)
            //    {
            //        var findIndirectCost = _indirectCostRepository.GetById(indirectCost.Id);
            //        findIndirectCost.EnterpriseId = 1;
            //        _indirectCostRepository.Update(findIndirectCost);
            //    }
            //}

            //var clients = _clientRepository.GetAll().Data.Where(x => x.EnterpriseId == null);
            //foreach (var client in clients) 
            //{ 
            //    if (!client.EnterpriseId.HasValue)
            //    {
            //        var findClient = _clientRepository.GetById(client.Id);
            //        findClient.EnterpriseId = 1;
            //        _clientRepository.Update(findClient);
            //    }
            //}

            var users = _userRepository.GetAll().Data.Where(x => x.EnterpriseId == null && x.RoleId == 3);
            foreach (var user in users)
            {
                if (!user.EnterpriseId.HasValue)
                {
                    var findUser = _userRepository.GetById(user.Id);
                    findUser.EnterpriseId = 1;
                    _userRepository.Update(findUser);
                }
            }

            //var libraries = _libraryRepository.GetAll().Data.Where(x => x.EnterpriseId == null);
            //foreach (var library in libraries)
            //{
            //    if (!library.EnterpriseId.HasValue)
            //    {
            //        var findLibrary = _libraryRepository.GetById(library.Id);
            //        findLibrary.EnterpriseId = 1;
            //        _libraryRepository.Update(findLibrary);
            //    }
            //}
        }
    }
}
