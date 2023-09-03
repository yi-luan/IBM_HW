using AutoMapper;
using System.Collections.Generic;
using System.Text.Json;
using TwseApp.API.Data;
using TwseApp.API.Dto;
using TwseApp.API.Entities;
using TwseApp.API.Repository;

namespace TwseApp.API.Service
{
    #region Interface

    public interface IBranchCompanyService
    {
        /// <summary>
        /// 取得 外部子公司資料API 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<List<BranchCompanyData>> GetAllBranchCompanyDataFromRemote();

        /// <summary>
        /// 匯入 子公司資料 至 DB
        /// </summary>
        /// <param name="Companies">公司列表</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        List<Branch> InsertAllBranchCompanyData(List<BranchCompanyData> branchCompanies);

        /// <summary>
        /// 依據條件 搜尋所有子公司
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        List<Branch> FindAllBranchesByCondition(QueryParentCompanyListRequest request);
    }

    #endregion

    public class BranchCompanyService : IBranchCompanyService
    {
        #region Fields 

        private readonly HttpClient httpClient;
        private readonly TwseDbContext Context;
        private readonly ILogger<BranchCompanyService> Logger;
        private readonly IMapper _mapper;
        private readonly ICompanyRepository CompanyRepository;

        #endregion

        #region Constructor

        public BranchCompanyService(
            TwseDbContext context,
            ILogger<BranchCompanyService> logger,
            IMapper mapper,
            ICompanyRepository companyRepository
        )
        {
            _mapper = mapper;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://openapi.twse.com.tw/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            Context = context;
            CompanyRepository = companyRepository;
            Logger = logger;
        }

        #endregion

        #region Service

        /// <summary>
        /// 取得 外部子公司資料API 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<BranchCompanyData>> GetAllBranchCompanyDataFromRemote()
        {

            List<BranchCompanyData> branchCompanyDataResponse  = new();

            try
            {
                string httpResponseContent = string.Empty;

                HttpResponseMessage httpResponse = await httpClient.GetAsync("https://openapi.twse.com.tw/v1/opendata/OpenData_BRK02");
                if (httpResponse.IsSuccessStatusCode)
                {
                    httpResponseContent = await httpResponse.Content.ReadAsStringAsync();
                    branchCompanyDataResponse = JsonSerializer.Deserialize<List<BranchCompanyData>>(httpResponseContent);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.StackTrace);
                Logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }

            return branchCompanyDataResponse;
        }

        /// <summary>
        /// 匯入 子公司資料 至 DB
        /// </summary>
        /// <param name="Companies">公司列表</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Branch> InsertAllBranchCompanyData(List<BranchCompanyData> branchCompanies)
        {
            List<Branch> branchList = new();
            try
            {
                Context.Branches.RemoveRange(Context.Branches);

                foreach (BranchCompanyData branchCompany in branchCompanies)
                {
                    Branch branch = new()
                    {
                        SecuritiesFirmName = branchCompany.證券商名稱,
                        DateOfPublication = branchCompany.出表日期,
                        Address = branchCompany.地址,
                        Telephone = branchCompany.電話,
                        SecuritiesFirmCode = branchCompany.證券商代號,
                        OpeningDate = branchCompany.開業日
                    };

                    Headquarters correspondHeadQuarterCompany = new();
                    correspondHeadQuarterCompany = CompanyRepository.GetCorrespondHeadQuarterByBranchCode(branch.SecuritiesFirmCode.Substring(0, 3));

                    if (correspondHeadQuarterCompany != null) {
                        branch.Code = correspondHeadQuarterCompany.Id;
                        branchList.Add(branch);
                        Context.Branches.Add(branch);
                    }
                }

                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.StackTrace);
                Logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
            return branchList;
        }

        /// <summary>
        /// 依據條件 搜尋所有子公司
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Branch> FindAllBranchesByCondition(QueryParentCompanyListRequest request)
        {
            List<Branch> branchList = new();
            try
            {
                branchList = Context.Branches.AsEnumerable().ToList();

                if (!(string.IsNullOrEmpty(request.Start_date) && string.IsNullOrEmpty(request.End_date)))
                {
                    DateTime startDate = DateTime.Parse(request.Start_date);
                    DateTime endDate = DateTime.Parse(request.End_date);
                    branchList = branchList.Where(c => TransformIntoDateTime(c.OpeningDate) > startDate && TransformIntoDateTime(c.OpeningDate) < endDate).ToList();
                }

                if (!string.IsNullOrEmpty(request.Search_code)) branchList = branchList.Where(x => x.SecuritiesFirmCode.Substring(0,3).Contains(request.Search_code)).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.StackTrace);
                Logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
            return branchList;
        }

        #endregion

        #region 其餘方法

        private DateTime TransformIntoDateTime(string tw_date)
        {

            DateTime response = new();

            if (tw_date.Contains("/"))
            {
                string[] splitDateString = tw_date.Split("/");
                int formattedYear = int.Parse(splitDateString[0]) + 1911;
                string reconstructString = formattedYear + "/" + splitDateString[1] + "/" + splitDateString[2];

                response = DateTime.Parse(reconstructString);
            }
            else
            {
                string years = tw_date.Substring(0, 3);
                string month = tw_date.Substring(3, 2);
                string day = tw_date.Substring(5, 2);
                int formattedYear = int.Parse(years) + 1911;
                string reconstructString = formattedYear + "/" + month + "/" + day;

                response = DateTime.Parse(reconstructString);
            }
            return response;
        }

        #endregion
    }
}
