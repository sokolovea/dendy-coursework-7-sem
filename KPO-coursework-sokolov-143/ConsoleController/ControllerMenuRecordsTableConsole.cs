using BaseMenuModel.ConcreteScreen.Help;
using ConsoleView;
using ControllerConsole;

namespace ConsoleController
{
    /// <summary>
    /// Контроллер меню с рекордами для консоли
    /// </summary>
    public class ControllerMenuRecordsTableConsole : BaseControllerConsole
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ControllerMenuRecordsTableConsole()
        {
            Menu = new MenuRecordsTable();
            ViewMenu = new ViewMenuRecordsTableConsole(Menu);
            ViewMenu.InitMenuItemPositionByIndex((int)MenuRecordsItemCodes.Back, 120, 15);

            Menu[(int)MenuRecordsItemCodes.Back].Selected += () =>
            {
                this.Stop();
                new ControllerMenuMainConsole().Start();
            };
        }
    }
}
