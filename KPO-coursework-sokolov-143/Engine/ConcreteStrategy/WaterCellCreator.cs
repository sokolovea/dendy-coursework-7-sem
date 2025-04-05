using Engine.Field;
using Engine.StaticObjects;

namespace Engine.ConcreteStrategy
{
    /// <summary>
    /// Стратегия для создания ячейки с водой
    /// </summary>
    public class WaterCellCreator : ICellCreatorFabric
    {
        /// <summary>
        /// Создает ячейку с водой
        /// </summary>
        /// <returns>Ячейка с объектом типа Water</returns>
        public Cell CreateCell()
        {
            return new Cell { StaticObject = new Water() };
        }
    }

}
