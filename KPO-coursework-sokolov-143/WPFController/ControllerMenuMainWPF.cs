using BaseMenuModel.ConcreteScreen.Main;
using BaseMenuModel.Components;
using WPFView;
using Engine;

namespace WPFController
{
    /// <summary>
    /// Контроллер главного меню приложения
    /// </summary>
    public class ControllerMenuMainWPF : BaseControllerWPF
    {
        /// <summary>
        /// Текущее состояние игры и ее настройки
        /// </summary>
        GameCurrentStateAndSettingsSingleton _gameSettings = GameCurrentStateAndSettingsSingleton.GetInstance();

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ControllerMenuMainWPF()
        {
            Menu = new MenuMain();
            ViewMenu = new ViewMenuMainWPF(Menu);

            Menu[(int)MenuMainItemCodes.Exit].Selected += () => ((ViewMenuMainWPF)ViewMenu).Close();
            Menu[(int)MenuMainItemCodes.Help].Selected += () =>
            {
                this.Stop();
                new ControllerMenuHelpWPF().Start();
            };
            Menu[(int)MenuMainItemCodes.New].Selected += () =>
            {
                this.Stop();
                if (_gameSettings.IsPaused)
                {
                    new ControllerMenuPauseWPF().Start();
                }
                else
                {
                    new ControllerMenuDifficultyWPF().Start();
                }
            };
            Menu[(int)MenuMainItemCodes.Achievements].Selected += () =>
            {
                this.Stop();
                new ControllerMenuRecordsTableWPF().Start();
            };

            foreach (MenuItem elMenuItem in Menu.Items)
            {
                ((ViewMenuItemWPF)ViewMenu[elMenuItem.ID]).Enter += id =>
                {
                    Menu.FocusItemById(id);
                    Menu.SelectFocusedItem();
                };
            }
            ((ViewMenuMainWPF)ViewMenu).Init();
        }

    }
}

