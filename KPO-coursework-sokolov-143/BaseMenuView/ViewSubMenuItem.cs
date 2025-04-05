using BaseMenuModel.Components;

namespace BaseMenuView
{
    /// <summary>
    /// Представление для контейнера пунктов меню
    /// </summary>
    public abstract class ViewSubMenuItem : ViewMenuItem
    {
        /// <summary>
        /// Список представлений пунктов меню
        /// </summary>
        private List<ViewMenuItem> _items = new List<ViewMenuItem>();

        /// <summary>
        /// Создает представление для контейнера пунктов меню
        /// </summary>
        /// <param name="parSubMenuItem">Модель контейнера пунктов меню</param>
        public ViewSubMenuItem(SubMenuItem parSubMenuItem) : base(parSubMenuItem)
        {

        }

        /// <summary>
        /// Добавить представление нового пункта меню
        /// </summary>
        /// <param name="parViewMenuItem">Представление нового пункта меню</param>
        protected void AddItem(ViewMenuItem parViewMenuItem)
        {
            _items.Add(parViewMenuItem);
        }
    }
}
