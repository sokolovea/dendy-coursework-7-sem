using BaseMenuModel.Components;
using BaseMenuView;
using System.Windows;

namespace WPFView
{
    /// <summary>
    /// Базовый класс ыида для различных меню приложения
    /// </summary>
    public abstract class ViewWPF : BaseMenuView.ViewMenu, WPFView.IMenu
    {
        /// <summary>
        /// Минимальная высота окна меню
        /// </summary>
        public static readonly int MENU_HEIGHT = 720;

        /// <summary>
        /// Максимальная ширина окна меню
        /// </summary>
        public static readonly int MENU_WIDTH = 1280;

        /// <summary>
        /// Ссылка на текущее окно
        /// </summary>
        private Window _window = WindowSingleton.GetInstance().GetWindow();

        /// <summary>
        /// Ссылка на текущее окно
        /// </summary>
        public Window Window
        {
            get
            {
                return _window;
            }
            protected set
            {
                _window = value;
            }
        }

        /// <summary>
        /// Конструктор вида
        /// </summary>
        /// <param name="parMenu">Меню вида</param>
        public ViewWPF(Menu parMenu) : base(parMenu)
        {
            Draw();
        }

        /// <summary>
        /// Установить родительский контейнер
        /// </summary>
        /// <param name="parControl">Родительский контейнер</param>
        public virtual void SetParentControl(FrameworkElement parControl)
        {
            BaseMenuView.ViewMenuItem[] menu = Menu;
            foreach (BaseMenuView.ViewMenuItem elViewMenuItem in menu)
            {
                ((IMenu)elViewMenuItem).SetParentControl(parControl);
            }
        }

        /// <summary>
        /// Установить родительский контейнер для конкретного пункта меню
        /// </summary>
        /// <param name="parControl">Родительский контейнер</param>
        /// <param name="parControl">Индекс пункта меню</param>
        public virtual void SetParentControl(FrameworkElement parControl, int parMenuIndex)
        {
            BaseMenuView.ViewMenuItem[] menu = Menu;
            if (parMenuIndex >= 0 || parMenuIndex < menu.Length)
            {
                ((IMenu)menu[parMenuIndex]).SetParentControl(parControl);
            }
        }

        /// <summary>
        /// Добавить пункт меню
        /// </summary>
        /// <param name="parMenuItem">Модель пункта меню</param>
        /// <returns></returns>
        protected override ViewMenuItem CreateItem(MenuItem parMenuItem)
        {
            if (parMenuItem is SubMenuItem)
                return new ViewSubMenuItemWPF((SubMenuItem)parMenuItem);
            else if (parMenuItem is MenuItem)
                return new ViewMenuItemWPF(parMenuItem);
            return null;
        }

        /// <summary>
        /// Перерисовать меню
        /// </summary>
        protected override void NeedRedraw()
        {
            foreach (BaseMenuView.ViewMenuItem elViewMenuItem in Menu)
            {
                elViewMenuItem.Draw();
            }
        }

    }
}
