using BaseMenuView;

namespace BaseController
{
    /// <summary>
    /// Базовый контроллер для контроллеров меню
    /// </summary>
    public abstract class ControllerMenu : ControllerBase
    {
        /// <summary>
        /// Модель меню
        /// </summary>
        private BaseMenuModel.Components.Menu _menu = null;

        /// <summary>
        /// Вид меню
        /// </summary>
        private ViewMenu _viewMenu = null;

        /// <summary>
        /// Модель меню
        /// </summary>
        protected BaseMenuModel.Components.Menu Menu
        {
            get
            {
                return _menu;
            }
            set
            {
                _menu = value;
            }
        }

        /// <summary>
        /// Вид меню
        /// </summary>
        protected ViewMenu ViewMenu
        {
            get
            {
                return _viewMenu;
            }
            set
            {
                _viewMenu = value;
            }
        }

    }
}
