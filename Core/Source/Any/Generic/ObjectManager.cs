using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace App.Core
{
    public delegate void EventVoid();

    public delegate Task EventTaskAsync();

    public static class ObjectManager
    {
        public static string GetPropertyName<T>(Expression<Func<T>> propertyLambda)
        {
            var me = propertyLambda.Body as MemberExpression;
            if (me == null)
            {
                throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }

            return me.Member.Name;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetDaoClassAndMethod(int frameIndex = 0)
        {
            var st = new StackTrace();
            if (st.FrameCount < frameIndex)
            {
                return string.Empty;
            }

            var sf = st.GetFrame(frameIndex);
            var method = sf.GetMethod().DeclaringType.Name + "." + sf.GetMethod().Name;

            return method;
        }

        public static string GetStackTrace(Exception ex)
        {
            var st = new StackTrace(true);
            var sf = st.GetFrames();

            var frames = sf.ToList();
            frames.RemoveRange(0, 4);

            string result = "[Stack]" + Environment.NewLine;
            if (ex.NotNull())
            {
                result += "File : " + ex.StackTrace.Split(Environment.NewLine.ToCharArray()).First().Split('\\').Last() + Environment.NewLine;
            }

            var skipClass = new List<string> 
            {
                "System."
            };

            var lineNumber = 0;

            foreach (var frame in frames)
            {
                var frameClass = frame.GetMethod();
                var frameClassName = frameClass.DeclaringType.FullName;
                var frameMethodName = frameClass.Name;

                if (skipClass.Any(x =>
                {
                    return frameClassName.IndexOf(x, StringComparison.CurrentCultureIgnoreCase) == 0;
                }))
                {
                    continue;
                }

                if (frameMethodName == ".ctor")
                {
                    frameMethodName = frameClassName;
                }

                if (frameMethodName == "MoveNext")
                {
                    lineNumber = frame.GetFileLineNumber();
                    continue;
                }

                if (new List<string> { "WndProc" }.Contains(frameMethodName))
                {
                    break;
                }

                if (lineNumber == 0)
                {
                    lineNumber = frame.GetFileLineNumber();
                }

                result += lineNumber.ToString().PadLeft(3, '0');
                result += " : " + frameClassName + "." + frameMethodName;
                result += Environment.NewLine;
                lineNumber = 0;
            }

            return result;
        }

        public static int? GetID<T>(string idPropertyName, T entity) where T : new()
        {
            var obj = typeof(T);
            object idValue = null;

            if (idPropertyName != string.Empty)
            {
                var property = obj.GetProperty(idPropertyName);
                
                if (property != null)
                {
                    idValue = property.GetValue(entity);
                }
            }

            return Cast.ToIntNull(idValue);
        }

        public static string GetLog<T>(string idPropertyName, DatabaseAction action, T oldObject, T newObject, string userLogin) where T : new()
        {
            var obj = typeof(T);
            var props = obj.GetProperties();
            var updatedProps = new List<string>();

            if (oldObject == null)
            {
                oldObject = new T();
            }

            foreach (var prop in props)
            {
                if (prop.Name == idPropertyName || prop.CanWrite == false || prop.PropertyType.IsPrimitiveOrSimple() == false)
                {
                    continue;
                }

                var oldValue = prop.GetValue(oldObject) == null ? "null" : prop.GetValue(oldObject).ToString();
                var newValue = prop.GetValue(newObject) == null ? "null" : prop.GetValue(newObject).ToString();

                if (oldValue != newValue)
                {
                    if (action == DatabaseAction.Insert)
                    {
                        updatedProps.Add("\"" + prop.Name + "\": \"" + newValue + "\"");
                    }
                    else
                    {
                        updatedProps.Add("\"" + prop.Name + "\": \"" + oldValue + " -> " + newValue + "\"");
                    }
                }
            }

            var logText = string.Empty;

            if (updatedProps.Any())
            {
                var idValue = obj.GetProperty(idPropertyName).GetValue(newObject);

                updatedProps.Insert(0, "\"" + idPropertyName + "\": \"" + idValue + "\"");

                updatedProps.Insert(0, "\"Log_Entity\": \"" + obj.Name + "\"");
                updatedProps.Insert(0, "\"Log_Login\": \"" + userLogin + "\"");
                updatedProps.Insert(0, "\"Log_Action\": \"" + action.ToString().ToUpper() + "\"");
                updatedProps.Insert(0, "\"Log_Date\": \"" + DateTime.Now + "\"");

                logText = "{" + string.Join(", ", updatedProps) + "}";
            }

            return logText;
        }
    }
}