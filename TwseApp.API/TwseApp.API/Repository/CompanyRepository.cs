using AutoMapper;
using System.Net.Http;
using TwseApp.API.Data;
using TwseApp.API.Entities;
using TwseApp.API.Service;

namespace TwseApp.API.Repository
{

    #region Interface

    public interface ICompanyRepository
    {
        /// <summary>
        /// 根據分公司代號取得母公司資訊
        /// </summary>
        /// <param name="branchCode"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Headquarters GetCorrespondHeadQuarterByBranchCode(string branchCode);

        /// <summary>
        /// 刪除 母公司及子公司 By Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        bool DeleteAllHeadquarterAndBranchBelowByCode(string code);
    }

    #endregion

    public class CompanyRepository : ICompanyRepository
    {
        #region Fields

        private readonly TwseDbContext Context;
        private readonly ILogger<CompanyRepository> Logger;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public CompanyRepository
        (
            TwseDbContext context,
            ILogger<CompanyRepository> logger,
            IMapper mapper
        )
        {
            _mapper = mapper;
            Context = context;
            Logger = logger;
        }


        #endregion

        #region Repository

        /// <summary>
        /// 根據分公司代號取得母公司資訊
        /// </summary>
        /// <param name="branchCode"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Headquarters GetCorrespondHeadQuarterByBranchCode(string branchCode) {

            Headquarters correspondHeadQuarter = new();
            try
            {
                correspondHeadQuarter = Context.Headquarters.Where(x => x.Id.Substring(0, 3) == branchCode).FirstOrDefault();
                if (correspondHeadQuarter == null) {
                    Console.Write(correspondHeadQuarter);
                }
            }
            catch (Exception ex) {
                Logger.LogError(ex.StackTrace);
                Logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
            return correspondHeadQuarter;
        }

        /// <summary>
        /// 取得目前所有母公司
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Headquarters> GetAllHeadquarters(int pageIndex) 
        {
            List<Headquarters> response = new();
            try {

                response = Context.Headquarters.ToList();

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
        /// 取得目前所有子公司
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Branch> GetAllChildCompany(int pageIndex)
        {
            List<Branch> response = new();
            try
            {
                response = Context.Branches.ToList();
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
        /// 刪除 母公司及子公司 By Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool DeleteAllHeadquarterAndBranchBelowByCode(string code) 
        {
            bool result = false;
            try
            {
                List<Branch> deleteBranchCompanies = new();
                List<Headquarters> deleteHeadquarters = new();

                deleteBranchCompanies = Context.Branches.AsQueryable().Where(x => x.Code.Substring(0,3) == code).ToList();
                deleteHeadquarters = Context.Headquarters.AsQueryable().Where(x => x.Id.Substring(0,3) == code).ToList();

                foreach ( Branch company in deleteBranchCompanies ) {
                    Context.Branches.Remove(company);
                }

                foreach ( Headquarters headquarter in deleteHeadquarters ) { 
                    Context.Headquarters.Remove(headquarter);
                }

                Context.SaveChanges();
                result = true;
            }
            catch (Exception ex) {
                Logger.LogError(ex.StackTrace);
                Logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
            return result;
        }

        #endregion
    }
}
