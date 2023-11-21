namespace PQCDEMO
{
    internal static class Program
    {
        static Mutex mutex = new Mutex(true, "PQC");

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("應用程式已經在運行中，無法再次啟動。");
            }
        }
    }
}