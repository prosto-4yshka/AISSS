using System.ComponentModel;
using System.Runtime.CompilerServices;
using DataAccessLayer;

namespace Presenter.ViewModels
{
    public class HeroDTO : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _role;
        private string _attribute;
        private int _complexity;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Role
        {
            get => _role;
            set { _role = value; OnPropertyChanged(); }
        }

        public string Attribute
        {
            get => _attribute;
            set { _attribute = value; OnPropertyChanged(); }
        }

        public int Complexity
        {
            get => _complexity;
            set { _complexity = value; OnPropertyChanged(); }
        }

        public static HeroDTO FromDomain(Hero hero)
        {
            if (hero == null) return null;

            return new HeroDTO
            {
                Id = hero.Id,
                Name = hero.Name,
                Role = hero.Role,
                Attribute = hero.Attribute,
                Complexity = hero.Complexity
            };
        }

        public Hero ToDomain()
        {
            return new Hero
            {
                Id = this.Id,
                Name = this.Name,
                Role = this.Role,
                Attribute = this.Attribute,
                Complexity = this.Complexity
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}