using System.Windows.Controls;
using System.Windows;
using WPFView.Custom;
using BaseMenuModel.ConcreteScreen.Difficulty;

namespace WPFView
{
    /// <summary>
    /// Вид для меню выбора уровня сложности
    /// </summary>
    public class ViewMenuDifficultyWPF : ViewWPF
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
        /// Контейнер для кнопок выбора сложности
        /// </summary>
        private StackPanel _stackPanelDifficulty = new StackPanel();

        /// <summary>
        /// Заголовок меню
        /// </summary>
        private String _title = null;

        /// <summary>
        /// Конструктор, инициализирует вид для меню выбора уровня сложности
        /// </summary>
        /// <param name="parSubMenuItem">Параметр, передающий данные меню</param>
        public ViewMenuDifficultyWPF(BaseMenuModel.Components.Menu parSubMenuItem) : base(parSubMenuItem)
        {
            _title = ((MenuDifficulty)parSubMenuItem).Title;
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

            SetParentControl(_stackPanelBack, (int)MenuDifficultyItemCodes.Back);

            Grid.SetRowSpan(_stackPanelDifficulty, 2);
            Grid.SetRow(_stackPanelDifficulty, 1);
            Grid.SetColumn(_stackPanelDifficulty, 1);
            _grid.Children.Add(_stackPanelDifficulty);

            SetParentControl(_stackPanelDifficulty, (int)MenuDifficultyItemCodes.Easy);
            SetParentControl(_stackPanelDifficulty, (int)MenuDifficultyItemCodes.Normal);
            SetParentControl(_stackPanelDifficulty, (int)MenuDifficultyItemCodes.Hardcore);
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
