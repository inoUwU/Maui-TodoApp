namespace Maui_TodoApp.Models
{
    internal class MainPageModel : Extends.BindableBase
    {
        private int _count;

        public int Count
        {
            get => _count;
            set
            {
                if (_count == value)
                {
                    if (value == 1)
                        _ClickText = $"Click {value} time";
                    else _ClickText = $"Click {value} times";
                }

                _count = value;
            }
        }
        private string _ClickText = "Click me";

        public string ClickText
        {
            get => _ClickText;
            set => SetProperty(ref _ClickText, value);
        }
    }
}
