using BaseMenuModel.Components;

namespace BaseMenuView
{
    /// <summary>
    /// Представление пункта меню
    /// </summary>
    public abstract class ViewMenuItem : IViewBase
    {
        /// <summary>
        /// Модель пункта меню
        /// </summary>
        private MenuItem _item = null;

        /// <summary>
        /// Модель пункта меню
        /// </summary>
        public MenuItem Item
        {
            get
            {
                return _item;
            }
        }

        /// <summary>
        /// Абсцисса пункта меню
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Ордината пункта меню
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Ширина пункта меню
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// Высота пункта меню
        /// </summary>
        public int Height { get; protected set; }

        /// <summary>
        /// Создает модель пункта меню
        /// </summary>
        /// <param name="parItem">Модель пункта меню</param>
        public ViewMenuItem(MenuItem parItem)
        {
            _item = parItem;
        }

        /// <summary>
        /// Нарисовать пункт меню
        /// </summary>
        public abstract void Draw();
    }
}
