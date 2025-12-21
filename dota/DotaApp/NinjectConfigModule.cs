using DataAccessLayer;
using Ninject.Modules;

namespace DotaApp
{
    public class NinjectConfigModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository<Hero>>().To<EntityRepository<Hero>>().InSingletonScope();

            //Bind<IRepository<Hero>>().To<DapperRepository<Hero>>().InSingletonScope();

            Bind<DotaLogic>().ToSelf();
        }
    }
}