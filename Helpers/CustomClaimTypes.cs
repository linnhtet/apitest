
namespace WebApi.Helpers
{
    //short helper class to create custsom claim types
    public static class CustomClaimTypes
    {
        public const string AMSSessionID = "http://schemas.xmlsoap.org/ws/2014/03/ams/claims/amssessionid";
        public const string AMSRandomValue = "http://schemas.xmlsoap.org/ws/2014/03/ams/claims/amsrandomvalue";
        public const string CompanyID = "CompanyID";
        public const string CompanyCode = "CompanyCode";
        public const string LoginName = "LoginName";
        public const string StaffName = "StaffName";
        public const string StaffEmail = "StaffEmail";
        public const string UserRoles = "UserRoles";
        public const string UserRights = "UserRights";
    }
}