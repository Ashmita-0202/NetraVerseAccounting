
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.ServiceInterface;
using TN.Authentication.Domain.Entities;
using TN.Authentication.Domain.Static.Roles;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.Initialize;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Infrastructure.Data;
using TN.Shared.Infrastructure.DataSeed;

namespace TN.Setup.Infrastructure.ServiceImpl
{
    public class InitializeServices : IInitializeServices
    {
        private readonly ApplicationDbContext _context;
        private readonly DataSeeder _dataSeeder;
        private readonly IAuthenticationServices _authenticationService;
        public InitializeServices(ApplicationDbContext applicationDbContext, DataSeeder dataSeeder, IAuthenticationServices authenticationService)
        {
            _authenticationService = authenticationService;
            _dataSeeder = dataSeeder;
            _context = applicationDbContext;
            
        }
        public async Task<Result<InitializeCommandResponse>> InitializeAsync()
        {
            //await _context.Database.MigrateAsync();
            var successMessage = await SeedAsync();
            return Result<InitializeCommandResponse>.Success(successMessage);
        }


        private async Task<InitializeCommandResponse> SeedAsync()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Step 1: Check if already initialized
                //if (await _context.Organizations.AnyAsync())
                //{
                //    return new InitializeCommandResponse(null!, null!, "Already Initialized");
                //}

                // Step 2: Delete all data safely in FK order
                var entityTypes = _context.Model.GetEntityTypes()
                    .Where(e => !e.IsOwned() && e.GetTableName() != null)
                    .ToList();

                var dependencyMap = new Dictionary<string, List<string>>();
                foreach (var entity in entityTypes)
                {
                    var tableName = entity.GetTableName()!;
                    var dependencies = entity.GetForeignKeys()
                        .Select(fk => fk.PrincipalEntityType.GetTableName())
                        .Where(t => !string.IsNullOrEmpty(t))
                        .Distinct()
                        .ToList();

                    dependencyMap[tableName] = dependencies!;
                }

                List<string> TopologicalSort(Dictionary<string, List<string>> map)
                {
                    var visited = new HashSet<string>();
                    var sorted = new List<string>();

                    void Visit(string table)
                    {
                        if (!visited.Contains(table))
                        {
                            visited.Add(table);
                            foreach (var dep in map.GetValueOrDefault(table, new List<string>()))
                            {
                                Visit(dep);
                            }
                            sorted.Add(table);
                        }
                    }

                    foreach (var table in map.Keys)
                        Visit(table);

                    sorted.Reverse();
                    return sorted;
                }

                var sortedTables = TopologicalSort(dependencyMap);

                foreach (var table in sortedTables)
                {
                    await _context.Database.ExecuteSqlRawAsync($"DELETE FROM [{table}]");
                }

                // Step 3: Seed base data
                await _dataSeeder.Seed();

                // Step 4: Create Super Admin User
                var superAdminUser = new ApplicationUser
                {
                    UserName = "superadminuser",
                    Email = "nep.pankaj@gmail.com"
                };
                var superAdminPassword = "accountingappfe#1402";

                await _authenticationService.CreateUserAsync(superAdminUser, superAdminPassword);
                await _authenticationService.AssignRoles(superAdminUser, Role.SuperAdmin);

                // Step 5: Create Developer User
                var developerUser = new ApplicationUser
                {
                    UserName = "developeruser",
                    Email = "developeruser@gmail.com"
                };
                var developerPassword = "Admin@123";

                await _authenticationService.CreateUserAsync(developerUser, developerPassword);
                await _authenticationService.AssignRoles(developerUser, Role.DeveloperUser);

                // Commit transaction
                await transaction.CommitAsync();

                return new InitializeCommandResponse(
                    username: superAdminUser.Email,
                    password: superAdminPassword,
                    message: "Initialized Successfully"
                );
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Seeding failed: {ex.Message}", ex);
            }
        }




    }
}
