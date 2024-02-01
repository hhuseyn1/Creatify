namespace Creatify.Web.Utility;

public class StaticDetails
{
    public static string CouponAPIBase {  get; set; }
    public enum APIType
    {
        GET,
        POST, 
        PUT,
        DELETE
    }
}
