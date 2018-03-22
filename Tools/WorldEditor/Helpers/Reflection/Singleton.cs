using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorldEditor.Helpers.Reflection
{
    public abstract class Singleton<T> where T : class
    {
        internal static class SingletonAllocator
        {
            internal static T instance;

            static SingletonAllocator()
            {
                CreateInstance(typeof(T));
            }

            public static T CreateInstance(Type type)
            {
                var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
                if (constructors.Length > 0)
                {
                    instance = (T)(Activator.CreateInstance(type));
                    return instance;
                }
                else
                {
                    var constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, new ParameterModifier[0]);
                    if (constructor == null)
                    {
                        throw new Exception(string.Format("{0} doesn't have a private/protected constructor so the property cannot be enforced.", type.FullName)); ;
                    }
                    try
                    {
                        instance = (T)constructor.Invoke(new object[0]);
                        return instance;
                    }
                    catch (Exception innerException)
                    {
                        throw new Exception(string.Format("The Singleton couldnt be constructed, check if {0} has a default constructor", type.FullName), innerException);
                    }
                }
            }
        }

        public static T Instance
        {
            get { return SingletonAllocator.instance; }
            protected set { SingletonAllocator.instance = value; }
        }
    }
}
