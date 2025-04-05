using BaseMenuModel.Components;
using BaseMenuModel.ConcreteScreen.Difficulty;
using Engine.Enum;
using WPFView;

namespace WPFController
{
    /// <summary>
    /// Контроллер меню выбора сложности игры
    /// </summary>
    public class ControllerMenuDifficultyWPF : BaseControllerWPF
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ControllerMenuDifficultyWPF()
        {
            Menu = new MenuDifficulty();
            ViewMenu = new ViewMenuDifficultyWPF(Menu);

            Menu[(int)MenuDifficultyItemCodes.Back].Selected += () =>
            {
                this.Stop();
                new ControllerMenuMainWPF().Start();
            };

            Menu[(int)MenuDifficultyItemCodes.Easy].Selected += () =>
            {
                this.Stop();
                new ControllerGameWPF(DifficultyEnum.Easy).Start();
            };

            Menu[(int)MenuDifficultyItemCodes.Normal].Selected += () =>
            {
                this.Stop();
                new ControllerGameWPF(DifficultyEnum.Normal).Start();
            };

            Menu[(int)MenuDifficultyItemCodes.Hardcore].Selected += () =>
            {
                this.Stop();
                new ControllerGameWPF(DifficultyEnum.Hardcore).Start();
            };

            foreach (MenuItem elMenuItem in Menu.Items)
            {
                ((ViewMenuItemWPF)ViewMenu[elMenuItem.ID]).Enter += id =>
                {
                    Menu.FocusItemById(id);
                    Menu.SelectFocusedItem();
                };
            }
            ((ViewMenuDifficultyWPF)ViewMenu).Init();
        }
    }
}
