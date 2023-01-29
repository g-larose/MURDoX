using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MURDoX.Services.Models;

namespace MURDoX.Services.Helpers
{
    public class TagHelper
    {

        #region REQUEST TAG
        public static DiscordEmbed RequestTag(DiscordClient client, string author, string tag = "")
        {
            var messageAuthor = author;
            var bot = client.CurrentUser;

            var embedBuilder = new EmbedBuilderHelper();
            if (tag == "" || tag == null)
            {
                
                var embed = new Embed()
                {
                    Title = "No tage Found",
                    Author = $"{bot.Username}",
                    Desc = $"No Tag Found.",
                    Footer = $"{bot.Username} ©️ {DateTime.Now.ToLongDateString()}",
                    AuthorAvatar = bot.GetAvatarUrl(ImageFormat.Jpeg),
                    ImgUrl = null,
                    ThumbnailImgUrl = null,
                    FooterImgUrl = bot.GetAvatarUrl(ImageFormat.Jpeg),
                    Color = "blurple",
                };
                                      
                return embedBuilder.Build(embed);
            }
            tag = tag.ToLower();
            
            switch (tag)
            {
                case "rulesofwpf":
                    //TODO: creat rulesofwpf tag
                break;
                case "helloworld":
                    var embed = new Embed()
                    {
                        Title = "$helloworld",
                        Author = $"{bot.Username}",
                        Desc = $"hello world is a series of beginner tutorials to help you learn C#",
                        Footer = $"{bot.Username} ©️ {DateTime.Now.ToLongDateString()}",
                        AuthorAvatar = bot.GetAvatarUrl(ImageFormat.Jpeg),
                        LinkUrl = "https://dotnet.microsoft.com/en-us/learn/dotnet/hello-world-tutorial/intro",
                        ThumbnailImgUrl = null,
                        FooterImgUrl = bot.GetAvatarUrl(ImageFormat.Jpeg),
                        Color = "blurple",
                    };
                    return embedBuilder.Build(embed);

                case "wpfuilibs":
                    embed = new Embed()
                    {
                        Title = "$wpfuilibs",
                        Author = $"{messageAuthor} ",
                        Desc = "``MahApps``\r\nhttps://github.com/MahApps/MahApps.Metro\r\n``ModernWPF``\r\nhttps://github.com/Kinnara/ModernWpf\r\n``Adonis UI``\r\nhttps://github.com/benruehl/adonis-ui\r\n" +
                        "``Material Design``\r\nhttps://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit\r\n``Biaui``\r\nhttps://github.com/YoshihiroIto/Biaui\r\n``HandyControls``" +
                        "\r\nhttps://github.com/HandyOrg/HandyControl\r\n``WPFUI(Windows 11 inspired)``\r\nhttps://github.com/lepoco/wpfui",
                        Footer = $"{bot.Username} ©️",
                        AuthorAvatar = bot.GetAvatarUrl(ImageFormat.Jpeg),
                        LinkUrl = "",
                        ThumbnailImgUrl = null,
                        TimeStamp = DateTimeOffset.Now,
                        FooterImgUrl = bot.GetAvatarUrl(ImageFormat.Jpeg),
                        Color = "blurple",
                    };
                    return embedBuilder.Build(embed);

                case "rules":
                    embed = new Embed()
                    {
                        Title = "$rules",
                        Author = $"{messageAuthor} ",
                        Desc = "``1.Be respectful``\r\nYou must respect all users, regardless of your liking towards them.Treat others the way you want to be treated.\r\n" +
                        "``2. No Inappropriate Language``\r\nThe use of profanity should be kept to a minimum. However, any derogatory language towards any user is prohibited.\r\n" +
                        "``3. No spamming``\r\nDon't send a lot of small messages right after each other. Do not disrupt chat by spamming.\r\n" +
                        "``4. No pornographic/adult/other NSFW material``\r\nThis is a community server and not meant to share this kind of material.\t\n" +
                        "``5. Don't post about projects that would be considered to be illegal and or related to hacking.``\r\nThis is a community server and not meant to share this kind of material.\r\n" +
                        "``6. No advertisements``\r\nWe do not tolerate any kind of advertisements, whether it be for other communities or streams. You can post your content in the media channel if it is relevant and provides actual value (Video/Art)\r\n" +
                        "``7. No offensive names and profile pictures``\r\nYou will be asked to change your name or picture if the staff deems them inappropriate.\r\n" +
                        "``8. No Direct or Indirect Threats``\r\nThreats to other users of DDoS, Death, DoX, abuse, and other malicious threats are absolutely prohibited and disallowed.\r\n" +
                        "``9. No Asking For Source Code``\r\nAsking for source code that is only available for Patreons will get you banned immediately.\r\n" +
                        "``10. Follow the Discord Community Guidelines``\r\nYou can find them here: https://discordapp.com/guidelines\r\n",
                        Footer = $"{bot.Username} ©️",
                        AuthorAvatar = bot.GetAvatarUrl(ImageFormat.Jpeg),
                        LinkUrl = "",
                        ThumbnailImgUrl =  null,
                        TimeStamp = DateTimeOffset.Now,
                        FooterImgUrl = bot.GetAvatarUrl(ImageFormat.Jpeg),
                        Color = "blurple",
                    };
                    return embedBuilder.Build(embed);
                case "learnc#":
                    embed = new Embed()
                    {
                        Title = "$c#resources",
                        Author = $"{messageAuthor} ",
                        Desc = "``Getting Started:``\r\n https://docs.microsoft.com/en-us/dotnet/csharp/getting-started\r\n" +
                        "``Introduction to C# (Interactive tutorial):``\r\n https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/tutorials/hello-world\r\n" +
                        "``C# Fundamentals:``\r\n https://channel9.msdn.com/Series/CSharp-Fundamentals-for-Absolute-Beginners\r\n" +
                        "``C# Documentation:``\r\n https://docs.microsoft.com/en-us/dotnet/csharp\r\n" +
                        "``.NET API Documentation:``\r\n https://docs.microsoft.com/en-us/dotnet/api\r\n" +
                        "``Debugging in VS:``\r\n https://docs.microsoft.com/en-us/visualstudio/debugger/using-breakpoints?view=vs-2022\r\n" +
                        "``How to use Breakpoints in VS:``\r\n https://docs.microsoft.com/en-us/visualstudio/debugger/using-breakpoints?view=vs-2022",
                        Footer = $"{bot.Username} ©️",
                        AuthorAvatar = bot.GetAvatarUrl(ImageFormat.Jpeg),
                        LinkUrl = "https://dotnet.microsoft.com/en-us/learn/csharp",
                        ThumbnailImgUrl =  null,
                        TimeStamp = DateTimeOffset.Now,
                        FooterImgUrl = bot.GetAvatarUrl(ImageFormat.Jpeg),
                        Color = "blurple",
                    };
                    return embedBuilder.Build(embed);
                case "itdepends":
                    embed = new Embed()
                    {
                        Title = "$itdepends",
                        Author = $"{messageAuthor}",
                        Footer = $"{bot.Username} ©️",
                        ImgUrl = "https://i.imgur.com/uPkTGJM.png",
                        TimeStamp = DateTimeOffset.Now,
                        FooterImgUrl = bot.GetAvatarUrl(ImageFormat.Jpeg),
                        Color = "green",
                    };
                    return embedBuilder.Build(embed);

                default:
                    
                    break;
            }


            return embedBuilder.Build(null);
        }
        #endregion

        //Getting Started: https://docs.microsoft.com/en-us/dotnet/csharp/getting-started
        //Introduction to C# (Interactive tutorial): https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/tutorials/hello-world
        //C# Fundamentals: https://channel9.msdn.com/Series/CSharp-Fundamentals-for-Absolute-Beginners
        //C# Documentation: https://docs.microsoft.com/en-us/dotnet/csharp
        //.NET API Documentation: https://docs.microsoft.com/en-us/dotnet/api
        //Debugging in VS: https://docs.microsoft.com/en-us/visualstudio/debugger/using-breakpoints?view=vs-2022
        //How to use Breakpoints in VS: https://docs.microsoft.com/en-us/visualstudio/debugger/using-breakpoints?view=vs-2022
    }
}
