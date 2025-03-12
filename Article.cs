using System;
using System.Windows.Media.Imaging;

public class Article
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public string Image { get; set; }
    public DateTime DateArticle { get; set; }  // Ajout de la date de l'article
    public DateTime CreatedAt { get; set; }

    public BitmapImage ImageSource
    {
        get
        {
            if (string.IsNullOrEmpty(Image))
                return null;

            try
            {
                return new BitmapImage(new Uri(Image, UriKind.Absolute));
            }
            catch
            {
                return null;
            }
        }
    }
}
