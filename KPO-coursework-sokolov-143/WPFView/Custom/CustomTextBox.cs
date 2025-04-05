using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFView.Custom
{
    /// <summary>
    /// Кастомный элемент для текстового ввода с отображением рамки, фона и текста
    /// </summary>
    public class CustomTextBox : FrameworkElement
    {
        /// <summary>
        /// Содержимое текстового поля
        /// </summary>
        private string _text;

        /// <summary>
        /// Цвет фона текстового поля
        /// </summary>
        private Brush _background;

        /// <summary>
        /// Цвет текста в текстовом поле
        /// </summary>
        private Brush _foreground;

        /// <summary>
        /// Цвет границы текстового поля
        /// </summary>
        private Brush _borderBrush;

        /// <summary>
        /// Толщина границы текстового поля
        /// </summary>
        private double _borderThickness;

        /// <summary>
        /// Событие, которое вызывается при изменении текста в поле
        /// </summary>
        public event EventHandler TextChanged;

        /// <summary>
        /// Свойство для получения и установки текста в поле
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    TextChanged?.Invoke(this, EventArgs.Empty);
                    InvalidateVisual();
                }
            }
        }

        /// <summary>
        /// Свойство для получения и установки фона текстового поля
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
        /// Свойство для получения и установки цвета текста в текстовом поле
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
        /// Свойство для получения и установки цвета границы текстового поля
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
        /// Свойство для получения и установки толщины границы текстового поля
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
        /// Конструктор для создания кастомного текстового поля с дефолтными значениями
        /// </summary>
        public CustomTextBox()
        {
            _text = string.Empty;
            _background = Brushes.White;
            _foreground = Brushes.Black;
            _borderBrush = Brushes.Black;
            _borderThickness = 2;

            this.Width = 240;
            this.Height = 40;

            this.Focusable = true;

            this.MouseLeftButtonDown += (s, e) => SetFocus();
        }

        /// <summary>
        /// Метод для отрисовки содержимого текстового поля
        /// </summary>
        /// <param name="parDrawingContext">Контекст рисования</param>
        protected override void OnRender(DrawingContext parDrawingContext)
        {
            // Рисуем рамку
            parDrawingContext.DrawRectangle(
                _borderBrush,
                null,
                new Rect(0, 0, this.Width, this.Height));

            // Рисуем фон
            parDrawingContext.DrawRectangle(
                _background,
                null,
                new Rect(
                    _borderThickness,
                    _borderThickness,
                    this.Width - 2 * _borderThickness,
                    this.Height - 2 * _borderThickness));

            // Определяем максимальную длину текста, который умещается в поле
            var formattedText = new FormattedText(
                _text,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"),
                16,
                _foreground,
                VisualTreeHelper.GetDpi(this).PixelsPerDip);

            string displayedText = _text;
            while (formattedText.Width > this.Width - 2 * _borderThickness - 10 && displayedText.Length > 0)
            {
                displayedText = displayedText.Substring(1); // Убираем символы слева
                formattedText = new FormattedText(
                    displayedText,
                    System.Globalization.CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Arial"),
                    16,
                    _foreground,
                    VisualTreeHelper.GetDpi(this).PixelsPerDip);
            }

            // Рисуем текст
            parDrawingContext.DrawText(
                formattedText,
                new Point(
                    _borderThickness + 5,
                    this.Height / 2 - formattedText.Height / 2));
        }

        /// <summary>
        /// Обработка нажатия клавиш в текстовом поле
        /// </summary>
        /// <param name="parE">Событие нажатия клавиши</param>
        protected override void OnKeyDown(KeyEventArgs parE)
        {
            base.OnKeyDown(parE);

            if (parE.Key == Key.Back)
            {
                if (_text.Length > 0)
                {
                    Text = _text.Substring(0, _text.Length - 1);
                }
            }
            else if (parE.Key == Key.Space)
            {
                Text += " ";
            }
            else if (parE.Key == Key.Enter)
            {
            }
            else
            {
                var keyChar = KeyToChar(parE.Key);
                if (keyChar != null)
                {
                    Text += keyChar;
                }
            }
        }

        /// <summary>
        /// Преобразование нажатой клавиши в символ
        /// </summary>
        /// <param name="parKey">Клавиша</param>
        /// <returns>Символ, соответствующий клавише</returns>
        private char? KeyToChar(Key parKey)
        {
            if (parKey >= Key.A && parKey <= Key.Z)
            {
                bool isShiftPressed = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
                char baseChar = (char)('a' + (parKey - Key.A));
                return isShiftPressed ? char.ToUpper(baseChar) : baseChar;
            }

            if (parKey >= Key.D0 && parKey <= Key.D9)
            {
                return (char)('0' + (parKey - Key.D0));
            }

            return null;
        }

        /// <summary>
        /// Установка фокуса на элемент
        /// </summary>
        public void SetFocus()
        {
            Keyboard.Focus(this);
        }
    }
}
