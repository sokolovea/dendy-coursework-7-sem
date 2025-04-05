using BaseMenuModel.Components;
using Engine;

namespace BaseMenuModel.ConcreteScreen.GameEnd
{
    /// <summary>
    /// Модель меню завершения игры (запись рекордов)
    /// </summary>
    public class MenuGameEnd : Menu
    {
        /// <summary>
        /// Заголовое меню
        /// </summary>
        private String _title;

        /// <summary>
        /// Счет игрока
        /// </summary>
        private String _score;

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
        /// Счет игрока
        /// </summary>
        public String Score
        {
            get
            {
                return _score;
            }
            protected set
            {
                _score = value;
            }
        }

        /// <summary>
        /// Создает меню записи рекордов
        /// </summary>
        /// <param name="parGameEndStatusEnum">Победа или поражение</param>
        /// <param name="parPlayerGameScore">Счет игрока</param>
        public MenuGameEnd(GameEndStatusEnum parGameEndStatusEnum, int parPlayerGameScore) : base(Properties.Resources.MainMenuGame)
        {
            if (parGameEndStatusEnum == GameEndStatusEnum.Victory)
            {
                Title = "Победа!";
            }
            else
            {
                Title = "Поражение!";
            }
            Title += " Введите логин:";
            Score = $"Игровой счет: {parPlayerGameScore}";
            AddItem(new MenuItem((int)MenuGameEndItemCodes.PlayerName, "")
            {
                IsInput = true,
            });
            AddItem(new MenuItem((int)MenuGameEndItemCodes.Ok, "Записать рекорд"));
            AddItem(new MenuItem((int)MenuGameEndItemCodes.Cancel, "Выйти без записи"));
            FocusItemById((int)MenuGameEndItemCodes.Ok);
        }
    }
}
