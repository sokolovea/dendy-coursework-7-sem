using BaseController;
using BaseMenuModel.Components;
using ConsoleView;

namespace ControllerConsole
{
    /// <summary>
    /// Базовый контроллер для консольного приложения
    /// </summary>
    public abstract class BaseControllerConsole : ControllerMenu
    {
        /// <summary>
        /// Вид меню
        /// </summary>
        protected ViewMenuConsole ViewMenu { get; set; }

        /// <summary>
        /// Флаг завершения работы контроллера
        /// </summary>
        protected bool NeedExit { get; set; }

        /// <summary>
        /// Основной цикл обработки ввода
        /// </summary>
        public override void Start()
        {
            NeedExit = false;

            while (!NeedExit)
            {
                if (ViewMenu != null)
                {
                    ViewMenu.Draw();
                }
                ProcessInput();
            }
        }

        /// <summary>
        /// Остановить контроллер
        /// </summary>
        public override void Stop()
        {
        }

        /// <summary>
        /// Обработка пользовательского ввода
        /// </summary>
        protected virtual void ProcessInput()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.LeftArrow:
                    Menu.FocusPrevious();
                    break;

                case ConsoleKey.DownArrow:
                case ConsoleKey.RightArrow:
                    Menu.FocusNext();
                    break;

                case ConsoleKey.Enter:
                    Menu.SelectFocusedItem();
                    break;

                //case ConsoleKey.Escape:
                //    NeedExit = true;
                //    break;
            }
        }
    }
}
