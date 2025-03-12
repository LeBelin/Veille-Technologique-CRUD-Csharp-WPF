using System;
using System.Windows.Input;  // Pour le double-clic
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using Microsoft.VisualBasic;  // Pour InputBox
using Microsoft.Win32; // Pour OpenFileDialog
using System.IO;
using System.Net; // Pour FTP

namespace GC
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            BtnChargerArticles_Click(null, null);  // Charger les articles au démarrage

        }

        // Empêcher le double-clic sur les lignes de la DataGrid
        private void DataGrid_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true; // Empêche l'action par défaut de double-clic
        }



        // Méthode pour effectuer la recherche d'un article par titre
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = searchTextBox.Text.Trim(); // Récupérer le texte de la barre de recherche

            Bdd database = new Bdd();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                // Recherche les articles par titre
                List<Article> articles = database.SearchArticlesByTitle(searchQuery);
                dataGridArticles.ItemsSource = articles; // Mettre à jour la DataGrid avec les articles filtrés
            }
            else
            {
                // Si la barre de recherche est vide, charger tous les articles
                BtnChargerArticles_Click(null, null); // Appelle la méthode qui charge tous les articles
            }
        }

        // Méthode pour charger les articles dans la DataGrid
        private void BtnChargerArticles_Click(object sender, RoutedEventArgs e)
        {
            Bdd database = new Bdd();
            List<Article> articles = database.GetArticles(); // Charge tous les articles
            dataGridArticles.ItemsSource = articles; // Mettre à jour la DataGrid
        }





        // Méthode pour ajouter un article
        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible; // Afficher le ProgressBar

            string titleArticle = Interaction.InputBox("Entrez le titre de l'article :", "Ajouter un article");
            string urlArticle = Interaction.InputBox("Entrez l'URL de l'article :", "Ajouter un article");

            // Ouvre la fenêtre pour sélectionner une date
            DateSelectionWindow dateSelectionWindow = new DateSelectionWindow();
            if (dateSelectionWindow.ShowDialog() == true)  // Si la date est validée
            {
                DateTime dateArticle = dateSelectionWindow.SelectedDate.Value;

                // Ouvre la fenêtre pour choisir entre Lien et Image sur le PC
                ImageChoiceDialog imageChoiceDialog = new ImageChoiceDialog();
                if (imageChoiceDialog.ShowDialog() == true)
                {
                    string imageUrl = "";

                    if (imageChoiceDialog.IsLien)  // Si l'utilisateur choisit "Lien"
                    {
                        imageUrl = Interaction.InputBox("Entrez l'URL de l'image :", "Lien d'image");
                    }
                    else if (imageChoiceDialog.IsImage)  // Si l'utilisateur choisit "Image sur le PC"
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog
                        {
                            Filter = "Images (*.png;*.jpg;*.jpeg;*.gif)|*.png;*.jpg;*.jpeg;*.gif",
                            Title = "Sélectionner une image"
                        };

                        if (openFileDialog.ShowDialog() == true)
                        {
                            imageUrl = UploadImageToServer(openFileDialog.FileName);
                        }
                    }

                    if (string.IsNullOrEmpty(titleArticle) || string.IsNullOrEmpty(urlArticle) || string.IsNullOrEmpty(imageUrl))
                    {
                        MessageBox.Show("Veuillez remplir tous les champs.");
                        progressBar.Visibility = Visibility.Hidden; // Masquer le ProgressBar
                        return;
                    }

                    Bdd database = new Bdd();
                    database.AddArticle(titleArticle, urlArticle, imageUrl, dateArticle);

                    BtnChargerArticles_Click(null, null); // Recharge la liste des articles

                    progressBar.Visibility = Visibility.Hidden; // Masquer le ProgressBar

                    // Message de confirmation
                    MessageBox.Show("L'article a été ajouté avec succès.", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }


        // Méthode pour modifier un article
        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible; // Afficher le ProgressBar

            if (dataGridArticles.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un article à modifier.");
                progressBar.Visibility = Visibility.Hidden; // Masquer le ProgressBar
                return;
            }

            Article selectedArticle = (Article)dataGridArticles.SelectedItem;
            int articleId = selectedArticle.Id;

            string nouveauTitle = Interaction.InputBox("Entrez le nouveau titre de l'article :", "Modifier l'article", selectedArticle.Title);
            string nouveauUrl = Interaction.InputBox("Entrez la nouvelle URL de l'article :", "Modifier l'article", selectedArticle.Url);

            // Demander si l'utilisateur veut modifier la date
            MessageBoxResult result = MessageBox.Show("Voulez-vous modifier la date de l'article ?", "Modification de la date", MessageBoxButton.YesNo, MessageBoxImage.Question);

            DateTime nouvelleDate = selectedArticle.DateArticle; // Par défaut, conserver l'ancienne date

            if (result == MessageBoxResult.Yes)
            {
                // Ouvrir la fenêtre de sélection de date
                DateSelectionWindow dateSelectionWindow = new DateSelectionWindow();
                if (dateSelectionWindow.ShowDialog() == true) // Si l'utilisateur valide la nouvelle date
                {
                    nouvelleDate = dateSelectionWindow.SelectedDate.Value;
                }
            }

            string nouvelleImage = selectedArticle.Image;
            var changeImage = MessageBox.Show("Voulez-vous changer l'image ?", "Modification d'image", MessageBoxButton.YesNo);

            if (changeImage == MessageBoxResult.Yes)
            {
                // Ouvre la fenêtre pour choisir entre Lien et Image sur le PC
                ImageChoiceDialog imageChoiceDialog = new ImageChoiceDialog();
                if (imageChoiceDialog.ShowDialog() == true)
                {
                    if (imageChoiceDialog.IsLien)  // Si l'utilisateur choisit "Lien"
                    {
                        nouvelleImage = Interaction.InputBox("Entrez l'URL de l'image :", "Lien d'image");
                    }
                    else if (imageChoiceDialog.IsImage)  // Si l'utilisateur choisit "Image sur le PC"
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog
                        {
                            Filter = "Images (*.png;*.jpg;*.jpeg;*.gif)|*.png;*.jpg;*.jpeg;*.gif",
                            Title = "Sélectionner une nouvelle image"
                        };

                        if (openFileDialog.ShowDialog() == true)
                        {
                            nouvelleImage = UploadImageToServer(openFileDialog.FileName);
                        }
                    }
                }
            }

            Bdd database = new Bdd();
            database.UpdateArticle(articleId, nouveauTitle, nouveauUrl, nouvelleImage, nouvelleDate);

            BtnChargerArticles_Click(null, null); // Recharge la liste des articles

            progressBar.Visibility = Visibility.Hidden; // Masquer le ProgressBar

            // Message de confirmation
            MessageBox.Show("L'article a été modifié avec succès.", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        // Méthode pour supprimer un article
        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible; // Afficher le ProgressBar

            if (dataGridArticles.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un article à supprimer.");
                progressBar.Visibility = Visibility.Hidden; // Masquer le ProgressBar
                return;
            }

            Article selectedArticle = (Article)dataGridArticles.SelectedItem;
            int articleId = selectedArticle.Id;

            var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cet article ?", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                progressBar.Visibility = Visibility.Hidden; // Masquer le ProgressBar
                return;
            }

            Bdd database = new Bdd();
            database.DeleteArticle(articleId);

            BtnChargerArticles_Click(null, null); // Recharge la liste des articles

            progressBar.Visibility = Visibility.Hidden; // Masquer le ProgressBar

            // Message de confirmation
            MessageBox.Show("L'article a été supprimé avec succès.", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        // Méthodes d'upload d'image
        private string UploadImageToServer(string localFilePath)
        {
            string ftpServer = "ftp://votre_serveur/";
            string httpServer = "https://votre_site/votre_url/";
            string ftpUser = "votre_id";
            string ftpPassword = "votre_mdp";

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(localFilePath);
            string ftpFullPath = ftpServer + fileName;

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                    client.UploadFile(ftpFullPath, WebRequestMethods.Ftp.UploadFile, localFilePath);
                }
                return httpServer + fileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'upload : " + ex.Message);
                return null;
            }
        }
    }
}
