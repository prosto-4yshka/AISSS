using System.Windows;
using Presenter;
using Presenter.ViewModels;

namespace DotaWPF
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ViewManager.Register<MainViewModel, MainView>();

            var mainViewModel = new MainViewModel();
            ViewManager.Show(mainViewModel);
        }
    }
}