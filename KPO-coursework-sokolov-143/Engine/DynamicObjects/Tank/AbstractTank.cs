using Engine.Field;
using System.Net.WebSockets;
using Point = Engine.Field.Point;

namespace Engine.DynamicObjects.Tank
{
    /// <summary>
    /// Абстрактный танк
    /// </summary>
    public abstract class AbstractTank : AbstractDynamicObject
    {
        /// <summary>
        /// Скорость танка в писклелях в секунду по умолчанию
        /// </summary>
        public static readonly int DEFAULT_SPEED = 40;

        /// <summary>
        /// Скорострельность танка (максимальное количество одновременно летящих снарядов) по умолчанию
        /// </summary>
        public static readonly int DEFAULT_FIRING_RATE = 1;

        /// <summary>
        /// Базовый урон, наносимый снарядом танка
        /// </summary>
        public static readonly int DEFAULT_DAMAGE = 10;

        /// <summary>
        /// Здоровье танка по умолчанию
        /// </summary>
        public static readonly int DEFAULT_HEALTH = 100;

        /// <summary>
        /// Базовая ширина танка
        /// </summary>
        public static readonly int BASE_WIDTH_TANK = 28;

        /// <summary>
        /// Базовая высота танка
        /// </summary>
        public static readonly int BASE_HEIGHT_TANK = 28;

        /// <summary>
        /// Скорострельность танка
        /// </summary>
        private int _firingRate = DEFAULT_FIRING_RATE;

        /// <summary>
        /// Урон, наносимый снарядом танка
        /// </summary>
        private int _damage = DEFAULT_DAMAGE;

        /// <summary>
        /// Здоровье танка
        /// </summary>
        private int _health = DEFAULT_HEALTH;

        /// <summary>
        /// Количество пуль, выпущенных данным танком, 
        /// которые еще продолжают лететь
        /// </summary>
        private int _activeBullets = 0;

        /// <summary>
        /// Время последнего изменения направления
        /// </summary>
        private DateTime _lastDirectionChangeTime = DateTime.MinValue;

        /// <summary>
        /// Время последней стрельбы
        /// </summary>
        private DateTime _lastFireTime = DateTime.MinValue;

        /// <summary>
        /// Минимальный интервал времени между сменами направления
        /// </summary>
        private readonly TimeSpan _directionChangeCooldown = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Минимальный интервал времени между стрельбой
        /// </summary>
        private TimeSpan _fireCooldown = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Генератор псевдослучайных чисел
        /// </summary>
        private Random _random = new Random();

        /// <summary>
        /// Минимальный интервал времени между сменами направления
        /// </summary>
        public TimeSpan DirectionChangeCooldown
        {
            get
            {
                return _directionChangeCooldown;
            }  
        }

        /// <summary>
        /// Скорострельность танка
        /// </summary>
        public int FiringRate
        {
            get
            {
                return _firingRate;
            }
            protected set
            {
                _firingRate = value;
            }
        }

        /// <summary>
        /// Количество пуль, выпущенных данным танком, 
        /// которые еще продолжают лететь
        /// </summary>
        public int ActiveBullets
        {
            get
            {
                return _activeBullets;
            }
            set
            {
                _activeBullets = value;
            }
        }

        /// <summary>
        /// Минимальный интервал времени между стрельбой
        /// </summary>
        public TimeSpan FireCooldown
        {
            get
            {
                return _fireCooldown;
            }
            set
            {
                _fireCooldown = value;
            }
        }

        /// <summary>
        /// Здоровье танка
        /// </summary>
        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
            }
        }

        /// <summary>
        /// Урон, наносимый выпускаемой пулей
        /// </summary>
        public int Damage
        {
            get
            {
                return _damage;
            }
            set
            {
                _damage = value;
            }
        }

        /// <summary>
        /// Конструктор абстрактного танка
        /// </summary>
        /// <param name="parStartPoint">Стартовая точка танка (в пикселях)</param>
        protected AbstractTank(Point parStartPoint) : base(Guid.NewGuid(), DEFAULT_SPEED, parStartPoint, DirectionEnum.Down, false, BASE_WIDTH_TANK, BASE_HEIGHT_TANK)
        {
        }

        /// <summary>
        /// Уменьшить значение здоровья на некоторую величину
        /// </summary>
        /// <param name="parLostHealth">Вычитаемое значение здоровья</param>
        public void ApplyDamage(Bullet parBullet)
        {
            if (parBullet.ParentTankId == Id)
            {
                return;
            }
            if (Health <= parBullet.Damage)
            {
                Health = 0;
            }
            else
            {
                Health -= parBullet.Damage;
            }
        }

        /// <summary>
        /// Обновить поведение танка
        /// </summary>
        /// <param name="parGameModel">Модель игры</param>
        public void UpdateAI(GameModel parGameModel)
        {
            if (parGameModel.Player.PlayerTank == null) return;

            var copyPlayerPositionCells = new Point(parGameModel.Player.PlayerTank.Point);
            copyPlayerPositionCells.X /= (parGameModel.Field.CellSize);
            copyPlayerPositionCells.Y /= (parGameModel.Field.CellSize);
            var copyTankPositionCells = new Point(this.Point);
            copyTankPositionCells.X /= (parGameModel.Field.CellSize);
            copyTankPositionCells.Y /= (parGameModel.Field.CellSize);

            // Проверка на прямую видимость
            List<Point> targetPoints = new();
            targetPoints.Add(parGameModel.Player.PlayerTank.Point);
            targetPoints.AddRange(parGameModel.StaffBlocksCoordinates.Values.ToList());
            for (int i = 0; i < targetPoints.Count; i++)
            {
                Point elTargetPoint = targetPoints[i];
                if (HasDirectLineOfSight(parGameModel.Field, this.Point, elTargetPoint, i != 0))
                {
                    var directionToTarget = GetDirectionToPoint(elTargetPoint);
                    if (Direction != directionToTarget && (DateTime.Now - _lastDirectionChangeTime >= _directionChangeCooldown))
                    {
                        _lastDirectionChangeTime = DateTime.Now;
                        Direction = directionToTarget; // Поворот танка к цели
                        parGameModel.OnTankDirectionUpdated(this);
                        parGameModel.Fire(this); // Стрельба
                        return;
                    }
                    parGameModel.Fire(this); // Стрельба
                    if (i == 0 || _random.Next(4) != 0)
                    {
                        return;
                    }
                }
            }

            var field = parGameModel.Field;

            // Вычисление пути с помощью алгоритма Ли
            var path = FindPathWithLee(field, copyTankPositionCells, copyPlayerPositionCells, this.Width, this.Height);

            if (path != null && path.Count > 1)
            {
                var nextPointInCells = path[1];
                var nextPointInPixels = new Point(nextPointInCells.X * parGameModel.Field.CellSize + _random.Next(-12, 12), nextPointInCells.Y * parGameModel.Field.CellSize + _random.Next(-12, 13));
                ChangeDirection(nextPointInPixels);
                parGameModel.OnTankDirectionUpdated(this);
                IsMoving = true;
            }
            else
            {
                IsMoving = false;
            }
        }

        /// <summary>
        /// Получить направление танка, чтобы он встал передом к заданной точке
        /// </summary>
        /// <param name="parTarget">Целевая точка</param>
        /// <returns>Направление танка</returns>
        public DirectionEnum GetDirectionToPoint(Point parTarget)
        {
            float dx = parTarget.X - Point.X;
            float dy = parTarget.Y - Point.Y;

            if (Math.Abs(dx) > Math.Abs(dy))
            {
                return dx > 0 ? DirectionEnum.Right : DirectionEnum.Left;
            }
            else
            {
                return dy > 0 ? DirectionEnum.Down : DirectionEnum.Up;
            }
        }
        // Волновой алгоритм Ли с учетом размеров танка
        public List<Point> FindPathWithLee(GameField field, Point start, Point end, int tankWidthInCells, int tankHeightInCells)
        {
            int width = field.GetWidthInCells();
            int height = field.GetHeightInCells();
            var wave = new int[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    wave[x, y] = -1;
                }
            }

            var queue = new Queue<Point>();
            queue.Enqueue(start);
            wave[(int)(start.X), (int)(start.Y)] = 0;

            var directions = new[]
            {
                new Point(-1, 0), new Point(1, 0),
                new Point(0, -1), new Point(0, 1)
            };

            // Волновое распространение
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                foreach (var elDir in directions)
                {
                    var next = new Point(current.X + elDir.X, current.Y + elDir.Y);

                    if (IsValidCellForTank(field, next, tankWidthInCells, tankHeightInCells) &&
                        wave[(int)(next.X), (int)(next.Y)] == -1)
                    {
                        wave[(int)(next.X), (int)(next.Y)] = wave[(int)(current.X), (int)(current.Y)] + 1;
                        queue.Enqueue(next);

                        if (next.X == end.X && next.Y == end.Y)
                        {
                            queue.Clear();
                            break;
                        }
                    }
                }
            }

            // Построение пути
            if (wave[(int)(end.X), (int)(end.Y)] == -1) return null; // Путь не найден

            var path = new List<Point>();
            var currentPoint = end;
            int counter = 0;
            while (!IsCellsPointEquals(currentPoint, start) && (path.Count == 0 || !IsCellsPointEquals(currentPoint, path.Last())) && counter <= 1000)
            {
                counter++;
                path.Add(currentPoint);
                foreach (var elDir in directions)
                {
                    var next = new Point(currentPoint.X - elDir.X, currentPoint.Y - elDir.Y);
                    if ((int)(next.X) >= 50 || (int)(next.Y) >= 40)
                    {
                        float c = next.X / 2;
                    }

                    if (next.X >= 0 && next.X < width &&
                        next.Y >= 0 && next.Y < height &&
                        wave[(int)(next.X), (int)(next.Y)] == wave[(int)(currentPoint.X), (int)(currentPoint.Y)] - 1)
                    {
                        currentPoint = next;
                        break;
                    }
                }
            }

            path.Reverse();
            return path;
        }


        /// <summary>
        /// Проверка, может ли танк занять или проехать через указанную клетку
        /// </summary>
        /// <param name="parGameField">Модель игрового поля</param>
        /// <param name="parTopLeft">Крайняя левая верхняя координата танка</param>
        /// <param name="parTankWidth">Ширина танка</param>
        /// <param name="parTankHeight">Высота танка</param>
        /// <returns>Истина, если танк может проехать через клетку</returns>
        public bool IsValidCellForTank(GameField parGameField, Point parTopLeft, int parTankWidth, int parTankHeight)
        {
            int fieldWidth = parGameField.GetWidthInCells();
            int fieldHeight = parGameField.GetHeightInCells();

            for (int dx = 0; dx < parTankWidth / parGameField.CellSize; dx++)
            {
                for (int dy = 0; dy < parTankHeight/ parGameField.CellSize; dy++)
                {
                    int x = (int)Math.Round(parTopLeft.X + dx);
                    int y = (int)Math.Round(parTopLeft.Y + dy);

                    if (x < 0 || x >= fieldWidth || y < 0 || y >= fieldHeight)
                        return false;

                    var cell = parGameField.CellField[x, y];
                    if (cell.StaticObject != null && !cell.StaticObject.IsPassableByTank)
                        return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Являются ли точки эквивалентными (в масшабе клеток)
        /// </summary>
        /// <param name="parFirst">Первая точка</param>
        /// <param name="parSecond">Вторая точка</param>
        /// <returns>Истина, если точки эквивалентны</returns>
        public bool IsCellsPointEquals(Point parFirst, Point parSecond)
        {
            return (Math.Abs(parFirst.X - parSecond.X) < 0.5 && Math.Abs(parFirst.Y - parSecond.Y) < 0.5);
        }

        /// <summary>
        /// Находится ли точка parEnd в прямой видимости (на луче нет неразрушимой стены),
        /// т.е. до этой точки с позиции parStart может долететь снаряд
        /// </summary>
        /// <param name="parGameField">Модель игрового поля</param>
        /// <param name="parStart">Начальная точка</param>
        /// <param name="parEnd">Целевая конечная точка</param>
        /// <param name="parIsCell">Является ли цель клеткой (блоком)</param>
        /// <returns>Истина, если точка в досягаемости снарядом</returns>
        public bool HasDirectLineOfSight(GameField parGameField, Point parStart, Point parEnd, bool parIsCell = false)
        {
            double closeCoeff = parIsCell ? 0.2 : 0.5;
            int dx = Math.Sign(parEnd.X - parStart.X) * (Math.Abs(parEnd.X - parStart.X) / parGameField.CellSize > closeCoeff ? 1 : 0);
            int dy = Math.Sign(parEnd.Y - parStart.Y) * (Math.Abs(parEnd.Y - parStart.Y) / parGameField.CellSize > closeCoeff ? 1 : 0);
            if (dx == 0 && dy == 0)
            {
                return true;
            }
            float currentX = parStart.X;
            float currentY = parStart.Y;

            while (Math.Abs(currentX - parEnd.X) > parGameField.CellSize || Math.Abs(currentY - parEnd.Y) > parGameField.CellSize)
            {
                if (currentX < 0 || currentY < 0
                    || currentY >= parGameField.GetHeightInPixels() || currentX >= parGameField.GetWidthInPixels() ||
                    !IsCellPassableByBulletOrDestructible(parGameField, (int)currentX, (int)currentY)) return false;

                if (currentX != parEnd.X) currentX += dx;
                if (currentY != parEnd.Y) currentY += dy;
            }

            return true;
        }

        /// <summary>
        /// Определяется, преодолима ли клетка пулей (либо в ней находится разрушимый объект)
        /// </summary>
        /// <param name="parField">Модель игрового поля</param>
        /// <param name="parX">Абсцисса клетки</param>
        /// <param name="parY">Ордината клетки</param>
        /// <returns>Истина, если клетка преодолима пулей или разрушима ей</returns>
        public bool IsCellPassableByBulletOrDestructible(GameField parField, int parX, int parY)
        {
            var cell = parField.GetCellByCoordinates(parX, parY);
            return cell.StaticObject == null || cell.StaticObject.IsPassableByBullet ||
                cell.StaticObject.IsDestructible;
        }

        /// <summary>
        /// Изменить направление танка, чтобы он повернулся к целевой точке
        /// </summary>
        /// <param name="parTargetPoint">Целевая точка</param>
        public void ChangeDirection(Point parTargetPoint)
        {
            if (DateTime.Now - _lastDirectionChangeTime < _directionChangeCooldown)
            {
                return; // Слишком частая смена направления запрещена
            }
            int dx = ((int)parTargetPoint.X - (int)Point.X);
            int dy = ((int)parTargetPoint.Y - (int)Point.Y);
            if (dx == 0 && dy == 0) return;
            DirectionEnum newDirection;
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                newDirection = dx > 0 ? DirectionEnum.Right : DirectionEnum.Left;
            }
            else
            {
                newDirection = dy > 0 ? DirectionEnum.Down : DirectionEnum.Up;
            }

            if (Direction != newDirection)
            {
                Direction = newDirection;
                
                _lastDirectionChangeTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Произвести выстрел
        /// </summary>
        /// <returns>Модель пули (null, если выстрела не было)</returns>
        public virtual Bullet? Fire()
        {
            if (DateTime.Now - _lastFireTime < _fireCooldown)
            {
                return null; // Слишком рано стрелять
            }
            if (Health == 0)
            {
                return null;
            }
            if (_activeBullets <= FiringRate - 1)
            {
                Bullet bullet = new Bullet(Point, Direction, Id, Damage);
                bullet.ObjectDestroyed += UpdateActiveBulletsCounter;
                _activeBullets++;
                _lastFireTime = DateTime.Now;
                return bullet;
            } else
            {
                return null;
            }
        }

        /// <summary>
        /// Обновить счетчик выпущенных пуль, которые еще летят
        /// </summary>
        public void UpdateActiveBulletsCounter() {
            if (_activeBullets > 0)
            {
                _activeBullets -= 1;
            }
        }
    }
}
