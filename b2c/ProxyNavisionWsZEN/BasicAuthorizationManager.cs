using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
namespace ProxyNavisionWsZEN
{
    public class BasicAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            string authHeader = request.Headers[HttpRequestHeader.Authorization];

            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                string usernamePassword = Encoding.GetEncoding("iso-8859-1").GetString(Convert.FromBase64String(encodedUsernamePassword));
                string[] credentials = usernamePassword.Split(':');

                if (credentials.Length != 2)
                {
                    DenyAccess("Invalid Authorization header format.");
                    return false;
                }

                string username = credentials[0];
                string password = credentials[1];

                if (IsValidUser(username, password))
                {
                    return true; // Authorized
                }
                else
                {
                    DenyAccess("Invalid username or password.");
                    return false;
                }
            }
            else
            {
                DenyAccess("Missing Authorization header.");
                return false;
            }


            // Unauthorized
            return false;
        }
        private void DenyAccess(string message)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
            WebOperationContext.Current.OutgoingResponse.StatusDescription = message;
            WebOperationContext.Current.OutgoingResponse.Headers.Add("WWW-Authenticate", "Basic realm=\"MyService\"");
        }

        public bool IsValidUser(string username, string password)
        {
            // Hardcoded usernames and passwords (replace these with your actual values)
            Dictionary<string, string> users = new Dictionary<string, string>
    {
        { "Deltasoft", "Delt@S0ft" },
        // Add more users as needed
    };

            // Check if the provided username exists in the dictionary
            if (users.ContainsKey(username))
            {
                // Compare the provided password with the stored password for the username
                string storedPassword = users[username];
                if (storedPassword == password)
                {
                    // Passwords match, user is valid
                    return true;
                }
            }

            // No matching user found or password doesn't match
            return false;
        }

    }
}