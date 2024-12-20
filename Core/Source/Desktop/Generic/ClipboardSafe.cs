﻿using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public static class ClipboardSafe
    {
        public static string GetText()
        {
            var text = string.Empty;

            var thread = new Thread(() => text = Clipboard.GetText());
            StartThread(thread);

            return text;
        }

        public static void SetText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            var thread = new Thread(() => Clipboard.SetText(text));
            StartThread(thread);
        }

        public static void SetImage(Image image)
        {
            var thread = new Thread(() => Clipboard.SetImage(image));
            StartThread(thread);
        }

        public static void Clear()
        {
            var thread = new Thread(Clipboard.Clear);
            StartThread(thread);
        }

        private static void StartThread(Thread thread)
        {
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
    }
}