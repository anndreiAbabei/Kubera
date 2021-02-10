using System;

namespace Kubera.App.IntegrationTests.TestData
{
    public class GenericEqualiable<T1, T2> : IEqualiable<T1, T2>
        where T1 : class
        where T2 : class
    {
        private static readonly Type stringType = typeof(string);

        public virtual bool AreEqual(T1 t1, T2 t2)
        {
            if (t1 == null && t1 != t2)
                return false;

            var type1 = t1.GetType();
            var type2 = t2.GetType();

            foreach(var property1 in type1.GetProperties())
            {
                var property2 = type2.GetProperty(property1.Name);

                if (property2 == null)
                    continue;

                var typeProp1 = property1.PropertyType;
                var typeProp2 = property2.PropertyType;

                var value1 = property1.GetValue(t1);
                var value2 = property2.GetValue(t2);

                if (typeProp1.IsValueType)
                {
                    if (!typeProp2.IsValueType)
                        return false;

                    if (!AreEqualInt(value1, value2))
                        return false;
                }
                else if (typeProp1 == stringType)
                {
                    if (typeProp2 != stringType)
                        return false;

                    if (!AreEqualInt(value1, value2))
                        return false;
                }
                else if (!Equals(value1, value2))
                    return false;
            }

            return true;
        }

        private static bool AreEqualInt(object o1, object o2)
        {
            return (o1 == null && o2 == null) || o1.Equals(o2);
        }
    }
}
