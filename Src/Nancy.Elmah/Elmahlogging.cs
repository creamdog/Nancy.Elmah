using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Elmah;
using Nancy.Bootstrapper;
using Nancy.Security;
using ErrorSignal = Elmah.ErrorSignal;

namespace Nancy.Elmah
{
    /// <summary>
    /// 
    /// </summary>
    public class Elmahlogging : NancyModule
    {
        private static string _elmahPath = string.Empty;
        private static string[] _requiredClaims = new string[0];
        private static HttpStatusCode[] _loggedHttpStatusCodes;

        /// <summary>
        /// Enables Elmah logging of application exceptions and errors.
        /// Using this enabled will catch and log exceptions only.
        /// The Elmah admin interface at the path defined by <param name="elmahModuleBasePath">elmahModuleBasePath</param> will be public and visible to anybody
        /// </summary>
        /// <param name="pipelines"></param>
        /// <param name="elmahModuleBasePath"></param>
        public static void Enable(IPipelines pipelines, string elmahModuleBasePath)
        {
            _loggedHttpStatusCodes = new HttpStatusCode[0];
            _requiredClaims = new string[0];
            _elmahPath = elmahModuleBasePath;
            pipelines.OnError.AddItemToEndOfPipeline(LogError);
            pipelines.AfterRequest.AddItemToEndOfPipeline(LogHttpStatusCode);
        }

        /// <summary>
        /// Enables Elmah logging of application exceptions and errors.
        /// Using this enabled will catch and log exceptions only.
        /// The Elmah admin interface at the path defined by <param name="elmahModuleBasePath">elmahModuleBasePath</param> will be visible to users that has the claims specified by <param name="requiredClaims">requiredClaims</param>
        /// </summary>
        /// <param name="pipelines"></param>
        /// <param name="elmahModuleBasePath"></param>
        /// <param name="requiredClaims"></param>
        public static void Enable(IPipelines pipelines, string elmahModuleBasePath, IEnumerable<string> requiredClaims)
        {
            _loggedHttpStatusCodes = new HttpStatusCode[0];
            _requiredClaims = requiredClaims.ToArray();
            _elmahPath = elmahModuleBasePath;
            pipelines.OnError.AddItemToEndOfPipeline(LogError);
            pipelines.AfterRequest.AddItemToEndOfPipeline(LogHttpStatusCode);
        }

        /// <summary>
        /// Enables Elmah logging of application exceptions and errors.
        /// Using this enabled will catch and log exceptions and HttpStatusCodes defined by <param name="loggedHttpStatusCodes">loggedHttpStatusCodes</param>.
        /// The Elmah admin interface at the path defined by <param name="elmahModuleBasePath">elmahModuleBasePath</param> will be visible to users that has the claims specified by <param name="requiredClaims">requiredClaims</param>.
        /// </summary>
        /// <param name="pipelines"></param>
        /// <param name="elmahModuleBasePath"></param>
        /// <param name="requiredClaims"></param>
        /// <param name="loggedHttpStatusCodes"></param>
        public static void Enable(IPipelines pipelines, string elmahModuleBasePath, IEnumerable<string> requiredClaims, IEnumerable<HttpStatusCode> loggedHttpStatusCodes)
        {
            _loggedHttpStatusCodes = loggedHttpStatusCodes.ToArray();
            _requiredClaims = requiredClaims.ToArray();
            _elmahPath = elmahModuleBasePath;
            pipelines.OnError.AddItemToEndOfPipeline(LogError);
            pipelines.AfterRequest.AddItemToEndOfPipeline(LogHttpStatusCode);
        }

        public static void LogHttpStatusCode(NancyContext context)
        {
            if(context == null || context.Response == null)
                return;

            if (!_loggedHttpStatusCodes.Contains(context.Response.StatusCode))
                return;

            var url = context.Request == null ? string.Empty : context.Request.Url.ToString();
            var statusCode = context.Response == null ? string.Empty : context.Response.StatusCode.ToString();
            var nancyModuleName = context.NegotiationContext == null ? string.Empty : context.NegotiationContext.ModuleName;
            var nancyModulePath = context.NegotiationContext == null ? string.Empty : context.NegotiationContext.ModulePath;
            var user = context.CurrentUser == null ? string.Empty : context.CurrentUser.UserName;

            var message = string.Format("url: {0}, statuscode: {1}, user: {2}, moduleName: {3}, modulePath: {4}", url, statusCode, user, nancyModuleName, nancyModulePath);

            var exception = new HttpException((int) context.Response.StatusCode, message);

            LogError(context, exception);
        }

        public static Response LogError(NancyContext context, Exception exception)
        {
            ErrorSignal.FromCurrentContext().Raise(exception);
            return null;
        }

        public Elmahlogging() : base(_elmahPath)
        {
            if (_requiredClaims.Length>0)
            this.RequiresClaims(_requiredClaims);

            if(!string.IsNullOrEmpty(_elmahPath))
            {
                Get["/"] = args =>
                {

                    switch((string)Request.Query.get.Value)
                    {
                        case "stylesheet":
                            return Response.AsElmahEmbeddedResource("ErrorLog.css", "text/css").WithContentType("text/css;charset=UTF-8");
                        case "about":
                            return Response.AsElmahEmbeddedPage("AboutPage");
                        case "detail":
                            return Response.AsElmahEmbeddedPage("ErrorDetailPage");
                        case "xml":
                            return Response.AsElmahEmbeddedPage("ErrorXmlHandler").WithContentType("text/xml;charset=UTF-8");
                        case "json":
                            return Response.AsElmahEmbeddedPage("ErrorJsonHandler").WithContentType("application/json;charset=UTF-8");
                        case "digestrss":
                            return Response.AsElmahEmbeddedPage("ErrorDigestRssHandler").WithContentType("application/rss+xml;charset=UTF-8");
                        case "rss":
                            return Response.AsElmahEmbeddedPage("ErrorRssHandler").WithContentType("application/rss+xml;charset=UTF-8");
                        case "download":
                            return Response.AsElmahEmbeddedPage("ErrorLogDownloadHandler").WithContentType("application/rss+xml;charset=UTF-8");
                        default:
                            return Response.AsElmahEmbeddedPage("ErrorLogPage");
                    }
                };

                Get["/{resource}"] = args =>
                {
                    var q = (Request.Query as DynamicDictionary).Keys.ToDictionary(key => key.Replace("?", ""), key => (string)(Request.Query as DynamicDictionary)[key]);
                    q["get"] = (string) args.resource;
                    var queryString = "?"+string.Join("&", q.Select(kv => string.Format("{0}={1}", kv.Key, kv.Value)));
                    return Response.AsRedirect("/" + _elmahPath + queryString);
                };
            }
        }
    }
}
