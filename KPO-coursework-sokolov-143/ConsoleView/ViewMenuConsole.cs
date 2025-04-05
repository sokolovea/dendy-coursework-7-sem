using BaseMenuView;
using BaseMenuModel.Components;

namespace ConsoleView
{
    /// <summary>
    /// Базовый класс для консольных представлений меню
    /// </summary>
    public class ViewMenuConsole : ViewMenu
    {
        /// <summary>
        /// Ширина консоли
        /// </summary>
        public static readonly int WIDTH = 150;

        /// <summary>
        /// Высота консоли
        /// </summary>
        public static readonly int HEIGHT = 45;

        /// <summary>
        /// Создает представление меню для консоли
        /// </summary>
        /// <param name="parSubMenuItem">Модель меню для отрисовки</param>
        /// <param name="parIsDrawImmediatelyNeed">Нужна ли отрисовка меню немедленно</param>
        public ViewMenuConsole(Menu parSubMenuItem, bool parIsDrawImmediatelyNeed = true) : base(parSubMenuItem)
        {
            Init();
            if (parIsDrawImmediatelyNeed)
            {
                Draw();
            }
        }

        /// <summary>
        /// Создает представление пункта меню для консоли
        /// </summary>
        /// <param name="parMenuItem">Модель пункта меню</param>
        /// <returns>Представление пункта меню для консоли</returns>
        protected override ViewMenuItem CreateItem(MenuItem parMenuItem)
        {
            if (parMenuItem is SubMenuItem)
                return new ViewSubMenuItemConsole((SubMenuItem)parMenuItem);
            else if (parMenuItem is MenuItem)
                return new ViewMenuItemConsole(parMenuItem);
            return null;
        }

        /// <summary>
        /// Инициализация представления меню
        /// </summary>
        protected void Init()
        {
            Console.WindowHeight = HEIGHT;
            Console.WindowWidth = WIDTH;

            Console.SetBufferSize(WIDTH, HEIGHT);

            Console.CursorVisible = false;

            ViewMenuItem[] menu = Menu;
            Height = menu.Length;
            Width = menu.Max((x) => x.Width);
        }

        /// <summary>
        /// Отрисовать все пункты меню автоматически
        /// </summary>
        public void InitAllMenuItemsPosition()
        {
            X = Console.WindowWidth / 2 - Width / 2;
            Y = Console.WindowHeight / 2 - Height / 2;

            int y = Y;
            foreach (ViewMenuItem elViewMenuItem in Menu)
            {
                elViewMenuItem.X = X;
                elViewMenuItem.Y = y++;
            }
        }

        /// <summary>
        /// Отрисовать конкретный пункт меню на определенной позиции экрана
        /// </summary>
        /// <param name="parMenuItemIndex">Индекс пункта меню для отрисовки</param>
        /// <param name="parX">Абсцисса позиции отрисовки</param>
        /// <param name="parY">Ордината позиции отрисовки</param>
        public void InitMenuItemPositionByIndex(int parMenuItemIndex, int parX, int parY)
        {
            if ((parMenuItemIndex < 0) || (parMenuItemIndex >= Menu.Length))
            {
                return;
            }
            Menu[parMenuItemIndex].X = parX;
            Menu[parMenuItemIndex].Y = parY;
        }

        /// <summary>
        /// Получить рекомендованное значение для абсциссы начала отрисовки пункта меню
        /// </summary>
        /// <returns>Рекомендованное значение для абсциссы начала отрисовки пункта меню</returns>
        public int GetMenuRecommendedX()
        {
            return Console.WindowWidth / 2 - Width / 2;
        }

        /// <summary>
        /// Вызывается при необходимости перерисовки меню
        /// </summary>
        protected override void NeedRedraw()
        {
            Draw();
        }

        /// <summary>
        /// Нарисовать меню
        /// </summary>
        public override void Draw()
        {
            Console.Clear();
            foreach (ViewMenuItem elViewMenuItem in Menu)
            {
                elViewMenuItem.Draw();
            }
        }

        /// <summary>
        /// Центрирует текст относительно ширины консоли
        /// </summary>
        /// <param name="parText">Текст для центрирования</param>
        /// <returns>Центрированный текст</returns>
        public string CenterText(string parText)
        {
            int padding = (Console.WindowWidth - parText.Length) / 2;
            return parText.PadLeft(padding + parText.Length).PadRight(Console.WindowWidth);
        }
    }
}
