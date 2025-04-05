using BaseMenuModel.Components;
using System.Windows;

namespace WPFView
{
    /// <summary>
    /// Графическое представление пункта меню
    /// </summary>
    public class ViewSubMenuItemWPF : BaseMenuView.ViewSubMenuItem, WPFView.IMenu
    {
        /// <summary>
        /// Создает графическое представление пункта меню
        /// </summary>
        /// <param name="parSubMenuItem">Модель подменю</param>
        public ViewSubMenuItemWPF(SubMenuItem parSubMenuItem) : base(parSubMenuItem)
        {

        }

        /// <summary>
        /// Установить родительский контейнер
        /// </summary>
        /// <param name="parControl">Родительский контейнер</param>
        public void SetParentControl(FrameworkElement parControl)
        {
        }

        /// <summary>
        /// Нарисовать пункт меню 
        /// </summary>
        public override void Draw()
        {
        }
    }
}
