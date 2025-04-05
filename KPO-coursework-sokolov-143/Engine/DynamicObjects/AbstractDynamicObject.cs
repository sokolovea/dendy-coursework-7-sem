using Engine.Field;

namespace Engine.DynamicObjects
{
    /// <summary>
    /// Абстрактный динамический (движущийся) объект
    /// </summary>
    public abstract class AbstractDynamicObject
    {
        /// <summary>
        /// Уникальный id динамического объекта
        /// </summary>
        private readonly Guid _id;

        /// <summary>
        /// Направление движения
        /// </summary>
        private DirectionEnum _direction = DirectionEnum.Down;

        /// <summary>
        /// Движется ли объект
        /// </summary>
        private bool _isMoving = false;

        /// <summary>
        /// Скорость движения
        /// </summary>
        private int _speed = 0;

        /// <summary>
        /// Ширина объекта в пикселях
        /// </summary>
        private int _width = 0;

        /// <summary>
        /// Высота объекта в пискселях
        /// </summary>
        private int _height = 0;

        /// <summary>
        /// Текущие координаты объекта
        /// </summary>
        private Point _point = new();

        /// <summary>
        /// Делегат для события уничтожения
        /// </summary>
        public delegate void dVoid();

        /// <summary>
        /// Событие уничтожения
        /// </summary>
        public event dVoid ObjectDestroyed;

        /// <summary>
        /// Уникальный id динамического объекта
        /// </summary>
        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Направление движения
        /// </summary>
        public DirectionEnum Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
            }
        }

        /// <summary>
        /// Движется ли объект
        /// </summary>
        public bool IsMoving
        {
            get
            {
                return _isMoving;
            }
            set
            {
                _isMoving = value;
            }
        }

        /// <summary>
        /// Скорость движения
        /// </summary>
        public int Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }

        /// <summary>
        /// Текущие координаты объекта (пиксели)
        /// </summary>
        public Point Point
        {
            get
            {
                return _point;
            }
            set
            {
                _point = value;
            }
        }

        /// <summary>
        /// Ширина объекта
        /// </summary>
        public virtual int Width
        {
            get
            {
                return _width;
            }
            protected set
            {
                _width = value;
            }
        }

        /// <summary>
        /// Высота объекта
        /// </summary>
        public virtual int Height
        {
            get
            {
                return _height;
            }
            protected set
            {
                _height = value;
            }
        }

        /// <summary>
        /// Метод для вызова события уничтожения объекта
        /// </summary>
        public virtual void OnObjectDestroyed()
        {
            ObjectDestroyed?.Invoke();
        }

        /// <summary>
        /// Конструктор по умолчанию для динамического объекта
        /// </summary>
        public AbstractDynamicObject()
        {
        }

        /// <summary>
        /// Конструктор для динамического объекта с заданными параметрами
        /// </summary>
        /// <param name="parId">Id объекта</param>
        /// <param name="parSpeed">Скорость объекта</param>
        /// <param name="parPoint">Начальные координаты объекта</param>
        /// <param name="parDirection">Начальное направление объекта/param>
        /// <param name="parIsMoving">Движется ли объект</param>
        /// <param name="parWidth">Ширина объекта/param>
        /// <param name="parHeight">Высота объекта/param>
        public AbstractDynamicObject(Guid parId, int parSpeed, Point parPoint, DirectionEnum parDirection, bool parIsMoving, int parWidth, int parHeight)
        {
            _id = parId;
            Speed = parSpeed;
            Point = parPoint;
            Direction = parDirection;
            IsMoving = parIsMoving;
            Width = parWidth;
            Height = parHeight;
        }

        /// <summary>
        /// Вычислить новые координаты для объекта для следующего тика
        /// </summary>
        /// <param name="parMaxX">Максимальная допустимое значение абсциссы</param>
        /// <param name="parMaxY">Максимальное допустимое значение ординаты</param>
        /// <param name="parDeltaTime">Время игрового тика в секундах</param>
        /// <returns>Координаты новой позиции</returns>
        public Point CalculateNewPosition(uint parMaxX, uint parMaxY, float parDeltaTime)
        {
            if (!IsMoving)
            {
                return Point;
            }

            int coeffX = 0;
            int coeffY = 0;
            if (Direction == DirectionEnum.Up)
            {
                coeffY = -1;
            }
            else if (Direction == DirectionEnum.Down)
            {
                coeffY = 1;
            }
            else if (Direction == DirectionEnum.Left)
            {
                coeffX = -1;
            }
            else if (Direction == DirectionEnum.Right)
            {
                coeffX = 1;
            }

            float deltaX = coeffX * Speed * parDeltaTime;
            float deltaY = coeffY * Speed * parDeltaTime;

            float newX = Point.X + deltaX;
            float newY = Point.Y + deltaY;

            if ((int)newX - Width / 2 < 0)
            {
                newX = Width / 2;
            }

            if ((int)newY - Height / 2 < 0)
            {
                newY = Height / 2;
            }

            if (newX + Width / 2 > parMaxX)
            {
                newX = parMaxX - Width / 2;
            }

            if (newY + Height / 2 > parMaxY)
            {
                newY = parMaxY - Width / 2;
            }

            return new(newX, newY);

        }

        /// <summary>
        /// Пересекается ли объект с другим на плоскости
        /// </summary>
        /// <param name="parOtherObject">Другой объект для проверки пересечения</param>
        /// <returns>Истина, если объекты пересекаются, иначе ложь</returns>
        public bool IsOverlayed(AbstractDynamicObject parOtherObject)
        {
            if (this == parOtherObject)
                return false;

            bool isIntersectingX =
                (Point.X > parOtherObject.Point.X
                    ? Point.X - parOtherObject.Point.X
                    : parOtherObject.Point.X - Point.X)
                < (Width + parOtherObject.Width) / 2;

            bool isIntersectingY =
                (Point.Y > parOtherObject.Point.Y
                    ? Point.Y - parOtherObject.Point.Y
                    : parOtherObject.Point.Y - Point.Y)
                < (Height + parOtherObject.Height) / 2;

            return isIntersectingX && isIntersectingY;
        }


    }
}
