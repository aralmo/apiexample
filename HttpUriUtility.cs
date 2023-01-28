using System.Globalization;
using System.Web;

public static class HttpUriUtility
{
    public static Uri BuildUri(string url, params (string name, object value)[] parameters)
    {
        var builder = new UriBuilder(url);
        var paramValues = HttpUtility.ParseQueryString(builder.Query);
        foreach(var param in parameters)
            paramValues.Add(param.name, Convert.ToString(param.value,CultureInfo.InvariantCulture));

        builder.Query = paramValues.ToString();
        return builder.Uri;
    }
}