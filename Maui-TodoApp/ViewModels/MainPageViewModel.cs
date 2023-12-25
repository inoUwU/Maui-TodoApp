using Maui_TodoApp.Extends;
using Maui_TodoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maui_TodoApp.ViewModels
{
    internal class MainPageViewModel
    {
        public string Text { get; set; } = "Initial Text";

        public MainPageModel model { get; set; } = new();

        public ICommand AddCounterCommand { get; set; }

        public MainPageViewModel()
        {
            AddCounterCommand = new DelegateCommand(Add);

        }

        private void Add(object obj)
        {
            model.Count++;
        }
    }
}
