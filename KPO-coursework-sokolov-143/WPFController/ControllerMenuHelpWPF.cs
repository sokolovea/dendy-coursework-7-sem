using BaseMenuModel.Components;
using BaseMenuModel.ConcreteScreen.Help;
using WPFView;

namespace WPFController
{
    /// <summary>
    /// Конструктор меню справки
    /// </summary>
    public class ControllerMenuHelpWPF: BaseControllerWPF
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ControllerMenuHelpWPF()
        {
            Menu = new MenuHelp();
            ViewMenu = new ViewMenuHelpWPF(Menu);

            Menu[(int)MenuHelpItemCodes.Back].Selected += () =>
            {
                this.Stop();
                new ControllerMenuMainWPF().Start();
            };

            foreach (MenuItem elMenuItem in Menu.Items)
            {
                ((ViewMenuItemWPF)ViewMenu[elMenuItem.ID]).Enter += id =>
                {
                    Menu.FocusItemById(id);
                    Menu.SelectFocusedItem();
                };
            }
            ((ViewMenuHelpWPF)ViewMenu).Init();
        }
    }
}
