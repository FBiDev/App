using System;

namespace App.Core
{
    public static class ActionExtension
    {
        public static void Run(this Action action)
        {
            if (action != null)
            {
                action();
            }
        }

        public static void Run(this Action<bool, string> action, bool arg1, string arg2)
        {
            if (action != null)
            {
                action(arg1, arg2);
            }
        }

        public static void Run(this BoolAction action, bool arg)
        {
            if (action != null)
            {
                action(arg);
            }
        }
    }
}