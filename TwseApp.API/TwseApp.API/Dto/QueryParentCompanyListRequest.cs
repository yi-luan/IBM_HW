namespace TwseApp.API.Dto
{
    public class QueryParentCompanyListRequest
    {
        public string Start_date { get; set; }

        public string End_date { get; set;}

        public string Search_code { get; set; }

        public int Current_page { get; set; }
    }
}
