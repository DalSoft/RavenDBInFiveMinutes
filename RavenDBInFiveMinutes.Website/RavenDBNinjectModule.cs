using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Database.Server;

namespace RavenDBInFiveMinutes.Website
{
    public class RavenDBNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDocumentStore>()
           .ToMethod(context =>
           {
               NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8080);
               var documentStore = new EmbeddableDocumentStore { DataDirectory = "App_Data", UseEmbeddedHttpServer = true, };
               return documentStore.Initialize();
           })
           .InSingletonScope();

            Bind<IDocumentSession>().ToMethod(context => context.Kernel.Get<IDocumentStore>().OpenSession())
                .InRequestScope()
                .OnDeactivation(x =>
                {
                    if (x == null)
                        return;

                    x.SaveChanges();
                    x.Dispose();
                });
        }
    }
}



