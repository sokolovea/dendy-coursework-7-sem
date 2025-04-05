using System;
using Engine;
using Engine.Enum;
using ConsoleView;
using ControllerConsole;

namespace ConsoleController
{
    /// <summary>
    /// Контроллер игры для консоли
    /// </summary>
    public class ControllerGameConsole : BaseControllerConsole
    {
        /// <summary>
        /// Прололжается ли игра
        /// </summary>
        private bool _isPlaying = false;

        /// <summary>
        /// Представление игры
        /// </summary>
        private ViewGameConsole _viewGameConsole;

        /// <summary>
        /// Настройки игры
        /// </summary>
        private readonly GameCurrentStateAndSettingsSingleton _gameSettings = GameCurrentStateAndSettingsSingleton.GetInstance();

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ControllerGameConsole()
        {
            Init();
            _viewGameConsole.ClearAllField();
            _gameSettings.GameModel.OnPlayerTankInfoUpdate();
        }

        /// <summary>
        /// Создает контроллер с заданным уровнем сложности и параметрами восстановления здоровья
        /// </summary>
        /// <param name="parDifficultyEnum">Сложность игры</param>
        /// <param name="parIsRecoverHealth">Следует ли восстанавливать здоровье танка игрока</param>
        public ControllerGameConsole(DifficultyEnum parDifficultyEnum, bool parIsRecoverHealth = false)
        {
            InitializeGame(parDifficultyEnum, parIsRecoverHealth);
        }

        /// <summary>
        /// Инициализация игры
        /// </summary>
        /// <param name="parDifficultyEnum">Сложность игры</param>
        /// <param name="parIsRecoverHealth">Следует ли восстанавливать здоровье танка игрока</param>
        private void InitializeGame(DifficultyEnum parDifficultyEnum, bool parIsRecoverHealth)
        {
            int? oldHealth = _gameSettings.GameModel?.Player?.PlayerTank.Health;
            int? oldLifes = _gameSettings.LifesCount;

            _gameSettings.GameModel = new GameModel(50, 40, 16, new Engine.Field.Point(20, 200));
            _gameSettings.Init(_gameSettings.GameModel, parDifficultyEnum);

            Init();
            _viewGameConsole.ClearAllField();
            _viewGameConsole.Draw();

            if (parIsRecoverHealth && oldHealth.HasValue && oldLifes.HasValue)
            {
                _gameSettings.GameModel.Player.PlayerTank.Health = oldHealth.Value;
                _gameSettings.LifesCount = oldLifes.Value;
            }
            _gameSettings.GameModel.OnPlayerTankInfoUpdate();
        }

        /// <summary>
        /// Инициализация контроллера
        /// </summary>
        private void Init()
        {

            Console.Clear();
            Console.ResetColor();
            if (!_gameSettings.IsPaused)
            {
                _gameSettings.GetCurrentLevelModel();
            }

            _viewGameConsole = new ViewGameConsole(_gameSettings.GameModel);
            _viewGameConsole.Draw();

            RegisterEvents();
        }

        /// <summary>
        /// Запускает игру
        /// </summary>
        public override void Start()
        {
            _isPlaying = true;
            while (_isPlaying)
            {
                _viewGameConsole.Draw();
                ProcessInput();
                _gameSettings.GameModel.Player.PlayerTank.IsMoving = false;
            }
        }

        /// <summary>
        /// Обработка ввода
        /// </summary>
        private void ProcessInput()
        {
            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        _gameSettings.GameModel.Player.MoveUpImmediatelly();
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        _gameSettings.GameModel.Player.MoveLeftImmediatelly();
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        _gameSettings.GameModel.Player.MoveDownImmediatelly();
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        _gameSettings.GameModel.Player.MoveRightImmediatelly();
                        break;
                    case ConsoleKey.Spacebar:
                        _gameSettings.GameModel.Fire(_gameSettings.GameModel.Player.PlayerTank);
                        break;
                    case ConsoleKey.Escape:
                        _isPlaying = false;
                        _viewGameConsole.Stop();
                        PauseGame();
                        return;
                }
            }
        }

        /// <summary>
        /// Постановка игры на паузу
        /// </summary>
        private void PauseGame()
        {
            lock (_viewGameConsole._consoleLock)
            {
                _isPlaying = false;
                _viewGameConsole.Stop();
                Stop();
                _gameSettings.IsPaused = true;
                Console.ForegroundColor = ConsoleColor.White;
            }
            new ControllerMenuMainConsole().Start();
        }

        /// <summary>
        /// Привязка событий к обработчикам
        /// </summary>
        private void RegisterEvents()
        {
            _gameSettings.OnGameEnd += HandleGameEnd;
            _gameSettings.OnNextLevel += HandleNextLevel;
        }

        /// <summary>
        /// Обработчик завершения игры
        /// </summary>
        /// <param name="status">Статус игры</param>
        private void HandleGameEnd(GameEndStatusEnum status)
        {
            Stop();
            lock (_viewGameConsole._consoleLock)
            {
                _viewGameConsole.ClearAllField();
                Console.Clear();
                Console.ResetColor();
            }
            new ControllerMenuGameEndConsole(status, _gameSettings.GameCount, (int)_gameSettings.CurrentLevel + 1, _gameSettings.CurrentDifficulty).Start();
        }

        /// <summary>
        /// Обработчик перехода на следующий уровень
        /// </summary>
        private void HandleNextLevel()
        {
            Stop();
            lock (_viewGameConsole._consoleLock)
            {
                _viewGameConsole.ClearGameField();
                Console.Clear();
                Console.ResetColor();
            }
            new ControllerGameConsole(_gameSettings.CurrentDifficulty, true).Start();
        }

        /// <summary>
        /// Остановка игры
        /// </summary>
        public override void Stop()
        {
            _gameSettings.OnGameEnd -= HandleGameEnd;
            _gameSettings.OnNextLevel -= HandleNextLevel;

            _gameSettings.GameModel.Stop();

            lock (_viewGameConsole._consoleLock)
            {
                Console.Clear();
                Console.ResetColor();
            }

            _viewGameConsole.Stop();
            base.Stop();

            lock (_viewGameConsole._consoleLock)
            {
                Console.Clear();
                Console.ResetColor();
            }
        }


    }
}
