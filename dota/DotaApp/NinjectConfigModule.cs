using DataAccessLayer;
using Ninject.Modules;

namespace DotaApp
{
    public class NinjectConfigModule : NinjectModule
    {
        public override void Load()
        {
            // === ВАРИАНТ 1: Entity Framework (раскомментировать для использования) ===
            Bind<IRepository<Hero>>().To<EntityRepository<Hero>>().InSingletonScope();

            // === ВАРИАНТ 2: Dapper (закомментировать если не нужен) ===
            //Bind<IRepository<Hero>>().To<DapperRepository<Hero>>().InSingletonScope();

            Bind<DotaLogic>().ToSelf();
        }
    }
}