namespace Nancy.Elmah.Asp.Net.Example
{
    public class Bootstrapper : Nancy.DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            Elmahlogging.Enable(pipelines, "elmah", new string[0], new HttpStatusCode[]{HttpStatusCode.NotFound});
        }
    }
}