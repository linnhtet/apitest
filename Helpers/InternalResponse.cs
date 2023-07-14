using System.Net;

namespace WebApi.Helpers
{
    /// <summary>
    /// this class will be use in communication between controller and service layer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InternalResponse<T>
    {
        public bool status { get; set; }
        public string message { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public T Value { get; set; }
    }
}
