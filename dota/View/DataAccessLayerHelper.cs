using System;
using DataAccessLayer;
using Ninject;
using Presenter;
using Shared;

namespace View
{
    public static class DataAccessLayerHelper
    {
        private static IKernel _kernel;

        static DataAccessLayerHelper()
        {
            try
            {
                Console.WriteLine("[DI] Инициализация контейнера...");
                _kernel = new StandardKernel();

                // Регистрируем репозиторий (Entity Framework)
                _kernel.Bind<IRepository<DomainEntity>>()
                       .To<EntityRepository<DomainEntity>>()
                       .InSingletonScope();

                Console.WriteLine("[DI] Конфигурация завершена!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DI] Ошибка: {ex.Message}");
                throw;
            }
        }

        public static IRepository<DomainEntity> GetRepository()
        {
            return _kernel.Get<IRepository<DomainEntity>>();
        }

        public static DotaPresenter CreatePresenter(IView view)
        {
            var repository = GetRepository();
            Model.Hero.InitializeRepository(repository);
            var model = new Model.Hero(); // Hero теперь и есть модель
            return new DotaPresenter(view, model);
        }
    }
}