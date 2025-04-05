using System.Windows;
using System.Windows.Media;

namespace WPFView.Custom
{
    /// <summary>
    /// Кастомный элемент для отображения текста с настройками фона, границы и цвета текста
    /// </summary>
    public class CustomLabel : FrameworkElement
    {
        /// <summary>
        /// Текстовое содержимое, отображаемое в элементе
        /// </summary>
        private String _content;

        /// <summary>
        /// Цвет фона элемента
        /// </summary>
        private Brush _background;

        /// <summary>
        /// Цвет текста
        /// </summary>
        private Brush _foreground;

        /// <summary>
        /// Цвет границы элемента
        /// </summary>
        private Brush _borderBrush;

        /// <summary>
        /// Толщина границы элемента
        /// </summary>
        private double _borderThickness;

        /// <summary>
        /// Текстовое содержимое, отображаемое в элементе
        /// </summary>
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                this.Dispatcher.Invoke(new Action(() =>
                {
                    InvalidateVisual();
                }));
            }
        }

        /// <summary>
        /// Свойство для управления цветом фона элемента. При изменении вызывает перерисовку
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
        /// Свойство для управления цветом текста элемента. При изменении вызывает перерисовку
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
        /// Свойство для управления цветом границы элемента. При изменении вызывает перерисовку
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
        /// Свойство для управления толщиной границы элемента. При изменении вызывает перерисовку
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
        /// Конструктор для создания кастомного текстового элемента с заданным содержимым
        /// </summary>
        /// <param name="content">Текстовое содержимое</param>
        public CustomLabel(string content)
        {
            Content = content;
            _background = Brushes.Transparent;
            _foreground = Brushes.Black;
            _borderBrush = Brushes.Transparent;
            _borderThickness = 1;

            this.Width = 120;
            this.Height = 40;

            this.Focusable = false;
        }

        /// <summary>
        /// Метод для отрисовки содержимого элемента
        /// </summary>
        /// <param name="drawingContext">Контекст рисования</param>
        [Obsolete]
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (_borderBrush != Brushes.Transparent)
            {
                drawingContext.DrawRectangle(
                    _borderBrush,
                    null,
                    new Rect(0, 0, this.Width, this.Height));
            }

            drawingContext.DrawRectangle(
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
                new Typeface("Arial"), 16, _foreground);

            drawingContext.DrawText(
                formattedText,
                new Point(
                    Width / 2 - formattedText.Width / 2,
                    Height / 2 - formattedText.Height / 2));
        }
    }
}
