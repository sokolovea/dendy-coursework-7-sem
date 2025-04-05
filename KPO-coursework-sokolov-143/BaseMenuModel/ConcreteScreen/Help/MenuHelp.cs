using BaseMenuModel.Components;

namespace BaseMenuModel.ConcreteScreen.Help
{
    /// <summary>
    /// Модель меню справки
    /// </summary>
    public class MenuHelp: Menu
    {
        /// <summary>
        /// Справка по правилам игры
        /// </summary>
        private String _rules = "Battle City – классическая аркадная игра, где игрок управляет танком, \n" +
            "защищая свою базу и уничтожая врагов. Цель игры — уничтожить все вражеские танки на уровне, \n" +
            "не допустив разрушения своего штаба, обозначенного символом орла. В игре есть различные\n" +
            "уровни сложности и типы танков.";

        /// <summary>
        /// Справка по управлению танком
        /// </summary>
        private String _controls = "Управление движением танка - клавиши WASD (вверх, влево, вниз, вправо)\n" +
                                    "или стрелки. Для стрельбы нажмите пробел. При отжатии клавиш движения танк\n" +
                                    "останавливается. Выйти в главное меню из игры можно при помощи клавиши ESC.";
        /// <summary>
        /// Справка по правилам игры
        /// </summary>
        public String Rules
        {
            get
            {
                return _rules;
            }
            protected set
            {
                _rules = value;
            }
        }

        /// <summary>
        /// Справка по управлению танком
        /// </summary>
        public String Controls
        {
            get
            {
                return _controls;
            }
            protected set
            {
                _controls = value;
            }
        }

        /// <summary>
        /// Создает модель меню справки
        /// </summary>
        public MenuHelp() : base(Properties.Resources.MainMenuGame)
        {
            AddItem(new MenuItem((int)MenuHelpItemCodes.Back, "Назад"));
            FocusItemById((int)MenuHelpItemCodes.Back);
        }
    }
}
