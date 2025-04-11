namespace BugTracker.Application.Errors
{
    public class Error
    {
        public string Code { get; set; }
        public string Descriptin { get; set; }
        public int statusCode { get; set; }

        public Error(string code, string desc, int status)
        {
            Code = code;
            Descriptin = desc;
            statusCode = status;
        }
    }
}
