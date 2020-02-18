using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace De.HsFlensburg.BookManager074.Service.ServiceBus
{
    public class Messenger : IMessenger
    {
        private static Messenger instance;
        private static object lockObject = new object();
        private Dictionary<Type, List<ActionIdentifier>> references =
            new Dictionary<Type, List<ActionIdentifier>>();

        #region singleton
        private Messenger() { }

        public static Messenger Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                        instance = new Messenger();
                    return instance;
                }
            }
        }
        #endregion singletion

        public void Register<TNotification>(object recipient, Action action)
        {
            Type messageType = typeof(TNotification);
            if (!references.ContainsKey(messageType))
                references.Add(messageType, new List<ActionIdentifier>());
            ActionIdentifier actionIdent = new ActionIdentifier();
            actionIdent.Action = new WeakReferenceAction(recipient, action);
            references[messageType].Add(actionIdent);
        }

        public void Send<TNotification>(TNotification notification)
        {
            Type type = typeof(TNotification);
            List<ActionIdentifier> typeActionIdentifiers = references[type];
            foreach (ActionIdentifier ai in typeActionIdentifiers)
            {
                WeakReferenceAction actionParameter = ai.Action;
                ai.Action.Execute();
            }
        }

        public void Unregister<TNotification>(object recipient)
        {
            bool lockTaken = false;
            try
            {
                Monitor.Enter(references, ref lockTaken);
                foreach (Type targetType in references.Keys)
                {
                    foreach (ActionIdentifier wra in references[targetType])
                    {
                        if (wra.Action != null && wra.Action.Target == recipient)
                            wra.Action.Unload();
                    }
                }
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(references);
            }
        }
    }
}

