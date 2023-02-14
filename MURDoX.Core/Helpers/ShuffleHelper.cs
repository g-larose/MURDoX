// using MURDoX.Core.Models;
//
// namespace MURDoX.Core.Helpers
// {
//     public class ShuffleHelper
//     {
//         public static string ShuffleCategory(string[] cats)
//         {
//             Random rnd = new();
//             int index = rnd.Next(0, cats.Length);
//             string result = cats[index];
//
//             switch (result)
//             {
//                 case "nature":
//                     return "17";
//                 case "computers":
//                     return "18";
//                 case "mathmatics":
//                     return "19";
//                 case "gadgets":
//                     return "30";
//                 case "comics":
//                     return "29";
//                 case "anime":
//                     return "31";
//                 case "cartoons":
//                     return "32";
//                 default:
//                     return "17";
//             }
//         }
//
//         public static async Task<string> GetRandomEmbedColorAsync()
//         {
//             Random random = new();
//             Dictionary<string, DiscordColor> colors = EmbedColors.Colors;
//
//             int index = random.Next(0, colors.Count);
//             string color = colors.ElementAt(index).Key;
//
//             return color;
//            
//         }
//     }
// }