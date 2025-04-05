using BaseMenuModel.ConcreteScreen.Difficulty;
using Engine;
using ConsoleView;
using ControllerConsole;

namespace ConsoleController
{
    /// <summary>
    /// Контроллер для меню паузы в консольной версии
    /// </summary>
    public class ControllerMenuPauseConsole : BaseControllerConsole
    {
        /// <summary>
        /// Текущее состояние игры и ее настройки
        /// </summary>
        private GameCurrentStateAndSettingsSingleton _gameSettings = GameCurrentStateAndSettingsSingleton.GetInstance();

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        public ControllerMenuPauseConsole()
        {
            Menu = new MenuPause();
            ViewMenu = new ViewMenuPauseConsole(Menu);
            ViewMenu.InitMenuItemPositionByIndex((int)MenuPauseItemCodes.Back, 120, 15);
            ViewMenu.InitMenuItemPositionByIndex((int)MenuPauseItemCodes.Old, ViewMenu.GetMenuRecommendedX(), 25);
            ViewMenu.InitMenuItemPositionByIndex((int)MenuPauseItemCodes.New, ViewMenu.GetMenuRecommendedX(), 26);

            Menu[(int)MenuPauseItemCodes.Back].Selected += () =>
            {
                this.Stop();
                new ControllerMenuMainConsole().Start();
            };

            Menu[(int)MenuPauseItemCodes.New].Selected += () =>
            {
                this.Stop();
                _gameSettings.IsPaused = false;
                _gameSettings.Reset();
                new ControllerMenuDifficultyConsole().Start();
            };

            Menu[(int)MenuPauseItemCodes.Old].Selected += () =>
            {
                this.Stop();
                new ControllerGameConsole().Start();
            };
        }
    }
}
