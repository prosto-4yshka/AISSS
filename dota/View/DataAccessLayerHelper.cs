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
                _kernel.Bind<IRepository<DataAccessLayer.DomainEntity>>()
                       .To<EntityRepository<DataAccessLayer.DomainEntity>>()
                       .InSingletonScope();

                // Регистрируем модель
                _kernel.Bind<IModel>()
                       .To<Model.DotaModel>()
                       .InSingletonScope();

                Console.WriteLine("[DI] Конфигурация завершена!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DI] Ошибка: {ex.Message}");
                throw;
            }
        }

        public static IModel GetModel()
        {
            return _kernel.Get<IModel>();
        }

        public static DotaPresenter CreatePresenter(IView view)
        {
            var model = GetModel();
            return new DotaPresenter(view, model);
        }
    }
}