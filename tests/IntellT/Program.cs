namespace IntellT {
    internal static class Program {
        ///<summary>The main entry point for the application.</summary>
        [STAThread]
        static void Main() {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            var stream = new MemoryStream(500);
            stream.Write(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 0, 10);
            stream.Flush();

            var bytes = new byte[10];
            stream.Read(bytes, 0, 2);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.Run(new EntryForm());
        }
    }
}
