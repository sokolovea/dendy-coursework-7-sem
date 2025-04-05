using BaseMenuModel.ConcreteScreen.Main;
using Engine;
using ConsoleView;
using ControllerConsole;

namespace ConsoleController
{
    /// <summary>
    /// Контроллер главного меню для консольного приложения
    /// </summary>
    public class ControllerMenuMainConsole : BaseControllerConsole
    {
        /// <summary>
        /// Текущее состояние и настройки игры
        /// </summary>
        GameCurrentStateAndSettingsSingleton _gameSettings = GameCurrentStateAndSettingsSingleton.GetInstance();

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ControllerMenuMainConsole()
        {
            Menu = new MenuMain();
            ViewMenu = new ViewMenuConsole(Menu);
            ViewMenu.InitAllMenuItemsPosition();

            Menu[(int)MenuMainItemCodes.Exit].Selected += () =>
            {
                Stop();
                Environment.Exit(0);
            };
            Menu[(int)MenuMainItemCodes.Help].Selected += () =>
            {
                Stop();
                new ControllerMenuHelpConsole().Start();
            };
            Menu[(int)MenuMainItemCodes.New].Selected += () =>
            {
                Stop();
                if (_gameSettings.IsPaused)
                {
                    new ControllerMenuPauseConsole().Start();
                }
                else
                {
                    new ControllerMenuDifficultyConsole().Start();
                }
            };
            Menu[(int)MenuMainItemCodes.Achievements].Selected += () =>
            {
                Stop();
                new ControllerMenuRecordsTableConsole().Start();
            };

            ((ViewMenuConsole)ViewMenu).Draw();
        }
    }
}
