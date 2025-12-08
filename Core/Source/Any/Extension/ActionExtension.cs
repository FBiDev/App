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

        public static void Run(this Action<bool, string> action, bool isTrue, string message)
        {
            if (action != null)
            {
                action(isTrue, message);
            }
        }

        public static void Run(this BoolAction action, bool isCrop)
        {
            if (action != null)
            {
                action(isCrop);
            }
        }
    }
}