using TwseApp.API.Entities;

namespace TwseApp.API.Dto
{
    public class QueryBranchCompanyListResponse
    {
        public List<Branch> branchCompanies { get; set; }

        public int totalNumber { get; set; }
    }
}
