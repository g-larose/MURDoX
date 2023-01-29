using MURDoX.Services.Interfaces;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MURDoX.Services.Services
{
    public static class GameService 
    {
        static  List<Game> games = new List<Game>();
        public static bool IsRunning { get; set; }
        public static Game StartNewGame(Game game)
        {
            var newGame = new Game();
            if (IsValidGame(game.GameName))
            {
                newGame = new Game 
                { 
                    GameName = game.GameName,
                    GameId= game.GameId,
                    Players= game.Players,
                    IsRunning= game.IsRunning,
                };
                games.Add(newGame);
            }

            return newGame;
        }

        public static bool IsValidGame(string gameName)
        {
            foreach (var game in games)
            {
                if (game.GameName == gameName) 
                {
                    return false;
                }
            }
            return true;
        }

        public static void ClearGames()
        {
            games.Clear();
        }

        public static void SavePlayerScoreToXml(string playerName, int wins, string game)
        {
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xml"));
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xml", "playerscores.xml");
            XDocument doc;
            if (!File.Exists(path))
            {
                doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                new XElement("dice_scores",
                                    new XElement("score",
                                        new XAttribute("game", game),
                                        new XAttribute("player_name", playerName),
                                        new XAttribute("wins", wins.ToString()))));
                doc.Save(path);
            }
            else 
            {
                doc = XDocument.Load(path);
                var playerNode = doc.Descendants("score")!.Attributes("player_name").Where(x => x.Value.Equals(playerName));

                if (playerNode.Count() > 0)
                {
                    foreach (var player in playerNode)
                    {
                        if (player.Value.Equals(playerName))
                        {
                            var node = player.NextAttribute;
                            var intWins = node.Value;
                            var newWins = int.Parse(intWins);
                            newWins += 1;
                            node.SetValue(newWins);
                            doc.Save(path);
                        }
                    }
                }
                else
                {
                    var node = new XElement("score",
                                    new XAttribute("game", game),
                                    new XAttribute("player_name", playerName),
                                    new XAttribute("wins", wins));
                    doc.Root!.Add(node);
                    doc.Save(path);

                }

            }

        }
    }
}
