// ReSharper disable InconsistentNaming
namespace SGDE.SeedData
{
    #region Using

    using System;
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Diagnostics;
    using System.Linq;
    using DataEFCoreSQL;
    using DataEFCoreMySQL;

    #endregion

    internal static class Program
    {
        static void Main()
        {
            Console.Clear();
            Console.WriteLine("*****************************");
            Console.WriteLine("*         Seed Data         *");
            Console.WriteLine("*****************************");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Choose Provider ...");
            Console.WriteLine("");
            Console.WriteLine("1 - SQL");
            Console.WriteLine("2 - MySQL");
            Console.WriteLine("");

            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                var configuration = builder.Build();

                var typeSeed = Console.ReadKey();
                Console.WriteLine("");
                Console.WriteLine("wait ...");
                switch (typeSeed.KeyChar)
                {
                    case '1':
                        SeedFromSQL(configuration.GetSection("ConnectionStrings")["SQL"]);
                        break;

                    case '2':
                        SeedFromMySQL(configuration.GetSection("ConnectionStrings")["MySQL"]);
                        break;

                    default:
                        Console.WriteLine("Error: Seed no contemplated!!!");
                        break;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("Press any key to exit ...");
                Console.ReadKey();
            }
        }

        private static void SeedFromSQL(string options)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();           

            var optionsBuilder = new DbContextOptionsBuilder<EFContextSQL>();
            optionsBuilder.UseSqlServer(options);

            using (var context = new EFContextSQL(optionsBuilder.Options))
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                if (!context.Role.Any())
                {
                    context.Role.Add(new Role 
                    {
                        AddedDate = DateTime.Now,
                        Name = "Super"
                    });
                    context.Role.Add(new Role
                    {
                        AddedDate = DateTime.Now,
                        Name = "Administrador"
                    });
                    context.Role.Add(new Role
                    {
                        AddedDate = DateTime.Now,
                        Name = "Trabajador"
                    });

                    context.SaveChanges();
                }

                if (!context.Profession.Any())
                {
                    context.Profession.Add(new Profession 
                    {
                        AddedDate = DateTime.Now,
                        Name = "Peon 1ª",
                        Description = "Peon 1ª"
                    });

                    context.Profession.Add(new Profession
                    {
                        AddedDate = DateTime.Now,
                        Name = "Peon aprendiz",
                        Description = "Peon aprendiz"
                    });

                    context.SaveChanges();
                }

                if (!context.User.Any())
                {
                    context.User.Add(new User
                    {
                        Name = "Jesús",
                        Surname = "Sánchez Corzo",
                        Username = "jsanchco",
                        AddedDate = DateTime.Now,
                        BirthDate = new DateTime(1972, 8, 1),
                        Email = "jsanchco@gmail.com",
                        Password = "123456",
                        PhoneNumber = "616203002",
                        RoleId = 1
                    });
                    context.SaveChanges();

                    context.User.Add(new User
                    {
                        Name = "Raul",
                        Surname = "Pérez Rodríguez",
                        Username = "rperez",
                        AddedDate = DateTime.Now,
                        BirthDate = new DateTime(1989, 3, 23),
                        Email = "rperez@gmail.com",
                        Password = "123456",
                        PhoneNumber = "678963366",
                        RoleId = 2,
                        ProfessionId = 1
                    });
                    context.SaveChanges();

                    for (var i = 0; i < 40; i++)
                    {
                        context.User.Add(new User
                        {
                            Name = "Trabajador" + (i + 1),
                            Surname = "Apellidos" + (i + 1),
                            Username = "trabajador" + (i + 1),
                            AddedDate = DateTime.Now,
                            Password = "123456",
                            RoleId = 3,
                            ProfessionId = 1
                        });
                        context.SaveChanges();
                    }                   
                }

                dbContextTransaction.Commit();

                stopWatch.Stop();
                var ts = stopWatch.Elapsed;                

                Console.WriteLine("");

                Console.WriteLine($"Table User -> {context.User.Count()} rows");
                Console.WriteLine($"Table Profession -> {context.Profession.Count()} rows");
                Console.WriteLine($"\t{ts.Seconds}.{ts.Milliseconds} sg.ms");

                Console.WriteLine("");
            }
        }

        private static void SeedFromMySQL(string options)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var optionsBuilder = new DbContextOptionsBuilder<EFContextMySQL>();
            optionsBuilder.UseMySql(options);

            using (var context = new EFContextMySQL(optionsBuilder.Options))
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                if (!context.Role.Any())
                {
                    context.Role.Add(new Role
                    {
                        AddedDate = DateTime.Now,
                        Name = "Super"
                    });
                    context.Role.Add(new Role
                    {
                        AddedDate = DateTime.Now,
                        Name = "Administrador"
                    });
                    context.Role.Add(new Role
                    {
                        AddedDate = DateTime.Now,
                        Name = "Trabajador"
                    });

                    context.SaveChanges();
                }

                if (!context.Profession.Any())
                {
                    context.Profession.Add(new Profession
                    {
                        AddedDate = DateTime.Now,
                        Name = "Peon 1ª",
                        Description = "Peon 1ª"
                    });

                    context.Profession.Add(new Profession
                    {
                        AddedDate = DateTime.Now,
                        Name = "Peon aprendiz",
                        Description = "Peon aprendiz"
                    });

                    context.SaveChanges();
                }

                if (!context.User.Any())
                {
                    context.User.Add(new User
                    {
                        Name = "Jesús",
                        Surname = "Sánchez Corzo",
                        Username = "jsanchco",
                        AddedDate = DateTime.Now,
                        BirthDate = new DateTime(1972, 8, 1),
                        Email = "jsanchco@gmail.com",
                        Password = "123456",
                        PhoneNumber = "616203002",
                        RoleId = 1
                    });

                    context.User.Add(new User
                    {
                        Name = "Raul",
                        Surname = "Pérez Rodríguez",
                        Username = "rperez",
                        AddedDate = DateTime.Now,
                        BirthDate = new DateTime(1989, 3, 23),
                        Email = "rperez@gmail.com",
                        Password = "123456",
                        PhoneNumber = "678963366",
                        RoleId = 3,
                        ProfessionId = 1
                    });

                    context.SaveChanges();
                }

                dbContextTransaction.Commit();

                stopWatch.Stop();
                var ts = stopWatch.Elapsed;

                Console.WriteLine("");

                Console.WriteLine($"Table User -> {context.User.Count()} rows");
                Console.WriteLine($"Table Profession -> {context.Profession.Count()} rows");
                Console.WriteLine($"\t{ts.Seconds}.{ts.Milliseconds} sg.ms");

                Console.WriteLine("");
            }
        }
    }
}
