using Engine.DynamicObjects.Tank;
using Engine.Enum;
using Point = Engine.Field.Point;

namespace Engine
{
    /// <summary>
    /// Текущее состояние игры вместе с ее настройками
    /// </summary>
    public class GameCurrentStateAndSettingsSingleton
    {
        /// <summary>
        /// Объект одиночки, существует в единственном экземпляре
        /// </summary>
        private static GameCurrentStateAndSettingsSingleton? _instance = null;

        /// <summary>
        /// Текущая модель игры
        /// </summary>
        private GameModel _gameModel;

        /// <summary>
        /// Текущий уровень игры
        /// </summary>
        private LevelEnum _currentLevel;

        /// <summary>
        /// Заданная сложность игры
        /// </summary>
        private DifficultyEnum _currentDifficulty;

        /// <summary>
        /// Текущее количество жизней
        /// </summary>
        private int _lifesCount = 1;

        /// <summary>
        /// Текущий игровой счет
        /// </summary>
        private int _gameCount = 0;

        /// <summary>
        /// Делегат завершения игры
        /// </summary>
        /// <param name="parGameEndStatus">Результат завершения игры</param>
        public delegate void dGameEnd(GameEndStatusEnum parGameEndStatus);

        /// <summary>
        /// Поставлена ли игра на паузу
        /// </summary>
        private bool _isPaused = false;

        /// <summary>
        /// Имя текущего игрока
        /// </summary>
        private String _playerName = "";

        /// <summary>
        /// Обработчик завершения игры
        /// </summary>
        public event dGameEnd OnGameEnd;

        /// <summary>
        /// Void-делегат
        /// </summary>
        public delegate void dVoid();

        /// <summary>
        /// Делегат перехода на следущий уровень
        /// </summary>
        public event dVoid OnNextLevel;

        /// <summary>
        /// Текущий уровень игры
        /// </summary>
        public GameModel GameModel
        {
            get
            {
                return _gameModel;
            }
            set
            {
                _gameModel = value;
            }
        }

        /// <summary>
        /// Текущий уровень игры
        /// </summary>
        public LevelEnum CurrentLevel
        {
            get
            {
                return _currentLevel;
            }
            set
            {
                _currentLevel = value;
            }
        }

        /// <summary>
        /// Заданная сложность игры
        /// </summary>
        public DifficultyEnum CurrentDifficulty
        {
            get
            {
                return _currentDifficulty;
            }
            private set
            {
                _currentDifficulty = value;
                if (value == DifficultyEnum.Easy)
                {
                    _lifesCount = 3;
                }
                else if (value == DifficultyEnum.Normal)
                {
                    _lifesCount = 2;
                }
                else if (value == DifficultyEnum.Hardcore)
                {
                    _lifesCount = 1;
                }
            }
        }

        /// <summary>
        /// Текущее количество жизней
        /// </summary>
        public int LifesCount
        {
            get
            {
                return _lifesCount;
            }
            set
            {
                _lifesCount = value;
            }
        }

        /// <summary>
        /// Игровой счет
        /// </summary>
        public int GameCount
        {
            get
            {
                return _gameCount;
            }
        }

        /// <summary>
        /// Поставлена ли игра на паузу
        /// </summary>
        public bool IsPaused
        {
            get
            {
                return _isPaused;
            }
            set
            {
                _isPaused = value;
            }
        }

        /// <summary>
        /// Имя текущего игрока
        /// </summary>
        public String PlayerName
        {
            get
            {
                return _playerName;
            }
            set
            {
                _playerName = value;
            }
        }


        /// <summary>
        /// Конструктор настроек игры
        /// </summary>
        private GameCurrentStateAndSettingsSingleton()
        {
            CurrentDifficulty = DifficultyEnum.Easy;
            _currentLevel = LevelEnum.First;
        }

        /// <summary>
        /// Получить объект одиночки с настройками и состояния системы
        /// </summary>
        /// <returns>Объект одиночки с настройками и состояния системы</returns>
        public static GameCurrentStateAndSettingsSingleton GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameCurrentStateAndSettingsSingleton();
            }
            return _instance;
        }

        /// <summary>
        /// Инициализация настроек игры
        /// <param name="parGameModel">Текущая модель игры</param>
        /// <param name="parGameModel">Заданный уровень сложности</param>
        /// </summary>
        public void Init(GameModel parGameModel, DifficultyEnum parCurrentDifficulty)
        {
            IsPaused = false;
            CurrentDifficulty = parCurrentDifficulty;
            _gameModel = parGameModel;

            _gameModel.PlayerTankDestroyed += () =>
            {
                if (LifesCount > 1)
                {
                    ConfigurePlayerTank();
                    LifesCount--;
                    _gameModel.OnPlayerTankInfoUpdate();
                }
                else
                {
                    OnGameEndHandler(GameEndStatusEnum.Defeat);
                }
            };

            _gameModel.AllEnemyTanksDestroyed += () =>
            {
                if (IsNextLevelExist())
                {
                    SetLevelToNext();
                    OnNextLevelHandler();
                }
                else
                {
                    OnGameEndHandler(GameEndStatusEnum.Victory);
                }
            };

            _gameModel.StaffDestroyed += () =>
            {
                OnGameEndHandler(GameEndStatusEnum.Defeat);
            };

            // Если вражеский танк уничтожен именно пользователем, то добавить на счет игроку очки
            _gameModel.EnemyTankDestroyed += (AbstractTank parTank, bool parIsKilledByPlayer) => {
                if (parIsKilledByPlayer)
                {
                    if (parTank is GeneralTank)
                    {
                        _gameCount += 100;
                    }
                    else if (parTank is HighSpeedTank || parTank is RapidFireTank)
                    {
                        _gameCount += 125;
                    }
                    else //Armored
                    {
                        _gameCount += 150;
                    }
                    _gameModel.OnPlayerTankInfoUpdate();
                }
            };
        }

        /// <summary>
        /// Существует ли следующий уровень игры
        /// </summary>
        /// <returns>Истина, если текущий уровень не последний</returns>
        public bool IsNextLevelExist()
        {
            return _currentLevel != LevelEnum.Third;
        }

        /// <summary>
        /// Установить уровень на следующий
        /// </summary>
        public void SetLevelToNext()
        {
            if (_currentLevel == LevelEnum.First)
            {
                _currentLevel = LevelEnum.Second;
            }
            else if (_currentLevel == LevelEnum.Second)
            {
                _currentLevel = LevelEnum.Third;
            }
            else
            {
                _currentLevel = LevelEnum.First;
            }
            _gameModel.OnPlayerTankInfoUpdate();
        }

        /// <summary>
        /// Получить модель текущего уровня игры с заданным уровнем сложности
        /// </summary>
        /// <returns>Модель текущего уровня игры с заданным уровнем сложности</returns>
        public void GetCurrentLevelModel()
        {
            LevelGenerator factoryMethod = new LevelGenerator();
            if (_currentLevel == LevelEnum.First)
            {
                _gameModel.Field = factoryMethod.CreateLevel(LevelEnum.First);

                if (_currentDifficulty == DifficultyEnum.Easy)
                {
                    _gameModel.Tanks.Add(new GeneralTank(new Point(16 * 20, 16 * 3)));
                    _gameModel.Tanks.Add(new GeneralTank(new Point(16 * 25, 16 * 7)));
                    _gameModel.Tanks.Add(new GeneralTank(new Point(16 * 5, 16 * 1)));
                }
                else if (_currentDifficulty == DifficultyEnum.Normal)
                {
                    _gameModel.Tanks.Add(new GeneralTank(new Point(16 * 20, 16 * 3)));
                    _gameModel.Tanks.Add(new HighSpeedTank(new Point(16 * 25, 16 * 7)));
                    _gameModel.Tanks.Add(new RapidFireTank(new Point(16 * 30, 16 * 3)));
                    _gameModel.Tanks.Add(new GeneralTank(new Point(16 * 5, 16 * 1)));
                }
                else if (_currentDifficulty == DifficultyEnum.Hardcore)
                {
                    _gameModel.Tanks.Add(new GeneralTank(new Point(16 * 20, 16 * 3)));
                    _gameModel.Tanks.Add(new HighSpeedTank(new Point(16 * 25, 16 * 7)));
                    _gameModel.Tanks.Add(new RapidFireTank(new Point(16 * 30, 16 * 3)));
                    _gameModel.Tanks.Add(new HighSpeedTank(new Point(16 * 5, 16 * 1)));
                    _gameModel.Tanks.Add(new ArmoredTank(new Point(16 * 46, 16 * 15)));
                    _gameModel.Tanks.Add(new ArmoredTank(new Point(16 * 3, 16 * 17)));
                }
            }
            else if (_currentLevel == LevelEnum.Second)
            {
                _gameModel.Field = factoryMethod.CreateLevel(LevelEnum.Second);

                if (_currentDifficulty == DifficultyEnum.Easy)
                {
                    _gameModel.Tanks.Add(new GeneralTank(new Point(16 * 18, 16 * 3)));
                    _gameModel.Tanks.Add(new GeneralTank(new Point(16 * 25, 16 * 6)));
                    _gameModel.Tanks.Add(new GeneralTank(new Point(16 * 32, 16 * 9)));
                }
                else if (_currentDifficulty == DifficultyEnum.Normal)
                {
                    _gameModel.Tanks.Add(new GeneralTank(new Point(16 * 18, 16 * 3)));
                    _gameModel.Tanks.Add(new GeneralTank(new Point(16 * 25, 16 * 6)));
                    _gameModel.Tanks.Add(new GeneralTank(new Point(16 * 32, 16 * 9)));
                    _gameModel.Tanks.Add(new ArmoredTank(new Point(16 * 3, 16 * 3)));
                    _gameModel.Tanks.Add(new ArmoredTank(new Point(16 * 6, 16 * 6)));
                }
                else if (_currentDifficulty == DifficultyEnum.Hardcore)
                {
                    _gameModel.Tanks.Add(new RapidFireTank(new Point(16 * 18, 16 * 3)));
                    _gameModel.Tanks.Add(new RapidFireTank(new Point(16 * 25, 16 * 6)));
                    _gameModel.Tanks.Add(new HighSpeedTank(new Point(16 * 32, 16 * 9)));
                    _gameModel.Tanks.Add(new ArmoredTank(new Point(16 * 3, 16 * 3)));
                    _gameModel.Tanks.Add(new ArmoredTank(new Point(16 * 6, 16 * 6)));
                }
            }
            else
            {
                _gameModel.Field = factoryMethod.CreateLevel(LevelEnum.Third);

                if (_currentDifficulty == DifficultyEnum.Easy)
                {
                    _gameModel.Tanks.Add(new GeneralTank(new Point(16 * 3, 16 * 12)));
                    _gameModel.Tanks.Add(new GeneralTank(new Point(16 * 10, 16 * 12)));
                    _gameModel.Tanks.Add(new GeneralTank(new Point(16 * 36, 16 * 12)));
                }
                else if (_currentDifficulty == DifficultyEnum.Normal)
                {
                    _gameModel.Tanks.Add(new RapidFireTank(new Point(16 * 3, 16 * 12)));
                    _gameModel.Tanks.Add(new RapidFireTank(new Point(16 * 10, 16 * 12)));
                    _gameModel.Tanks.Add(new ArmoredTank(new Point(16 * 36, 16 * 12)));
                    _gameModel.Tanks.Add(new GeneralTank(new Point(16 * 17, 16 * 12)));
                }
                else if (_currentDifficulty == DifficultyEnum.Hardcore)
                {
                    _gameModel.Tanks.Add(new RapidFireTank(new Point(16 * 3, 16 * 12)));
                    _gameModel.Tanks.Add(new RapidFireTank(new Point(16 * 10, 16 * 12)));
                    _gameModel.Tanks.Add(new ArmoredTank(new Point(16 * 36, 16 * 12)));
                    _gameModel.Tanks.Add(new HighSpeedTank(new Point(16 * 17, 16 * 12)));
                    _gameModel.Tanks.Add(new HighSpeedTank(new Point(16 * 37, 16 * 3)));
                }
            }
            _gameModel.UpdateStaffBlocksCoordinates();
            ConfigurePlayerTank();
        }


        /// <summary>
        /// Настроить танк игрока в зависимости от уровня сложности
        /// </summary>
        public void ConfigurePlayerTank()
        {
            if (_currentDifficulty == DifficultyEnum.Easy)
            {
                _gameModel.Player.PlayerTank.Health = AbstractTank.DEFAULT_HEALTH * 2;
                _gameModel.Player.PlayerTank.Speed = (int)(AbstractTank.DEFAULT_SPEED * 1.5);
                _gameModel.Player.PlayerTank.Damage = (int)(AbstractTank.DEFAULT_DAMAGE * 2);
            }
            else if (_currentDifficulty == DifficultyEnum.Normal)
            {
                _gameModel.Player.PlayerTank.Health = AbstractTank.DEFAULT_HEALTH;
                _gameModel.Player.PlayerTank.Speed = (int)(AbstractTank.DEFAULT_SPEED * 1.25);
            }
            else //Hardcore
            {
                _gameModel.Player.PlayerTank.Health = AbstractTank.DEFAULT_HEALTH;
            }
            _gameModel.Player.PlayerTank.Point = new Point(16 * 20, 16 * 38);
            _gameModel.Player.PlayerTank.Direction = DynamicObjects.DirectionEnum.Up;
        }

        /// <summary>
        /// Вызвать обработчик завершения игры
        /// </summary>
        /// <param name="parGameEndStatus">Статус завершения (победа или поражение)</param>
        public void OnGameEndHandler(GameEndStatusEnum parGameEndStatus)
        {
            OnGameEnd?.Invoke(parGameEndStatus);
        }

        /// <summary>
        /// Вызвать обработчик перехода на новый уровень
        /// </summary>
        public void OnNextLevelHandler()
        {
            OnNextLevel?.Invoke();
            _gameModel.OnPlayerTankInfoUpdate();
        }

        /// <summary>
        /// Сброс параметров игры
        /// </summary>
        public void Reset()
        {
            _currentLevel = LevelEnum.First;
            _gameCount = 0;
            _playerName = "";
        }

        /// <summary>
        /// Получить основную информацию о текущем состоянии игры для представлений
        /// </summary>
        /// <returns>Основная информацию о текущем состоянии игры для представлений</returns>
        public String GetImportantInfoForView()
        {
            return $"Здоровье = {_gameModel.Player.PlayerTank.Health}, Жизней = {_lifesCount}, Уровень = {CurrentLevel}, Счет = {_gameCount}";
        }
    }
}
