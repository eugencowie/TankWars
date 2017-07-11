using System;

namespace TankWars
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new TankWarsGame())
            {
                game.Run();
            }
        }
    }
#endif
}
