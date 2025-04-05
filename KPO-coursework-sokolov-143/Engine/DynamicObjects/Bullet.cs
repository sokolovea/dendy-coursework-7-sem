using Engine.Field;

namespace Engine.DynamicObjects
{
    /// <summary>
    /// Пуля, выпускаемая танком
    /// </summary>
    public class Bullet : AbstractDynamicObject
    {
        /// <summary>
        /// Базовая скорость пули
        /// </summary>
        private static readonly int BASE_SPEED_BULLET = 300;

        /// <summary>
        /// Базовая ширина пули
        /// </summary>
        private static readonly int BASE_WIDTH_BULLET = 8;

        /// <summary>
        /// Базовая высота пули
        /// </summary>
        private static readonly int BASE_HEIGHT_BULLET = 8;

        /// <summary>
        /// Guid танка, выпустившего пулю
        /// </summary>
        private Guid _parentTank;

        /// <summary>
        /// Урон, наносимый пулей
        /// </summary>
        private int _damage;

        /// <summary>
        /// Guid танка, выпустившего пулю
        /// </summary>
        public Guid ParentTankId
        {
            get
            {
                return _parentTank;
            }
            protected set
            {
                _parentTank = value;
            }
        }

        /// <summary>
        /// Урон, наносимый пулей
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
        /// Конструктор пули с начальными координатами
        /// </summary>
        /// <param name="parStartPoint">Точка появления пули</param>
        /// <param name="parDirection">Направление движения</param>
        /// <param name="parParentTank">Точка появления пули</param>
        /// <param name="parDamage">Наносимый урон</param>
        public Bullet(Point parStartPoint, DirectionEnum parDirection, Guid parParentTank, int parDamage)
            : base(Guid.NewGuid(), BASE_SPEED_BULLET, parStartPoint, parDirection, true, BASE_WIDTH_BULLET, BASE_HEIGHT_BULLET)
        {
            _parentTank = parParentTank;
            _damage = parDamage;
        }
    }
}
