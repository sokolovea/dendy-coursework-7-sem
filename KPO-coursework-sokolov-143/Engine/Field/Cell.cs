using Engine.DynamicObjects;
using Engine.StaticObjects;

namespace Engine.Field
{
    /// <summary>
    /// Ячейка игрового поля
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Статический объект, связанный с ячейкой
        /// </summary>
        AbstractStaticObject? _staticObject;

        /// <summary>
        /// Динамический объект, связанный с ячейкой
        /// </summary>
        AbstractDynamicObject? _dynamicObject;

        /// <summary>
        /// Статический объект, связанный с ячейкой
        /// </summary>
        public AbstractStaticObject? StaticObject
        {
            get
            {
                return _staticObject;
            }
            set
            {
                _staticObject = value;
            }
        }

        /// <summary>
        /// Динамический объект, связанный с ячейкой
        /// </summary>
        public AbstractDynamicObject? DynamicObject
        {
            get
            {
                return _dynamicObject;
            }
            set
            {
                _dynamicObject = value;
            }
        }


        /// <summary>
        /// Конструктор ячейки
        /// </summary>
        public Cell()
        {
            _staticObject = null;
        }

        /// <summary>
        /// Проходима ли клетка танком
        /// </summary>
        /// <returns>Истина, если клетка проходима танком</returns>
        public bool IsPassableByTank()
        {
            return (_staticObject == null || _staticObject.IsPassableByTank);
        }

        /// <summary>
        /// Проходима ли клетка пулей
        /// </summary>
        /// <returns>Истина, если через клетку может пролететь пуля</returns>
        public bool IsPassableByBullet()
        {
            return _staticObject == null || _staticObject.IsPassableByBullet;
        }

        /// <summary>
        /// Разрушима ли клетка пулей
        /// </summary>
        /// <returns>Истина, если блок на клетке разрушим пулей</returns>
        public bool IsDestructible()
        {
            return _staticObject != null && _staticObject.IsDestructible;
        }
    }
}
