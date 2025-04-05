using BaseMenuModel.ConcreteScreen.Help;
using System.Windows;
using System.Windows.Controls;
using WPFView.Custom;

namespace WPFView
{
    /// <summary>
    /// Вид для справки
    /// </summary>
    public class ViewMenuHelpWPF : ViewWPF
    {
        /// <summary>
        /// Сетка для размещения элементов управления
        /// </summary>
        private Grid _grid = new Grid();

        /// <summary>
        /// Панель для отображения содержимого (правила и управление)
        /// </summary>
        private StackPanel _stackPanel = new StackPanel();

        /// <summary>
        /// Строки с правилами игры
        /// </summary>
        private String _rules = null;

        /// <summary>
        /// Строки с описанием управления
        /// </summary>
        private String _controls = null;

        /// <summary>
        /// Конструктор, инициализирует вид для меню помощи
        /// </summary>
        /// <param name="parSubMenuItem">Параметр, передающий данные меню</param>
        public ViewMenuHelpWPF(BaseMenuModel.Components.Menu parSubMenuItem) : base(parSubMenuItem)
        {
            _rules = ((MenuHelp)parSubMenuItem).Rules;
            _controls = ((MenuHelp)parSubMenuItem).Controls;
        }

        /// <summary>
        /// Инициализация компонентов интерфейса меню помощи
        /// </summary>
        public void Init()
        {
            Window.ShowActivated = true;

            _grid.VerticalAlignment = VerticalAlignment.Stretch;
            _grid.HorizontalAlignment = HorizontalAlignment.Stretch;

            for (int i = 0; i < 3; i++)
            {
                _grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            Grid.SetRow(_stackPanel, 0);
            Grid.SetColumn(_stackPanel, 2);
            _grid.Children.Add(_stackPanel);

            Window.Content = _grid;

            CustomLabel rules = new CustomLabel(_rules)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            Grid.SetRow(rules, 1);
            Grid.SetColumnSpan(rules, 3);
            _grid.Children.Add(rules);

            CustomLabel controls = new CustomLabel(_controls);
            Grid.SetRow(controls, 2);
            Grid.SetColumnSpan(controls, 3);
            _grid.Children.Add(controls);

            SetParentControl(_stackPanel);

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
    }
}
