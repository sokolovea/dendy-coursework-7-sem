using BaseMenuView;
using Engine;
using Engine.DynamicObjects;
using Engine.DynamicObjects.Tank;
using Engine.StaticObjects;
using Point = Engine.Field.Point;

namespace ConsoleView
{
    /// <summary>
    /// Продставление игры для консольного режима
    /// </summary>
    public class ViewGameConsole : IViewBase
    {
        /// <summary>
        /// Модель игры
        /// </summary>
        private readonly GameModel _gameModel;

        /// <summary>
        /// Настройки игры
        /// </summary>
        private readonly GameCurrentStateAndSettingsSingleton _gameSettings = GameCurrentStateAndSettingsSingleton.GetInstance();

        /// <summary>
        /// Буфер консоли
        /// </summary>
        private readonly char[,] _gameFieldCharView;

        /// <summary>
        /// Буфер предыдущего состояния консоли
        /// </summary>
        private readonly char[,] _previousGameFieldCharView;

        /// <summary>
        /// Ширина игрового поля в клетках
        /// </summary>
        private readonly int _width;

        /// <summary>
        /// Высота игрового поля в клетках
        /// </summary>
        private readonly int _height;

        /// <summary>
        /// Массив текущих цветов для клеток
        /// </summary>
        private readonly ConsoleColor[,] _gameFieldColorView;

        /// <summary>
        /// Массив цветов для клеток, актуальный тик назад
        /// </summary>
        private readonly ConsoleColor[,] _previousGameFieldColorView;

        /// <summary>
        /// Предыдущие позиции танков и пуль
        /// </summary>
        private readonly Dictionary<AbstractDynamicObject, Point> _previousPositions = new();

        /// <summary>
        /// Мьютекс на вывод в консоль
        /// </summary>
        public readonly object _consoleLock = new object();

        /// <summary>
        /// Конструктор представления игры для консольного режима 
        /// </summary>
        /// <param name="parGameModel">Модель игры</param>
        public ViewGameConsole(GameModel parGameModel)
        {
            _gameModel = parGameModel;
            _width = _gameModel.Field.GetWidthInCells();
            _height = _gameModel.Field.GetHeightInCells();
            _gameFieldCharView = new char[_height, _width];
            _previousGameFieldCharView = new char[_height, _width];

            _gameFieldColorView = new ConsoleColor[_height, _width];
            _previousGameFieldColorView = new ConsoleColor[_height, _width];

            lock (_consoleLock)
            {
                Console.CursorVisible = false;
                Console.Clear();
                Console.ResetColor();
            }

            // Подписка на события
            _gameModel.ObjectMoved += OnObjectMoved;
            _gameModel.BulletDestroyed += OnBulletDestroyed;
            _gameModel.EnemyTankDestroyed += OnTankDestroyed;

            _gameModel.PlayerTankInfoUpdate += PrintUserInfo;
            _gameModel.PlayerTankDestroyed += () =>
            {
                OnTankDestroyed(_gameModel.Player.PlayerTank, false);
            };

            _gameModel.OnPlayerTankInfoUpdate();
            ClearAllField();
            DrawFieldObjects();
            _gameModel.Start();

        }

        void PrintUserInfo()
        {
            lock (_consoleLock)
            {
                var color = Console.ForegroundColor;
                int x = Console.CursorLeft;
                int y = Console.CursorTop;
                Console.ForegroundColor = ConsoleColor.White;
                Console.CursorLeft = 0;
                Console.CursorTop = 42;
                Console.WriteLine(_gameSettings.GetImportantInfoForView());
                Console.CursorLeft = x;
                Console.CursorTop = y;
                Console.ForegroundColor = color;
            }
        }

        /// <summary>
        /// Событие перемещения объекта
        /// </summary>
        /// <param name="parObj">Перемещаемый объект</param>
        /// <param name="parNewPosition">Новые координаты объекта</param>
        private void OnObjectMoved(AbstractDynamicObject parObj, Point parNewPosition)
        {
            if (parObj == null) return;

            if (parObj is AbstractTank tank)
            {
                DrawTank(tank, tank.Id == _gameModel.Player.PlayerTank.Id);
                _previousPositions[tank] = parNewPosition;
            }
            else if (parObj is Bullet bullet)
            {
                DrawBullet(bullet);
                _previousPositions[bullet] = parNewPosition;
            }
            RefreshView();

            Render();
        }

        /// <summary>
        /// Очистить ячейки под представлением танка
        /// </summary>
        /// <param name="parTopLeftX">Верхний левый угол танка</param>
        /// <param name="parTopLeftY"></param>
        private void RefreshView()
        {
            for (int y = 0; y < 40; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    if (x >= 0 && x < _width && y >= 0 && y < _height)
                    {
                        RestoreStaticObjectAt(x, y);
                    }
                }
            }
        }

        /// <summary>
        /// Отрисовать поле
        /// </summary>
        public void Draw()
        {
            DrawGameObjects();
        }

        /// <summary>
        /// Метод, вызываемый при остановке контроллера
        /// </summary>
        public void Stop()
        {
            _gameModel.ObjectMoved -= OnObjectMoved;
            _gameModel.BulletDestroyed -= OnBulletDestroyed;
            _gameModel.EnemyTankDestroyed -= OnTankDestroyed;
            _gameModel.PlayerTankInfoUpdate -= PrintUserInfo;
            _gameModel.Stop();
        }

        /// <summary>
        /// Отрисовать объекты на поле
        /// </summary>
        /// <param name="parIsPartialRefresh">Требуется ли частичное обновление</param>
        private void DrawFieldObjects(bool parIsPartialRefresh = false)
        {
            if (!parIsPartialRefresh)
            {
                lock (_consoleLock)
                {
                    Console.Clear();
                    Console.ResetColor();
                }
            } 

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    var cell = _gameModel.Field.CellField[x, y];
                    if (cell.StaticObject != null)
                    {
                        char symbol = GetStaticObjectChar(cell.StaticObject, out ConsoleColor color);
                        UpdateField(x, y, symbol, color);
                    }
                    if (!parIsPartialRefresh)
                    {
                        UpdateField(x, y, ' ', ConsoleColor.Black); //только при полном обновлении
                    }
                }
            }
            if (!parIsPartialRefresh)
            {
                Render();
            }
        }

        /// <summary>
        /// Отрисовать и статические, и динамические элементы игры
        /// </summary>
        private void DrawGameObjects()
        {
            var tanksCopy = _gameModel.Tanks.ToArray();
            foreach (var tank in _gameModel.Tanks)
            {
                DrawTank(tank, false);
            }
            DrawTank(_gameModel.Player.PlayerTank, true);

            var bulletsCopy = _gameModel.Bullets.ToArray();
            foreach (var bullet in bulletsCopy)
            {
                DrawBullet(bullet);
            }
        }

        /// <summary>
        /// Отрисовать танк
        /// </summary>
        /// <param name="parTank">Танк для отрисовки</param>
        /// <param name="parIsPlayer">Является ли танк танком игрока</param>
        private void DrawTank(AbstractTank parTank, bool parIsPlayer)
        {
            if (parTank == null) return;

            char[,] tankChars = GetTankUnicodeRepresentation(parTank.Direction);
            ConsoleColor tankColor = parIsPlayer ? ConsoleColor.Yellow : ConsoleColor.White;
            if (parTank is ArmoredTank)
            {
                tankColor = ConsoleColor.DarkGray;
            }
            else if (parTank is HighSpeedTank)
            {
                tankColor = ConsoleColor.Magenta;
            }
            else if (parTank is RapidFireTank)
            {
                tankColor = ConsoleColor.DarkGreen;
            }

            int topY = (int)Math.Round((parTank.Point.Y - parTank.Height / 2) / 16);
            int leftX = (int)Math.Round((parTank.Point.X - parTank.Width / 2) / 16);

            topY = Math.Max(0, Math.Min(topY, _height - 1));
            leftX = Math.Max(0, Math.Min(leftX, _width - 1));

            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 2; x++)
                {
                    int drawX = leftX + x;
                    int drawY = topY + y;

                    if (drawX >= 0 && drawX < _width && drawY >= 0 && drawY < _height)
                    {
                        var cell = _gameModel.Field.CellField[drawX, drawY];
                        if (cell.StaticObject != null && cell.StaticObject is Bush)
                        {
                            char symbol = GetStaticObjectChar(cell.StaticObject, out ConsoleColor color);
                            UpdateField(drawX, drawY, symbol, color);
                        }
                        else
                        {
                            UpdateField(drawX, drawY, tankChars[y, x], tankColor);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Получить вид танка в консоли
        /// </summary>
        /// <returns>Вид танка в консоли</returns>
        private char[,] GetTankUnicodeRepresentation(DirectionEnum parDirectionEnum)
        {
            if (parDirectionEnum == DirectionEnum.Up)
            {
                return new[,] { { '▲', '▲' }, { '█', '█' } };
            }
            if (parDirectionEnum == DirectionEnum.Left)
            {
                return new[,] { { '◄', '█' }, { '◄', '█' } };
            }
            if (parDirectionEnum == DirectionEnum.Right)
            {
                return new[,] { { '█', '►' }, { '█', '►' } };
            }
            if (parDirectionEnum == DirectionEnum.Down)
            {
                return new[,] { { '█', '█' }, { '▼', '▼' } };
            }
            return new[,] { { '▲', '▲' }, { '█', '█' } };
        }

        /// <summary>
        /// Перерисовать блоки по координатам
        /// </summary>
        /// <param name="parX">Абсцисса перерисовки</param>
        /// <param name="parY">Ордината перерисовки</param>
        private void RestoreStaticObjectAt(int parX, int parY)
        {
            var cell = _gameModel.Field.CellField[parX, parY];
            char symbol;
            if (cell.StaticObject != null)
            {
                symbol = GetStaticObjectChar(cell.StaticObject, out ConsoleColor color);
                UpdateField(parX, parY, symbol, color);
            }
            else
            {
                symbol = ' ';
                UpdateField(parX, parY, symbol, ConsoleColor.Black);
            }
        }

        /// <summary>
        /// Отрисовать представление пули
        /// </summary>
        /// <param name="parBullet">Модель пули</param>
        private void DrawBullet(Bullet parBullet)
        {
            if (parBullet == null) return;
            int x = (int)(parBullet.Point.X / 16);
            int y = (int)(parBullet.Point.Y / 16);
            if (x < 0 || y < 0 || x >= _width || y >= _height)
            {
                return;
            }
            var cell = _gameModel.Field.CellField[x, y];
            if (cell.StaticObject != null && cell.StaticObject is Bush)
            {
                char symbol = GetStaticObjectChar(cell.StaticObject, out ConsoleColor color);
                UpdateField(x, y, symbol, color);
            }
            else
            {
                UpdateField(x, y, '*');
            }
        }

        /// <summary>
        /// Обработчик события уничтожения пули
        /// </summary>
        /// <param name="parBullet">Уничтожаемая пуля</param>
        private void OnBulletDestroyed(Bullet parBullet)
        {
            if (parBullet == null) return;
            parBullet.OnObjectDestroyed();

            if (_previousPositions.TryGetValue(parBullet, out var oldPosition))
            {
                _previousPositions.Remove(parBullet);
            }
            RefreshView();
        }

        /// <summary>
        /// Событие, вызываемое при уничтожении танка
        /// </summary>
        /// <param name="parTank">Уничтожаемый танк</param>
        /// <param name="parIsKilledByPlayer">Танк уничтоден игроком?</param>
        private void OnTankDestroyed(AbstractTank parTank, bool parIsKilledByPlayer)
        {
            if (parTank == null) return;

            if (_previousPositions.TryGetValue(parTank, out var oldPosition))
            {
                _previousPositions.Remove(parTank);
            }
            RefreshView();
            Render();
        }

        /// <summary>
        /// Получить символ статического объекта
        /// </summary>
        /// <param name="parStaticObject">Статический объект (блок)</param>
        /// <param name="parColor">Цвет блока</param>
        /// <returns></returns>
        private char GetStaticObjectChar(AbstractStaticObject parStaticObject, out ConsoleColor parColor)
        {
            return parStaticObject switch
            {
                Wall => AssignColorAndChar(ConsoleColor.Red, '▒', out parColor),
                IndestructibleWall => AssignColorAndChar(ConsoleColor.Gray, '█', out parColor),
                Water => AssignColorAndChar(ConsoleColor.Blue, '▒', out parColor),
                Bush => AssignColorAndChar(ConsoleColor.Green, '▒', out parColor),
                Staff => AssignColorAndChar(ConsoleColor.White, '▒', out parColor),
                _ => AssignColorAndChar(ConsoleColor.Black, ' ', out parColor),
            };
        }

        /// <summary>
        /// Назначить цвет символу
        /// </summary>
        /// <param name="parColor">Цвет</param>
        /// <param name="parSymbol">Символ</param>
        /// <param name="outAssignedColor">Назначенный цвет</param>
        /// <returns></returns>
        private char AssignColorAndChar(ConsoleColor parColor, char parSymbol, out ConsoleColor outAssignedColor)
        {
            outAssignedColor = parColor;
            return parSymbol;
        }

        /// <summary>
        /// Обновить ячейку поля
        /// </summary>
        /// <param name="parX">Абсцисса обновляемой ячейки</param>
        /// <param name="parY">Ордината обновляемой ячейки</param>
        /// <param name="parC">Символ для обновления</param>
        /// <param name="parColor">Цвет символа</param>
        private void UpdateField(int parX, int parY, char parC, ConsoleColor parColor = ConsoleColor.White)
        {
            if (parX < 0 || parX >= _width || parY < 0 || parY >= _height)
            {
                return;
            }
            _gameFieldCharView[parY, parX] = parC;
            _gameFieldColorView[parY, parX] = parColor;
        }

        /// <summary>
        /// Очистить ячейку поля
        /// </summary>
        /// <param name="parX">Абсцисса ячейки</param>
        /// <param name="parY">Ордината ячейки</param>
        private void ClearField(int parX, int parY)
        {
            if (parX >= 50 || parY >= 40 || parX < 0 || parY < 0)
            {
                return;
            }
            RestoreStaticObjectAt(parX, parY);
        }

        /// <summary>
        /// Очистка ячеек на всем поле
        /// </summary>
        public void ClearAllField()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    ClearField(x, y);
                }
            }
        }

        /// <summary>
        /// Печать поля на консоль
        /// </summary>
        private void Render()
        {
            lock (_consoleLock)
            {
                DrawFieldObjects(true);
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        if (_gameFieldCharView[y, x] != _previousGameFieldCharView[y, x] ||
                            _gameFieldColorView[y, x] != _previousGameFieldColorView[y, x])
                        {
                            Console.SetCursorPosition(x * 2, y); // Учитываем ширину символа (2 знакоместа)
                            Console.ForegroundColor = _gameFieldColorView[y, x];
                            if (_gameFieldCharView[y, x] == '*')
                            {
                                Console.Write('*');
                            }
                            else
                            {
                                Console.Write(new string(_gameFieldCharView[y, x], 2)); // Дублируем символ
                            }
                            _previousGameFieldCharView[y, x] = _gameFieldCharView[y, x];
                            _previousGameFieldColorView[y, x] = _gameFieldColorView[y, x];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Очистка игрового поля
        /// </summary>
        public void ClearGameField()
        {
            lock (_consoleLock)
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        _gameFieldCharView[y, x] = ' ';
                        _previousGameFieldCharView[y, x] = ' ';
                        _gameFieldColorView[y, x] = ConsoleColor.Black;
                        _previousGameFieldColorView[y, x] = ConsoleColor.Black;
                    }
                }
            }
            Console.Clear();
        }


    }
}
