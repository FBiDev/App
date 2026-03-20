using System;

namespace App.Core.Desktop
{
    public static class ActionExtension
    {
        public static void Run(this Action<string, LabelType> action, string message, LabelType type)
        {
            if (action != null)
            {
                action(message, type);
            }
        }
    }
}