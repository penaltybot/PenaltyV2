﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PenaltyV2.Data;

namespace PenaltyV2.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("PenaltyV2.Models.Bets", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("GoalsAwayTeam");

                    b.Property<int?>("GoalsHomeTeam");

                    b.Property<int>("IdmatchAPI");

                    b.Property<int?>("Matchday");

                    b.Property<int?>("Perfect");

                    b.Property<string>("Result");

                    b.Property<decimal?>("Score");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Bets");
                });

            modelBuilder.Entity("PenaltyV2.Models.Globalconstants", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Value");

                    b.Property<string>("Variable");

                    b.HasKey("Id");

                    b.ToTable("Globalconstants");
                });

            modelBuilder.Entity("PenaltyV2.Models.Leagues", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LeagueIDAPI");

                    b.Property<string>("LeagueName");

                    b.HasKey("Id");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("PenaltyV2.Models.LegacyScores", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("Position");

                    b.Property<decimal>("Score");

                    b.Property<string>("Season");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("LegacyScores");
                });

            modelBuilder.Entity("PenaltyV2.Models.Matches", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Awayteam");

                    b.Property<int?>("Awayteamgoals");

                    b.Property<string>("Competitionyear");

                    b.Property<string>("Hometeam");

                    b.Property<int?>("Hometeamgoals");

                    b.Property<int?>("Idawayteam");

                    b.Property<int?>("Idhometeam");

                    b.Property<int?>("IdmatchAPI");

                    b.Property<int>("LeagueID");

                    b.Property<int?>("Matchday");

                    b.Property<int?>("Matchnumber");

                    b.Property<decimal?>("Multiplier");

                    b.Property<decimal?>("Oddsaway");

                    b.Property<decimal?>("Oddsdraw");

                    b.Property<decimal?>("Oddshome");

                    b.Property<string>("Result1");

                    b.Property<string>("Status");

                    b.Property<DateTime?>("UtcDate");

                    b.HasKey("Id");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("PenaltyV2.Models.Teams", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LogoUri");

                    b.Property<string>("Name");

                    b.Property<string>("TeamId");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("PenaltyV2.Models.TeamsStandings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Draws");

                    b.Property<string>("Form");

                    b.Property<int?>("GoalsAgainst");

                    b.Property<int?>("GoalsFor");

                    b.Property<int?>("Losses");

                    b.Property<int?>("MatchesPlayed");

                    b.Property<int?>("Points");

                    b.Property<int?>("Rank");

                    b.Property<string>("TeamID");

                    b.Property<int?>("Wins");

                    b.HasKey("Id");

                    b.ToTable("TeamsStandings");
                });

            modelBuilder.Entity("PenaltyV2.Models.UsersCumulativeScores", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CorrectPredictions");

                    b.Property<string>("LeagueID");

                    b.Property<string>("Matchday");

                    b.Property<decimal?>("Score");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("UsersCumulativeScores");
                });

            modelBuilder.Entity("PenaltyV2.Models.Userscores", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Competitionyear");

                    b.Property<string>("Favoriteteam");

                    b.Property<string>("Name");

                    b.Property<int?>("Perfects");

                    b.Property<int?>("Position");

                    b.Property<decimal?>("Score");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Userscores");
                });

            modelBuilder.Entity("PenaltyV2.Models.Usersinfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Favoriteteam");

                    b.Property<string>("Leagues");

                    b.Property<string>("Name");

                    b.Property<int>("Notifications");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Usersinfo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
