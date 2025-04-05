namespace Engine.StaticObjects
{

    /// <summary>
    /// Блок воды
    /// </summary>
    public class Water : AbstractStaticObject
    {
        /// <summary>
        /// Создает блок воды (непроходим танком, неразрушимый, но пропускает пулю)
        /// </summary>
        public Water() : base(false, true, false)
        {
        }
    }
}
