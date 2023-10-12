using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace DA_Lab_1
{
    public static class WindowsResponsible
    {
        private static Dictionary<Type, Window> _activeWindows = new();

        public static void Initialize(Window mainWindow)
        {
            AddWindow(mainWindow);
        }

        public static Window ShowWindow<T>() where T : Window, new()
        {
            AddWindow(new T());

            var key = typeof(T);

            _activeWindows[key].Show();

            return _activeWindows[key];
        }

        public static void HideWindow<T>() where T : Window
        {
            var key = typeof(T);

            if (!_activeWindows.ContainsKey(key))
                return;

            var window = _activeWindows[key];

            RemoveWindow(window);

            window.Close();
        }

        public static Window? GetWindow<T>() where T : Window
        {
            return _activeWindows.GetValue(typeof(T));
        }

        public static bool IsWindowOpened<T>() where T : Window
        {
            return _activeWindows.ContainsKey(typeof(T));
        }

        private static void AddWindow(Window window)
        {
            var type = window.GetType();

            if (_activeWindows.ContainsKey(type))
            {
                var previousWindow = _activeWindows[type];

                _activeWindows.Remove(type);

                previousWindow.Close();

                previousWindow.Closing -= OnWindowClosed;
            }

            _activeWindows[type] = window;

            _activeWindows[type].Closing += OnWindowClosed;
        }

        private static void RemoveWindow(Window window)
        {
            var type = window.GetType();

            if (!_activeWindows.ContainsKey(type))
                return;

            _activeWindows[type].Closing -= OnWindowClosed;

            _activeWindows.Remove(type);

            if (_activeWindows.Count == 0)
                Environment.Exit(0);
        }

        private static void OnWindowClosed(object? sender, CancelEventArgs e)
        {
            var window = (Window)sender;

            window.Closing -= OnWindowClosed;

            _activeWindows.Remove(window.GetType());
        }
    }
}
