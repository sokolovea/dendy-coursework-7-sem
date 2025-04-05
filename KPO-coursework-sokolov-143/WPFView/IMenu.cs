using System.Windows;

namespace WPFView
{
    /// <summary>
    /// Интерфейс для всех графических меню
    /// </summary>
    public interface IMenu
    {
        /// <summary>
        /// Установить родительский элемент
        /// </summary>
        /// <param name="parControl">Родитель</param>
        void SetParentControl(FrameworkElement parControl);
    }
}
