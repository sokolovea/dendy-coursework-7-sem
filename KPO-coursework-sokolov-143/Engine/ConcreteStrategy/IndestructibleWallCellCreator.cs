using Engine.Field;
using Engine.StaticObjects;

namespace Engine.ConcreteStrategy
{
    /// <summary>
    /// Стратегия для создания ячейки с нерушимой стеной
    /// </summary>
    public class IndestructibleWallCellCreator : ICellCreatorFabric
    {
        /// <summary>
        /// Создает ячейку с нерушимой стеной
        /// </summary>
        /// <returns>Ячейка с объектом типа IndestructibleWall</returns>
        public Cell CreateCell()
        {
            return new Cell { StaticObject = new IndestructibleWall() };
        }
    }

}
