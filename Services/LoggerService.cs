using System.Threading.Tasks;
using System;
using WebApi.Helpers;
using WebApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace WebApi.Services
{
    public interface ILoggerService
    {
        Task LogMessage(string logDescription, string usuarioAlta, DataContext _dbContext, int? customMarketCode);

        Task<List<Log>> GetAllLogs(DataContext _dbContext);
    }

    public class LoggerService : ILoggerService
    {
        public LoggerService()
        {

        }

        public async Task LogMessage( string logDescription, string usuarioAlta, DataContext _dbContext, int? customMarketCode)
        {
            try
            {
                //UserLogged.userLogged = new User()
                //{
                //    FirstName = "Fred",
                //    LastName = "Bigioni",
                //    CompanyId = 1,
                //    RolId = 1,
                //    UserName = "TEST\\abigioni"
                //};


                if (logDescription == null || usuarioAlta == null)
                {


                    var logDefault = new Log
                    {
                        Date = DateTime.Now,
                        Description = "",
                        UserLog = "",
                        CustomMarketCode = customMarketCode

                    };

                    _dbContext.Logs.Add(logDefault);
                    return;
                }

                var logEntry = new Log
                {
                    Date = DateTime.Now,
                    Description = logDescription,
                    UserLog = usuarioAlta,
                    CustomMarketCode = customMarketCode

                };


                _dbContext.Logs.Add(logEntry);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Log>> GetAllLogs(DataContext _dbContext)
        {
            try
            {
                return await _dbContext.Logs.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching logs: " + ex.Message);
            }
        }
    }
}
