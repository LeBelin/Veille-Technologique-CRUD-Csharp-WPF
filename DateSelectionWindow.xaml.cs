using System;
using System.Windows;

namespace GC
{
    public partial class DateSelectionWindow : Window
    {
        public DateTime? SelectedDate { get; private set; }

        public DateSelectionWindow()
        {
            InitializeComponent();
        }

        // Méthode pour valider la date sélectionnée
        private void ValiderButton_Click(object sender, RoutedEventArgs e)
        {
            if (datePicker.SelectedDate.HasValue)
            {
                SelectedDate = datePicker.SelectedDate.Value;
                DialogResult = true;  // Indique que l'utilisateur a validé
                Close();  // Ferme la fenêtre
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une date.");
            }
        }

        // Méthode pour annuler et fermer la fenêtre sans sélectionner de date
        private void AnnulerButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;  // Indique que l'utilisateur a annulé
            Close();
        }
    }
}
