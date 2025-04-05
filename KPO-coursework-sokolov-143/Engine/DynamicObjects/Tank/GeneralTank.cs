using Engine.Field;

namespace Engine.DynamicObjects.Tank
{
    /// <summary>
    /// Обычный танк
    /// </summary>
    public class GeneralTank : AbstractTank
    {
        /// <summary>
        /// Конструктор обычного танка с заданными начальными координатами
        /// </summary>
        /// <param name="parStartPoint">Начальные координаты</param>
        public GeneralTank(Point parStartPoint) : base(parStartPoint)
        {
        }
    }
}
