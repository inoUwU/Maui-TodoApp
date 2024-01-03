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
                if (value == 1)
                    ClickText = $"Click {value} time";
                else ClickText = $"Click {value} times";

                _count = value;
            }
        }
        private string _ClickText = "Click me";

        public string ClickText
        {
            get => _ClickText;
            set => _ = SetProperty(ref _ClickText, value);
        }

        private int _PortNumber;

        public int PortNumber
        {
            get { return _PortNumber; }
            set
            {
                if (value != 0)
                {
                    SetProperty(ref _PortNumber, value);
                }
            }
        }
    }
}
