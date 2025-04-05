using BaseMenuModel.Components;
using BaseMenuView;

namespace ConsoleView
{
    /// <summary>
    /// Вид для главного меню приложения в консоли
    /// </summary>
    public class ViewMenuMainConsole : ViewMenuConsole
    {
        /// <summary>
        /// Конструктор для создания представления главного меню
        /// </summary>
        /// <param name="parSubMenuItem">Модель меню</param>
        public ViewMenuMainConsole(Menu parSubMenuItem) : base(parSubMenuItem)
        {
        }

        /// <summary>
        /// Инициализация представления
        /// </summary>
        public new void Init()
        {
            Console.WindowHeight = HEIGHT;
            Console.WindowWidth = WIDTH;

            Console.SetBufferSize(WIDTH, HEIGHT);

            Console.CursorVisible = false;

            ViewMenuItem[] menu = Menu;
            Height = menu.Length;
            Width = menu.Max((x) => x.Width);

            X = Console.WindowWidth / 2 - Width / 2;
            Y = Console.WindowHeight / 2 - Height / 2;

            int y = Y;
            foreach (ViewMenuItem elViewMenuItem in menu)
            {
                elViewMenuItem.X = X;
                elViewMenuItem.Y = y++;
            }
        }

        /// <summary>
        /// Закрыть меню
        /// </summary>
        public void Close()
        {
            Console.Clear();
            Console.CursorVisible = true;
        }

        /// <summary>
        /// Нарисовать текстовое меню
        /// </summary>
        public override void Draw()
        {
            Console.Clear();

            foreach (ViewMenuItem elViewMenuItem in Menu)
            {
                elViewMenuItem.Draw();
            }
        }
    }
}
