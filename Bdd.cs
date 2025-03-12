using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

public class Bdd
{
    private string connectionString = "Server=votre_serveur;Database=votre_bdd;User ID=votre_identifiant;Password=votre_mdp;";

    // Méthode pour obtenir une connexion MySQL
    private MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }

    // Méthode pour récupérer tous les articles
    // Méthode pour rechercher des articles par titre
    // Méthode pour récupérer tous les articles
    // Méthode pour rechercher des articles par le titre
    // Méthode pour récupérer tous les articles
    public List<Article> GetArticles()
    {
        List<Article> articles = new List<Article>();

        string query = "SELECT id, title, url, image, date_article, created_at FROM articles ORDER BY created_at DESC LIMIT 20";

        using (MySqlConnection conn = GetConnection())
        {
            conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    articles.Add(new Article
                    {
                        Id = reader.GetInt32("id"),
                        Title = reader.GetString("title"),
                        Url = reader.GetString("url"),
                        Image = reader.GetString("image"),
                        DateArticle = reader.GetDateTime("date_article"),
                        CreatedAt = reader.GetDateTime("created_at")
                    });
                }
            }
        }

        return articles;
    }


    // Méthode pour rechercher des articles par titre
    public List<Article> SearchArticlesByTitle(string titleSearch)
    {
        List<Article> articles = new List<Article>();

        string query = "SELECT id, title, url, image, date_article, created_at FROM articles WHERE title LIKE @titleSearch ORDER BY created_at DESC";

        using (MySqlConnection conn = GetConnection())
        {
            conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                // Utilisation de LIKE pour rechercher le titre contenant la chaîne de recherche
                cmd.Parameters.AddWithValue("@titleSearch", "%" + titleSearch + "%");

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        articles.Add(new Article
                        {
                            Id = reader.GetInt32("id"),
                            Title = reader.GetString("title"),
                            Url = reader.GetString("url"),
                            Image = reader.GetString("image"),
                            DateArticle = reader.GetDateTime("date_article"),
                            CreatedAt = reader.GetDateTime("created_at")
                        });
                    }
                }
            }
        }

        return articles;
    }




    // Méthode pour ajouter un article avec date_article
    public void AddArticle(string title, string url, string image, DateTime dateArticle)
    {
        using (MySqlConnection conn = GetConnection())
        {
            conn.Open();
            string query = "INSERT INTO articles (title, url, image, date_article, created_at) VALUES (@title, @url, @image, @date_article, NOW())";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@url", url);
                cmd.Parameters.AddWithValue("@image", image);
                cmd.Parameters.AddWithValue("@date_article", dateArticle);
                cmd.ExecuteNonQuery();
            }
        }
    }

    // Méthode pour modifier un article avec date_article
    public void UpdateArticle(int id, string title, string url, string image, DateTime dateArticle)
    {
        using (MySqlConnection conn = GetConnection())
        {
            conn.Open();
            string query = "UPDATE articles SET title = @title, url = @url, image = @image, date_article = @date_article WHERE id = @id";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@url", url);
                cmd.Parameters.AddWithValue("@image", image);
                cmd.Parameters.AddWithValue("@date_article", dateArticle);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }

    // Méthode pour supprimer un article
    public void DeleteArticle(int id)
    {
        using (MySqlConnection conn = GetConnection())
        {
            conn.Open();
            string query = "DELETE FROM articles WHERE id = @id";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
