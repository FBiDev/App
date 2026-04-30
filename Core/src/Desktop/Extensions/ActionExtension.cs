using System;

namespace App.Core.Desktop
{
    public static class ActionExtension
    {
        public static void Run(this Action<LabelType, string> action, LabelType type, string message)
        {
            if (action != null)
            {
                action(type, message);
            }
        }
    }
}