using Point = Engine.Field.Point;

namespace Engine.StaticObjects
{
    /// <summary>
    /// Абстрактное препятствие (статический объект, т.е. блок)
    /// </summary>
    public class AbstractStaticObject
    {
        /// <summary>
        /// Проходимо ли препятствие танком
        /// </summary>
        private bool _isPassableByTank = true;

        /// <summary>
        /// Проходимо ли препятствие пулей
        /// </summary>
        private bool _isPassableByBullet = true;

        /// <summary>
        /// Разрушимо ли препятствие
        /// </summary>
        private bool _isDestructible = false;

        /// <summary>
        /// Координата препятствия в клетках
        /// </summary>
        private Point _coordinateInCells = new(0,0);

        /// <summary>
        /// Проходимо ли препятствие танком
        /// </summary>
        public bool IsPassableByTank
        {
            get
            {
                return _isPassableByTank;
            }
            protected set
            {
                _isPassableByTank = value;
            }
        }

        /// <summary>
        /// Проходимо ли препятствие пулей
        /// </summary>
        public bool IsPassableByBullet
        {
            get
            {
                return _isPassableByBullet;
            }
            protected set
            {
                _isPassableByBullet = value;
            }
        }

        /// <summary>
        /// Разрушимо ли препятствие
        /// </summary>
        public bool IsDestructible
        {
            get
            {
                return _isDestructible;
            }
            protected set
            {
                _isDestructible = value;
            }
        }

        /// <summary>
        /// Координата препятствия в клетках
        /// </summary>
        public Point Point
        {
            get
            {
                return _coordinateInCells;
            }
            set
            {
                _coordinateInCells = value;
            }
        }

        /// <summary>
        /// Создает объект статического объекта
        /// </summary>
        /// <param name="parIsPassableByTank">Проходим ли объект танком</param>
        /// <param name="parIsPassableByBullet">Проходим ли объект пулей</param>
        /// <param name="parIsDestructible">Разрушим ли объект</param>
        public AbstractStaticObject(bool parIsPassableByTank, bool parIsPassableByBullet, bool parIsDestructible)
        {
            IsPassableByTank = parIsPassableByTank;
            IsPassableByBullet = parIsPassableByBullet;
            IsDestructible = parIsDestructible;
        }
    }
}
