﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using entityFrameworkCore;

#nullable disable

namespace entityFrameworkCore.Migrations
{
    [DbContext(typeof(LocalDbContext))]
    partial class LocalDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("OrganizationUser", b =>
                {
                    b.Property<Guid>("MembersId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MembershipsId")
                        .HasColumnType("TEXT");

                    b.HasKey("MembersId", "MembershipsId");

                    b.HasIndex("MembershipsId");

                    b.ToTable("OrganizationUser");
                });

            modelBuilder.Entity("UserWorkspace", b =>
                {
                    b.Property<Guid>("ContactsId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("WorkspaceId")
                        .HasColumnType("TEXT");

                    b.HasKey("ContactsId", "WorkspaceId");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("WorkspaceContacts", (string)null);
                });

            modelBuilder.Entity("domain.models.organization.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("_ownerId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("_ownerId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("domain.models.project.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("End")
                        .HasColumnType("TEXT");

                    b.Property<int>("Priority")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Start")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("WorkspaceId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("domain.models.resource.Resource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("OrganizationId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("WorkItemId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("WorkspaceId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("WorkItemId");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("domain.models.user.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("domain.models.workItem.WorkItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AssignedToId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Priority")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AssignedToId");

                    b.HasIndex("ParentId");

                    b.HasIndex("ProjectId");

                    b.ToTable("WorkItems");
                });

            modelBuilder.Entity("domain.models.workspace.Workspace", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Workspaces");
                });

            modelBuilder.Entity("OrganizationUser", b =>
                {
                    b.HasOne("domain.models.user.User", null)
                        .WithMany()
                        .HasForeignKey("MembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("domain.models.organization.Organization", null)
                        .WithMany()
                        .HasForeignKey("MembershipsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UserWorkspace", b =>
                {
                    b.HasOne("domain.models.user.User", null)
                        .WithMany()
                        .HasForeignKey("ContactsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("domain.models.workspace.Workspace", null)
                        .WithMany()
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("domain.models.organization.Organization", b =>
                {
                    b.HasOne("domain.models.user.User", "Owner")
                        .WithMany("OwnedOrganizations")
                        .HasForeignKey("_ownerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("domain.models.project.Project", b =>
                {
                    b.HasOne("domain.models.workspace.Workspace", "Workspace")
                        .WithMany("Projects")
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("domain.models.resource.Resource", b =>
                {
                    b.HasOne("domain.models.organization.Organization", null)
                        .WithMany("Resources")
                        .HasForeignKey("OrganizationId");

                    b.HasOne("domain.models.project.Project", null)
                        .WithMany("Resources")
                        .HasForeignKey("ProjectId");

                    b.HasOne("domain.models.workItem.WorkItem", null)
                        .WithMany("Resources")
                        .HasForeignKey("WorkItemId");

                    b.HasOne("domain.models.workspace.Workspace", null)
                        .WithMany("Resources")
                        .HasForeignKey("WorkspaceId");
                });

            modelBuilder.Entity("domain.models.workItem.WorkItem", b =>
                {
                    b.HasOne("domain.models.user.User", "AssignedTo")
                        .WithMany()
                        .HasForeignKey("AssignedToId");

                    b.HasOne("domain.models.workItem.WorkItem", "Parent")
                        .WithMany("Subitems")
                        .HasForeignKey("ParentId");

                    b.HasOne("domain.models.project.Project", "Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssignedTo");

                    b.Navigation("Parent");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("domain.models.workspace.Workspace", b =>
                {
                    b.HasOne("domain.models.organization.Organization", "Owner")
                        .WithMany("Workspaces")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("domain.models.organization.Organization", b =>
                {
                    b.Navigation("Resources");

                    b.Navigation("Workspaces");
                });

            modelBuilder.Entity("domain.models.project.Project", b =>
                {
                    b.Navigation("Resources");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("domain.models.user.User", b =>
                {
                    b.Navigation("OwnedOrganizations");
                });

            modelBuilder.Entity("domain.models.workItem.WorkItem", b =>
                {
                    b.Navigation("Resources");

                    b.Navigation("Subitems");
                });

            modelBuilder.Entity("domain.models.workspace.Workspace", b =>
                {
                    b.Navigation("Projects");

                    b.Navigation("Resources");
                });
#pragma warning restore 612, 618
        }
    }
}
