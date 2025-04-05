using Engine;
using Engine.Enum;
using System.Windows;
using System.Windows.Input;
using WPFView;

namespace WPFController
{
    /// <summary>
    /// Контроллер игры
    /// </summary>
    public class ControllerGameWPF : BaseControllerWPF
    {
        /// <summary>
        /// Вид для игры
        /// </summary>
        private ViewGameWPF _viewGameWPF;

        /// <summary>
        /// Словарь нажатых клавиш (необходимо для корректной обработки множественного нажатия клавиш)
        /// </summary>
        private readonly Dictionary<Key, bool> _keysPressed = new Dictionary<Key, bool>();

        /// <summary>
        /// Текущее состояние и настройки игры
        /// </summary>
        private GameCurrentStateAndSettingsSingleton _gameSettings = GameCurrentStateAndSettingsSingleton.GetInstance();

        /// <summary>
        /// Конструктор контроллера с заданной сложностью игры
        /// </summary>
        /// <param name="parDifficultyEnum">Заданная сложность игры</param>
        /// <param name="parIsRecoverHealth">Нужно ли восстанавливать значение здоровья</param>
        public ControllerGameWPF(DifficultyEnum parDifficultyEnum, bool parIsRecoverHealth = false)
        {
            int? oldHeathValue = null;
            int? oldLifesCount = null;
            if (_gameSettings.GameModel != null && _gameSettings.GameModel.Player != null)
            {
                oldHeathValue = _gameSettings.GameModel.Player.PlayerTank.Health;
                oldLifesCount = _gameSettings.LifesCount;
            }
            _gameSettings.GameModel = new GameModel(50, 40, 16, new Engine.Field.Point(20, 200));
            _gameSettings.Init(_gameSettings.GameModel, parDifficultyEnum);
            Init();
            if (parIsRecoverHealth && oldHeathValue != null && oldLifesCount != null)
            {
                _gameSettings.GameModel.Player.PlayerTank.Health = (int)oldHeathValue;
                _gameSettings.LifesCount = (int)oldLifesCount;
            }
        }

        /// <summary>
        /// Конструктор контроллера, снимающий игру с паузы
        /// </summary>
        /// <param name="parDifficultyEnum">Заданная сложность игры</param>
        public ControllerGameWPF()
        {
            _gameSettings.GameModel = _gameSettings.GameModel;
            Init();
            UpdateTankAction();
        }

        /// <summary>
        /// Инициализация контроллера
        /// </summary>
        private void Init()
        {
            LevelGenerator levelFactory = new LevelGenerator();
            GameCurrentStateAndSettingsSingleton gameSettings = GameCurrentStateAndSettingsSingleton.GetInstance();

            if (!gameSettings.IsPaused)
            {
                gameSettings.GetCurrentLevelModel();
            }

            _viewGameWPF = new ViewGameWPF(_gameSettings.GameModel);
            _viewGameWPF.Draw();

            _keysPressed.Add(Key.Up, false);
            _keysPressed.Add(Key.W, false);
            _keysPressed.Add(Key.Left, false);
            _keysPressed.Add(Key.A, false);
            _keysPressed.Add(Key.Down, false);
            _keysPressed.Add(Key.S, false);
            _keysPressed.Add(Key.Right, false);
            _keysPressed.Add(Key.D, false);
            _keysPressed.Add(Key.Space, false);
            _keysPressed.Add(Key.Escape, false);

            _gameSettings.OnGameEnd += (GameEndStatusEnum parGameEndStatus) =>
            {
                _viewGameWPF.GameCanvas.Dispatcher.Invoke(() =>
                {
                    _viewGameWPF.Stop();
                    Stop();
                    int gameCount = _gameSettings.GameCount;
                    int gameLevel = (int)_gameSettings.CurrentLevel + 1;
                    DifficultyEnum currentDifficulty = _gameSettings.CurrentDifficulty;
                    new ControllerMenuGameEndWPF(parGameEndStatus, gameCount, gameLevel, currentDifficulty).Start();
                });
            };

            _gameSettings.OnNextLevel += () =>
            {
                _viewGameWPF.GameCanvas.Dispatcher.Invoke(() =>
                {
                    _viewGameWPF.Stop();
                    Stop();
                    new ControllerGameWPF(_gameSettings.CurrentDifficulty, true).Start();
                });
            };
        }

        /// <summary>
        /// Открывает графическое окно
        /// </summary>
        public override void Start()
        {
            base.Start();
            Window window = WindowSingleton.GetInstance().GetWindow();
            window.KeyUp += KeyUpEventHandler;
        }
        
        /// <summary>
        /// Закрывает графическое окно
        /// </summary>
        public override void Stop()
        {
            base.Stop();
            Window window = WindowSingleton.GetInstance().GetWindow();
            window.KeyDown -= KeyUpEventHandler;
            window.KeyUp -= KeyUpEventHandler;
        }

        /// <summary>
        /// Обработчик нажатия клавиш
        /// </summary>
        /// <param name="parSender">Отправитель</param>
        /// <param name="parArgs">Нажатые клавиши</param>
        protected override void KeyDownEventHandler(object parSender, KeyEventArgs parArgs)
        {
            if (!_keysPressed.ContainsKey(parArgs.Key)) return;

            _keysPressed[parArgs.Key] = true;

            UpdateTankAction();
        }

        /// <summary>
        /// Обработчик отпускания клавиш
        /// </summary>
        /// <param name="parSender">Отправитель</param>
        /// <param name="parArgs">Отпущенные клавиши</param>
        protected void KeyUpEventHandler(object parSender, KeyEventArgs parArgs)
        {
            if (!_keysPressed.ContainsKey(parArgs.Key)) return;

            _keysPressed[parArgs.Key] = false;

            UpdateTankAction();
        }

        /// <summary>
        /// Привязывает команды, отправляемые танку, к нажатию определенных клавиш
        /// </summary>
        private void UpdateTankAction()
        {
            _gameSettings.GameModel.Player.PlayerTank.IsMoving = false;

            if (_keysPressed[Key.Up] || _keysPressed[Key.W])
            {
                _gameSettings.GameModel.Player.PlayerTank.IsMoving = true;
                _gameSettings.GameModel.Player.MoveUp();
            }
            if (_keysPressed[Key.Left] || _keysPressed[Key.A])
            {
                _gameSettings.GameModel.Player.PlayerTank.IsMoving = true;
                _gameSettings.GameModel.Player.MoveLeft();
            }
            if (_keysPressed[Key.Down] || _keysPressed[Key.S])
            {
                _gameSettings.GameModel.Player.PlayerTank.IsMoving = true;
                _gameSettings.GameModel.Player.MoveDown();
            }
            if (_keysPressed[Key.Right] || _keysPressed[Key.D])
            {
                _gameSettings.GameModel.Player.PlayerTank.IsMoving = true;
                _gameSettings.GameModel.Player.MoveRight();
            }

            if (_keysPressed[Key.Space])
            {
                _gameSettings.GameModel.Fire(_gameSettings.GameModel.Player.PlayerTank);
            }
            if (_keysPressed[Key.Escape])
            {
                _viewGameWPF.Stop();
                Stop();
                GameCurrentStateAndSettingsSingleton gameSettings = GameCurrentStateAndSettingsSingleton.GetInstance();
                gameSettings.IsPaused = true;
                new ControllerMenuMainWPF().Start();
            }
        }
    }
}