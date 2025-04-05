using Engine.Field;

namespace Engine
{
    /// <summary>
    /// Абстрактная фабрика для созлания различных блоков игрового поля
    /// </summary>
    public interface ICellCreatorFabric
    {
        /// <summary>
        /// Создает блок игрового поля
        /// </summary>
        /// <returns>Блок игрового поля</returns>
        Cell CreateCell();
    }

}