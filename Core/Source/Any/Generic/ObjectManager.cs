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
            var me = (propertyLambda.Body as MemberExpression);
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
            if (st.FrameCount < frameIndex) return string.Empty;

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
                result += "File : " + ex.StackTrace.Split(Environment.NewLine.ToCharArray()).First().Split('\\').Last() + Environment.NewLine;

            var skipClass = new List<string> {
                "System."
            };

            var lineNumber = 0;

            foreach (var frame in frames)
            {
                var frameClass = frame.GetMethod();
                var frameClassName = frameClass.DeclaringType.FullName;
                var frameMethodName = frameClass.Name;

                if (skipClass.Any(x => { return frameClassName.IndexOf(x, StringComparison.CurrentCultureIgnoreCase) == 0; })) { continue; }

                if (frameMethodName == ".ctor") frameMethodName = frameClassName;

                if (frameMethodName == "MoveNext")
                {
                    lineNumber = frame.GetFileLineNumber();
                    continue;
                }

                if (new List<string> { "WndProc" }.Contains(frameMethodName)) break;

                if (lineNumber == 0) lineNumber = frame.GetFileLineNumber();

                result += lineNumber.ToString().PadLeft(3, '0');
                result += " : " + frameClassName + "." + frameMethodName;
                result += Environment.NewLine;
                lineNumber = 0;
            }
            return result;
        }
    }
}