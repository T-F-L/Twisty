using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace Twitsy
{
    class TaskTrayApplicationContext : ApplicationContext
    {
        NotifyIcon notifyIcon = new NotifyIcon();
        Configuration configWindow = new Configuration();
        Boolean Continue = true;
        Keyboard keyboard = new Keyboard();
        Thread thread1 = new Thread(Wekker);
        //Twitsy.Keyboard.ScanCodeShort key = Twitsy.Keyboard.ScanCodeShort.F15;

        public TaskTrayApplicationContext()
        {
            MenuItem configMenuItem = new MenuItem("Configuration", new EventHandler(ShowConfig));
            MenuItem exitMenuItem = new MenuItem("Exit", new EventHandler(Exit));

            notifyIcon.Icon = Properties.Resources.tammy_ico;
            notifyIcon.DoubleClick += new EventHandler(ShowConfig);
            notifyIcon.ContextMenu = new ContextMenu(new MenuItem[] { configMenuItem, exitMenuItem });
            notifyIcon.Visible = true;

            
            thread1.Start(this);
        }

      
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);


        public static void Wekker(object param )
        {
            TaskTrayApplicationContext parrent = (TaskTrayApplicationContext)param;
            
                       
            while (((TaskTrayApplicationContext)parrent).Continue)
            {
                //SetCursorPos(500, 500);
                parrent.keyboard.Send(Twitsy.Keyboard.ScanCodeShort.F15);
                Thread.Sleep(3 * 1000);
                //SetCursorPos(600, 600);
                //parrent.keyboard.Send(Twitsy.Keyboard.ScanCodeShort.F15);
                //Thread.Sleep(3 * 1000);
            }
        }

        void ShowConfig(object sender, EventArgs e)
        {
            // If we are already showing the window meerly focus it.
            if (configWindow.Visible)
                configWindow.Focus();
            else
                configWindow.ShowDialog();
        }

        void Exit(object sender, EventArgs e)
        {
            // We must manually tidy up and remove the icon before we exit.
            // Otherwise it will be left behind until the user mouses over.
            notifyIcon.Visible = false;
            Continue = false;
            thread1.Abort();

            Application.Exit();
        }
    }
}
