using BaseMenuView;
using BaseMenuModel.Components;

namespace ConsoleView
{
    /// <summary>
    /// Представление консоли для контейнера пунктов меню
    /// </summary>
    public class ViewSubMenuItemConsole : ViewSubMenuItem
    {
        /// <summary>
        /// Высота пункта меню
        /// </summary>
        public int HEIGHT = 1;

        /// <summary>
        /// Создает представление консоли для контейнера пунктов меню
        /// </summary>
        /// <param name="parSubMenuItem">Контейнер пунктов меню</param>
        public ViewSubMenuItemConsole(SubMenuItem parSubMenuItem) : base(parSubMenuItem)
        {
            Height = HEIGHT;
            Width = parSubMenuItem.Text.Length + 2;
        }

        /// <summary>
        /// Нарисовать представление
        /// </summary>
        public override void Draw()
        {
            Console.CursorLeft = X;
            Console.CursorTop = Y;
            Console.Write(string.Format("{0} >", Item.Text));
        }
    }
}
