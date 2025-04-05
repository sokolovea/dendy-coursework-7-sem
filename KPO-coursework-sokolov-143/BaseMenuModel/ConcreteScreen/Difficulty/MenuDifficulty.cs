using BaseMenuModel.Components;
using Engine;

namespace BaseMenuModel.ConcreteScreen.Difficulty
{
    /// <summary>
    /// Меню выбора уровня сложности игры
    /// </summary>
    public class MenuDifficulty: Menu
    {
        /// <summary>
        /// Заголовок меню
        /// </summary>
        private String _title = "Выберите уровень сложности";

        /// <summary>
        /// Заголовок меню
        /// </summary>
        public String Title
        {
            get
            {
                return _title;
            }
        }

        /// <summary>
        /// Меню выбора уровня сложности игры
        /// </summary>
        public MenuDifficulty() : base(Properties.Resources.MainMenuGame)
        {
            GameCurrentStateAndSettingsSingleton gameSettings = GameCurrentStateAndSettingsSingleton.GetInstance();
            gameSettings.Reset();
            AddItem(new MenuItem((int)MenuDifficultyItemCodes.Back, "Назад"));
            AddItem(new MenuItem((int)MenuDifficultyItemCodes.Easy, "Легкий"));
            AddItem(new MenuItem((int)MenuDifficultyItemCodes.Normal, "Средний"));
            AddItem(new MenuItem((int)MenuDifficultyItemCodes.Hardcore, "Сложный"));
            FocusItemById((int)MenuDifficultyItemCodes.Easy);
        }
    }
}
