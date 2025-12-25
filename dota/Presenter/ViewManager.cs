using System;
using System.Collections.Generic;
using System.Windows;

namespace Presenter
{
    public class ViewManager
    {
        private static readonly Dictionary<Type, Type> _viewModelToViewMap = new Dictionary<Type, Type>();
        private static readonly Dictionary<object, Window> _openWindows = new Dictionary<object, Window>();

        public static void Register<TViewModel, TView>() where TView : Window
        {
            _viewModelToViewMap[typeof(TViewModel)] = typeof(TView);
        }

        public static void Show<TViewModel>(TViewModel viewModel) where TViewModel : class
        {
            if (_viewModelToViewMap.TryGetValue(typeof(TViewModel), out Type viewType))
            {
                var view = (Window)Activator.CreateInstance(viewType);
                view.DataContext = viewModel;
                _openWindows[viewModel] = view;
                view.Show();
            }
        }

        public static void ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : class
        {
            if (_viewModelToViewMap.TryGetValue(typeof(TViewModel), out Type viewType))
            {
                var view = (Window)Activator.CreateInstance(viewType);
                view.DataContext = viewModel;
                view.ShowDialog();
            }
        }

        public static void Close(object viewModel)
        {
            if (_openWindows.TryGetValue(viewModel, out Window window))
            {
                window.Close();
                _openWindows.Remove(viewModel);
            }
        }
    }
}