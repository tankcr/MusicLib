using System;
using Gtk;
using System.Runtime.InteropServices;

namespace MediaLib
{
public partial class MainWindow : Gtk.Window
    {

    public MainWindow()
            : base(Gtk.WindowType.Toplevel)
        {
            Build();
        }
        protected void OnDeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
            a.RetVal = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
			GetDevices.GetDeviceData ();
        }
			
    }
}