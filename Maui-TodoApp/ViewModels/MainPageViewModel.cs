using Maui_TodoApp.Extends;
using Maui_TodoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui_TodoApp.ViewModels
{
    internal class MainPageViewModel
    {
        public string Text { get; set; } = "Initial Text";

        public MainPageModel model { get; set; } = new();

        public DelegateCommand<int> AddCounterCommand { get; set; }

        public MainPageViewModel()
        {
            AddCounterCommand = new DelegateCommand<int>((vlaue) =>
            {
                model.Count++;
            });
        }
    }

}
