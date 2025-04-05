using ConsoleController;

namespace ConsoleStart
{
    /// <summary>
    /// Класс для запуска приложения
    /// </summary>
    class Runner
    {
        /// <summary>
        /// Точка входа в приложение
        /// </summary>
        /// <param name="parArgs">Входные параметры приложения</param>
        static void Main(string[] parArgs)
        {
            new ControllerMenuMainConsole().Start();
        }
    }
}