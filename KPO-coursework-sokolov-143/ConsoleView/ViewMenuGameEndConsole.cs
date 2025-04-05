using System;
using BaseMenuModel.Components;
using BaseMenuModel.ConcreteScreen.GameEnd;

namespace ConsoleView
{
    /// <summary>
    /// Вид меню подтверждения сохранения рекордов в конце игры для консольной версии
    /// </summary>
    public class ViewMenuGameEndConsole : ViewMenuConsole
    {
        /// <summary>
        /// Объект, содержащий данные для меню окончания игры
        /// </summary>
        private MenuGameEnd _menuGameEnd;

        /// <summary>
        /// Конструктор, инициализирует вид для меню окончания игры
        /// </summary>
        /// <param name="parSubMenuItem">Параметр, передающий данные меню</param>
        public ViewMenuGameEndConsole(BaseMenuModel.Components.Menu parSubMenuItem) : base(parSubMenuItem, false)
        {
            _menuGameEnd = (MenuGameEnd)parSubMenuItem;
        }

        /// <summary>
        /// Отображение меню на консоли
        /// </summary>
        public override void Draw()
        {
            Console.Clear();
            Console.CursorTop = 15;
            Console.WriteLine(CenterText(new string('=', 30)));
            Console.WriteLine(CenterText($" {_menuGameEnd.Title}"));
            InitMenuItemPositionByIndex((int)MenuGameEndItemCodes.PlayerName, 45, Console.CursorTop);
            Console.CursorTop++;
            Console.WriteLine(CenterText(new string('-', 30)));
            Console.WriteLine(CenterText($"Ваш счет: {_menuGameEnd.Score}"));
            Console.WriteLine(CenterText(new string('=', 30)));

            foreach (var elViewMenuItem in Menu)
            {
                elViewMenuItem.Draw();
            }
        }

        /// <summary>
        /// Инициализация компонентов интерфейса меню справки
        /// </summary>
        public new void Init()
        {
            Console.Clear();
            Draw();
        }

    }
}
