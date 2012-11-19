Nancy.Elmah
===========

Integrated Elmah-logging in Nancy

##Installation

Install via nuget https://nuget.org/packages/Nancy.Elmah

```
PM> Install-Package Nancy.Elmah
```

Or build from source and drop Nancy.Elmah.dll and Elmah.dll into your solution

##Configuration

####Basic logging
- Add the following to your bootstrapper to log all uncaught exceptions with elmah. 
- Elmah will log all uncaught exceptions.
- You can browse the Elmah error log at http://yoursite/elmah.

```c#
namespace Nancy.Elmah.Asp.Net.Example
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            Elmahlogging.Enable(pipelines, "elmah");
        }
    }
}
```

####Basic logging with secured Elmah interface
- Add the following to your bootstrapper to log all uncaught exceptions with elmah. 
- Elmah will log all uncaught exceptions.
- If you are logged in with the claim "administrator" you can browse the Elmah error log at http://yoursite/elmah.

```c#
namespace Nancy.Elmah.Asp.Net.Example
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            Elmahlogging.Enable(pipelines, "elmah", new []{"administrator"});
        }
    }
}
```

####Exception logging and select HttpStatusCode logging
- Add the following to your bootstrapper to log all uncaught exceptions with elmah. 
- Elmah will log all uncaught exceptions and configured HttpStatusCode's
- You can browse the Elmah error log at http://yoursite/elmah

```c#
namespace Nancy.Elmah.Asp.Net.Example
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            Elmahlogging.Enable(pipelines, "elmah", new string[0], new HttpStatusCode[] { HttpStatusCode.NotFound, HttpStatusCode.InsufficientStorage, });
        }
    }
}
```