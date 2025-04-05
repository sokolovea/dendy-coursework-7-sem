using BaseMenuModel.ConcreteScreen.Help;
using ConsoleView;
using ControllerConsole;

namespace ConsoleController
{
    /// <summary>
    /// Контроллер меню справки для консольного интерфейса
    /// </summary>
    public class ControllerMenuHelpConsole : BaseControllerConsole
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ControllerMenuHelpConsole()
        {
            Menu = new MenuHelp();
            ViewMenu = new ViewMenuHelpConsole(Menu);
            ViewMenu.InitMenuItemPositionByIndex((int)MenuHelpItemCodes.Back, 120, 15);

            Menu[(int)MenuHelpItemCodes.Back].Selected += () =>
            {
                this.Stop();
                new ControllerMenuMainConsole().Start();
            };

            ((ViewMenuHelpConsole)ViewMenu).Init();
        }
    }
}
