using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.BookManager074.Service.ServiceBus
{
    public class WeakReferenceAction
    {
        private WeakReference target;
        private Action action;
        public WeakReferenceAction(object target, Action action)
        {
            this.target = new WeakReference(target);
            this.action = action;
        }
        public WeakReference Target
        {
            get
            {
                return target;
            }
        }
        public void Execute()
        {
            if (action != null && Target != null && Target.IsAlive)
                this.action();
        }
        public void Unload()
        {
            target = null;
            action = null;
        }

    }
}
