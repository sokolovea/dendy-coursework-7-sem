using BaseMenuModel.ConcreteScreen.Difficulty;
using ConsoleView;
using ControllerConsole;
using Engine.Enum;

namespace ConsoleController
{
    /// <summary>
    /// Контроллер меню выбора сложности игры для консольной версии
    /// </summary>
    public class ControllerMenuDifficultyConsole : BaseControllerConsole
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ControllerMenuDifficultyConsole()
        {
            Menu = new MenuDifficulty();
            ViewMenu = new ViewMenuDifficultyConsole(Menu);
            ViewMenu.InitMenuItemPositionByIndex((int)MenuDifficultyItemCodes.Back, 120, 15);
            ViewMenu.InitMenuItemPositionByIndex((int)MenuDifficultyItemCodes.Easy, ViewMenu.GetMenuRecommendedX(), 25);
            ViewMenu.InitMenuItemPositionByIndex((int)MenuDifficultyItemCodes.Normal, ViewMenu.GetMenuRecommendedX(), 26);
            ViewMenu.InitMenuItemPositionByIndex((int)MenuDifficultyItemCodes.Hardcore, ViewMenu.GetMenuRecommendedX(), 27);

            Menu[(int)MenuDifficultyItemCodes.Back].Selected += () =>
            {
                this.Stop();
                new ControllerMenuMainConsole().Start();
            };

            Menu[(int)MenuDifficultyItemCodes.Easy].Selected += () =>
            {
                this.Stop();
                new ControllerGameConsole(DifficultyEnum.Easy).Start();
            };

            Menu[(int)MenuDifficultyItemCodes.Normal].Selected += () =>
            {
                this.Stop();
                new ControllerGameConsole(DifficultyEnum.Normal).Start();
            };

            Menu[(int)MenuDifficultyItemCodes.Hardcore].Selected += () =>
            {
                this.Stop();
                new ControllerGameConsole(DifficultyEnum.Hardcore).Start();
            };
        }
    }
}
