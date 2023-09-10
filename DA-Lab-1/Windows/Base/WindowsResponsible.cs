using System;
using System.Collections.Generic;
using System.Windows;

namespace DA_Lab_1.Windows.Base
{
    internal static class WindowsResponsible
    {
        private static Dictionary<Type, Window> _activeWindows = new Dictionary<Type, Window>()
        {
            { typeof(MainWindow), new Window() }
        };

        public static Window MainWindow => _activeWindows[typeof(MainWindow)];

        public static Window ShowWindow<T>() where T : Window, new()
        {
            var key = typeof(T);

            if (_activeWindows.ContainsKey(key))
            {
                _activeWindows[key].Close();
                _activeWindows[key] = new T();
            }
            else
            {
                _activeWindows.Add(typeof(T), new T());
            }

            _activeWindows[key].Show();

            return _activeWindows[key];
        }

        public static void HideWindow<T>() where T : Window
        {
            var key = typeof(T);

            if (!_activeWindows.ContainsKey(key))
                throw new InvalidOperationException($"Trying to hide {key} window, but there was no one of this type!");

            _activeWindows[key].Close();

            _activeWindows.Remove(key);
        }
    }
}
