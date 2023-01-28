using System.Web;

public static class HttpUriUtility
{
    public static Uri BuildUri(string url, params (string name, string value)[] parameters)
    {
        var builder = new UriBuilder(url);
        var paramValues = HttpUtility.ParseQueryString(builder.Query);
        paramValues.Add("param1", "value1");
        paramValues.Add("param2", "value2");
        builder.Query = paramValues.ToString();
        return builder.Uri;
    }
}