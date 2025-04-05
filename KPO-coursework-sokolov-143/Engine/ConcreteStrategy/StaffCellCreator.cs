using Engine.Field;
using Engine.StaticObjects;

namespace Engine.ConcreteStrategy
{

    /// <summary>
    /// Стратегия для создания ячейки с объектом штаба
    /// </summary>
    public class StaffCellCreator : ICellCreatorFabric
    {
        /// <summary>
        /// Создает ячейку с объектом штаба
        /// </summary>
        /// <returns>Ячейка с объектом типа Staff</returns>
        public Cell CreateCell()
        {
            return new Cell { StaticObject = new Staff() };
        }
    }

}
