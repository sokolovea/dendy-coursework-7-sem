using Engine.DynamicObjects.Tank;
using Engine.Field;

namespace Engine
{
    /// <summary>
    /// Модель игрока
    /// </summary>
    public class PlayerModel
    {
        /// <summary>
        /// Текущие настройки игры и ее состояние
        /// </summary>
        GameCurrentStateAndSettingsSingleton _gameCurrentStateAndSettings = GameCurrentStateAndSettingsSingleton.GetInstance(); 

        /// <summary>
        /// Танк игрока
        /// </summary>
        private AbstractTank _playerTank;

        /// <summary>
        /// Танк игрока
        /// </summary>
        public AbstractTank PlayerTank
        {
            get
            {
                return _playerTank;
            }
            set
            {
                _playerTank = value;
            }
        }

        /// <summary>
        /// Конструктор модели игрока с начальными координатами
        /// </summary>
        /// <param name="parStartCoordinates">Начальные координаты игрока</param>
        public PlayerModel(Point parStartCoordinates)
        {
            _playerTank = new GeneralTank(parStartCoordinates);
        }

        /// <summary>
        /// Перемещение игрока вверх
        /// </summary>
        public void MoveUp()
        {
            _playerTank.Direction = DynamicObjects.DirectionEnum.Up;
        }
        
        /// <summary>
        /// Перемещение игрока вверх принудительно
        /// </summary>
        public void MoveUpImmediatelly()
        {
            _playerTank.Direction = DynamicObjects.DirectionEnum.Up;
            _playerTank.IsMoving = true;
            _gameCurrentStateAndSettings.GameModel.UpdatePlayerTankPosition();
            _playerTank.IsMoving = false;
        }

        /// <summary>
        /// Перемещение игрока вверх принудительно
        /// </summary>
        public void MoveDownImmediatelly()
        {
            _playerTank.Direction = DynamicObjects.DirectionEnum.Down;
            _playerTank.IsMoving = true;
            _gameCurrentStateAndSettings.GameModel.UpdatePlayerTankPosition();
            _playerTank.IsMoving = false;
        }

        /// <summary>
        /// Перемещение игрока вверх принудительно
        /// </summary>
        public void MoveRightImmediatelly()
        {
            _playerTank.Direction = DynamicObjects.DirectionEnum.Right;
            _playerTank.IsMoving = true;
            _gameCurrentStateAndSettings.GameModel.UpdatePlayerTankPosition();
            _playerTank.IsMoving = false;
        }

        /// <summary>
        /// Перемещение игрока вверх принудительно
        /// </summary>
        public void MoveLeftImmediatelly()
        {
            _playerTank.Direction = DynamicObjects.DirectionEnum.Left;
            _playerTank.IsMoving = true;
            _gameCurrentStateAndSettings.GameModel.UpdatePlayerTankPosition();
            _playerTank.IsMoving = false;
        }

        /// <summary>
        /// Перемещение игрока вниз
        /// </summary>
        public void MoveDown()
        {
            _playerTank.Direction = DynamicObjects.DirectionEnum.Down;
        }

        /// <summary>
        /// Перемещение игрока влево
        /// </summary>
        public void MoveLeft()
        {
            _playerTank.Direction = DynamicObjects.DirectionEnum.Left;
        }

        /// <summary>
        /// Перемещение игрока вправо
        /// </summary>
        public void MoveRight()
        {
            _playerTank.Direction = DynamicObjects.DirectionEnum.Right;
        }

    }
}
