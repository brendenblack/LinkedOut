﻿// <auto-generated />
using System;
using LinkedOut.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LinkedOut.Infrastructure.Persistence.Migrations.PostgreSQL
{
    [DbContext(typeof(PostgreSqlApplicationDbContext))]
    [Migration("20210116205831_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("LinkedOut.Domain.Entities.JobApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("CoverLetter")
                        .HasColumnType("text")
                        .HasColumnName("cover_letter");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<string>("JobDescription")
                        .HasColumnType("text")
                        .HasColumnName("job_description");

                    b.Property<string>("JobTitle")
                        .HasColumnType("text")
                        .HasColumnName("job_title");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("last_modified");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<string>("OrganizationName")
                        .HasColumnType("text")
                        .HasColumnName("organization_name");

                    b.Property<int>("ParentSearchId")
                        .HasColumnType("integer")
                        .HasColumnName("parent_search_id");

                    b.Property<string>("Resume")
                        .HasColumnType("text")
                        .HasColumnName("resume");

                    b.Property<string>("Source")
                        .HasColumnType("text")
                        .HasColumnName("source");

                    b.HasKey("Id")
                        .HasName("pk_job_applications");

                    b.HasIndex("ParentSearchId")
                        .HasDatabaseName("ix_job_applications_parent_search_id");

                    b.ToTable("job_applications");
                });

            modelBuilder.Entity("LinkedOut.Domain.Entities.JobSearch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("last_modified");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<string>("OwnerId")
                        .HasColumnType("text")
                        .HasColumnName("owner_id");

                    b.Property<string>("Title")
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_job_searches");

                    b.ToTable("job_searches");
                });

            modelBuilder.Entity("LinkedOut.Domain.Entities.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("ApplicationId")
                        .HasColumnType("integer")
                        .HasColumnName("application_id");

                    b.Property<string>("Author")
                        .HasColumnType("text")
                        .HasColumnName("author");

                    b.Property<string>("Contents")
                        .HasColumnType("text")
                        .HasColumnName("contents");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("last_modified");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<string>("Subject")
                        .HasColumnType("text")
                        .HasColumnName("subject");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("timestamp");

                    b.HasKey("Id")
                        .HasName("pk_note");

                    b.HasIndex("ApplicationId")
                        .HasDatabaseName("ix_note_application_id");

                    b.ToTable("note");
                });

            modelBuilder.Entity("LinkedOut.Domain.Entities.Offer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("ApplicationId")
                        .HasColumnType("integer")
                        .HasColumnName("application_id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<string>("Details")
                        .HasColumnType("text")
                        .HasColumnName("details");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("expires");

                    b.Property<DateTime>("Extended")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("extended");

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_accepted");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("last_modified");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("Responded")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("responded");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_offer");

                    b.HasIndex("ApplicationId")
                        .IsUnique()
                        .HasDatabaseName("ix_offer_application_id");

                    b.ToTable("offer");
                });

            modelBuilder.Entity("LinkedOut.Domain.Entities.StatusTransition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("ApplicationId")
                        .HasColumnType("integer")
                        .HasColumnName("application_id");

                    b.Property<int>("Resolution")
                        .HasColumnType("integer")
                        .HasColumnName("resolution");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("timestamp");

                    b.Property<int>("TransitionFrom")
                        .HasColumnType("integer")
                        .HasColumnName("transition_from");

                    b.Property<int>("TransitionTo")
                        .HasColumnType("integer")
                        .HasColumnName("transition_to");

                    b.HasKey("Id")
                        .HasName("pk_status_transition");

                    b.HasIndex("ApplicationId")
                        .HasDatabaseName("ix_status_transition_application_id");

                    b.ToTable("status_transition");
                });

            modelBuilder.Entity("LinkedOut.Domain.Entities.JobApplication", b =>
                {
                    b.HasOne("LinkedOut.Domain.Entities.JobSearch", "ParentSearch")
                        .WithMany("Applications")
                        .HasForeignKey("ParentSearchId")
                        .HasConstraintName("fk_job_applications_job_searches_parent_search_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("LinkedOut.Domain.ValueObjects.Location", "Location", b1 =>
                        {
                            b1.Property<int>("JobApplicationId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasColumnName("id")
                                .UseIdentityByDefaultColumn();

                            b1.Property<string>("CityName")
                                .HasColumnType("text")
                                .HasColumnName("location_city_name");

                            b1.Property<string>("Province")
                                .HasColumnType("text")
                                .HasColumnName("location_province");

                            b1.HasKey("JobApplicationId")
                                .HasName("pk_job_applications");

                            b1.ToTable("job_applications");

                            b1.WithOwner()
                                .HasForeignKey("JobApplicationId")
                                .HasConstraintName("fk_job_applications_job_applications_id");
                        });

                    b.Navigation("Location");

                    b.Navigation("ParentSearch");
                });

            modelBuilder.Entity("LinkedOut.Domain.Entities.Note", b =>
                {
                    b.HasOne("LinkedOut.Domain.Entities.JobApplication", "Application")
                        .WithMany("Notes")
                        .HasForeignKey("ApplicationId")
                        .HasConstraintName("fk_note_job_applications_application_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("LinkedOut.Domain.Entities.Offer", b =>
                {
                    b.HasOne("LinkedOut.Domain.Entities.JobApplication", "Application")
                        .WithOne("Offer")
                        .HasForeignKey("LinkedOut.Domain.Entities.Offer", "ApplicationId")
                        .HasConstraintName("fk_offer_job_applications_application_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("LinkedOut.Domain.Entities.StatusTransition", b =>
                {
                    b.HasOne("LinkedOut.Domain.Entities.JobApplication", "Application")
                        .WithMany("Transitions")
                        .HasForeignKey("ApplicationId")
                        .HasConstraintName("fk_status_transition_job_applications_application_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("LinkedOut.Domain.Entities.JobApplication", b =>
                {
                    b.Navigation("Notes");

                    b.Navigation("Offer");

                    b.Navigation("Transitions");
                });

            modelBuilder.Entity("LinkedOut.Domain.Entities.JobSearch", b =>
                {
                    b.Navigation("Applications");
                });
#pragma warning restore 612, 618
        }
    }
}
