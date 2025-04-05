namespace BaseController
{
    /// <summary>
    /// Базовый класс для всех контроллеров
    /// </summary>
    public abstract class ControllerBase
    {
        /// <summary>
        /// Запуск контроллера
        /// </summary>
        public abstract void Start();
        /// <summary>
        /// Остановка контроллера при передаче управления другому контроллеру
        /// </summary>
        public abstract void Stop();
    }
}
