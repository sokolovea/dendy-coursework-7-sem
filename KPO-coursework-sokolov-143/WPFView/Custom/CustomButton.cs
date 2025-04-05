using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFView.Custom
{
    /// <summary>
    /// Кастомная реализация кнопки
    /// </summary>
    public class CustomButton : FrameworkElement
    {
        /// <summary>
        /// Цвет фона кнопки
        /// </summary>
        private Brush _background;

        /// <summary>
        /// Цвет текста кнопки
        /// </summary>
        private Brush _foreground;

        /// <summary>
        /// Цвет границы кнопки
        /// </summary>
        private Brush _borderBrush;

        /// <summary>
        /// Толщина границы кнопки
        /// </summary>
        private double _borderThickness;

        /// <summary>
        /// Надпись на кнопке
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Событие, вызываемое при нажатии кнопки
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        /// Цвет фона кнопки
        /// </summary>
        public Brush Background
        {
            get => _background;
            set
            {
                _background = value;
                InvalidateVisual();
            }
        }

        /// <summary>
        /// Цвет текста кнопки
        /// </summary>
        public Brush Foreground
        {
            get => _foreground;
            set
            {
                _foreground = value;
                InvalidateVisual();
            }
        }

        /// <summary>
        /// Цвет границы кнопки
        /// </summary>
        public Brush BorderBrush
        {
            get => _borderBrush;
            set
            {
                _borderBrush = value;
                InvalidateVisual();
            }
        }

        /// <summary>
        /// Толщина границы кнопки
        /// </summary>
        public double BorderThickness
        {
            get => _borderThickness;
            set
            {
                _borderThickness = value;
                InvalidateVisual();
            }
        }

        /// <summary>
        /// Конструктор, создающий кнопку с текстом
        /// </summary>
        /// <param name="parContent">Текст, отображаемый на кнопке</param>
        public CustomButton(string parContent)
        {
            Content = parContent;
            _background = Brushes.LightGray;
            _foreground = Brushes.Black;
            _borderBrush = Brushes.Black;
            _borderThickness = 3;

            this.Width = 240;
            this.Height = 80;

            this.MouseLeftButtonDown += CustomButton_MouseLeftButtonDown;
            this.MouseEnter += CustomButton_MouseEnter;
            this.MouseLeave += CustomButton_MouseLeave;

            this.Focusable = true;
        }

        /// <summary>
        /// Обработчик события клика левой кнопкой мыши
        /// </summary>
        /// <param name="parSender">Отправитель сообщения</param>
        /// <param name="parE">Подробности события</param>
        private void CustomButton_MouseLeftButtonDown(object parSender, MouseButtonEventArgs parE)
        {
            if (IsFocused)
            {
                Click?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Обработчик события наведения мыши на кнопку
        /// </summary>
        /// <param name="parSender">Отправитель сообщения</param>
        /// <param name="parE">Подробности события</param>
        private void CustomButton_MouseEnter(object parSender, MouseEventArgs parE)
        {
            _background = Brushes.Gray;
            InvalidateVisual();
            this.Focus();
        }

        /// <summary>
        /// Обработчик события смещения курсора мыши с кнопки
        /// </summary>
        /// <param name="parSender">Отправитель сообщения</param>
        /// <param name="parE">Подробности события</param>
        private void CustomButton_MouseLeave(object parSender, MouseEventArgs parE)
        {
            _background = Brushes.LightGray;
            InvalidateVisual();
        }

        /// <summary>
        /// Переопределенный метод отрисовки кнопки
        /// </summary>
        /// <param name="parDrawingContext">Контекст для рисования кнопки</param>
        [Obsolete]
        protected override void OnRender(DrawingContext parDrawingContext)
        {
            // Рисование границы
            parDrawingContext.DrawRectangle(
                _borderBrush,
                null,
                new Rect(0, 0, this.Width, this.Height));

            // Рисование внутренней области
            parDrawingContext.DrawRectangle(
                _background,
                null,
                new Rect(
                    _borderThickness,
                    _borderThickness,
                    this.Width - 2 * _borderThickness,
                    this.Height - 2 * _borderThickness));

            FormattedText formattedText = new FormattedText(Content,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"), 20, _foreground);

            // Рисование текста в центре кнопки
            parDrawingContext.DrawText(
                formattedText,
                new Point(
                    Width / 2 - formattedText.Width / 2,
                    Height / 2 - formattedText.Height / 2));
        }

        /// <summary>
        /// Устанавливает фокус на кнопке.
        /// </summary>
        public void SetFocus()
        {
            Keyboard.Focus(this);
        }
    }
}
