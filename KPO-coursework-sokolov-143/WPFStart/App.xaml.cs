﻿using System.Windows;
using WPFController;

namespace WPFStart
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            new ControllerMenuMainWPF().Start();
        }
    }

}
