using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.BookManager074.Service.ServiceBus
{
    interface IMessenger
    {
        void Register<TNotification>(object recipient, Action action);
        void Send<TNotification>(TNotification notification);
        void Unregister<TNotification>(object recipient);
    }
}

