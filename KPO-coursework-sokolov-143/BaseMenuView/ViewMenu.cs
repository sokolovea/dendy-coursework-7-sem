
using BaseMenuModel.Components;

namespace BaseMenuView
{
    /// <summary>
    /// Вид для меню
    /// </summary>
    public abstract class ViewMenu : IViewBase
    {
        /// <summary>
        /// Модель меню
        /// </summary>
        private Menu _menu = null;

        /// <summary>
        /// Словарь представлений пунктов меню
        /// </summary>
        private Dictionary<int, ViewMenuItem> _subMenu = null;
        
        /// <summary>
        /// Массив пунктов меню
        /// </summary>
        protected ViewMenuItem[] Menu => _subMenu.Values.ToArray();

        /// <summary>
        /// Координаты абсциссы начала области меню
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Координаты ординаты начала области меню
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Ширина меню
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// Высота меню
        /// </summary>
        public int Height { get; protected set; }

        /// <summary>
        /// Получить представление пункта меню по его id
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        public ViewMenuItem this[int parId]
        {
            get
            {
                return _subMenu[parId];
            }
        }

        /// <summary>
        /// Создает представление меню
        /// </summary>
        /// <param name="parSubMenuItem">Модель меню</param>
        public ViewMenu(Menu parSubMenuItem)
        {
            _menu = parSubMenuItem;
            _subMenu = new Dictionary<int, ViewMenuItem>();
            foreach (MenuItem elMenuItem in parSubMenuItem.Items)
            {
                _subMenu.Add(elMenuItem.ID, CreateItem(elMenuItem));
            }
            _menu.NeedRedraw += NeedRedraw;
        }
        
        /// <summary>
        /// Перерисовать меню
        /// </summary>
        protected abstract void NeedRedraw();

        /// <summary>
        /// Добавить пункт меню в представление
        /// </summary>
        /// <param name="parMenuItem">Модель пункта меню</param>
        /// <returns></returns>
        protected abstract ViewMenuItem CreateItem(MenuItem parMenuItem);

        /// <summary>
        /// Нарисовать меню
        /// </summary>
        public abstract void Draw();
    }
}
