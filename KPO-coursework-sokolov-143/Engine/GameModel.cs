using System.Diagnostics;
using System.Drawing;
using System.Linq.Expressions;
using Engine.DynamicObjects;
using Engine.DynamicObjects.Tank;
using Engine.Field;
using Engine.StaticObjects;
using Point = Engine.Field.Point;

namespace Engine
{
    /// <summary>
    /// Модель игры
    /// </summary>
    public class GameModel
    {
        /// <summary>
        /// Игровое поле
        /// </summary>
        private GameField _field;

        /// <summary>
        /// Модель игрока
        /// </summary>
        private PlayerModel _player;

        /// <summary>
        /// Множество вражеских танков
        /// </summary>
        private HashSet<AbstractTank> _enemyTanks = new HashSet<AbstractTank>();

        /// <summary>
        /// Множество пуль
        /// </summary>
        private HashSet<Bullet> _bullets = new HashSet<Bullet>();

        /// <summary>
        /// Словарь блоков базы с их координатами
        /// </summary>
        private Dictionary<Staff, Point> _staffBlocksCoordinates = new Dictionary<Staff, Point>();

        /// <summary>
        /// Токен для остановки потока модели игры
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Счетчик блоков базы
        /// </summary>
        private int _staffBlocksCounter = 0;

        /// <summary>
        /// Блокировка для синхронизации между потоками
        /// </summary>
        object _globalLock = new();

        /// <summary>
        /// Делегат уничтожения танка
        /// </summary>
        /// <param name="parTank">Уничтоженный танк</param>
        /// <param name="parIsKilledByPlayer">Уничтожен ли танк пользователем</param>
        public delegate void dTankDestroyedHandler(AbstractTank parTank, bool parIsKilledByPlayer);

        /// <summary>
        /// Событие уничтожения танка противника
        /// </summary>
        public event dTankDestroyedHandler EnemyTankDestroyed;

        /// <summary>
        /// Делегат события обновления статического объекта
        /// </summary>
        /// <param name="parAbstractStaticObject">Статический объект (блок) для обновления</param>
        public delegate void dStaticObjectRefreshHandler(AbstractStaticObject parAbstractStaticObject);

        /// <summary>
        /// Событие обновления статического объекта
        /// </summary>
        public event dStaticObjectRefreshHandler StaticObjectRefreshed;

        /// <summary>
        /// Делегат события уничтожения статического объекта
        /// </summary>
        /// <param name="parAbstractStaticObject">Статический объект (блок) для уничтожения</param>
        public delegate void dStaticObjectDestroyHandler(AbstractStaticObject parAbstractStaticObject);

        /// <summary>
        /// Событие уничтожения статического объекта
        /// </summary>
        public event dStaticObjectDestroyHandler StaticObjectDestroyed;

        /// <summary>
        /// Void-делегат
        /// </summary>
        public delegate void dVoid();

        /// <summary>
        /// Событие уничтожения танка игрока
        /// </summary>
        public event dVoid PlayerTankDestroyed;

        /// <summary>
        /// Событие уничтожения блока штаба
        /// </summary>
        public event dVoid StaffDestroyed;

        /// <summary>
        /// Событие уничтожения всех танков противников
        /// </summary>
        public event dVoid AllEnemyTanksDestroyed;
        
        /// <summary>
        /// Делегат уничтожения пули
        /// </summary>
        /// <param name="parBullet">Пуля для уничтожения</param>
        public delegate void dBulletDestroyedHandler(Bullet parBullet);

        /// <summary>
        /// Событие уничтожения пули
        /// </summary>
        public event dBulletDestroyedHandler BulletDestroyed;

        /// <summary>
        /// Делегат перемещения динамического объекта (танка, пули) на новую позицию
        /// </summary>
        /// <param name="parAbstractDynamicObject">Динамический объект длят перемещения</param>
        /// <param name="parNewPosition">Координаты новой позиции</param>
        public delegate void dObjectMovedHandler(AbstractDynamicObject parAbstractDynamicObject, Point parNewPosition);

        /// <summary>
        /// Обработчик перемещения динамического объекта (танка, пули) на новую позицию
        /// </summary>
        public event dObjectMovedHandler ObjectMoved;

        /// <summary>
        /// Делегат обновления информации по танку игрока
        /// </summary>
        public delegate void dPlayerTankInfoUpdateHandler();

        /// <summary>
        /// Событие обновления информации по танку игрока
        /// </summary>
        public event dPlayerTankInfoUpdateHandler PlayerTankInfoUpdate;

        /// <summary>
        /// Модель игрока
        /// </summary>
        public PlayerModel Player
        {
            get
            {
                lock (_globalLock)
                {
                    return _player;
                }
            }
            protected set
            {
                lock (_globalLock)
                {
                    _player = value;
                }
            }
        }

        /// <summary>
        /// Множество вражеских танков
        /// </summary>
        public HashSet<AbstractTank> Tanks
        {
            get
            {
                lock (_globalLock)
                {
                    return _enemyTanks;
                }
            }
            private set
            {
                lock (_globalLock)
                {
                    _enemyTanks = value;
                }
            }
        }

        /// <summary>
        /// Множество пуль
        /// </summary>
        public HashSet<Bullet> Bullets
        {
            get
            {
                lock (_globalLock)
                {
                    return _bullets;
                }
            }
            private set
            {
                lock (_globalLock)
                {
                    _bullets = value;
                }
            }
        }

        /// <summary>
        /// Модель игрового поля
        /// </summary>
        public GameField Field
        {
            get
            {
                lock (_globalLock)
                {
                    return _field;
                }
            }
            set
            {
                lock (_globalLock)
                {
                    _field = value;
                }
            }
        }

        /// <summary>
        /// Словарь блоков штаба и их координат
        /// </summary>
        public Dictionary<Staff, Point> StaffBlocksCoordinates
        {
            get
            {
                return _staffBlocksCoordinates;
            }
        }

        /// <summary>
        /// Создает модель игры с заданными параметрами
        /// </summary>
        /// <param name="parFieldWidth">Ширина поля в блоках</param>
        /// <param name="parFieldHeight">Высота поля в блоках</param>
        /// <param name="parCellSize">Ширина ячейки</param>
        /// <param name="parPlayerStartPosition">Стартовая позиция игрока</param>
        public GameModel(int parFieldWidth, int parFieldHeight, int parCellSize, Point parPlayerStartPosition)
        {
            _field = new GameField(parFieldWidth, parFieldHeight, parCellSize);
            _player = new PlayerModel(parPlayerStartPosition);
            _enemyTanks = new HashSet<AbstractTank>();
            _bullets = new HashSet<Bullet>();
        }

        /// <summary>
        /// Обновить координаты блоков базы
        /// </summary>
        public void UpdateStaffBlocksCoordinates()
        {
            int counter = 0;
            for (int i = 0; i < _field.GetWidthInCells(); i++)
            {
                for (int j = 0; j < _field.GetHeightInCells(); j++)
                {
                    counter++;
                    Cell eachFieldCell = _field.CellField[i, j];
                    if (eachFieldCell.StaticObject != null && eachFieldCell.StaticObject is Staff)
                    {
                        _staffBlocksCoordinates[(Staff)(eachFieldCell.StaticObject)] = new Point(i * _field.CellSize, j * _field.CellSize);
                        _staffBlocksCounter++;
                    }
                }
            }
        }

        /// <summary>
        /// Игровой тик
        /// </summary>
        /// <param name="parDeltaTime">Время в секундах на выполнение предыдущего тика</param>
        public void Tick(float parDeltaTime)
        {
            ProcessTanks(parDeltaTime);
            ProcessBullets(parDeltaTime);
            if (Player.PlayerTank != null)
            {
                foreach (var elTank in Tanks)
                {
                    elTank.UpdateAI(this);
                }
            }
        }

        /// <summary>
        /// Переместить танк по карте
        /// </summary>
        /// <param name="parTank">Перемещаемый танк</param>
        /// <param name="parNewPosition">Координаты новой позиции</param>
        private void UpdateTankPosition(AbstractTank parTank, Point parNewPosition)
        {
            parTank.Point = parNewPosition;
            ObjectMoved?.Invoke(parTank, parNewPosition);
        }

        /// <summary>
        /// Обработчик события перемещения танка
        /// </summary>
        /// <param name="parTank">Перемещаемый танк</param>
        public void OnTankDirectionUpdated(AbstractTank parTank)
        {
            ObjectMoved?.Invoke(parTank, parTank.Point);
        }

        /// <summary>
        /// Обработчик события обновления информации по танку игрока
        /// </summary>
        public void OnPlayerTankInfoUpdate()
        {
            PlayerTankInfoUpdate?.Invoke();
        }

        /// <summary>
        /// Проверяет корректность новой позиции танка
        /// </summary>
        /// <param name="parTank">Танк, для которого выполняется проверка</param>
        /// <param name="parNewPosition">Проверяемая позиция</param>
        /// <returns>Истина, если позиция корректна</returns>
        private bool IsTankPositionValid(AbstractTank parTank, Point parNewPosition)
        {
            if (!Field.IsWithinBounds(parNewPosition))
                return false;

            // Проверяются ячейки на целевой позиции (включая соседние, т.к. танк шире одной ячейки)
            HashSet<Cell> targetCells = [];
            int stepX = parTank.Width / 2;
            int stepY = parTank.Height / 2;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var offsetX = parNewPosition.X - parTank.Width / 2 + stepX * i;
                    var offsetY = parNewPosition.Y - parTank.Height / 2 + stepY * j;

                    targetCells.Add(_field.GetCellByCoordinates((int)offsetX, (int)offsetY));
                }
            }

            foreach (var elTargetCell in targetCells)
            {
                if (elTargetCell.StaticObject != null && !elTargetCell.StaticObject.IsPassableByTank)
                    return false;
            }

            return !IsCollidingWithOtherTanks(parTank, parNewPosition);
        }

        /// <summary>
        /// Проверка на коллизии с другими танками
        /// </summary>
        /// <param name="parTank">Проверяемый танк</param>
        /// <param name="parNewPosition">Проверяемая позиция</param>
        /// <returns>Истина, если на данной позиции уже находится танк</returns>
        private bool IsCollidingWithOtherTanks(AbstractTank parTank, Point parNewPosition)
        {
            return Tanks.Append(_player.PlayerTank).Any(otherTank =>
                otherTank != parTank &&
                IsColliding(parTank, parNewPosition, otherTank));
        }

        /// <summary>
        /// Обработка танков модели
        /// </summary>
        /// <param name="parDeltaTime">Время, затраченное на предыдущий игровой тик</param>
        private void ProcessTanks(float parDeltaTime)
        {
            long fieldWidth = _field.GetWidthInPixels();
            long fieldHeight = _field.GetHeightInPixels();
            var allTanks = Tanks.Append(_player.PlayerTank).ToList();

            foreach (var elTank in allTanks)
            {
                Point newCoordinatePoint = elTank.CalculateNewPosition((uint)fieldWidth, (uint)fieldHeight, parDeltaTime);

                HashSet<Cell> targetCells = [];
                int stepX = elTank.Width / 2;
                int stepY = elTank.Height / 2;

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        var offsetX = newCoordinatePoint.X - elTank.Width / 2 + stepX * i;
                        var offsetY = newCoordinatePoint.Y - elTank.Height / 2 + stepY * j;

                        targetCells.Add(_field.GetCellByCoordinates((int)offsetX, (int)offsetY));
                    }
                }

                foreach (var elCell in targetCells)
                {
                    if (elCell.StaticObject != null) {
                        StaticObjectRefreshed?.Invoke(elCell.StaticObject);
                    }
                }

                if (!IsTankPositionValid(elTank, newCoordinatePoint))
                    continue;

                UpdateTankPosition(elTank, newCoordinatePoint);
            }
        }


        /// <summary>
        /// Обновить координаты танка игрока
        /// </summary>
        public void UpdatePlayerTankPosition()
        {
            long fieldWidth = _field.GetWidthInPixels();
            long fieldHeight = _field.GetHeightInPixels();
            var playerTank = Player.PlayerTank;
            Point newCoordinatePoint = Player.PlayerTank.CalculateNewPosition((uint)fieldWidth, (uint)fieldHeight, 0.05f);

            HashSet<Cell> targetCells = [];
            int stepX = playerTank.Width / 2;
            int stepY = playerTank.Height / 2;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var offsetX = newCoordinatePoint.X - playerTank.Width / 2 + stepX * i;
                    var offsetY = newCoordinatePoint.Y - playerTank.Height / 2 + stepY * j;

                    targetCells.Add(_field.GetCellByCoordinates((int)offsetX, (int)offsetY));
                }
            }

            foreach (var elCell in targetCells)
            {
                if (elCell.StaticObject != null)
                {
                    StaticObjectRefreshed?.Invoke(elCell.StaticObject);
                }
            }

            if (!IsTankPositionValid(playerTank, newCoordinatePoint))
                return;

            UpdateTankPosition(playerTank, newCoordinatePoint);
        }

        /// <summary>
        /// Проверка коллизий танков с танками и пуль с танками
        /// </summary>
        /// <param name="parDynamicObject">Проверяемый танк или пуля</param>
        /// <param name="parNewPosition">Проверяемая позиция</param>
        /// <param name="parOtherTank">Другой проверяемый танк</param>
        /// <returns>Истина, если объекты сталкиваются</returns>
        private bool IsColliding(AbstractDynamicObject parDynamicObject, Point parNewPosition, AbstractTank parOtherTank)
        {
            // Обработка частного случая для пули
            if (parDynamicObject is Bullet && ((Bullet)parDynamicObject).ParentTankId == parOtherTank.Id)
            {
                return false;
            }
            int tankWidth = (int)parDynamicObject.Width;
            int tankHeight = (int)parDynamicObject.Height;
            int otherTankWidth = (int)parOtherTank.Width;
            int otherTankHeight = (int)parOtherTank.Height;
            var tankBounds = new Rectangle((int)parNewPosition.X - tankWidth / 2,
                (int)parNewPosition.Y - tankHeight / 2, tankWidth, tankHeight);
            var otherTankBounds = new Rectangle((int)parOtherTank.Point.X - otherTankWidth / 2,
                (int)parOtherTank.Point.Y - otherTankHeight / 2, otherTankWidth, otherTankHeight);
            return tankBounds.IntersectsWith(otherTankBounds);
        }

        /// <summary>
        /// Обработка всех пуль
        /// </summary>
        /// <param name="parDeltaTime">Время на выполнение предыдущего игрового тика</param>
        private void ProcessBullets(float parDeltaTime)
        {
            long fieldWidth = _field.GetWidthInPixels();
            long fieldHeight = _field.GetHeightInPixels();
            var bulletsCopy = Bullets.ToList();
            var tanksCopy = Tanks.ToList().Append(_player.PlayerTank);

            foreach (var elBullet in bulletsCopy)
            {
                Point newCoordinatePoint = elBullet.CalculateNewPosition((uint)fieldWidth, (uint)fieldHeight, parDeltaTime);
                if (newCoordinatePoint.X <= elBullet.Width / 2 || newCoordinatePoint.X >= fieldWidth - elBullet.Width / 2
                    || newCoordinatePoint.Y <= elBullet.Height / 2 || newCoordinatePoint.Y >= fieldHeight - elBullet.Height / 2)
                {
                    Bullets.Remove(elBullet);
                    BulletDestroyed?.Invoke(elBullet);
                    continue;
                }
                HashSet<Cell> currentCells = [];
                currentCells.Add(_field.GetCellByCoordinates((int)newCoordinatePoint.X, (int)newCoordinatePoint.Y));
                currentCells.Add(_field.GetCellByCoordinates((int)newCoordinatePoint.X - elBullet.Width, (int)newCoordinatePoint.Y - elBullet.Height));
                currentCells.Add(_field.GetCellByCoordinates((int)newCoordinatePoint.X - elBullet.Width, (int)newCoordinatePoint.Y + elBullet.Height));
                currentCells.Add(_field.GetCellByCoordinates((int)newCoordinatePoint.X + elBullet.Width, (int)newCoordinatePoint.Y - elBullet.Height));
                currentCells.Add(_field.GetCellByCoordinates((int)newCoordinatePoint.X + elBullet.Width, (int)newCoordinatePoint.Y + elBullet.Height));

                bool isBulletDestroyed = false;
                foreach (var elCell in currentCells)
                {
                    if (elCell.IsPassableByBullet())
                    {
                        elBullet.Point = newCoordinatePoint;
                        ObjectMoved?.Invoke(elBullet, newCoordinatePoint);
                        if (elCell.StaticObject != null)
                        {
                            StaticObjectRefreshed?.Invoke(elCell.StaticObject);
                        }
                    }
                    else
                    {
                        if (elCell.IsDestructible())
                        {
                            StaticObjectDestroyed?.Invoke(elCell.StaticObject);
                            if (elCell.StaticObject != null && elCell.StaticObject is Staff)
                            {
                                _staffBlocksCounter--;
                                _staffBlocksCoordinates.Remove((Staff)(elCell.StaticObject));
                                if (_staffBlocksCounter == 0)
                                {
                                    StaffDestroyed?.Invoke();
                                }
                            }
                            elCell.StaticObject = null;
                        }
                        isBulletDestroyed = true;
                    }
                }
                if (isBulletDestroyed)
                {
                    Bullets.Remove(elBullet);
                    BulletDestroyed?.Invoke(elBullet);
                    continue;
                }

                foreach (var elTank in tanksCopy)
                {
                    if (IsColliding(elBullet, newCoordinatePoint, elTank))
                    {
                        bool isFiredByPlayer = elBullet.ParentTankId == _player.PlayerTank.Id;
                        elTank.ApplyDamage(elBullet);
                        Bullets.Remove(elBullet);
                        BulletDestroyed?.Invoke(elBullet);
                        if (elTank.Id == _player.PlayerTank.Id)
                        {
                            PlayerTankInfoUpdate?.Invoke();
                        }

                        if (elTank.Health == 0)
                        {
                            Tanks.Remove(elTank);
                            if (elTank.Id == Player.PlayerTank.Id)
                            {
                                PlayerTankInfoUpdate?.Invoke();
                                PlayerTankDestroyed?.Invoke();
                            }
                            else
                            {
                                EnemyTankDestroyed?.Invoke(elTank, isFiredByPlayer);
                            }
                            if (Tanks.Count == 0)
                            {
                                AllEnemyTanksDestroyed?.Invoke();
                            }
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Выстрелить из заданного танка
        /// </summary>
        /// <param name="parAbstractTank">Танк, который будет стрелять</param>
        public void Fire(AbstractTank parAbstractTank)
        {
            if (parAbstractTank.Id == _player.PlayerTank.Id)
            {
                int c = parAbstractTank.Health;
            }
            Bullet? bullet = parAbstractTank.Fire();
            if (bullet != null)
            {
                Bullets.Add(bullet);
            }
        }

        //public void MovePlayer()
        //{
        //    float currentX = Player.PlayerTank.Point.X;
        //    float currentY = Player.PlayerTank.Point.Y;
        //    if (Player.PlayerTank.Direction == DirectionEnum.Up)
        //    {
        //        if (_player.PlayerTank.Point.X + Player.
        //        Player.PlayerTank.Point.X -= 5;
        //    }
           
        //}

        /// <summary>
        /// Запуск модели игры в отдельном потоке
        /// </summary>
        public void Start()
        {
            for (int x = 0; x < _field.GetWidthInCells(); x++)
            {
                for (int y = 0; y < _field.GetHeightInCells(); y++)
                {
                    var cell = _field.CellField[x, y];
                    if (cell.StaticObject != null)
                    {
                        cell.StaticObject.Point = new Point(x, y);
                    }
                }
            }
            // Если игра уже запущена, то повторно не запускается
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;

            Task.Run(async () =>
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                long previousTime = stopwatch.ElapsedMilliseconds;

                while (!cancellationToken.IsCancellationRequested)
                {
                    // Замер времени, прошедшего с последней итерации
                    long currentTime = stopwatch.ElapsedMilliseconds;
                    float deltaTime = (currentTime - previousTime) / 1000f;
                    previousTime = currentTime;

                    Tick(deltaTime);
                }
            }, cancellationToken);
        }

        /// <summary>
        /// Остановка игрового процесса
        /// </summary>
        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}
