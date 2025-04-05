using BaseMenuModel.Components;

namespace BaseMenuModel.ConcreteScreen.Difficulty
{
    /// <summary>
    /// Модель меню для паузы
    /// </summary>
    public class MenuPause: Menu
    {
        /// <summary>
        /// Заголовок меню
        /// </summary>
        private String _title = "Продолжить игру или начать новую?";

        /// <summary>
        /// Заголовок меню
        /// </summary>
        public String Title
        {
            get
            {
                return _title;
            }
            protected set
            {
                _title = value;
            }
        }

        /// <summary>
        /// Создает меню паузы
        /// </summary>
        public MenuPause() : base(Properties.Resources.MainMenuGame)
        {
            AddItem(new MenuItem((int)MenuPauseItemCodes.Back, "Назад"));
            AddItem(new MenuItem((int)MenuPauseItemCodes.Old, "Продолжить"));
            AddItem(new MenuItem((int)MenuPauseItemCodes.New, "Новая игра"));
            FocusItemById((int)MenuPauseItemCodes.Old);
        }
    }
}
