namespace WebApi.Helpers
{
    /// <summary>
    /// stand error response class
    /// </summary>
    public class ErrorResponse
    {
        //standard error response class for consitent respnse for evey request
        public string errormessage { get; set; } = "";

        public string errorstack { get; set; } = "";
    }
}
