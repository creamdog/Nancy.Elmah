using System;
using System.Linq;
using System.Text;
using System.Web;
using Elmah;

namespace Nancy.Elmah
{
    public static class ResponseExtensions
    {
        public static Response AsElmahEmbeddedResource(this IResponseFormatter response, string name, string contentType)
        {
            if (HttpContext.Current == null) return HttpStatusCode.InternalServerError;

            var type = typeof (ErrorLogPageFactory).Assembly.GetTypes().FirstOrDefault(a => a.Name == "ManifestResourceHandler");
            if (type == null) return HttpStatusCode.InternalServerError;

            var page = Activator.CreateInstance(type, name, contentType, Encoding.GetEncoding("Windows-1252")) as IHttpHandler;
            if (page == null) return HttpStatusCode.NotFound;
            page.ProcessRequest(HttpContext.Current);

            return new Response().WithContentType("text/css;charset=UTF-8");
        }

        public static Response AsElmahEmbeddedPage(this IResponseFormatter response, string name)
        {
            if (HttpContext.Current == null) return HttpStatusCode.InternalServerError;

            var type = typeof (ErrorLogPageFactory).Assembly.GetTypes().FirstOrDefault(t => t.Name == name);
            if (type == null) return HttpStatusCode.InternalServerError;

            var page = Activator.CreateInstance(type) as IHttpHandler;
            if (page == null) return HttpStatusCode.NotFound;
            page.ProcessRequest(HttpContext.Current);

            return new Response().WithContentType("text/html;charset=UTF-8").WithStatusCode(HttpStatusCode.OK);
        }
    }
}