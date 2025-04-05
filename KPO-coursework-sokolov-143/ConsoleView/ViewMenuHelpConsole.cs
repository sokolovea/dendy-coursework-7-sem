using BaseMenuModel.Components;
using BaseMenuModel.ConcreteScreen.Help;

namespace ConsoleView
{
    /// <summary>
    /// Вид для справки в консоли
    /// </summary>
    public class ViewMenuHelpConsole : ViewMenuConsole
    {
        /// <summary>
        /// Строка с правилами игры
        /// </summary>
        private string _rules;

        /// <summary>
        /// Строка с описанием управления
        /// </summary>
        private string _controls;

        /// <summary>
        /// Конструктор, инициализирует вид для меню справки
        /// </summary>
        /// <param name="parSubMenuItem">Модель меню</param>
        public ViewMenuHelpConsole(Menu parSubMenuItem) : base(parSubMenuItem, false)
        {
            _rules = ((MenuHelp)parSubMenuItem).Rules;
            _controls = ((MenuHelp)parSubMenuItem).Controls;
            Init();
        }

        /// <summary>
        /// Инициализация компонентов интерфейса меню справки
        /// </summary>
        public new void Init()
        {
            Console.Clear();
            Draw();
        }

        /// <summary>
        /// Закрытие окна
        /// </summary>
        public void Close()
        {
            Console.Clear();
            Console.CursorVisible = true;
        }

        /// <summary>
        /// Отображение всех элементов в меню
        /// </summary>
        public override void Draw()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            string[] rulesLines = _rules.Split('\n');
            string[] controlsLines = _controls.Split('\n');

            int totalLines = 3 + rulesLines.Length + 1 + controlsLines.Length + 1 + Menu.Length;

            int startY = (consoleHeight / 2) - (totalLines / 2);
            int currentY = startY;

            Console.SetCursorPosition((consoleWidth - "=== Правила игры ===".Length) / 2, currentY++);
            Console.WriteLine("=== Правила игры ===");

            foreach (string line in rulesLines)
            {
                Console.SetCursorPosition((consoleWidth - line.Length) / 2, currentY++);
                Console.WriteLine(line);
            }

            currentY++;

            Console.SetCursorPosition((consoleWidth - "=== Управление ===".Length) / 2, currentY++);
            Console.WriteLine("=== Управление ===");

            foreach (string line in controlsLines)
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
