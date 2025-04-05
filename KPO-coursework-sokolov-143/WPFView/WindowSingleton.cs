using System.Windows;

namespace WPFView
{
    /// <summary>
    /// Реализация шаблона "Одиночка" для графического окна
    /// </summary>
    public class WindowSingleton
    {
        /// <summary>
        /// Экземпляр класса
        /// </summary>
        private static WindowSingleton _instance;

        /// <summary>
        /// Окно
        /// </summary>
        private readonly Window _window;

        /// <summary>
        /// Конструктор
        /// </summary>
        private WindowSingleton()
        {
            _window = new Window()
            {
                ShowActivated = true,
                Width = ViewWPF.MENU_WIDTH,
                Height = ViewWPF.MENU_HEIGHT,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Title = "Battle City"
            };
        }

        /// <summary>
        /// Метод для получения экземпляра
        /// </summary>
        /// <returns>Экземпляр этого класса</returns>
        public static WindowSingleton GetInstance()
        {
            if (_instance == null)
            {
                _instance = new WindowSingleton();
            }
            return _instance;
        }

        /// <summary>
        /// Получить окно приложения
        /// </summary>
        /// <returns>Окно</returns>
        public Window GetWindow()
        {
            return _window;
        }
    }
}
