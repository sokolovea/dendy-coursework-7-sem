using Engine.Field;
using Engine.StaticObjects;

namespace Engine.ConcreteStrategy
{
    /// <summary>
    /// Стратегия для создания ячейки с кирпичной стеной
    /// </summary>
    public class WallCellCreator : ICellCreatorFabric
    {
        /// <summary>
        /// Создает ячейку с кирпичной стеной
        /// </summary>
        /// <returns>Ячейка с объектом типа Wall</returns>
        public Cell CreateCell()
        {
            return new Cell { StaticObject = new Wall() };
        }
    }

}
