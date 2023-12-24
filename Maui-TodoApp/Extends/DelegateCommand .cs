using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maui_TodoApp.Extends
{
    public class DelegateCommand<T>(Action<T> execute, Func<bool>? canExecute = null) : ICommand
    {
        private readonly Action<T> _execute = execute;
        private readonly Func<bool>? _canExecute = canExecute;

        public event EventHandler? CanExecuteChanged;

        public DelegateCommand(Action<T> execute) : this(execute, () => true) { }

        public void Execute(object? parameter)
        {
            if (parameter != null)
            {
                this._execute((T)parameter!);
            }
            
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }
    }
}
