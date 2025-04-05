namespace Engine.StaticObjects
{
    /// <summary>
    /// Блок обычной разрушимой стены
    /// </summary>
    public class Wall : AbstractStaticObject
    {
        /// <summary>
        /// Создает блок разрушимой стены (непроходима танком и пулей, но разрушима)
        /// </summary>
        public Wall() : base(false, false, true)
        {
        }
    }
}
