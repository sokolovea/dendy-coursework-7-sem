using System.Windows.Controls;
using System.Windows;
using WPFView.Custom;
using BaseMenuModel.ConcreteScreen.GameEnd;

namespace WPFView
{
    /// <summary>
    /// Вид меню подтверждения сохранения рекордов в конце игры
    /// </summary>
    public class ViewMenuGameEndWPF : ViewWPF
    {
        /// <summary>
        /// Панель для отображения содержимого экрана
        /// </summary>
        private StackPanel _stackPanelScreen = new StackPanel();

        /// <summary>
        /// Панель для отображения меню с опциями (например, кнопок)
        /// </summary>
        private StackPanel _stackPanelMenu = new StackPanel();

        /// <summary>
        /// Объект, содержащий данные для меню окончания игры
        /// </summary>
        private MenuGameEnd _menuGameEnd;

        /// <summary>
        /// Инициализирует вид для заданного меню
        /// </summary>
        /// <param name="parSubMenuItem">Параметр, передающий данные меню</param>
        public ViewMenuGameEndWPF(BaseMenuModel.Components.Menu parSubMenuItem) : base(parSubMenuItem)
        {
            _menuGameEnd = (MenuGameEnd)parSubMenuItem;
        }

        /// <summary>
        /// Инициализация компонентов интерфейса меню окончания игры
        /// </summary>
        public void Init()
        {
            Window.ShowActivated = true;
            _stackPanelScreen.VerticalAlignment = VerticalAlignment.Center;
            _stackPanelScreen.HorizontalAlignment = HorizontalAlignment.Center;

            _stackPanelMenu.VerticalAlignment = VerticalAlignment.Center;
            _stackPanelMenu.HorizontalAlignment = HorizontalAlignment.Center;

            _stackPanelScreen.Children.Add(new CustomLabel(_menuGameEnd.Title));
            _stackPanelScreen.Children.Add(new CustomLabel(_menuGameEnd.Score));
            _stackPanelScreen.Children.Add(_stackPanelMenu);
            Window.Content = _stackPanelScreen;

            SetParentControl(_stackPanelMenu);
            Window.Show();
        }

        /// <summary>
        /// Закрытие окна
        /// </summary>
        public void Close()
        {
            Window.Close();
        }

        /// <summary>
        /// Отображение всех элементов в меню
        /// </summary>
        public override void Draw()
        {
            foreach (BaseMenuView.ViewMenuItem elViewMenuItem in Menu)
            {
                elViewMenuItem.Draw();
            }
        }

        /// <summary>
        /// Устанавливает родительский элемент управления для всех элементов меню
        /// </summary>
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
