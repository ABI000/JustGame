using Engine.EventArgs;
using Engine.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Engine
{
    public class BaseNotificationClass : INotifyPropertyChanged
    {
        protected readonly MessageBroker _messageBroker = MessageBroker.GetInstance();

        /// <summary>
        /// 属性变更事件
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void RaiseMessage(string message)
        {
            _messageBroker.RaiseMessage(message);
        }

    }
}
