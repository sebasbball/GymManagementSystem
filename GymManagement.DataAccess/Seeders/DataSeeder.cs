using GymManagement.DataAccess.Context;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DataAccess.Seeders
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(GymDbContext context)
        {
            // Only seed if the database is empty
            if (await context.Trainers.AnyAsync()) return;

            // ── 1. TRAINERS ──
            var trainers = new List<Trainer>
            {
                new() { FirstName = "Carlos",   LastName = "Ramírez",  Email = "carlos.ramirez@gym.com",  Phone = "3001112233", Specialty = "Yoga and Flexibility" },
                new() { FirstName = "Andrea",   LastName = "López",    Email = "andrea.lopez@gym.com",    Phone = "3012223344", Specialty = "CrossFit and Strength" },
                new() { FirstName = "Miguel",   LastName = "Torres",   Email = "miguel.torres@gym.com",   Phone = "3023334455", Specialty = "Cardio and Spinning" },
                new() { FirstName = "Valentina", LastName = "Gómez",   Email = "valentina.gomez@gym.com", Phone = "3034445566", Specialty = "Pilates and Core" },
            };

            context.Trainers.AddRange(trainers);
            await context.SaveChangesAsync();

            // ── 2. MEMBERS ──
            var members = new List<Member>
            {
                new() { FirstName = "Juan",      LastName = "Pérez",     Email = "juan.perez@mail.com",     Phone = "3101112233", BirthDate = new DateTime(1995, 3, 15),  JoinDate = DateTime.UtcNow },
                new() { FirstName = "María",     LastName = "González",  Email = "maria.gonzalez@mail.com", Phone = "3112223344", BirthDate = new DateTime(1998, 7, 22),  JoinDate = DateTime.UtcNow },
                new() { FirstName = "Andrés",    LastName = "Martínez",  Email = "andres.martinez@mail.com",Phone = "3123334455", BirthDate = new DateTime(1990, 11, 5),  JoinDate = DateTime.UtcNow },
                new() { FirstName = "Laura",     LastName = "Sánchez",   Email = "laura.sanchez@mail.com",  Phone = "3134445566", BirthDate = new DateTime(2000, 1, 30),  JoinDate = DateTime.UtcNow },
                new() { FirstName = "Felipe",    LastName = "Ríos",      Email = "felipe.rios@mail.com",    Phone = "3145556677", BirthDate = new DateTime(1993, 6, 18),  JoinDate = DateTime.UtcNow },
                new() { FirstName = "Camila",    LastName = "Vargas",    Email = "camila.vargas@mail.com",  Phone = "3156667788", BirthDate = new DateTime(1997, 9, 25),  JoinDate = DateTime.UtcNow },
                new() { FirstName = "Sebastián", LastName = "Castro",    Email = "sebastian.castro@mail.com",Phone = "3167778899", BirthDate = new DateTime(1988, 4, 12), JoinDate = DateTime.UtcNow },
                new() { FirstName = "Daniela",   LastName = "Herrera",   Email = "daniela.herrera@mail.com",Phone = "3178889900", BirthDate = new DateTime(2001, 12, 8), JoinDate = DateTime.UtcNow },
                new() { FirstName = "Diego",     LastName = "Morales",   Email = "diego.morales@mail.com",  Phone = "3189990011", BirthDate = new DateTime(1996, 2, 28),  JoinDate = DateTime.UtcNow },
                new() { FirstName = "Natalia",   LastName = "Jiménez",   Email = "natalia.jimenez@mail.com",Phone = "3190001122", BirthDate = new DateTime(1992, 8, 14),  JoinDate = DateTime.UtcNow },
            };

            context.Members.AddRange(members);
            await context.SaveChangesAsync();

            // ── 3. MEMBERSHIPS (one active per member) ──
            var memberships = new List<Membership>();
            var membershipTypes = new[]
            {
                MembershipType.Basic,
                MembershipType.Premium,
                MembershipType.VIP,
                MembershipType.Basic,
                MembershipType.Premium,
                MembershipType.VIP,
                MembershipType.Basic,
                MembershipType.Premium,
                MembershipType.Basic,
                MembershipType.VIP,
            };

            for (int i = 0; i < members.Count; i++)
            {
                memberships.Add(new Membership
                {
                    MemberId = members[i].Id,
                    Type = membershipTypes[i],
                    StartDate = DateTime.UtcNow.AddDays(-30),
                    EndDate = DateTime.UtcNow.AddDays(335),
                    IsActive = true
                });
            }

            context.Memberships.AddRange(memberships);
            await context.SaveChangesAsync();

            // ── 4. GYM CLASSES ──
            var classes = new List<GymClass>
            {
                new() { Name = "Morning Yoga",      Description = "Relaxing yoga session to start the day.",   Capacity = 15, ScheduledAt = DateTime.UtcNow.AddDays(1).AddHours(7),  TrainerId = trainers[0].Id, Status = ClassStatus.Scheduled },
                new() { Name = "CrossFit Blast",    Description = "High intensity full body workout.",          Capacity = 12, ScheduledAt = DateTime.UtcNow.AddDays(1).AddHours(9),  TrainerId = trainers[1].Id, Status = ClassStatus.Scheduled },
                new() { Name = "Spinning Session",  Description = "45 minutes of intense cycling.",             Capacity = 20, ScheduledAt = DateTime.UtcNow.AddDays(2).AddHours(8),  TrainerId = trainers[2].Id, Status = ClassStatus.Scheduled },
                new() { Name = "Pilates Core",      Description = "Focus on core strength and posture.",        Capacity = 10, ScheduledAt = DateTime.UtcNow.AddDays(2).AddHours(10), TrainerId = trainers[3].Id, Status = ClassStatus.Scheduled },
                new() { Name = "Evening Stretch",   Description = "Wind down with full body stretching.",       Capacity = 15, ScheduledAt = DateTime.UtcNow.AddDays(3).AddHours(18), TrainerId = trainers[0].Id, Status = ClassStatus.Scheduled },
                new() { Name = "Power Lifting 101", Description = "Introduction to strength training basics.",  Capacity = 8,  ScheduledAt = DateTime.UtcNow.AddDays(4).AddHours(11), TrainerId = trainers[1].Id, Status = ClassStatus.Scheduled },
            };

            context.GymClasses.AddRange(classes);
            await context.SaveChangesAsync();

            // ── 5. ENROLLMENTS ──
            var enrollments = new List<Enrollment>
            {
                // Morning Yoga — 3 members
                new() { MemberId = members[0].Id, GymClassId = classes[0].Id, EnrolledAt = DateTime.UtcNow },
                new() { MemberId = members[1].Id, GymClassId = classes[0].Id, EnrolledAt = DateTime.UtcNow },
                new() { MemberId = members[2].Id, GymClassId = classes[0].Id, EnrolledAt = DateTime.UtcNow },

                // CrossFit Blast — 2 members
                new() { MemberId = members[3].Id, GymClassId = classes[1].Id, EnrolledAt = DateTime.UtcNow },
                new() { MemberId = members[4].Id, GymClassId = classes[1].Id, EnrolledAt = DateTime.UtcNow },

                // Spinning Session — 2 members
                new() { MemberId = members[5].Id, GymClassId = classes[2].Id, EnrolledAt = DateTime.UtcNow },
                new() { MemberId = members[6].Id, GymClassId = classes[2].Id, EnrolledAt = DateTime.UtcNow },

                // Pilates Core — 1 member
                new() { MemberId = members[7].Id, GymClassId = classes[3].Id, EnrolledAt = DateTime.UtcNow },
            };

            context.Enrollments.AddRange(enrollments);
            await context.SaveChangesAsync();
        }
    }
}