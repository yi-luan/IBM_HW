using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography.Xml;
using TwseApp.API.Data;
using TwseApp.API.Dto;
using TwseApp.API.Entities;
using TwseApp.API.Models;
using TwseApp.API.Models.Share;
using TwseApp.API.Service;

namespace TwseApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwseController : ControllerBase
    {
        #region Fields

        private readonly TwseDbContext _context;
        private readonly ICompanyService _companyService;
        private readonly ILogger<TwseController> _logger;
        private readonly IBranchCompanyService _branchCompanyService;

        #endregion

        #region Constructor

        public TwseController(
            TwseDbContext context, 
            ICompanyService companyService,
            IBranchCompanyService branchCompanyService,
            ILogger<TwseController> logger
        )
        {
            _logger = logger;
            _context = context;
            _companyService = companyService;
            _branchCompanyService = branchCompanyService;
        }

        #endregion

        #region TWSE API 

        [HttpPost("CheckIfDatabaseHaveBeenInit")]
        public async Task<IActionResult> CheckIfDatabaseHaveBeenInit()
        {
            ResponseModel<object> response = new();
            try
            {
                List<Headquarters> headquarters = _context.Headquarters.AsQueryable().ToList();
                if (headquarters.Count > 0)
                {
                    response.Status = true;
                    response.Message = "資料已載入";
                }
                else {
                    response.Status = false;
                    response.Message = "資料尚未載入";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }

            return Ok(response);
        }

        [HttpPost("InitializeTwseCompanyData")]
        public async Task<IActionResult> InitializeTwseCompanyData() {

            ResponseModel<object> response = new();
            try
            {
                List<CompanyData> companyHeadQuaters = new();
                companyHeadQuaters = await _companyService.GetAllMainCompanyDataFromRemote();
                List<Headquarters> headQuarters = _companyService.InsertAllMainCompanyData(companyHeadQuaters);

                List<BranchCompanyData> branchCompanies = new();
                branchCompanies = await _branchCompanyService.GetAllBranchCompanyDataFromRemote();
                List<Branch> branches = _branchCompanyService.InsertAllBranchCompanyData(branchCompanies);

                response.Status = true;
            }
            catch (Exception ex) {
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }

            return Ok(response);
        }


        [HttpPost("QueryAllParentsBrokerList")]
        public IActionResult QueryAllParentsBrokerList(QueryParentCompanyListRequest requestData)
        {
            QueryParentCompanyListResponse response = new();
            int pageIndex = requestData.Current_page;

            try
            {
                List<Headquarters> headquarterList = new();
                headquarterList = _companyService.FindAllHeadquartersByCondition(requestData);
                response.totalNumber = headquarterList.Count;
                if( headquarterList.Count > 10 ) headquarterList = headquarterList.Skip(pageIndex * 10).Take(10).ToList();
                response.headquarters = headquarterList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }

            return Ok(response);
        }


        [HttpPost("QueryAllChildBrokerList")]
        public IActionResult QueryAllChildBrokerList(QueryBranchCompanyListRequest requestData)
        {
            QueryBranchCompanyListResponse response = new();
            int pageIndex = requestData.Current_page;

            try
            {
                List<Branch> branchCompanyList = new();
                branchCompanyList = _branchCompanyService.FindAllBranchesByCondition(requestData);
                response.totalNumber = branchCompanyList.Count;
                if (branchCompanyList.Count > 10) branchCompanyList = branchCompanyList.Skip(pageIndex * 10).Take(10).ToList();
                response.branchCompanies = branchCompanyList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }

            return Ok(response);
        }

        [HttpPost("DeleteBrokerList")]
        public IActionResult DeleteBrokerList(DeleteCompaniesRequest deleteRequest)
        {
            ResponseModel<object> response = new();
            string deleteCode = deleteRequest.CompaniesCode;

            try
            {
                response.Status = _companyService.DeleteAllCompanyByCode(deleteCode);
                response.Message = "刪除完成";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }

            return Ok(response);
        } 

        #endregion



    }
}
