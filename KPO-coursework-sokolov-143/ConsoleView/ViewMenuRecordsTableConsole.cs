using BaseMenuModel.ConcreteScreen.Help;

namespace ConsoleView
{
    /// <summary>
    /// Вид для таблицы рекордов в консоли
    /// </summary>
    public class ViewMenuRecordsTableConsole : ViewMenuConsole
    {
        /// <summary>
        /// Заголовок таблицы рекордов
        /// </summary>
        private string _header;

        /// <summary>
        /// Тело таблицы с рекордами
        /// </summary>
        private string _tableBody;

        /// <summary>
        /// Конструктор для инициализации данных меню таблицы рекордов
        /// </summary>
        /// <param name="parSubMenuItem">Модель меню</param>
        public ViewMenuRecordsTableConsole(BaseMenuModel.Components.Menu parSubMenuItem) : base(parSubMenuItem, false)
        {
            _header = ((MenuRecordsTable)parSubMenuItem).Header;
            _tableBody = ((MenuRecordsTable)parSubMenuItem).GetRecordsTableBody(true);
            Init();
        }

        /// <summary>
        /// Инициализация компонентов интерфейса меню рекордов
        /// </summary>
        public void Init()
        {
            Console.Clear();
            Draw();
        }

        /// <summary>
        /// Отображение всех элементов в консольном меню
        /// </summary>
        public override void Draw()
        {
            Console.Clear();

            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            string[] headerLines = _header.Split('\n');
            string[] tableBodyLines = _tableBody.Split('\n');

            int totalLines = headerLines.Length + 1 + tableBodyLines.Length + 1 + Menu.Length;

            int startY = (consoleHeight / 2) - (totalLines / 2);
            int currentY = startY;

            foreach (string line in headerLines)
            {
                Console.SetCursorPosition((consoleWidth - line.Length) / 2, currentY++);
                Console.WriteLine(line);
            }

            currentY++;

            foreach (string line in tableBodyLines)
            {
                Console.SetCursorPosition((consoleWidth - line.Length) / 2, currentY++);
                Console.WriteLine(line);
            }

            currentY++;

            foreach (var elViewMenuItem in Menu)
            {
                Console.SetCursorPosition((consoleWidth - elViewMenuItem.Width) / 2, currentY++);
                elViewMenuItem.Draw();
            }
        }
    }
}
