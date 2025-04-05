using BaseMenuModel.ConcreteScreen.Help;
using System.Windows.Controls;
using System.Windows;
using WPFView.Custom;

namespace WPFView
{
    public class ViewMenuRecordsTableWPF : ViewWPF
    {
        /// <summary>
        /// Сетка для размещения элементов
        /// </summary>
        private Grid _grid = new Grid();

        /// <summary>
        /// Панель для размещения элементов
        /// </summary>
        private StackPanel _stackPanel = new StackPanel();

        /// <summary>
        /// Заголовок таблицы рекордов
        /// </summary>
        private String _header = null;

        /// <summary>
        /// Тело таблицы с рекордами
        /// </summary>
        private String _tableBody = null;

        /// <summary>
        /// Конструктор для инициализации данных меню рекордов
        /// </summary>
        /// <param name="parSubMenuItem">Параметры меню</param>
        public ViewMenuRecordsTableWPF(BaseMenuModel.Components.Menu parSubMenuItem) : base(parSubMenuItem)
        {
            _header = ((MenuRecordsTable)parSubMenuItem).Header;
            _tableBody = ((MenuRecordsTable)parSubMenuItem).GetRecordsTableBody();
        }

        /// <summary>
        /// Инициализация окна и размещение элементов
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


            CustomLabel headerLabel = new CustomLabel(_header)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            Grid.SetRow(headerLabel, 1);
            Grid.SetColumnSpan(headerLabel, 3);
            _grid.Children.Add(headerLabel);

            CustomLabel tableBodyLabel = new CustomLabel(_tableBody);
            Grid.SetRow(tableBodyLabel, 2);
            Grid.SetColumnSpan(tableBodyLabel, 3);
            _grid.Children.Add(tableBodyLabel);

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
        /// Отображение элементов меню
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
