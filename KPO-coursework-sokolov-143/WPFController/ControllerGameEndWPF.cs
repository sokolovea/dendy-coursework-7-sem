using BaseMenuModel.ConcreteScreen.GameEnd;
using Engine;
using WPFView;
using BaseMenuModel.Components;
using Engine.Records;
using Engine.Enum;

namespace WPFController
{
    /// <summary>
    /// Контроллер меню записи рекордов
    /// </summary>
    public class ControllerMenuGameEndWPF: BaseControllerWPF
    {
        /// <summary>
        /// Текущее состояние и настройки игры
        /// </summary>
        GameCurrentStateAndSettingsSingleton _gameCurrentStateAndSettings = GameCurrentStateAndSettingsSingleton.GetInstance();

        /// <summary>
        /// Конструктор контроллера меню записи рекордов
        /// </summary>
        /// <param name="parGameEndStatusEnum">Победа или поражение</param>
        /// <param name="parPlayerGameScore">Счет игрока</param>
        /// <param name="parGameLevel">Уровень, на котором закончилась игра</param>
        /// <param name="parDifficultyEnum">Уровень сложности</param>
        public ControllerMenuGameEndWPF(GameEndStatusEnum parGameEndStatusEnum, int parPlayerGameScore, int parGameLevel, DifficultyEnum parDifficultyEnum)
        {
            _gameCurrentStateAndSettings.IsPaused = false;
            Menu = new MenuGameEnd(parGameEndStatusEnum, parPlayerGameScore);
            ViewMenu = new ViewMenuGameEndWPF(Menu);

            Menu[(int)MenuGameEndItemCodes.Ok].Selected += () =>
            {
                var recordManager = new RecordManager();
                string status = parGameEndStatusEnum == GameEndStatusEnum.Victory ? "Победа" : "Поражение"; 
                string playerName = _gameCurrentStateAndSettings.PlayerName.Length > 0 ?
                    _gameCurrentStateAndSettings.PlayerName : "DEFAULT";
                recordManager.AddRecord(playerName, parPlayerGameScore, parGameLevel, status, parDifficultyEnum.ToString());

                this.Stop();
                new ControllerMenuMainWPF().Start();
            };
            Menu[(int)MenuGameEndItemCodes.Cancel].Selected += () =>
            {
                this.Stop();
                new ControllerMenuMainWPF().Start();
            };

            foreach (MenuItem elMenuItem in Menu.Items)
            {
                ((ViewMenuItemWPF)ViewMenu[elMenuItem.ID]).Enter += id =>
                {
                    Menu.FocusItemById(id);
                    Menu.SelectFocusedItem();
                };
            }
            ((ViewMenuGameEndWPF)ViewMenu).Init();
        }
    }
}
