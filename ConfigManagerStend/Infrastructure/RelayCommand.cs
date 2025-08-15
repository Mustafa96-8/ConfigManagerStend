using System.Windows.Input;

namespace ConfigManagerStend.Infrastructure
{
    public class RelayCommand : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _executeFunc;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> executeFunc = null)
        {
            _execute = execute;
            _executeFunc = executeFunc;
        }

        public bool CanExecute(object parametr)
        {
            return _executeFunc == null || _executeFunc(parametr);
        }

        public void Execute(object parametr)
        {
            _execute(parametr);
        }
    }
}

