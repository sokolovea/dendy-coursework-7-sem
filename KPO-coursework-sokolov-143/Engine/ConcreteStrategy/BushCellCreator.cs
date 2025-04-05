using Engine.Field;
using Engine.StaticObjects;

namespace Engine.ConcreteStrategy
{
    /// <summary>
    /// Стратегия для создания ячейки с кустами
    /// </summary>
    public class BushCellCreator : ICellCreatorFabric
    {
        /// <summary>
        /// Создает ячейку с кустами
        /// </summary>
        /// <returns>Ячейка с объектом типа Bush</returns>
        public Cell CreateCell()
        {
            return new Cell { StaticObject = new Bush() };
        }
    }

}
