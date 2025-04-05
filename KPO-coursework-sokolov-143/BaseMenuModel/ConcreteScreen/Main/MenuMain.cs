using BaseMenuModel.Components;

namespace BaseMenuModel.ConcreteScreen.Main
{
    /// <summary>
    /// Модель главного меню
    /// </summary>
    public class MenuMain : Menu
    {
        /// <summary>
        /// Создает главное меню игры
        /// </summary>
        public MenuMain() : base(Properties.Resources.MainMenuGame)
        {
            AddItem(new MenuItem((int)MenuMainItemCodes.New, "Начать игру"));
            AddItem(new MenuItem((int)MenuMainItemCodes.Achievements, "Рекорды"));
            AddItem(new MenuItem((int)MenuMainItemCodes.Help, "Справка"));
            AddItem(new MenuItem((int)MenuMainItemCodes.Exit, "Выход"));

            FocusItemById((int)MenuMainItemCodes.New);
        }

    }
}
