
using BaseController;
using BaseMenuModel.Components;
using System.Windows;
using System.Windows.Input;
using WPFView;

namespace WPFController
{
    /// <summary>
    /// Базовый контроллер для контроллеров WPF
    /// </summary>
    public abstract class BaseControllerWPF : ControllerMenu
    {
        /// <summary>
        /// Делегат нажатия на кнопку
        /// </summary>
        public delegate void dKeyPressed(object sender, KeyEventArgs args);

        /// <summary>
        /// Событие нажатия на кнопку
        /// </summary>
        public event dKeyPressed OnKeyPressed;

        /// <summary>
        /// Конструктор базового контроллера WPF
        /// </summary>
        public BaseControllerWPF()
        {
        }

        /// <summary>
        /// Обработчик нажатия клавиш
        /// </summary>
        /// <param name="parSender">Отправитель</param>
        /// <param name="parArgs">Нажатые клавиши</param>
        protected virtual void KeyDownEventHandler(object parSender, KeyEventArgs parArgs)
        {
            switch (parArgs.Key)
            {
                case Key.Up:
                case Key.Left:
                    Menu.FocusPrevious();
                    break;
                case Key.Down:
                case Key.Right:
                    Menu.FocusNext();
                    break;
                case Key.Enter:
                    Menu.SelectFocusedItem();
                    break;
            }
        }

        /// <summary>
        /// Открывает графическое окно
        /// </summary>
        public override void Start()
        {
            Window window = WindowSingleton.GetInstance().GetWindow();
            window.KeyDown += KeyDownEventHandler;
        }

        /// <summary>
        /// Закрывает графическое окно
        /// </summary>
        public override void Stop()
        {
            Window window = WindowSingleton.GetInstance().GetWindow();
            window.KeyDown -= KeyDownEventHandler;
        }

    }
}

