namespace BaseMenuModel.Components
{
    /// <summary>
    /// Контейнер для пунктов меню
    /// </summary>
    public class SubMenuItem : MenuItem
    {
        /// <summary>
        /// Словарь пунктов меню
        /// </summary>
        private Dictionary<int, MenuItem> _items = new Dictionary<int, MenuItem>();

        /// <summary>
        /// Пункты меню
        /// </summary>
        public MenuItem[] Items
        {
            get
            {
                return _items.Values.ToArray();
            }
        }

        /// <summary>
        /// Получить пункт меню по его Id
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        public MenuItem this[int parId]
        {
            get
            {
                return _items[parId];
            }
        }

        /// <summary>
        /// Конструктор для контейнера модели меню
        /// </summary>
        /// <param name="parId">Id меню</param>
        /// <param name="parName">Наименование приложения</param>
        public SubMenuItem(int parId, string parName) : base(parId, parName)
        {

        }
        protected void AddItem(MenuItem parMenuItem)
        {
            _items.Add(parMenuItem.ID, parMenuItem);
        }

    }
}
