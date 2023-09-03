namespace TwseApp.API.Entities
{
    public class Branch
    {
        public int Id { get; set; }
        public string DateOfPublication { get; set; }
        public string SecuritiesFirmCode { get; set; }
        public string SecuritiesFirmName { get; set; }
        public string OpeningDate { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Code { get; set; }

        public Headquarters Headquarters { get; set; }
    }
}
