using System;
using System.Windows.Forms;

namespace TestBed.WinForms
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var myGame = new MyGame())
            {
                //mainForm.Game = myGame;
                //mainForm.Show();

                myGame.Run();
            }

            //using (var mainForm = new MainForm())
            //{
            //    using (var myGame = new MyGame(mainForm))
            //    {
            //        mainForm.Game = myGame;
            //        mainForm.Show();
            //
            //        myGame.Run(mainForm.RenderTarget);
            //    }
            //}
        }
    }
}
