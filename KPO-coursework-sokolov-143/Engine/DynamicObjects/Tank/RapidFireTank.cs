using Engine.Field;

namespace Engine.DynamicObjects.Tank
{
    /// <summary>
    /// Скорострельный танк
    /// </summary>
    public class RapidFireTank : AbstractTank
    {
        /// <summary>
        /// Конструктор скоростного танка с заданными начальными координатами
        /// </summary>
        /// <param name="parStartPoint">Начальные координаты</param>
        public RapidFireTank(Point parStartPoint) : base(parStartPoint)
        {
            FiringRate = 2;
            FireCooldown = TimeSpan.FromMilliseconds(100);
        }
    }
}
