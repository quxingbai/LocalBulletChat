using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LocalBulletChat.Controls.Tool
{
    public class ActionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action ExecuteAction { get; set; }
        private Action<object> ExecuteActionArg { get; set; }
        public ActionCommand(Action action)
        {
            ExecuteAction = action;
        }
        public ActionCommand(Action<Object> action)
        {
            ExecuteActionArg = action;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter == null)
            {
                ExecuteAction?.Invoke();
            }
            else
            {
                ExecuteActionArg?.Invoke(parameter);
            }
        }
    }
}
