using Engine.Field;

namespace Engine.DynamicObjects.Tank
{
    /// <summary>
    /// Скоростной танк
    /// </summary>
    public class HighSpeedTank : AbstractTank
    {
        /// <summary>
        /// Конструктор скоростного танка с заданными начальными координатами
        /// </summary>
        /// <param name="parStartPoint">Начальные координаты</param>
        public HighSpeedTank(Point parStartPoint) : base(parStartPoint)
        {
            Speed = (int)(Speed * 1.25);
        }
    }
}
