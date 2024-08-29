using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using WebApi.Models;
using WebApi.Entities;
using WebApi.Views;
using WebApi.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using Microsoft.AspNetCore.Routing;
using System.Text.Json;
using System.Data;

namespace WebApi.Services
{
    public interface ICustomMarketService
    {
        IEnumerable<Object> GetAllCustomMarkets();
        Task<string> GetLastAuditory();
        Task<Response<string>> SignMarket(SignMarketModel sign);
        Task<Response<string>> SignAllMarket(int lineCode, int signedUser);
        IEnumerable<Object> GetAllCustomMarketsByLine(int[] linesCodesArray);
        Object GetByCode(int code);
        CustomMarket Create(CustomMarketRequest customMarket);
        Object Update(int customMarketCode, CustomMarketRequest customMarket);
        Object Clone(int customMarketCode, CustomMarketRequest customMarket);
        Object Delete(int customMarketCode);
        Task<MarketDetailResult> GetMarketDetailHistoricJsonAsync(int customMarketCode);
        IEnumerable<Object> GetPreviewByCode(int customMarketCode);
        IEnumerable<Object> GetTree();
        IEnumerable<Object> GetTree(User user);
        Task<List<CustomMarketVersionHistoricModel>> GetMarketDetailHistoricByCustomMarketCode(int customMarketCode);
        Task<List<CustomMarketVersionHistoricModel>> GetMarketDetailLastSignByCustomMarketCode(int customMarketCode);
        Task<List<ProductDetail>> GetMarketPreviewHistoricByCustomMarketCode(int versionCode);
        Task<List<ProductDetail>> GetLastSignedPreviewHistoricByCustomMarketCode(int code);

    }

    public class CustomMarketService : ICustomMarketService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;
        private IProductService _productService;
        private IProductPresentationService _productPresentationService;
        private IUserPermissionService _userPermissionService;
        private ILineService _lineService;
        private ILineGroupService _lineGroupService;
        private ICustomMarketGroupService _customMarketGroupService;
        private ILoggerService _loggerService;

        public CustomMarketService(
            DataContext context,
            IOptions<AppSettings> appSettings,
            IProductService productService,
            IProductPresentationService productPresentationService,
            IUserPermissionService userPermissionService,
            ILineService lineService,
            ILineGroupService lineGroupService,
            ICustomMarketGroupService customMarketGroupService,
            ILoggerService loggerService)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _productService = productService;
            _productPresentationService = productPresentationService;
            _userPermissionService = userPermissionService;
            _lineService = lineService;
            _lineGroupService = lineGroupService;
            _customMarketGroupService = customMarketGroupService;
            _loggerService = loggerService;
        }

        public async Task<string> GetLastAuditory()
        {
            try
            {
                var lastAuditory = await _context.Periods.SingleOrDefaultAsync(p => p.Code == 1);
                if (lastAuditory != null)
                    return string.Format("{0}-{1}", lastAuditory.Month, lastAuditory.Year);

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Response<string>> SignMarket(SignMarketModel sign)
        {
            var response = new Response<string>();
            try
            {
                var cm = await _context.CustomMarkets.SingleOrDefaultAsync(c => c.Code == sign.CustomMarketCode);
                var cmd = await _context.CustomMarketDetails.SingleOrDefaultAsync(c => c.CustomMarketCode == sign.CustomMarketCode);
                if (cm != null)
                {
                    if (!cm.TestMarket)
                    {
                        if(cmd != null)
                        {
                            //Ejecutar el SP de firma
                            await _context.SP_SignMarket(sign);

                            response.Message = string.Format("Mercado firmado correctamente");
                            response.Status = true;

                            if (UserLogged.userLogged.Usuario == null)
                            {
                                response.Message = string.Format("Session Expirada");
                                response.Status = false;

                                return response;
                            }
                            _loggerService.LogMessage($"El usuario {UserLogged.userLogged.Usuario} ha firmado el mercado {cm.Description}", UserLogged.userLogged.Usuario, _context, cm.Code);
                            _context.SaveChanges();
                        }

                            else
                            {
                            response.Message = string.Format("No se puede firmar un mercado vacio");
                            response.Status = false;
                        }
                        
                    }
                    else
                    {
                        response.Message = string.Format("No se puede firmar un mercado de prueba");
                        response.Status = false;
                    }
                }
                else
                {
                    response.Message = string.Format("No existe el mercado solicitado");
                    response.Status = false;
                }
            }
            catch (Exception ex)
            {
                response.Message = string.Format("{0}", ex.Message);
                response.Status = false;
            }

            return response;
        }
        public async Task<Response<string>> SignAllMarket(int lineCode, int signedUser)
        {
            var response = new Response<string>();
            response.Message = string.Empty;
            response.Status = true;
            try
            {
                var customMarkets = await _context.CustomMarkets.Where(c => c.LineCode == lineCode).ToListAsync();
                if (customMarkets.Any())
                {
                    foreach (var cm in customMarkets)
                    {
                        if (!cm.TestMarket)
                        {
                            var sign = new SignMarketModel()
                            {
                                CustomMarketCode = cm.Code,
                                SignedUser = signedUser
                            };
                            //Ejecutar el SP de firma
                            await _context.SP_SignMarket(sign);
                        }
                        else
                        {
                            response.Message += string.Format("No se puede firmar el mercado {0}, porque es de prueba\n", cm.Description);
                            response.Status = false;
                        }
                    }

                    if (response.Status)
                        response.Message += string.Format("Se han firmado correctamente todos los mercados de la linea {0}", lineCode);
                }
                else
                {
                    response.Message = string.Format("No existen Mercados para la linea {0}", lineCode);
                    response.Status = false;
                }
            }
            catch (Exception ex)
            {
                response.Message = string.Format("{0}", ex.Message);
                response.Status = false;
            }

            return response;
        }

        public IEnumerable<Object> GetAllCustomMarkets()
        {
            //   return _context.CustomMarkets
            //.Join(
            //_context.CustomMarketActualDefinitions,
            //customMarket => customMarket.Code,
            //customMarketActualDefinition => customMarketActualDefinition.CustomMarketCode,
            //(customMarket, customMarketActualDefinition) => new { CustomMarket = customMarket, CMAD = customMarketActualDefinition }
            //)
            //.OrderBy(c => c.CustomMarket.Description)
            //.Select(c => new
            //{
            //    Code = c.CustomMarket.Code,
            //    Description = c.CustomMarket.Description,
            //    Line = c.CustomMarket.Line.Description,
            //    LineGroup = c.CustomMarket.Line.LineGroup.Description,
            //    Signed = c.CMAD.SignedUser != null
            //})
            //.Distinct()
            //.OrderBy(cm => cm.Description)
            //.ToList();

            //return _context.CustomMarkets
            //    .OrderBy(c => c.Description)
            //    .Select(c => new
            //    {
            //        Code = c.Code,
            //        Description = c.Description,
            //        Line = c.Line.Description,
            //        LineGroup = c.Line.LineGroup.Description
            //    })
            //    .Distinct()
            //    .OrderBy(cm => cm.Description)
            //    .ToList();
            var result = from customMarket in _context.CustomMarkets
                         join customMarketActualDefinition in _context.CustomMarketActualDefinitions
                         on customMarket.Code equals customMarketActualDefinition.CustomMarketCode into joined
                         from subCustomMarketActualDefinition in joined.DefaultIfEmpty()
                         orderby customMarket.Description
                         select new
                         {
                             Code = customMarket.Code,
                             Description = customMarket.Description,
                             Line = customMarket.Line.Description,
                             LineGroup = customMarket.Line.LineGroup.Description,
                             Signed = subCustomMarketActualDefinition != null && subCustomMarketActualDefinition.SignedUser != null
                         };

            return result
                .Distinct()
                .OrderBy(cm => cm.Description)
                .ToList();
        }

        public IEnumerable<Object> GetAllCustomMarketsByLine(int[] linesCodesArray)
        {
            return _context.CustomMarkets
                .Where(c =>
                    linesCodesArray.Contains((int)c.LineCode)
                )
                .OrderBy(c => c.Description)
                .Select(c => new
                {
                    Code = c.Code,
                    Description = c.Description,
                    Line = c.Line.Description,
                    LineGroup = c.Line.LineGroup.Description
                })
                .Distinct()
                .OrderBy(cm => cm.Description)
                .ToList();
        }


        public Object GetByCode(int code)
        {
            var _customMarket = _context.CustomMarkets
                .Include(customMarket => customMarket.CustomMarketDetails)
                    .ThenInclude(customMarketDetails => customMarketDetails.Class)
                .Include(customMarket => customMarket.CustomMarketDetails)
                    .ThenInclude(customMarketDetails => customMarketDetails.CustomMarketGroup)
                .Include(customMarket => customMarket.CustomMarketDetails)
                    .ThenInclude(customMarketDetails => customMarketDetails.DetailCustomMarket)
                .Include(customMarket => customMarket.CustomMarketDetails)
                    .ThenInclude(customMarketDetails => customMarketDetails.Drug)
                .Include(customMarket => customMarket.CustomMarketDetails)
                    .ThenInclude(customMarketDetails => customMarketDetails.DrugGroup)
                .Include(customMarket => customMarket.CustomMarketDetails)
                    .ThenInclude(customMarketDetails => customMarketDetails.Laboratory)
                .Include(customMarket => customMarket.CustomMarketDetails)
                    .ThenInclude(customMarketDetails => customMarketDetails.LaboratoryGroup)
                .Include(customMarket => customMarket.CustomMarketDetails)
                    .ThenInclude(customMarketDetails => customMarketDetails.ProductGroup)
                .Include(customMarket => customMarket.CustomMarketDetails)
                    .ThenInclude(customMarketDetails => customMarketDetails.ProductPresentationGroup)
                .Include(customMarket => customMarket.CustomMarketDetails)
                    .ThenInclude(customMarketDetails => customMarketDetails.PharmaceuticalForm)
                .Include(customMarket => customMarket.CustomMarketDetails)
                    .ThenInclude(customMarketDetails => customMarketDetails.TherapeuticalClass)
                .Include(customMarket => customMarket.CustomMarketDetails)
                    .ThenInclude(customMarketDetails => customMarketDetails.ProductType)
                .Include(customMarket => customMarket.Line)
                    .ThenInclude(line => line.LineGroup)
                .SingleOrDefault(x => x.Code == code);

            List<CustomMarketDetailResponse> CustomMarketDetailResponses = new List<CustomMarketDetailResponse>();
            if (_customMarket == null)
            {
                return null;
            }
            else if (_customMarket.CustomMarketDetails != null)
            {
                foreach (CustomMarketDetail cmDetail in _customMarket.CustomMarketDetails)
                {
                    string productDescription = null;
                    string productPresentationDescription = null;
                    bool? ownProduct = null;
                    bool? ownProductMarket = false;
                    if (cmDetail.ProductCode != null)
                    {
                        var productComponent = _productService.GetProductComponentsByCode(cmDetail.ProductCode);
                        productDescription = productComponent.Description + " - " + productComponent.Class + " - " + productComponent.Laboratory;
                        ownProduct = productComponent.OwnProduct;
                        if (ownProduct == true)
                        {
                            var _ownProductMarket = _context.ProductMarkets
                                .SingleOrDefault(pc => pc.MarketCode == code && pc.ProductCode == cmDetail.ProductCode);

                            ownProductMarket = _ownProductMarket != null;
                        }

                    }
                    else if (cmDetail.ProductPresentationCode != null)
                    {
                        var productPresentationComponent = _productPresentationService.GetProductPresentationComponentByCode(cmDetail.ProductPresentationCode);
                        productPresentationDescription = productPresentationComponent.Description + " - " + productPresentationComponent.Class + " - " + productPresentationComponent.Laboratory + " - " + productPresentationComponent.TherapeuticalClass;
                    }
                    CustomMarketDetailResponse customMarketDetailResponse = new CustomMarketDetailResponse(cmDetail, productDescription, productPresentationDescription, ownProduct, ownProductMarket);
                    CustomMarketDetailResponses.Add(customMarketDetailResponse);
                }
            }
            return new CustomMarketResponse(
                _customMarket.Code,
                _customMarket.Description,
                _customMarket.DrugReport,
                _customMarket.Footer,
                _customMarket.Header,
                _customMarket.LabReport,
                _customMarket.Line,
                _customMarket.MarketClass,
                _customMarket.MarketFilter,
                _customMarket.MarketReference,
                _customMarket.Order,
                _customMarket.ProductReport,
                _customMarket.TestMarket,
                _customMarket.ControlPanel,
                _customMarket.Tcreport,
                _customMarket.TravelCrm,
                _customMarket.AdUser,
                _customMarket.ResponsibleName,
                _customMarket.ResponsibleLastName,
                CustomMarketDetailResponses
            );
        }

        public CustomMarket Create(CustomMarketRequest customMarket)
        {
            CustomMarket _customMarket = new CustomMarket();

            _customMarket.Description = customMarket.Description;
            _customMarket.DrugReport = customMarket.DrugReport;
            _customMarket.Footer = customMarket.Footer;
            _customMarket.Header = customMarket.Header;
            _customMarket.IsOtc = customMarket.IsOtc;
            _customMarket.LabReport = customMarket.LabReport;
            _customMarket.LineCode = customMarket.LineCode;
            _customMarket.MarketClass = customMarket.MarketClass;
            _customMarket.MarketFilter = customMarket.MarketFilter;
            _customMarket.MarketReference = customMarket.MarketReference;
            _customMarket.Order = customMarket.Order;
            _customMarket.ProductReport = customMarket.ProductReport;
            _customMarket.Tcreport = customMarket.Tcreport;
            _customMarket.TestMarket = customMarket.TestMarket;
            _customMarket.ControlPanel = customMarket.ControlPanel;
            _customMarket.TravelCrm = customMarket.TravelCrm;
            _customMarket.AdUser = customMarket.AdUser;
            _customMarket.ResponsibleName = customMarket.ResponsibleName;
            _customMarket.ResponsibleLastName = customMarket.ResponsibleLastName;

            _context.CustomMarkets.Add(_customMarket);

            _context.SaveChanges();
            _loggerService.LogMessage($"El usuario {UserLogged.userLogged.Usuario} creó el mercado {_customMarket.Description}", UserLogged.userLogged.Usuario, _context, _customMarket.Code);
            _context.SaveChanges();

            return _customMarket;
        }

        public Object Update(int customMarketCode, CustomMarketRequest customMarket)
        {
            CustomMarket _customMarket = _context.CustomMarkets
                .SingleOrDefault(x => x.Code == customMarketCode);

            if (_customMarket == null) return null;

            _customMarket.Description = customMarket.Description;
            _customMarket.DrugReport = customMarket.DrugReport;
            _customMarket.Footer = customMarket.Footer;
            _customMarket.Header = customMarket.Header;
            _customMarket.IsOtc = customMarket.IsOtc;
            _customMarket.LabReport = customMarket.LabReport;
            _customMarket.LineCode = customMarket.LineCode;
            _customMarket.MarketClass = customMarket.MarketClass;
            _customMarket.MarketFilter = customMarket.MarketFilter;
            _customMarket.MarketReference = customMarket.MarketReference;
            _customMarket.Order = customMarket.Order;
            _customMarket.ProductReport = customMarket.ProductReport;
            _customMarket.Tcreport = customMarket.Tcreport;
            _customMarket.TestMarket = customMarket.TestMarket;
            _customMarket.ControlPanel = customMarket.ControlPanel;
            _customMarket.TravelCrm = customMarket.TravelCrm;
            _customMarket.AdUser = customMarket.AdUser;
            _customMarket.ResponsibleName = customMarket.ResponsibleName;
            _customMarket.ResponsibleLastName = customMarket.ResponsibleLastName;

            if (customMarket.CustomMarketDetail != null)
            {

                List<CustomMarketDetail> cmds = _context.CustomMarketDetails.Where(d => d.CustomMarketCode == _customMarket.Code).ToList();
                if (cmds.Any())
                {
                    foreach (CustomMarketDetail cmd in cmds)
                    {
                        _context.CustomMarketDetails.Remove(cmd);
                    }
                }

                if (customMarket.CustomMarketDetail.Any())
                {
                    List<CustomMarketDetail> _customMarketDetails = new List<CustomMarketDetail>();
                    foreach (CustomMarketDetailRequest cmdr in customMarket.CustomMarketDetail)
                    {
                        if (cmdr.DetailCustomMarketCode != null)
                        {

                            if (cmdr.DetailCustomMarketCode == customMarketCode)
                            {
                                throw new Exception("is_not_valid_add_detail_custom_market," + cmdr.DetailCustomMarketCode.ToString());
                            }
                            else
                            {
                                bool isNotValidAddDetailCustomMarketCode = this.hasCustomMarketAssignDetailCustomMarket(cmdr.DetailCustomMarketCode);
                                if (isNotValidAddDetailCustomMarketCode)
                                {
                                    throw new Exception("is_not_valid_add_detail_custom_market," + cmdr.DetailCustomMarketCode.ToString());
                                }
                            }

                        }

                        CustomMarketDetail customMarketDetail = new CustomMarketDetail();

                        customMarketDetail.DetailCustomMarketCode = cmdr.DetailCustomMarketCode;
                        customMarketDetail.CustomMarketGroupCode = cmdr.CustomMarketGroupCode;
                        customMarketDetail.DrugCode = cmdr.DrugCode;
                        customMarketDetail.DrugGroupCode = cmdr.DrugGroupCode;
                        customMarketDetail.EnsureVisible = cmdr.EnsureVisible;
                        customMarketDetail.Expand = cmdr.Expand;
                        customMarketDetail.Graphs = cmdr.Graphs;
                        customMarketDetail.Intemodifier = double.Parse(cmdr.Intemodifier);
                        customMarketDetail.ItemCondition = cmdr.ItemCondition;
                        customMarketDetail.LaboratoryCode = cmdr.LaboratoryCode;
                        customMarketDetail.LaboratoryGroupCode = cmdr.LaboratoryGroupCode;
                        customMarketDetail.Modifier = double.Parse(cmdr.Modifier);
                        customMarketDetail.Order = cmdr.Order;
                        customMarketDetail.Pattern = cmdr.Pattern;
                        customMarketDetail.PharmaceuticalFormCode = cmdr.PharmaceuticalFormCode;
                        customMarketDetail.ProductCode = cmdr.ProductCode;
                        customMarketDetail.ProductGroupCode = cmdr.ProductGroupCode;
                        customMarketDetail.ProductPresentationCode = cmdr.ProductPresentationCode;
                        customMarketDetail.ProductPresentationGroupCode = cmdr.ProductPresentationGroupCode;
                        customMarketDetail.ProductTypeCode = cmdr.ProductTypeCode;
                        customMarketDetail.RegExPattern = cmdr.RegExPattern;
                        customMarketDetail.Resume = cmdr.Resume;
                        customMarketDetail.TherapeuticalClassCode = cmdr.TherapeuticalClassCode;

                        if (cmdr.ProductCode != null)
                        {
                            ProductComponent productComponent = _productService.GetProductComponentsByCode(cmdr.ProductCode);
                            if (productComponent.OwnProduct == true)
                            {
                                _context.Database.ExecuteSqlRaw("EXEC [setOwnProductInMarket] " + customMarketCode.ToString() + "," + (cmdr.OwnProduct == true) + "," + cmdr.ProductCode.ToString());
                            }
                        }

                        _customMarketDetails.Add(customMarketDetail);
                    }
                    _customMarket.CustomMarketDetails = _customMarketDetails;
                }
            }

            _context.Entry(_customMarket).State = EntityState.Modified;
            _loggerService.LogMessage($"El usuario {UserLogged.userLogged.Usuario} modificó el mercado {_customMarket.Description}", UserLogged.userLogged.Usuario, _context, _customMarket.Code);
            _context.SaveChanges();

            return this.GetByCode(_customMarket.Code);
        }

        public Object Clone(int customMarketCode, CustomMarketRequest customMarket)
        {
            CustomMarket _customMarket = _context.CustomMarkets
                .Include(customMarket => customMarket.CustomMarketDetails)
                .SingleOrDefault(x => x.Code == customMarketCode);

            if (_customMarket == null) return null;

            CustomMarket _copyCustomMarket = this.Create(customMarket);

            if (_customMarket.CustomMarketDetails != null)
            {

                List<int[]> arrayMapCustomMarketGroup = new List<int[]>();

                foreach (CustomMarketGroup cmg in _customMarketGroupService.GetAllByCustomMarket(_customMarket.Code))
                {
                    CustomMarketGroupRequest cmgr = new CustomMarketGroupRequest();
                    cmgr.Description = cmg.Description;
                    cmgr.GroupCondition = cmg.GroupCondition;
                    cmgr.CustomMarketCode = _copyCustomMarket.Code;
                    CustomMarketGroup copyCmg = _customMarketGroupService.Create(cmgr);

                    int[] values = new int[2];
                    values[0] = cmg.Code;
                    values[1] = copyCmg.Code;
                    arrayMapCustomMarketGroup.Add(values);
                }

                if (_customMarket.CustomMarketDetails.Any())
                {
                    List<CustomMarketDetail> _customMarketDetails = new List<CustomMarketDetail>();
                    foreach (CustomMarketDetail cmdr in _customMarket.CustomMarketDetails)
                    {

                        CustomMarketDetail customMarketDetail = new CustomMarketDetail();

                        if (cmdr.CustomMarketGroupCode != null)
                        {
                            for (int i = 0; i < arrayMapCustomMarketGroup.Count; ++i)
                            {
                                if (arrayMapCustomMarketGroup[i][0] == cmdr.CustomMarketGroupCode)
                                {
                                    customMarketDetail.CustomMarketGroupCode = arrayMapCustomMarketGroup[i][1];
                                }
                            }
                        }

                        customMarketDetail.DetailCustomMarketCode = cmdr.DetailCustomMarketCode;
                        customMarketDetail.DrugCode = cmdr.DrugCode;
                        customMarketDetail.DrugGroupCode = cmdr.DrugGroupCode;
                        customMarketDetail.EnsureVisible = cmdr.EnsureVisible;
                        customMarketDetail.Expand = cmdr.Expand;
                        customMarketDetail.Graphs = cmdr.Graphs;
                        customMarketDetail.Intemodifier = cmdr.Intemodifier;
                        customMarketDetail.ItemCondition = cmdr.ItemCondition;
                        customMarketDetail.LaboratoryCode = cmdr.LaboratoryCode;
                        customMarketDetail.LaboratoryGroupCode = cmdr.LaboratoryGroupCode;
                        customMarketDetail.Modifier = cmdr.Modifier;
                        customMarketDetail.Order = cmdr.Order;
                        customMarketDetail.Pattern = cmdr.Pattern;
                        customMarketDetail.PharmaceuticalFormCode = cmdr.PharmaceuticalFormCode;
                        customMarketDetail.ProductCode = cmdr.ProductCode;
                        customMarketDetail.ProductGroupCode = cmdr.ProductGroupCode;
                        customMarketDetail.ProductPresentationCode = cmdr.ProductPresentationCode;
                        customMarketDetail.ProductPresentationGroupCode = cmdr.ProductPresentationGroupCode;
                        customMarketDetail.ProductTypeCode = cmdr.ProductTypeCode;
                        customMarketDetail.RegExPattern = cmdr.RegExPattern;
                        customMarketDetail.Resume = cmdr.Resume;
                        customMarketDetail.TherapeuticalClassCode = cmdr.TherapeuticalClassCode;

                        /* 
                            customMarketDetail.OwnProductsReport = productComponent.OwnProduct;
                        */

                        _customMarketDetails.Add(customMarketDetail);
                    }
                    _copyCustomMarket.CustomMarketDetails = _customMarketDetails;
                }

                var _productMarkets = _context.ProductMarkets
                    .Where(pc => pc.MarketCode == customMarketCode)
                    .ToList();

                if (_productMarkets.Any())
                {
                    foreach (ProductMarket pm in _productMarkets)
                    {
                        _context.Database.ExecuteSqlRaw("EXEC [setOwnProductInMarket] " + _copyCustomMarket.Code.ToString() + ",1," + pm.ProductCode.ToString());
                    }
                }
            }

            _context.Entry(_copyCustomMarket).State = EntityState.Modified;
            _loggerService.LogMessage($"El usuario {UserLogged.userLogged.Usuario} clonó el mercado de origen {_customMarket.Description} en el mercado {_copyCustomMarket.Description}", UserLogged.userLogged.Usuario, _context, _copyCustomMarket.Code);
            _context.SaveChanges();

            return this.GetByCode(_copyCustomMarket.Code);
        }

        public Object Delete(int customMarketCode)
        {
            CustomMarket _customMarket = _context.CustomMarkets
                .Include(customMarket => customMarket.CustomMarketDetails)
                .SingleOrDefault(x => x.Code == customMarketCode);

            if (_customMarket == null) return null;

            List<String> cms = this.getCustomMarketsByMarketReference(_customMarket.Code);
            if (cms.Any())
            {
                throw new Exception("is_not_valid_delete_custom_market," + String.Join(" - ", cms.ToArray()));
            }

            if (_customMarket.CustomMarketDetails.Any())
            {
                foreach (CustomMarketDetail cmd in _customMarket.CustomMarketDetails)
                {
                    _context.CustomMarketDetails.Remove(cmd);
                }
            }

            List<CustomMarketGroup> cmgs = _context.CustomMarketGroups.Where(d => d.CustomMarketCode == _customMarket.Code).ToList();
            if (cmgs.Any())
            {
                foreach (CustomMarketGroup cmg in cmgs)
                {
                    _context.CustomMarketGroups.Remove(cmg);
                }
            }
            _context.Database.ExecuteSqlRaw($"DELETE customMarketPharmaceuticalForm WHERE customMarketCode = {customMarketCode}");



            var _productMarkets = _context.ProductMarkets
                .Where(pc => pc.MarketCode == customMarketCode)
                .ToList();

            if (_productMarkets.Any())
            {
                foreach (ProductMarket pm in _productMarkets)
                {
                    _context.Database.ExecuteSqlRaw("EXEC [setOwnProductInMarket] " + customMarketCode.ToString() + ",0," + pm.ProductCode.ToString());
                }
            }
            _context.CustomMarkets.Remove(_customMarket);
            _loggerService.LogMessage($"El usuario {UserLogged.userLogged.Usuario}  borró el mercado", UserLogged.userLogged.Usuario, _context, customMarketCode);
            return _context.SaveChanges();
        }

        public IEnumerable<Object> GetPreviewByCode(int customMarketCode)
        {

            return _context.CustomMarketPreviews
                .FromSqlRaw("EXEC [CustomMarketPreviewGet] " + customMarketCode.ToString())
                .ToList();
        }

        public async Task<MarketDetailResult> GetMarketDetailHistoricJsonAsync(int customMarketCode)
        {
            try
            {
                var parameter = new Microsoft.Data.SqlClient.SqlParameter("@customMarketCode", customMarketCode);

                var jsonResult = await _context.Set<JsonResult>()
                    .FromSqlRaw("SELECT dbo.fn_GetMarketDetailHistoric_JSON(@customMarketCode) AS JsonData", parameter)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (jsonResult == null || string.IsNullOrEmpty(jsonResult.JsonData))
                {
                    throw new Exception("No data returned from the SQL function.");
                }

                var marketDetailResult = JsonSerializer.Deserialize<MarketDetailResult>(jsonResult.JsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return marketDetailResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CustomMarketVersionHistoricModel>> GetMarketDetailHistoricByCustomMarketCode(int customMarketCode)
        {
            try
            {
                var result = _context.CustomMarketVersionHistorics.Where(e => e.CustomMarketCode == customMarketCode)
                               .Include(e => e.SignedUserNavigation)
                               .Select(x => new CustomMarketVersionHistoricModel
                               {
                                   Code = x.Code,
                                   VersionDate = x.VersionDate,
                                   Name = x.SignedUserNavigation.Usuario
                                   

                               })
                               .ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving custom market details.", ex);
            }
        }
        public async Task<List<CustomMarketVersionHistoricModel>> GetMarketDetailLastSignByCustomMarketCode(int customMarketCode)
        {
            try
            {
                var result = _context.CustomMarketActualDefinitions.Where(e => e.CustomMarketCode == customMarketCode)
                               .Select(x => new CustomMarketVersionHistoricModel
                               {
                                   Code = x.CustomMarketCode,
                                   VersionDate = x.VersionDate

                               })
                               .ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving custom market details.", ex);
            }
        }


        public async Task<List<ProductDetail>> GetLastSignedPreviewHistoricByCustomMarketCode(int code)
        {
            var productDetails = new List<ProductDetail>();

            try
            {
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("[dbo].[CustomMarketPreviewGetLastSigned]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@customMarketCode", code);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var productDetail = new ProductDetail
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    ProductCode = reader.GetInt32(reader.GetOrdinal("ProductCode")),
                                    Modifier = (decimal)reader.GetDouble(reader.GetOrdinal("Modifier")),
                                    INTEmodifier = (decimal)reader.GetDouble(reader.GetOrdinal("INTEmodifier")),
                                    OwnProduct = reader.GetBoolean(reader.GetOrdinal("OwnProduct")),
                                    EntityDesc = reader.GetString(reader.GetOrdinal("EntityDesc")),
                                    EntityID = reader.GetInt32(reader.GetOrdinal("EntityID")),
                                    ProductPresentationDescription = reader.GetString(reader.GetOrdinal("ProductPresentationDescription")),
                                    ProductPresentationCode = reader.GetInt32(reader.GetOrdinal("ProductPresentationCode")),
                                    ProductDescription = reader.GetString(reader.GetOrdinal("ProductDescription")),
                                    LabDescription = reader.GetString(reader.GetOrdinal("LabDescription")),
                                    LabCode = reader.GetInt32(reader.GetOrdinal("LabCode")),
                                    TCDescription = reader.GetString(reader.GetOrdinal("TCDescription")),
                                    TCCode = reader.GetString(reader.GetOrdinal("TCCode")),
                                    ClassCode = reader.IsDBNull(reader.GetOrdinal("ClassCode")) == true ? 0 : reader.GetInt32(reader.GetOrdinal("ClassCode")) ,
                                    PFCode = reader.IsDBNull(reader.GetOrdinal("PFCode")) == true ? null : reader.GetString(reader.GetOrdinal("PFCode")),
                                    PFDescription = reader.IsDBNull(reader.GetOrdinal("PFDescription")) == true ? null : reader.GetString(reader.GetOrdinal("PFDescription")),
                                    Pattern = reader.GetString(reader.GetOrdinal("Pattern")),
                                    EnsureVisible = reader.GetInt32(reader.GetOrdinal("EnsureVisible")),
                                    Resume = reader.GetInt32(reader.GetOrdinal("Resume")),
                                    Expand = reader.GetInt32(reader.GetOrdinal("Expand")),
                                    Graph = reader.GetInt32(reader.GetOrdinal("Graph")),
                                    ItemCondition = reader.GetInt32(reader.GetOrdinal("ItemCondition")),
                                    Orden = reader.GetInt32(reader.GetOrdinal("Orden")),
                                    Drug = reader.GetString(reader.GetOrdinal("Drug")),
                                    BusinessUnit = reader.GetString(reader.GetOrdinal("BusinessUnit"))
                                };

                                productDetails.Add(productDetail);
                            }
                        }
                    }

                    await connection.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving custom market details.", ex);
            }

            return productDetails;
        }
        public async Task<List<ProductDetail>> GetMarketPreviewHistoricByCustomMarketCode(int versionCode)
        {
            var productDetails = new List<ProductDetail>();

            try
            {
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("[dbo].[CustomMarketPreviewGetHistoric]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@versionCode", versionCode);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var productDetail = new ProductDetail
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    ProductCode = reader.GetInt32(reader.GetOrdinal("ProductCode")),
                                    Modifier = (decimal)reader.GetDouble(reader.GetOrdinal("Modifier")),
                                    INTEmodifier = (decimal)reader.GetDouble(reader.GetOrdinal("INTEmodifier")),
                                    OwnProduct = reader.GetBoolean(reader.GetOrdinal("OwnProduct")),
                                    EntityDesc = reader.GetString(reader.GetOrdinal("EntityDesc")),
                                    EntityID = reader.GetInt32(reader.GetOrdinal("EntityID")),
                                    ProductPresentationDescription = reader.GetString(reader.GetOrdinal("ProductPresentationDescription")),
                                    ProductPresentationCode = reader.GetInt32(reader.GetOrdinal("ProductPresentationCode")),
                                    ProductDescription = reader.GetString(reader.GetOrdinal("ProductDescription")),
                                    LabDescription = reader.GetString(reader.GetOrdinal("LabDescription")),
                                    LabCode = reader.GetInt32(reader.GetOrdinal("LabCode")),
                                    TCDescription = reader.GetString(reader.GetOrdinal("TCDescription")),
                                    TCCode = reader.GetString(reader.GetOrdinal("TCCode")),
                                    ClassCode = reader.GetInt32(reader.GetOrdinal("ClassCode")),
                                    PFCode = reader.GetString(reader.GetOrdinal("PFCode")),
                                    PFDescription = reader.GetString(reader.GetOrdinal("PFDescription")),
                                    Pattern = reader.GetString(reader.GetOrdinal("Pattern")),
                                    EnsureVisible = reader.GetInt32(reader.GetOrdinal("EnsureVisible")),
                                    Resume = reader.GetInt32(reader.GetOrdinal("Resume")),
                                    Expand = reader.GetInt32(reader.GetOrdinal("Expand")),
                                    Graph = reader.GetInt32(reader.GetOrdinal("Graph")),
                                    ItemCondition = reader.GetInt32(reader.GetOrdinal("ItemCondition")),
                                    Orden = reader.GetInt32(reader.GetOrdinal("Orden")),
                                    Drug = reader.GetString(reader.GetOrdinal("Drug")),
                                    BusinessUnit = reader.GetString(reader.GetOrdinal("BusinessUnit"))
                                };

                                productDetails.Add(productDetail);
                            }
                        }
                    }

                    await connection.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving custom market details.", ex);
            }

            return productDetails;
        }



        private bool hasCustomMarketAssignDetailCustomMarket(int? customMarketCode)
        {
            var results = _context.CustomMarketDetails
                .Where(d => d.CustomMarketCode == customMarketCode && d.DetailCustomMarketCode != null)
                .Count();

            return results > 0;
        }

        private List<String> getCustomMarketsByMarketReference(int customMarketCode)
        {
            return _context.CustomMarkets
                .Where(d => d.MarketReference == customMarketCode)
                .OrderBy(x => x.Description)
                .Select(d => d.Description)
                .ToList();
        }

        public IEnumerable<Object> GetTree()
        {
            var customerMarkertTree = _context.CustomMarketTrees.ToList();
            return this.GetTreeSerialize(customerMarkertTree);
        }

        public IEnumerable<Object> GetTree(User user)
        {
            UserPermissionResponse userPermissions = _userPermissionService.GetAllByUser(user);

            var customerMarkertTree = _context.CustomMarketTrees
                .Where(cmt =>
                    (userPermissions.FullAccess == true) ||
                    (userPermissions.CustomMarketCodes.Count > 0 && userPermissions.CustomMarketCodes.Contains((int)cmt.CustomMarketCode)) ||
                    (userPermissions.LineCodes.Count > 0 && (userPermissions.LineCodes.Contains((int)cmt.LineCode))) ||
                    (userPermissions.LineGroupCodes.Count > 0 && userPermissions.LineGroupCodes.Contains((int)cmt.LineGroupCode)) ||
                    (cmt.CustomMarketTest == true)
                )
                .ToList();

            return this.GetTreeSerialize(customerMarkertTree);
        }

        private IEnumerable<Object> GetTreeSerialize(IEnumerable<CustomMarketTree> customMarketTree)
        {
            //return customMarketTree
            //    .OrderBy(x => x.LineGroupDescription)
            //    .GroupBy(x => new { x.LineGroupCode, x.LineGroupDescription })
            //    .Select(g => new
            //    {
            //        LineGroupCode = g.Key.LineGroupCode,
            //        LineGroupDescription = g.Key.LineGroupDescription,
            //        Lines = g
            //            .OrderBy(x => x.LineDescription)
            //            .GroupBy(x => new { x.LineCode, x.LineDescription })
            //            .Select(l => new
            //            {
            //                LineCode = l.Key.LineCode,
            //                LineDescription = l.Key.LineDescription,
            //                CustomMarkets = l
            //                    .OrderBy(cm => cm.CustomMarketDescription)
            //                        .ThenBy(cm => cm.CustomMarketDescription)
            //                    .Select(cm => new
            //                    {
            //                        CustomMarketCode = cm.CustomMarketCode,
            //                        customMarketDescription = cm.CustomMarketDescription,
            //                        CustomMarketOrder = cm.CustomMarketOrder,
            //                        CustomMarketTest = cm.CustomMarketTest
            //                    })
            //                    .ToList()
            //            })
            //    });

            var result = from cm in customMarketTree
                         join cmad in _context.CustomMarketActualDefinitions
                         on cm.CustomMarketCode equals cmad.CustomMarketCode into cmadGroup
                         from cmad in cmadGroup.DefaultIfEmpty()
                         select new
                         {
                             LineGroupCode = cm.LineGroupCode,
                             LineGroupDescription = cm.LineGroupDescription,
                             LineCode = cm.LineCode,
                             LineDescription = cm.LineDescription,
                             CustomMarketCode = cm.CustomMarketCode,
                             CustomMarketDescription = cm.CustomMarketDescription,
                             //CustomMarketOrder = cm.CustomMarketOrder,
                             CustomMarketTest = cm.CustomMarketTest,
                             Signed = cmad != null && cmad.SignedUser != null
                         };

            return result
                .OrderBy(x => x.LineGroupDescription)
                .GroupBy(x => new { x.LineGroupCode, x.LineGroupDescription })
                .Select(g => new
                {
                    LineGroupCode = g.Key.LineGroupCode,
                    LineGroupDescription = g.Key.LineGroupDescription,
                    Lines = g
                        .OrderBy(x => x.LineDescription)
                        .GroupBy(x => new { x.LineCode, x.LineDescription })
                        .Select(l => new
                        {
                            LineCode = l.Key.LineCode,
                            LineDescription = l.Key.LineDescription,
                            CustomMarkets = l
                                .OrderBy(cm => cm.CustomMarketDescription)
                                .ThenBy(cm => cm.CustomMarketDescription)
                                .Select(cm => new
                                {
                                    CustomMarketCode = cm.CustomMarketCode,
                                    CustomMarketDescription = cm.CustomMarketDescription,
                                    //CustomMarketOrder = cm.CustomMarketOrder,
                                    CustomMarketTest = cm.CustomMarketTest,
                                    Signed = cm.Signed
                                })
                                .ToList()
                        })
                        .ToList()
                });

        }
    }
}