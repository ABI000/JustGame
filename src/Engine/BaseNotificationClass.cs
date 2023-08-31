using Engine.EventArgs;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Engine
{
    public class BaseNotificationClass : INotifyPropertyChanged
    {

        /// <summary>
        /// 消息事件
        /// </summary>
        public event EventHandler<GameMessageEventArgs>? OnMessageRaised;
        /// <summary>
        /// 属性变更事件
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }
    }
}
