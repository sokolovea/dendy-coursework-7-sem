using System.Windows.Controls;
using System.Windows;

namespace WPFView
{
    /// <summary>
    /// Вид для главного меню приложения
    /// </summary>
    public class ViewMenuMainWPF : ViewWPF
    {
        /// <summary>
        /// Контейнер для элементов меню
        /// </summary>
        private StackPanel _stackPanel = new StackPanel();

        /// <summary>
        /// Создает вид для главного меню
        /// </summary>
        /// <param name="parSubMenuItem">Пункты меню</param>
        public ViewMenuMainWPF(BaseMenuModel.Components.Menu parSubMenuItem) : base(parSubMenuItem)
        {
        }

        /// <summary>
        /// Инициализация вида
        /// </summary>
        public void Init()
        {
            Window.ShowActivated = true;

            _stackPanel.VerticalAlignment = VerticalAlignment.Center;
            _stackPanel.HorizontalAlignment = HorizontalAlignment.Center;
            Window.Content = _stackPanel;

            SetParentControl(_stackPanel);
            Window.Show();
        }

        /// <summary>
        /// Закрыть окно
        /// </summary>
        public void Close()
        {
            Window.Close();
        }

        /// <summary>
        /// Нарисовать графическое меню
        /// </summary>
        public override void Draw()
        {
            foreach (BaseMenuView.ViewMenuItem elViewMenuItem in Menu)
            {
                elViewMenuItem.Draw();
            }
        }

        /// <summary>
        /// Установить родительский контейнер
        /// </summary>
        /// <param name="parControl">Родительский контейнер</param>
        public override void SetParentControl(FrameworkElement parControl)
        {
            BaseMenuView.ViewMenuItem[] menu = Menu;
            foreach (BaseMenuView.ViewMenuItem elViewMenuItem in menu)
            {
                ((IMenu)elViewMenuItem).SetParentControl(parControl);
            }
        }
    }
}
