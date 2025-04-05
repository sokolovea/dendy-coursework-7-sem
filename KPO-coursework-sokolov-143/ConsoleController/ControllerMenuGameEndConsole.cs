using BaseMenuModel.Components;
using BaseMenuModel.ConcreteScreen.GameEnd;
using BaseMenuModel.ConcreteScreen.Help;
using BaseMenuView;
using ConsoleView;
using ControllerConsole;
using Engine;
using Engine.Enum;
using Engine.Records;

namespace ConsoleController
{
    /// <summary>
    /// Контроллер меню завершения игры и записи рекордов
    /// </summary>
    public class ControllerMenuGameEndConsole : BaseControllerConsole
    {
        /// <summary>
        /// Текущее состояние и настройки игры
        /// </summary>
        GameCurrentStateAndSettingsSingleton _gameCurrentStateAndSettings = GameCurrentStateAndSettingsSingleton.GetInstance();

        /// <summary>
        /// Модель поля ввода
        /// </summary>
        ViewMenuItemConsole _inputMenuViewMenuItem;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="parGameEndStatusEnum">Победа или поражение</param>
        /// <param name="parPlayerGameScore">Счет игрока</param>
        /// <param name="parGameLevel">Уровень, на котором закончилась игра</param>
        /// <param name="parDifficultyEnum">Уровень сложности</param>
        public ControllerMenuGameEndConsole(GameEndStatusEnum parGameEndStatusEnum, int parPlayerGameScore, int parGameLevel, DifficultyEnum parDifficultyEnum)
        {
            _gameCurrentStateAndSettings.IsPaused = false;
            Menu = new MenuGameEnd(parGameEndStatusEnum, parPlayerGameScore);
            ViewMenu = new ViewMenuGameEndConsole(Menu);
            ViewMenu.InitAllMenuItemsPosition();

            Menu[(int)MenuGameEndItemCodes.Ok].Selected += () =>
            {
                var recordManager = new RecordManager();
                string status = parGameEndStatusEnum == GameEndStatusEnum.Victory ? "Победа" : "Поражение";
                string playerName = ((ViewMenuItemConsole)ViewMenu[0]).Text.Length > 0 ?
                    ((ViewMenuItemConsole)ViewMenu[0]).Text : "DEFAULT";
                recordManager.AddRecord(playerName, parPlayerGameScore, parGameLevel, status, parDifficultyEnum.ToString());

                this.Stop();
                new ControllerMenuMainConsole().Start();
            };
            Menu[(int)MenuGameEndItemCodes.Cancel].Selected += () =>
            {
                this.Stop();
                new ControllerMenuMainConsole().Start();
            };

            ((ViewMenuGameEndConsole)ViewMenu).Init();
            _inputMenuViewMenuItem = new ViewMenuItemConsole(Menu);

        }

        /// <summary>
        /// Обработка пользовательского ввода
        /// </summary>
        protected override void ProcessInput()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.LeftArrow:
                    Menu.FocusPrevious();
                    break;

                case ConsoleKey.DownArrow:
                case ConsoleKey.RightArrow:
                    Menu.FocusNext();
                    break;

                case ConsoleKey.Enter:
                    Menu.SelectFocusedItem();
                    break;
            }

            if (Menu.FocusedItemIndex == 0)
            {
                ((ViewMenuItemConsole)ViewMenu[0]).HandleKey(keyInfo);
            }
        }
    }
}
