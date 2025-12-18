using DataAccessLayer;
using Ninject.Modules;

namespace DotaApp
{
    public class NinjectConfigModule : NinjectModule
    {
        public override void Load()
        {
            // Регистрируем зависимость как Singleton (по заданию)
            Bind<IRepository<Hero>>().To<EntityRepository<Hero>>().InSingletonScope();

            // Регистрируем сам DotaLogic (будет создаваться при каждом запросе)
            Bind<DotaLogic>().ToSelf();
        }
    }
}