using Engine;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using WPFView.Custom;

namespace WPFView
{
    /// <summary>
    /// Базовый класс для графического представления меню
    /// </summary>
    public class ViewMenuItemWPF : BaseMenuView.ViewMenuItem, WPFView.IMenu
    {
        /// <summary>
        /// Делегат для обработки события нажатия на элемент меню
        /// </summary>
        public delegate void dEnter(int parId);

        /// <summary>
        /// Событие для обработки нажатия на элемент меню
        /// </summary>
        public event dEnter Enter = null;

        /// <summary>
        /// Родительский элемент управления
        /// </summary>
        private FrameworkElement _parentControl = null;

        /// <summary>
        /// Кнопка для отображения элемента меню
        /// </summary>
        private CustomButton _button = null;

        /// <summary>
        /// Поле ввода текста для элемента меню
        /// </summary>
        private CustomTextBox _textBox = null;

        /// <summary>
        /// Текущие настройки игры
        /// </summary>
        GameCurrentStateAndSettingsSingleton _gameCurrentStateAndSettings = GameCurrentStateAndSettingsSingleton.GetInstance();

        /// <summary>
        /// Конструктор для создания элемента меню
        /// </summary>
        /// <param name="parItem">Параметры для элемента меню</param>
        public ViewMenuItemWPF(BaseMenuModel.Components.MenuItem parItem) : base(parItem)
        {
            if (!parItem.IsInput) // Кнопка
            {
                _button = new CustomButton(parItem.Text);
                _button.Click += (s, e) => { Enter?.Invoke(this.Item.ID); };
                Height = (int)_button.Height;
                Width = (int)_button.Width;
                parItem.Selected += ParItem_Selected;
            }
            else // Поле ввода
            {
                _textBox = new CustomTextBox
                {
                    Width = 240,
                    Height = 40,
                    Background = Brushes.White,
                    BorderBrush = Brushes.Black
                };

                _textBox.TextChanged += (s, e) =>
                {
                    _gameCurrentStateAndSettings.PlayerName = _textBox.Text;
                    Enter?.Invoke(this.Item.ID);
                };

                Height = (int)_textBox.Height;
                Width = (int)_textBox.Width;
                _textBox.Text = parItem.Text;

                parItem.Focused += () =>
                {
                    _textBox.SetFocus();
                };
            }
        }

        /// <summary>
        /// Обработчик события выбора элемента меню
        /// </summary>
        private void ParItem_Selected()
        {
            Draw();
        }

        /// <summary>
        /// Отображение элементов меню
        /// </summary>
        public override void Draw()
        {
            if (_button != null)
            {
                _button.Margin = new Thickness(X, Y + 20, 0, 20);
                _button.BorderBrush = this.Item.State == BaseMenuModel.Components.MenuItemStates.Focused
                    ? Brushes.Red
                    : Brushes.Black;
            }
            else if (_textBox != null)
            {
                _textBox.Margin = new Thickness(X, Y + 20, 0, 20);
                _textBox.BorderBrush = this.Item.State == BaseMenuModel.Components.MenuItemStates.Focused
                    ? Brushes.Red
                    : Brushes.Black;
            }
        }

        /// <summary>
        /// Устанавливает родительский элемент для кнопки или поля ввода
        /// </summary>
        /// <param name="parControl">Родительский элемент</param>
        public void SetParentControl(FrameworkElement parControl)
        {
            if (_parentControl == null)
            {
                _parentControl = parControl;

                if (_button != null)
                {
                    ((IAddChild)_parentControl).AddChild(_button);
                }
                else if (_textBox != null)
                {
                    ((IAddChild)_parentControl).AddChild(_textBox);
                }
            }
        }
    }
}
