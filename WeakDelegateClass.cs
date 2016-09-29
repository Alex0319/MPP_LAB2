using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WeakDelegate
{
    public class WeakDelegateClass
    {
        private MethodInfo weakMethod;
        private WeakReference weakReference;
        private DelegateFactory delFactory;

        public WeakDelegateClass(Delegate del)
        {
            weakReference = new WeakReference(del.Target);
            weakMethod = del.Method;
            delFactory = new DelegateFactory();
        }

        public static implicit operator Delegate(WeakDelegateClass weakTarget)
        {
            return weakTarget.weakRef;
        }

        public Delegate weakRef
        {
            get
            {
                return delFactory.CreateDelegate(weakMethod,weakReference);
            }
        }

        public bool isNullTarget
        {
            get 
            {
                return !weakReference.IsAlive;
            }
        }
    }
}
