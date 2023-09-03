using TwseApp.API.Entities;

namespace TwseApp.API.Dto
{
    public class QueryParentCompanyListResponse
    {
        public List<Headquarters> headquarters { get; set; }

        public int totalNumber { get; set;}

    }
}
