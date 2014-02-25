using System;

namespace Nancy.Elmah.Asp.Net.Example
{
    public class DefaultModule : NancyModule
    {
        public DefaultModule()
        {
            Get["/"] = _ => Response.AsText("<html><body><h1>Nancy.Elmah</h1><p>the Elmah interface is avilable <a href=\"/admin/elmah\">here</a></p><br/><img src=\"/exception\" alt=\"this image is broken on purpose\"/></body></html>").WithContentType("text/html;charset=UTF-8");

            Get["/exception"] = _ => { throw new Exception("this is just an example exception"); };
        }
    }
}