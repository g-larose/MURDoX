namespace MURDoX.Core.Helpers
{
    public class ConverterHelper
    {
        public static string ConvertCategoryString(string cat)
        {
            cat = cat.ToLower();
            string result = cat switch
            {
                "art" => "25",
                "general knowledge" => "9",
                "books" => "10",
                "film" => "11",
                "music" => "12",
                "theatre" => "13",
                "tv" => "14",
                "video games" => "15",
                "board games" => "16",
                "science" => ShuffleHelper.ShuffleCategory(new string[] { "nature", "computers", "mathmatics", "gadgets" }),
                "mythology" => "20",
                "sports" => "21",
                "geography" => "22",
                "history" => "23",
                "politics" => "24",
                "celebrities" => "26",
                "animals" => "27",
                "entertainment" => ShuffleHelper.ShuffleCategory(new string[] { "comics", "anime", "cartoons" }),
                _ => "9"
            };
            return result;
        }
    }
}