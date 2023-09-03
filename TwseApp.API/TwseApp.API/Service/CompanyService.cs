using AutoMapper;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text.Json;
using TwseApp.API.Data;
using TwseApp.API.Dto;
using TwseApp.API.Entities;
using TwseApp.API.Repository;

namespace TwseApp.API.Service
{

    #region Interface

    public interface ICompanyService
    {

        /// <summary>
        /// 取得 外部母公司資料API 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<List<CompanyData>> GetAllMainCompanyDataFromRemote();

        /// <summary>
        /// 匯入 母公司資料 至 DB
        /// </summary>
        /// <param name="Companies">公司列表</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        List<Headquarters> InsertAllMainCompanyData(List<CompanyData> Companies);

        /// <summary>
        /// 依據條件 搜尋所有母公司
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        List<Headquarters> FindAllHeadquartersByCondition(QueryParentCompanyListRequest request);

        /// <summary>
        /// 依據 Code 刪除符合條件母公司及其子公司
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        bool DeleteAllCompanyByCode(string code);
    }

    #endregion


    public class CompanyService : ICompanyService
    {

        #region Fields 

        private readonly HttpClient httpClient;
        private readonly TwseDbContext Context;
        private readonly ICompanyRepository CompanyRepository;
        private readonly ILogger<CompanyService> Logger;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public CompanyService(
             TwseDbContext context,
             ILogger<CompanyService> logger,
             IMapper mapper,
             ICompanyRepository companyRepository
        )
        {
            _mapper = mapper;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://openapi.twse.com.tw/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            CompanyRepository = companyRepository;
            Context = context;
            Logger = logger;
        }

        #endregion

        #region Company Service

        /// <summary>
        /// 取得 外部母公司資料API 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<CompanyData>> GetAllMainCompanyDataFromRemote()
        {
            List<CompanyData> response = new();
            try
            {
                string httpResponseContent = string.Empty;

                HttpResponseMessage httpResponse = await httpClient.GetAsync("https://openapi.twse.com.tw/v1/brokerService/brokerList");
                if (httpResponse.IsSuccessStatusCode)
                {
                    httpResponseContent = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonSerializer.Deserialize<List<CompanyData>>(httpResponseContent);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.StackTrace);
                Logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
            return response;
        }

        /// <summary>
        /// 匯入 母公司資料 至 DB
        /// </summary>
        /// <param name="Companies">公司列表</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Headquarters> InsertAllMainCompanyData(List<CompanyData> Companies)
        {
            List<Headquarters> headquarterList = new();
            try
            {
                Context.Headquarters.RemoveRange(Context.Headquarters);

                foreach (CompanyData company in Companies)
                {
                    Headquarters headquarters = _mapper.Map<Headquarters>(company);
                    Context.Headquarters.Add(headquarters);
                    headquarterList.Add(headquarters);
                }

                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.StackTrace);
                Logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
            return headquarterList;
        }

        /// <summary>
        /// 依據條件 搜尋所有母公司
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Headquarters> FindAllHeadquartersByCondition(QueryParentCompanyListRequest request)
        {
            List<Headquarters> headquarterList = new();
            try
            {
                headquarterList = Context.Headquarters.AsEnumerable().ToList();

                if (!(string.IsNullOrEmpty(request.Start_date) && string.IsNullOrEmpty(request.End_date))) {
                    DateTime startDate = DateTime.Parse(request.Start_date);
                    DateTime endDate = DateTime.Parse(request.End_date);
                    headquarterList = headquarterList.Where(c => TransformIntoDateTime(c.EstablishmentDate) > startDate && TransformIntoDateTime(c.EstablishmentDate) < endDate).ToList();
                }

                if (!string.IsNullOrEmpty(request.Search_code)) headquarterList = headquarterList.Where(x => x.Id.Contains(request.Search_code)).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.StackTrace);
                Logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
            return headquarterList;
        }

        /// <summary>
        /// 依據 Code 刪除符合條件母公司及其子公司
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool DeleteAllCompanyByCode(string code) 
        {
            bool result = false;
            try
            {
                if (code == "All") { 
                    Context.Headquarters.RemoveRange(Context.Headquarters);
                    Context.Branches.RemoveRange(Context.Branches);
                }

                result = CompanyRepository.DeleteAllHeadquarterAndBranchBelowByCode(code);
                Context.SaveChanges();
                result = true;
            }
            catch (Exception ex) {
                Logger.LogError(ex.StackTrace);
                Logger.LogError(ex.Message);
                throw new Exception();
            }
            return result;
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
            else {
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
