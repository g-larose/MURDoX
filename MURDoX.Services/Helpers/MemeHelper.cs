using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Helpers
{
    public class MemeHelper
    {
        #region GET MEME URL
        public static string GetMemeUrl(string query)
        {
            var rnd = new Random();
            string url = $"https://imgflip.com/memesearch?q={query}&nsfw=on";
           //string url = $"https://www.reddit.com/r/{query}memes/";
            var memeUrl = "";
            var fullUrl = "";
            HtmlWeb page = new();
            HtmlDocument doc = page.Load(url);

            HtmlNodeCollection imageLinks = doc.DocumentNode.SelectNodes("//a");
           
            //HtmlNodeCollection urls = doc.DocumentNode.SelectNodes("//a/div/div/img[contains(@class, 'ImageBox-image')]");
           
            int index = 0;
            //HtmlNode selectedNode = null;
            if (imageLinks != null)
            {
                index = rnd.Next(imageLinks.Count);
                var imageLink = imageLinks[index].Attributes["href"].Value;

                doc = page.Load($"https://imgflip.com{imageLink}");
                HtmlNodeCollection urls = doc.DocumentNode.SelectNodes("//a/img[contains(@class, 'base-img')]");

                if (urls is not null)
                {
                    index = rnd.Next(urls.Count);
                    HtmlNode selectedNode = urls[index];
                   
                    if (selectedNode is not null)
                    {
                        memeUrl = selectedNode.Attributes["src"].Value;
                    }
                }
                fullUrl = $"https:{memeUrl}";
            }
           
            return fullUrl;
        }
        #endregion
    }
}
