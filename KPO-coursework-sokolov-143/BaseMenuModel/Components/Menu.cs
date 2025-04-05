namespace BaseMenuModel.Components
{
    /// <summary>
    /// Модель меню
    /// </summary>
    public class Menu : SubMenuItem
    {
        /// <summary>
        /// Делегат для события перерисовки меню
        /// </summary>
        public delegate void dNeedRedraw();

        /// <summary>
        /// Событие для перерисовки меню
        /// </summary>
        public event dNeedRedraw NeedRedraw = null;

        /// <summary>
        /// Индекс активного пункта меню 
        /// </summary>
        private int _focusedItemIndex = -1;
        
        /// <summary>
        /// Индекс активного пункта меню 
        /// </summary>
        public int FocusedItemIndex
        {
            get
            {
                return _focusedItemIndex;
            }
            protected set
            {
                _focusedItemIndex = value;
            }
        }

        /// <summary>
        /// Конструктор для модели меню
        /// </summary>
        /// <param name="parName">Наименование приложения</param>
        public Menu(string parName) : base(0, parName)
        {
            if (Items.Length > 0)
            {
                _focusedItemIndex = 0;
                Items[0].State = MenuItemStates.Focused;
            }
        }

        /// <summary>
        /// Фокус на следующем элементе меню
        /// </summary>
        public void FocusNext()
        {
            if (Items.Length == 0)
            {
                return;
            }
            int savFocusedIndex = _focusedItemIndex;
            _focusedItemIndex = (_focusedItemIndex + 1) % Items.Length;
            UpdateFocusState(savFocusedIndex);
        }

        /// <summary>
        /// Фокус на предыдущем элементе меню
        /// </summary>
        public void FocusPrevious()
        {
            if (Items.Length == 0)
            {
                return;
            }
            int savFocusedIndex = _focusedItemIndex;
            _focusedItemIndex = (_focusedItemIndex - 1 + Items.Length) % Items.Length;
            UpdateFocusState(savFocusedIndex);
        }

        /// <summary>
        /// Сфокусироваться на определенном элементе меню
        /// </summary>
        /// <param name="parId">Id элемента для фокусировки</param>
        /// <exception cref="ArgumentException">Исключение, выбрасываемое при
        /// несуществующем индексе меню</exception>
        public void FocusItemById(int parId)
        {
            if (Items.Length == 0) return; // Проверяем, есть ли элементы

            int savFocusedIndex = _focusedItemIndex;
            MenuItem menuItem = this[parId];
            _focusedItemIndex = Array.IndexOf(Items, menuItem);

            if (_focusedItemIndex == -1)
                throw new ArgumentException($"Item with ID {parId} not found in the menu.");

            UpdateFocusState(savFocusedIndex);
        }

        /// <summary>
        /// Выбрать пункт меню
        /// </summary>
        /// <exception cref="InvalidOperationException">Выбрасывается при отсутствии
        /// активированного пункта</exception>
        public void SelectFocusedItem()
        {
            if (_focusedItemIndex < 0 || _focusedItemIndex >= Items.Length)
                throw new InvalidOperationException("No item is currently focused.");

            if (Items[_focusedItemIndex].IsInput)
            {
                //FocusNext();
            }
            else
            {
                Items[_focusedItemIndex].State = MenuItemStates.Selected;
            }
            NeedRedraw?.Invoke();
        }

        /// <summary>
        /// Обновить индекс выбранного пункта меню
        /// </summary>
        /// <param name="previousIndex"></param>
        private void UpdateFocusState(int previousIndex)
        {
            if (previousIndex != -1 && previousIndex < Items.Length)
                Items[previousIndex].State = MenuItemStates.Normal;

            if (_focusedItemIndex != -1 && _focusedItemIndex < Items.Length)
                Items[_focusedItemIndex].State = MenuItemStates.Focused;

            NeedRedraw?.Invoke();
        }
    }
}
