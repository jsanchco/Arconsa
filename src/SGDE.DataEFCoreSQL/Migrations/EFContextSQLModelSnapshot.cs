﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SGDE.DataEFCoreSQL;

namespace SGDE.DataEFCoreSQL.Migrations
{
    [DbContext(typeof(EFContextSQL))]
    partial class EFContextSQLModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SGDE.Domain.Entities.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cif")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PromoterId")
                        .HasColumnType("int");

                    b.Property<int?>("TypeClientId")
                        .HasColumnType("int");

                    b.Property<string>("WayToPay")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PromoterId")
                        .HasName("IFK_Promoter_Client");

                    b.HasIndex("TypeClientId");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.CostWorker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("PriceHourExtra")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PriceHourFestive")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PriceHourOrdinary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("ProfessionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProfessionId");

                    b.HasIndex("UserId")
                        .HasName("IFK_User_CostClient");

                    b.ToTable("CostWorker");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.DailySigning", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndHour")
                        .HasColumnType("datetime2");

                    b.Property<int?>("HourTypeId")
                        .HasColumnType("int");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartHour")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserHiringId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("WorkId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HourTypeId")
                        .HasName("IFK_HourType_DailySigning");

                    b.HasIndex("UserHiringId")
                        .HasName("IFK_UserHiring_DailySigning");

                    b.HasIndex("UserId");

                    b.HasIndex("WorkId");

                    b.ToTable("DailySigning");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.HourType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HourType");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.Profession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AddedDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Profession");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.ProfessionInClient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("PriceHourExtra")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PriceHourFestive")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PriceHourOrdinary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PriceHourSaleExtra")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PriceHourSaleFestive")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PriceHourSaleOrdinary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProfessionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .HasName("IFK_Client_ProfessionInClient");

                    b.HasIndex("ProfessionId")
                        .HasName("IFK_Profession_ProfessionInClient");

                    b.ToTable("ProfessionInClient");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.Promoter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cif")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Promoter");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Setting");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.Training", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Center")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("File")
                        .HasColumnType("varbinary(max)");

                    b.Property<decimal>("Hours")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .HasName("IFK_User_Training");

                    b.ToTable("Training");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.TypeClient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TypeClient");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.TypeDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TypeDocument");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("AddedDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("Dni")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Observations")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("ProfessionId")
                        .HasColumnType("int");

                    b.Property<int?>("PromoterId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("SecuritySocialNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("WorkId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("ProfessionId")
                        .HasName("IFK_Profession_User");

                    b.HasIndex("PromoterId");

                    b.HasIndex("RoleId")
                        .HasName("IFK_Role_User");

                    b.HasIndex("UserId");

                    b.HasIndex("WorkId")
                        .HasName("IFK_Work_User");

                    b.ToTable("User");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.UserDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("File")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Observations")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TypeDocumentId")
                        .HasColumnType("int");

                    b.Property<string>("TypeFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TypeDocumentId")
                        .HasName("IFK_TypeDocument_UserDocument");

                    b.HasIndex("UserId");

                    b.ToTable("UserDocument");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.UserHiring", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ProfessionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("WorkId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProfessionId")
                        .HasName("IFK_Profession_UserHiring");

                    b.HasIndex("UserId")
                        .HasName("IFK_User_UserHiring");

                    b.HasIndex("WorkId")
                        .HasName("IFK_Work_UserHiring");

                    b.ToTable("UserHiring");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.Work", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CloseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EstimatedDuration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Open")
                        .HasColumnType("bit");

                    b.Property<DateTime>("OpenDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("PassiveSubject")
                        .HasColumnType("bit");

                    b.Property<int?>("TypeClientId")
                        .HasColumnType("int");

                    b.Property<string>("WorksToRealize")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .HasName("IFK_Client_Work");

                    b.HasIndex("TypeClientId");

                    b.ToTable("Work");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.Client", b =>
                {
                    b.HasOne("SGDE.Domain.Entities.Promoter", "Promoter")
                        .WithMany("Clients")
                        .HasForeignKey("PromoterId")
                        .HasConstraintName("FK__Client__PromoterId");

                    b.HasOne("SGDE.Domain.Entities.TypeClient", "TypeClient")
                        .WithMany()
                        .HasForeignKey("TypeClientId");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.CostWorker", b =>
                {
                    b.HasOne("SGDE.Domain.Entities.Profession", null)
                        .WithMany("CostWorkers")
                        .HasForeignKey("ProfessionId");

                    b.HasOne("SGDE.Domain.Entities.User", "User")
                        .WithMany("CostWorkers")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__CostWorker__UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SGDE.Domain.Entities.DailySigning", b =>
                {
                    b.HasOne("SGDE.Domain.Entities.HourType", "HourType")
                        .WithMany("DailySignings")
                        .HasForeignKey("HourTypeId")
                        .HasConstraintName("FK__DailySigning__HourTypeId");

                    b.HasOne("SGDE.Domain.Entities.UserHiring", "UserHiring")
                        .WithMany("DailysSigning")
                        .HasForeignKey("UserHiringId")
                        .HasConstraintName("FK__DailySigning__UserHiringId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SGDE.Domain.Entities.User", null)
                        .WithMany("DailySignings")
                        .HasForeignKey("UserId");

                    b.HasOne("SGDE.Domain.Entities.Work", null)
                        .WithMany("DailySignings")
                        .HasForeignKey("WorkId");
                });

            modelBuilder.Entity("SGDE.Domain.Entities.ProfessionInClient", b =>
                {
                    b.HasOne("SGDE.Domain.Entities.Client", "Client")
                        .WithMany("ProfessionInClients")
                        .HasForeignKey("ClientId")
                        .HasConstraintName("FK__ProfessionInClient__ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SGDE.Domain.Entities.Profession", "Profession")
                        .WithMany("ProfessionInClients")
                        .HasForeignKey("ProfessionId")
                        .HasConstraintName("FK__ProfessionInClient__ProfessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SGDE.Domain.Entities.Training", b =>
                {
                    b.HasOne("SGDE.Domain.Entities.User", "User")
                        .WithMany("Trainings")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__Training__UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SGDE.Domain.Entities.User", b =>
                {
                    b.HasOne("SGDE.Domain.Entities.Client", "Client")
                        .WithMany("ClientResponsibles")
                        .HasForeignKey("ClientId");

                    b.HasOne("SGDE.Domain.Entities.Profession", "Profession")
                        .WithMany("Users")
                        .HasForeignKey("ProfessionId")
                        .HasConstraintName("FK__User__ProfessionId");

                    b.HasOne("SGDE.Domain.Entities.Promoter", null)
                        .WithMany("PromoterResponsibles")
                        .HasForeignKey("PromoterId");

                    b.HasOne("SGDE.Domain.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK__User__RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SGDE.Domain.Entities.User", null)
                        .WithMany("ContactPersons")
                        .HasForeignKey("UserId");

                    b.HasOne("SGDE.Domain.Entities.Work", "Work")
                        .WithMany("WorkResponsibles")
                        .HasForeignKey("WorkId")
                        .HasConstraintName("FK__User__WorkId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("SGDE.Domain.Entities.UserDocument", b =>
                {
                    b.HasOne("SGDE.Domain.Entities.TypeDocument", "TypeDocument")
                        .WithMany("UserDocuments")
                        .HasForeignKey("TypeDocumentId")
                        .HasConstraintName("FK__UserDocument__TypeDocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SGDE.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SGDE.Domain.Entities.UserHiring", b =>
                {
                    b.HasOne("SGDE.Domain.Entities.Profession", "Profession")
                        .WithMany("UserHirings")
                        .HasForeignKey("ProfessionId")
                        .HasConstraintName("FK__UserHiring__ProfessionId");

                    b.HasOne("SGDE.Domain.Entities.User", "User")
                        .WithMany("UserHirings")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__UserHiring__UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SGDE.Domain.Entities.Work", "Work")
                        .WithMany("UserHirings")
                        .HasForeignKey("WorkId")
                        .HasConstraintName("FK__UserHiring__WorkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SGDE.Domain.Entities.Work", b =>
                {
                    b.HasOne("SGDE.Domain.Entities.Client", "Client")
                        .WithMany("Works")
                        .HasForeignKey("ClientId")
                        .HasConstraintName("FK__Work__ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SGDE.Domain.Entities.TypeClient", null)
                        .WithMany("Works")
                        .HasForeignKey("TypeClientId");
                });
#pragma warning restore 612, 618
        }
    }
}
