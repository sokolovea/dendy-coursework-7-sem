using BaseMenuModel.ConcreteScreen.Difficulty;

namespace ConsoleView
{
    /// <summary>
    /// Вид для меню выбора уровня сложности в консольной версии
    /// </summary>
    public class ViewMenuDifficultyConsole : ViewMenuConsole
    {
        /// <summary>
        /// Заголовок меню
        /// </summary>
        private string _title;

        /// <summary>
        /// Конструктор, инициализирует вид для меню выбора уровня сложности
        /// </summary>
        /// <param name="parSubMenuItem">Параметр, передающий данные меню</param>
        public ViewMenuDifficultyConsole(BaseMenuModel.Components.Menu parSubMenuItem) : base(parSubMenuItem, false)
        {
            _title = ((MenuDifficulty)parSubMenuItem).Title;
            Init();
        }

        /// <summary>
        /// Отображение всех элементов в меню
        /// </summary>
        public override void Draw()
        {
            Console.Clear();

            Console.CursorTop = 21;
            Console.WriteLine("=".PadRight(Console.WindowWidth, '='));
            Console.WriteLine(CenterText(_title));
            Console.WriteLine("=".PadRight(Console.WindowWidth, '='));
            Console.WriteLine();

            foreach (var elViewMenuItem in Menu)
            {
                elViewMenuItem.Draw();
            }
        }
    }
}
