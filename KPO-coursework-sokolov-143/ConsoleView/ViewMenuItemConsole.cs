using BaseMenuView;
using BaseMenuModel.Components;
using System;
using System.Collections.Generic;

namespace ConsoleView
{
    /// <summary>
    /// Консольное представление пункта меню
    /// </summary>
    public class ViewMenuItemConsole : ViewMenuItem
    {
        /// <summary>
        /// Высота пункта меню
        /// </summary>
        public static readonly int HEIGHT = 1;

        /// <summary>
        /// Словарь цветов меню
        /// </summary>
        protected static Dictionary<MenuItemStates, ConsoleColor> ColorByState { get; private set; }

        /// <summary>
        /// Буфер для ввода текста
        /// </summary>
        private string _inputBuffer = "";

        /// <summary>
        /// Режим ввода текста
        /// </summary>
        private bool _isInputMode = false;

        /// <summary>
        /// Текст представления
        /// </summary>
        public String Text
        {
            get
            {
                return _inputBuffer;
            }
            set
            {
                _inputBuffer = value;
            }
        }

        /// <summary>
        /// Статический конструктор для инициализации цветов для различных состояний пунктов меню
        /// </summary>
        static ViewMenuItemConsole()
        {
            ColorByState = new Dictionary<MenuItemStates, ConsoleColor>();
            ColorByState[MenuItemStates.Focused] = ConsoleColor.Yellow;
            ColorByState[MenuItemStates.Normal] = ConsoleColor.Gray;
            ColorByState[MenuItemStates.Selected] = ConsoleColor.White;
        }

        /// <summary>
        /// Создает консольное представление пункта меню
        /// </summary>
        /// <param name="parItem">Модель пункта меню</param>
        public ViewMenuItemConsole(MenuItem parItem) : base(parItem)
        {
            Height = HEIGHT;
            Width = parItem.IsInput ? 20 : parItem.Text.Length; // Для полей ввода фиксированная ширина
            if (parItem.IsInput)
            {
                _isInputMode = true;
                _inputBuffer = parItem.Text; // Инициализация буфера начальным текстом
            }
        }

        /// <summary>
        /// Отрисовать представление пункта меню
        /// </summary>
        public override void Draw()
        {
            Console.CursorLeft = X;
            Console.CursorTop = Y;
            ConsoleColor savColor = Console.ForegroundColor;
            Console.ForegroundColor = ColorByState[Item.State];

            if (Item.IsInput)
            {
                // Если это поле ввода, отображаем текст из буфера
                Console.Write($"{_inputBuffer.PadRight(Width - 2)}");
            }
            else
            {
                Console.Write(Item.Text);
            }

            Console.ForegroundColor = savColor;
        }

        /// <summary>
        /// Обработать нажатие клавиши
        /// </summary>
        /// <param name="parKey">Нажатая клавиша</param>
        public void HandleKey(ConsoleKeyInfo parKey)
        {
            if (Item.IsInput && _isInputMode)
            {
                if (parKey.Key == ConsoleKey.Enter)
                {
                    Item.Text = _inputBuffer;
                }
                else if (parKey.Key == ConsoleKey.Backspace && _inputBuffer.Length > 0)
                {
                    _inputBuffer = _inputBuffer[..^1]; // Удаление последнего символа
                }
                else if (parKey.KeyChar >= 32 && parKey.KeyChar <= 126)
                {
                    if (_inputBuffer.Length < Width * 3 - 2)
                    {
                        _inputBuffer += parKey.KeyChar;
                    }
                }
            }
            Item.Text = _inputBuffer; // Сохранение текста в модель
        }
    }
}
