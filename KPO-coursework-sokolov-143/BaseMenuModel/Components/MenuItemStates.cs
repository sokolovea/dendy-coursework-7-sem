namespace BaseMenuModel.Components
{
    /// <summary>
    /// Режим (состояние) пункта меню
    /// </summary>
    public enum MenuItemStates : int
    {
        /// <summary>
        /// Обычное
        /// </summary>
        Normal,
        /// <summary>
        /// Фокус на пункте
        /// </summary>
        Focused,
        /// <summary>
        /// Выбрано
        /// </summary>
        Selected
    }
}
