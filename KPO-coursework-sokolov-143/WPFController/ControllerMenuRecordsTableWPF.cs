using BaseMenuModel.Components;
using BaseMenuModel.ConcreteScreen.Help;
using WPFView;

namespace WPFController
{
    /// <summary>
    /// Контроллер меню с рекордами
    /// </summary>
    public class ControllerMenuRecordsTableWPF: BaseControllerWPF
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ControllerMenuRecordsTableWPF()
        {
            Menu = new MenuRecordsTable();
            ViewMenu = new ViewMenuRecordsTableWPF(Menu);

            Menu[(int)MenuRecordsItemCodes.Back].Selected += () =>
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
            ((ViewMenuRecordsTableWPF)ViewMenu).Init();
        }
    }
}
