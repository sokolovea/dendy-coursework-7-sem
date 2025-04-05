using BaseMenuModel.Components;
using BaseMenuModel.ConcreteScreen.Difficulty;
using Engine;
using WPFView;

namespace WPFController
{
    /// <summary>
    /// Кнотроллер для меню паузы
    /// </summary>
    public class ControllerMenuPauseWPF : BaseControllerWPF
    {
        /// <summary>
        /// Текущеее состояние игры и ее настройки
        /// </summary>
        GameCurrentStateAndSettingsSingleton _gameSettings = GameCurrentStateAndSettingsSingleton.GetInstance();
        public ControllerMenuPauseWPF()
        {
            Menu = new MenuPause();
            ViewMenu = new ViewMenuPauseWPF(Menu);

            Menu[(int)MenuPauseItemCodes.Back].Selected += () =>
            {
                this.Stop();
                new ControllerMenuMainWPF().Start();
            };

            Menu[(int)MenuPauseItemCodes.New].Selected += () =>
            {
                this.Stop();
                _gameSettings.IsPaused = false;
                _gameSettings.Reset();
                new ControllerMenuDifficultyWPF().Start();
            };

            Menu[(int)MenuPauseItemCodes.Old].Selected += () =>
            {
                this.Stop();
                new ControllerGameWPF().Start();
            };

            foreach (MenuItem elMenuItem in Menu.Items)
            {
                ((ViewMenuItemWPF)ViewMenu[elMenuItem.ID]).Enter += id =>
                {
                    Menu.FocusItemById(id);
                    Menu.SelectFocusedItem();
                };
            }
            ((ViewMenuPauseWPF)ViewMenu).Init();
        }

    }
}
