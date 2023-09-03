using TwseApp.API.Entities;

namespace TwseApp.API.Dto
{
    public class BranchAndHeadQuartersList
    {
        public List<Branch> BranchList { get; set; }
        public List<Headquarters> HeadquarterList { get; set; }

        public int BranchTotalCount { get; set; }

        public int HeaderquartersTotalCount { get; set; }
    }
}
