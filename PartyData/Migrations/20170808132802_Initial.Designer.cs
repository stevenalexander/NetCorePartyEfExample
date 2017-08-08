using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using PartyData;

namespace PartyData.Migrations
{
    [DbContext(typeof(PartyDbContext))]
    [Migration("20170808132802_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
        }
    }
}
