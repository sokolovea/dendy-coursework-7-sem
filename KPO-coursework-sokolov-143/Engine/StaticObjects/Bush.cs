namespace Engine.StaticObjects
{
    /// <summary>
    /// Блок кустов
    /// </summary>
    public class Bush : AbstractStaticObject
    {
        /// <summary>
        /// Создает блок кустов (проходим танком и пулей, неразрушим)
        /// </summary>
        public Bush() : base(true, true, false)
        {
        }
    }
}
