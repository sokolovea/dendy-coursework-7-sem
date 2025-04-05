using System.Windows.Controls;
using System.Windows;
using WPFView.Custom;
using BaseMenuModel.ConcreteScreen.Difficulty;

namespace WPFView
{
    /// <summary>
    /// Вид для меню паузы
    /// </summary>
    public class ViewMenuPauseWPF : ViewWPF
    {
        /// <summary>
        /// Контейнер для разметки с элементами управления
        /// </summary>
        private Grid _grid = new Grid();

        /// <summary>
        /// Контейнер для кнопки "Назад"
        /// </summary>
        private StackPanel _stackPanelBack = new StackPanel();

        /// <summary>
        /// Контейнер для остальных кнопок
        /// </summary>
        private StackPanel _stackPanelPause = new StackPanel();

        /// <summary>
        /// Заголовок меню
        /// </summary>
        private String _title = null;

        /// <summary>
        /// Конструктор, инициализирует вид для меню паузы
        /// </summary>
        /// <param name="parSubMenuItem">Параметр, передающий данные меню</param>
        public ViewMenuPauseWPF(BaseMenuModel.Components.Menu parSubMenuItem) : base(parSubMenuItem)
        {
            _title = ((MenuPause)parSubMenuItem).Title;
        }

        /// <summary>
        /// Инициализация компонентов интерфейса меню
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

            Grid.SetRow(_stackPanelBack, 0);
            Grid.SetColumn(_stackPanelBack, 2);
            _grid.Children.Add(_stackPanelBack);

            Window.Content = _grid;

            CustomLabel title = new CustomLabel(_title)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            Grid.SetRow(title, 0);
            Grid.SetColumn(title, 1);
            _grid.Children.Add(title);

            SetParentControl(_stackPanelBack, (int)MenuPauseItemCodes.Back);

            Grid.SetRowSpan(_stackPanelPause, 2);
            Grid.SetRow(_stackPanelPause, 1);
            Grid.SetColumn(_stackPanelPause, 1);
            _grid.Children.Add(_stackPanelPause);

            SetParentControl(_stackPanelPause, (int)MenuPauseItemCodes.Old);
            SetParentControl(_stackPanelPause, (int)MenuPauseItemCodes.New);
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
