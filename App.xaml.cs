using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GC
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Applique la culture française pour l'ensemble de l'application
            CultureInfo frenchCulture = new CultureInfo("fr-FR");
            Thread.CurrentThread.CurrentCulture = frenchCulture;
            Thread.CurrentThread.CurrentUICulture = frenchCulture;

            base.OnStartup(e);
        }
    }
}
