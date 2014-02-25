namespace Nancy.Elmah.Asp.Net.Example
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            Elmahlogging.Enable(pipelines, "/admin/elmah", new string[0], new []{HttpStatusCode.NotFound});
        }
    }
}