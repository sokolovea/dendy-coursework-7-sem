namespace BaseMenuModel.Components
{
    /// <summary>
    /// Модель пункта меню
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Делегат для события выбора пункта меню
        /// </summary>
        public delegate void dSelected();

        /// <summary>
        /// Событие выбора пункта меню
        /// </summary>
        public event dSelected Selected = null;

        /// <summary>
        /// Делегат для события фокусировки на пункте меню
        /// </summary>
        public delegate void dFocused();

        /// <summary>
        /// Событие выбора пункта меню
        /// </summary>
        public event dFocused Focused = null;

        /// <summary>
        /// Текущий режим пукта меню
        /// </summary>
        private MenuItemStates _state = MenuItemStates.Normal;

        /// <summary>
        /// Является ли пункт меню полем ввода
        /// </summary>
        private bool _isInput;

        /// <summary>
        /// Текст пункта меню
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// ID пункта меню
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Текущий режим пункта меню
        /// </summary>
        public MenuItemStates State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                if (_state == MenuItemStates.Selected)
                {
                    Selected?.Invoke();
                }
                else if (_state == MenuItemStates.Focused)
                {
                    Focused?.Invoke();
                }
            }
        }

        /// <summary>
        /// Является ли пункт меню полем ввода
        /// </summary>
        public bool IsInput
        {
            get
            {
                return _isInput;
            }
            set
            {
                _isInput = value;
            }
        }

        /// <summary>
        /// Создает модель пункта меню
        /// </summary>
        /// <param name="parId">Id создаваемого пункта</param>
        /// <param name="parText">Текст пункта меню</param>
        public MenuItem(int parId, string parText)
        {
            ID = parId;
            State = MenuItemStates.Normal;
            Text = parText;
        }
    }
}
