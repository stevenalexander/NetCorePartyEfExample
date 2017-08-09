using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using PartyData;

namespace PartyData.Migrations
{
    [DbContext(typeof(PartyDbContext))]
    [Migration("20170808151352_Add-Organisation-Person")]
    partial class AddOrganisationPerson
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.HasIndex("PartyId");

                    b.ToTable("Organisation");
                });

            modelBuilder.Entity("PartyData.Entities.Party", b =>
                {
                    b.Property<int>("PartyId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("PartyId");

                    b.ToTable("Party");
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

                    b.HasIndex("PartyId");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("PartyData.Entities.Organisation", b =>
                {
                    b.HasOne("PartyData.Entities.Party", "Party")
                        .WithMany()
                        .HasForeignKey("PartyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PartyData.Entities.Person", b =>
                {
                    b.HasOne("PartyData.Entities.Party", "Party")
                        .WithMany()
                        .HasForeignKey("PartyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
