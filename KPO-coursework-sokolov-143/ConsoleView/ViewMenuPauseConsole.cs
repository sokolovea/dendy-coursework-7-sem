using BaseMenuModel.Components;
using BaseMenuModel.ConcreteScreen.Difficulty;

namespace ConsoleView
{
    /// <summary>
    /// Вид для меню паузы в консольной версии
    /// </summary>
    public class ViewMenuPauseConsole : ViewMenuConsole
    {
        /// <summary>
        /// Заголовок меню
        /// </summary>
        private string _title;

        /// <summary>
        /// Конструктор, инициализирует вид для меню паузы
        /// </summary>
        /// <param name="parSubMenuItem">Модель меню</param>
        public ViewMenuPauseConsole(Menu parSubMenuItem) : base(parSubMenuItem, false)
        {
            _title = ((MenuPause)parSubMenuItem).Title;
            Init();
        }

        /// <summary>
        /// Отображение всех элементов в меню
        /// </summary>
        public override void Draw()
        {
            Console.Clear();

            Console.CursorTop = 20;
            Console.WriteLine(new string('=', Console.WindowWidth));
            Console.WriteLine(CenterText(_title));
            Console.WriteLine(new string('=', Console.WindowWidth));
            Console.WriteLine();

            foreach (var elViewMenuItem in Menu)
            {
                elViewMenuItem.Draw();
            }
        }
    }
}
