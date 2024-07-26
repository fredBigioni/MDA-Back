using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using WebApi.Models;
using WebApi.Entities;
using WebApi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services
{
    public interface ILineService
    {
        IEnumerable<Line> GetAll();
        IEnumerable<Line> GetAllLinesByLineGroup(int[] linesGroupCodesArray);
        Line GetByCode(int Code);
        int Update(int code, LineRequest line);
        Line Create(LineRequest line);
    }

    public class LineService : ILineService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;

        public LineService(
            DataContext context,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public IEnumerable<Line> GetAll()
        {
            return _context.Lines
                .Include(lines => lines.LineGroup)
                .OrderBy(l => l.Description)
                .ToList();
        }

        public IEnumerable<Line> GetAllLinesByLineGroup(int[] linesGroupCodesArray)
        {
            return _context.Lines
                .Where(lines => linesGroupCodesArray.Contains((int) lines.LineGroupCode))
                .OrderBy(l => l.Description)
                .ToList();
        }        

        public Line GetByCode(int Code)
        {
            return _context.Lines
                .Include(lines => lines.LineGroup)
                .SingleOrDefault(x => x.Code == Code);
        }

        public int Update(int code, LineRequest line)
        {
            var _line = this.GetByCode(code);
            _line.Description = line.Description;
            _line.LaboratoryReportHeader = line.LaboratoryReportHeader;
            _line.LaboratoryReportFooter = line.LaboratoryReportFooter;
            _line.DrugReportHeader = line.DrugReportHeader;
            _line.DrugReportFooter = line.DrugReportFooter;

            var _lineGroup = _context.LineGroups.SingleOrDefault(x => x.Code == line.LineGroupCode);
            _line.LineGroup = _lineGroup;

            _context.Entry(_line).State = EntityState.Modified;
            return _context.SaveChanges();
        }

        public Line Create(LineRequest line)
        {
            Line _line = new Line();

            _line.Description = line.Description;
            _line.LineGroupCode = line.LineGroupCode;
            _line.LaboratoryReportHeader = line.LaboratoryReportHeader;
            _line.LaboratoryReportFooter = line.LaboratoryReportFooter;
            _line.DrugReportHeader = line.DrugReportHeader;
            _line.DrugReportFooter = line.DrugReportFooter;

            _context.Lines.Add(_line);  
            _context.SaveChanges();

            return _line;            
        }        
    }
}