namespace WebApi.Models.Token
{
    public class TokenInfo
    {
        public string AMSSessionID { get; set; }
        public int UserID { get; set; }
        public string LoginName { get; set; }
        public string StaffName { get; set; }
        public string StaffEmail { get; set; }
        public string CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public string UserRoles { get; set; }
        public string UserRights { get; set; }
    }
}
