using Engine.Field;

namespace Engine.ConcreteStrategy
{
    /// <summary>
    /// Стратегия для создания пустой ячейки
    /// </summary>
    public class EmptyCellCreator : ICellCreatorFabric
    {
        /// <summary>
        /// Создает пустую ячейку игрового поля
        /// </summary>
        /// <returns>Пустая ячейка</returns>
        public Cell CreateCell()
        {
            return new Cell();
        }
    }

}
