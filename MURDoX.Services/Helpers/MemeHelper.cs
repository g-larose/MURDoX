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
            string url = $"https://imgur.com/search?q={query} memes";
            var memeUrl = "";
            HtmlWeb page = new();
            HtmlDocument doc = page.Load(url);

            HtmlNodeCollection urls = doc.DocumentNode.SelectNodes("//a[contains(@class, 'image-list-link')]/img");

            if (urls is not null)
            {
                int index = rnd.Next(urls.Count);
                memeUrl = $"https:{urls[index].Attributes["src"].Value}";
            }
            return memeUrl;
        }
        #endregion
    }
}
