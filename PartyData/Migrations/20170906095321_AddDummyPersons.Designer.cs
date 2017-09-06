using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using PartyData;

namespace PartyData.Migrations
{
    [DbContext(typeof(PartyDbContext))]
    [Migration("20170906095321_AddDummyPersons")]
    partial class AddDummyPersons
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PartyData.Entities.CustomService", b =>
                {
                    b.Property<int>("CustomServiceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("CustomServiceId");

                    b.ToTable("CustomService");
                });

            modelBuilder.Entity("PartyData.Entities.Organisation", b =>
                {
                    b.Property<int>("OrganisationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("OrganisationName")
                        .IsRequired();

                    b.Property<int>("PartyId");

                    b.Property<string>("TradingName")
                        .IsRequired();

                    b.HasKey("OrganisationId");

                    b.HasIndex("PartyId")
                        .IsUnique();

                    b.ToTable("Organisation");
                });

            modelBuilder.Entity("PartyData.Entities.Party", b =>
                {
                    b.Property<int>("PartyId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CustomServiceId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("PartyId");

                    b.HasIndex("CustomServiceId");

                    b.ToTable("Party");
                });

            modelBuilder.Entity("PartyData.Entities.PartyCustomServiceRegistration", b =>
                {
                    b.Property<int>("PartyCustomServiceRegistrationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomServiceId");

                    b.Property<int>("PartyId");

                    b.Property<bool>("RegistrationStatus");

                    b.HasKey("PartyCustomServiceRegistrationId");

                    b.HasIndex("CustomServiceId");

                    b.HasIndex("PartyId");

                    b.ToTable("PartyCustomServiceRegistration");
                });

            modelBuilder.Entity("PartyData.Entities.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<int>("PartyId");

                    b.Property<string>("Surname")
                        .IsRequired();

                    b.HasKey("PersonId");

                    b.HasIndex("PartyId")
                        .IsUnique();

                    b.ToTable("Person");
                });

            modelBuilder.Entity("PartyData.Entities.Organisation", b =>
                {
                    b.HasOne("PartyData.Entities.Party", "Party")
                        .WithOne("Organisation")
                        .HasForeignKey("PartyData.Entities.Organisation", "PartyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PartyData.Entities.Party", b =>
                {
                    b.HasOne("PartyData.Entities.CustomService")
                        .WithMany("RegisteredParties")
                        .HasForeignKey("CustomServiceId");
                });

            modelBuilder.Entity("PartyData.Entities.PartyCustomServiceRegistration", b =>
                {
                    b.HasOne("PartyData.Entities.CustomService", "CustomService")
                        .WithMany()
                        .HasForeignKey("CustomServiceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PartyData.Entities.Party", "Party")
                        .WithMany("CustomServiceRegistrations")
                        .HasForeignKey("PartyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PartyData.Entities.Person", b =>
                {
                    b.HasOne("PartyData.Entities.Party", "Party")
                        .WithOne("Person")
                        .HasForeignKey("PartyData.Entities.Person", "PartyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
