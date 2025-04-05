using Engine.Field;

namespace Engine.DynamicObjects.Tank
{
    /// <summary>
    /// Бронированный танк
    /// </summary>
    public class ArmoredTank : AbstractTank
    {
        /// <summary>
        /// Конструктор бронированного танка
        /// </summary>
        /// <param name="parStartPoint">Стартовая точка появления танка</param>
        public ArmoredTank(Point parStartPoint) : base(parStartPoint)
        {
            Speed = (int)(Speed * 0.75);
            Damage = (int)(Damage * 1.5);
            Health *= 2;
        }
    }
}
