using System;
using System.Windows;

namespace GC
{
    public partial class ImageChoiceDialog : Window
    {
        public bool IsLien { get; private set; }  // Si vrai, l'utilisateur a choisi "Lien"
        public bool IsImage { get; private set; } // Si vrai, l'utilisateur a choisi "Image sur le PC"

        public ImageChoiceDialog()
        {
            InitializeComponent();
        }

        // Lorsque l'utilisateur clique sur "Lien"
        private void BtnLien_Click(object sender, RoutedEventArgs e)
        {
            IsLien = true;
            IsImage = false;
            this.DialogResult = true; // Ferme la fenêtre et indique que l'utilisateur a fait un choix
        }

        // Lorsque l'utilisateur clique sur "Image sur le PC"
        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            IsLien = false;
            IsImage = true;
            this.DialogResult = true; // Ferme la fenêtre et indique que l'utilisateur a fait un choix
        }
    }
}
