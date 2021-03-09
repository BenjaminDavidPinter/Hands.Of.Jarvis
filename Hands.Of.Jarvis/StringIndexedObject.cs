using System;
using System.Reflection;

namespace Hands.Of.Jarvis.Objects
{
    public class StringIndexedObject
    {
        public object this[string propertyName]
        {
            get
            {
                return GetType()
                        .GetProperty(propertyName)
                        .GetValue(this, null);
            }
            set
            {
                Type myType = this.GetType();
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                if (myPropInfo.PropertyType == typeof(DateTime))
                {
                    myPropInfo.SetValue(this, DateTime.Parse(value as string), null);
                }
                else
                {
                    myPropInfo.SetValue(this, value, null);
                }
            }
        }
    }
}
