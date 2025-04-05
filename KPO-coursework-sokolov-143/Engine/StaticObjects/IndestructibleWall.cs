namespace Engine.StaticObjects
{
    /// <summary>
    /// Блок нерушимой стены
    /// </summary>
    public class IndestructibleWall : AbstractStaticObject
    {
        /// <summary>
        /// Создает блок нерушимой стены (непроходим ни танком, ни пулей, неразрушим)
        /// </summary>
        public IndestructibleWall() : base(false, false, false)
        {
        }
    }
}
