using ProxyNavisionWsZEN.API;
using System.Net;
using System;

public class AuthenticatedAPIClient : API
{
    private string _token;

    public AuthenticatedAPIClient(string token)
    {
        _token = token;
    }

    protected override WebRequest GetWebRequest(Uri uri)
    {
        HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(uri);
        request.Headers.Add("Authorization", "Bearer " + _token);
        return request;
    }
}
