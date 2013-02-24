using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Orient.Client.Protocol;

namespace Orient.Client
{
    public class OrientConnection
    {
        private string _userAgent = "OrientDB-NET.rest/alpha";

        #region Properties

        /// <summary>
        /// Node host name or IP address (without http(s) prefix, e.g. arango.example.com).
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Node port number.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Determenis if node is connected through HTTPS.
        /// </summary>
        public bool IsSecured { get; set; }

        /// <summary>
        /// Name of the user which will be used for authentication.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password which will be used for authentication.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Name of the database.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Alias of the connection object.
        /// </summary>
        public string Alias { get; set; }

        internal Uri BaseUri { get; set; }

        internal CredentialCache Credentials { get; set; }

        #endregion

        public OrientConnection(string server, int port, bool isSecured, string userName, string password, string database, string alias)
        {
            Server = server;
            Port = port;
            IsSecured = isSecured;
            Username = userName;
            Password = password;
            Database = database;
            Alias = alias;

            BaseUri = new Uri((isSecured ? "https" : "http") + "://" + server + ":" + port + "/");

            Credentials = new CredentialCache();
            Credentials.Add(BaseUri, "Basic", new NetworkCredential(Username, Password));
        }

        internal Response Process(Request request)
        {
            var httpRequest = (HttpWebRequest)HttpWebRequest.Create(BaseUri + request.RelativeUri);
            httpRequest.KeepAlive = true;
            httpRequest.Method = request.Method;
            httpRequest.UserAgent = _userAgent;
            httpRequest.Credentials = Credentials;

            if ((request.Headers.Count > 0))
            {
                httpRequest.Headers = request.Headers;
            }

            if (!string.IsNullOrEmpty(request.Body))
            {
                byte[] data = Encoding.UTF8.GetBytes(request.Body);

                Stream stream = httpRequest.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
            }
            else
            {
                httpRequest.ContentLength = 0;
            }

            var response = new Response();

            try
            {
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                var reader = new StreamReader(httpResponse.GetResponseStream());

                response.StatusCode = httpResponse.StatusCode;
                response.Headers = httpResponse.Headers;
                response.JsonString = reader.ReadToEnd();

                if (!string.IsNullOrEmpty(response.JsonString))
                {
                    response.JsonObject.Load(response.JsonString);
                }
            }
            catch (WebException webException)
            {
                var httpResponse = (HttpWebResponse)webException.Response;
                var reader = new StreamReader(httpResponse.GetResponseStream());

                if ((httpResponse.StatusCode == HttpStatusCode.NotModified) ||
                    ((httpResponse.StatusCode == HttpStatusCode.NotFound) && (request.Method == RequestMethod.HEAD.ToString())))
                {
                    response.StatusCode = httpResponse.StatusCode;
                    response.Headers = httpResponse.Headers;
                    response.JsonString = reader.ReadToEnd();

                    if (!string.IsNullOrEmpty(response.JsonString))
                    {
                        response.JsonObject.Load(response.JsonString);
                    }
                }
                else
                {
                    var jsonString = reader.ReadToEnd();
                    /*Json jsonObject = new Json();
                    string errorMessage = "";

                    if (!string.IsNullOrEmpty(jsonString))
                    {
                        jsonObject.Load(jsonString);
                        errorMessage = string.Format(
                            "ArangoDB responded with error code {0}:\n{1} [error number {2}]",
                            jsonObject.Get("code"),
                            jsonObject.Get("errorMessage"),
                            jsonObject.Get("errorNum")
                        );
                    }

                    throw new ArangoException(
                        httpResponse.StatusCode,
                        errorMessage,
                        webException.Message,
                        webException.InnerException
                    );*/
                }
            }

            return response;
        }
    }
}
