using BaseMenuModel.Components;
using Engine.Records;
using System.Text;

namespace BaseMenuModel.ConcreteScreen.Help
{
    /// <summary>
    /// Модель меню рекордов
    /// </summary>
    public class MenuRecordsTable: Menu
    {
        /// <summary>
        /// Заголовок таблицы
        /// </summary>
        private String _header = "Логин                        Уровень                " +
            "Результат            Сложность              Очки";


        /// <summary>
        /// Заголовок таблицы
        /// </summary>
        public String Header
        {
            get
            {
                return _header;
            }
        }

        /// <summary>
        /// Получить тело таблицы рекордов
        /// </summary>
        /// <param name="parIsConsole">Будет ли показана таблица в консольном режиме</param>
        /// <returns>Тело таблицы рекордов одной строкой</returns>
        public String GetRecordsTableBody(bool parIsConsole = false)
        {
            var recordManager = new RecordManager();
            StringBuilder allRecords = new StringBuilder();

            // Ширина столбцов
            const int usernameWidth = 33;
            const int levelWidth = 23;
            const int statusWidth = 23;
            const int difficultyWidth = 23;
            const int scoreWidth = 23;

            int maxUsernameStringWidth = parIsConsole ? 30 : 15;

            foreach (var elRecord in recordManager.GetTopRecords())
            {
                string username = elRecord.Username.Length > maxUsernameStringWidth
                    ? elRecord.Username.Substring(0, maxUsernameStringWidth)
                    : elRecord.Username + new string(' ', maxUsernameStringWidth - elRecord.Username.Length);


                allRecords.AppendLine(
                    $"{username.PadRight(usernameWidth)} " +
                    $"{elRecord.Level.ToString().PadRight(levelWidth)} " +
                    $"{elRecord.Status.PadRight(statusWidth)} " +
                    $"{elRecord.Difficulty.PadRight(difficultyWidth)} " +
                    $"{elRecord.Score.ToString().PadRight(scoreWidth)}"
                );
            }

            return allRecords.ToString();
        }

        /// <summary>
        /// Создает модель меню рекордов
        /// </summary>
        public MenuRecordsTable() : base(Properties.Resources.MainMenuGame)
        {
            AddItem(new MenuItem((int)MenuRecordsItemCodes.Back, "Назад"));
            FocusItemById((int)MenuRecordsItemCodes.Back);
        }
    }
}
