using System.ComponentModel;
using System.Globalization;
using MURDoX.Core.Models.Games.EconomyGame.Inputs;
using MURDoX.Core.Models.Games.EconomyGame.Responses;
using MURDoX.Core.Services;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Results;
// ReSharper disable UnusedMember.Global

namespace MURDoX.DiscordAccess.Commands.TextCommands
{
    public class EconomyGameCommandGroup : CommandGroup
    {
        private readonly IDiscordRestChannelAPI _channels;
        private readonly ITextCommandContext _context;
        private readonly EconomyGameService _economyGameService;

        public EconomyGameCommandGroup(IDiscordRestChannelAPI channels, ITextCommandContext context, EconomyGameService economyGameService)
        {
            _channels = channels;
            _context = context;
            _economyGameService = economyGameService;
        }
        
        [Command("economy setup")]
        [Description("sets up the economy game")]
        public async Task<Result> EconomySetupAsync()
        {
            EmbedField[] fields = {
                new("economy setup money <value: int>", "Give amount of MUR-Dollar that every player starts with; Default is 1000"),
                new("economy setup resource <value: int> <value: int> <value: int>", "Give amount of resources that every players planet generates per day passively; Default is 100 Starstone 600 Chromotite 24 Zoridium"),
                new("economy setup shop <value: float>", "Give the multiplier for the shop effects; Default is 1"),
            };
            Embed embed = new(Title:"Economy Setup", Description:"Economy Setup", Fields: fields);
            Result<IMessage> result = await _channels.CreateMessageAsync(_context.Message.ChannelID.Value, embeds: new []{embed});
            return result.IsSuccess ? Result.FromSuccess() : Result.FromError(result.Error);
        }
        
        [Command("economy setup money")]
        [Description("sets up the economy game")]
        public async Task EconomySetupMoneyAsync(int money)
        {
            EconomySetupMoneyResponse response = await _economyGameService.SetupMoney(new EconomySetupMoneyInput
            {
                Money = money
            });
            EmbedField[] fields = {
                new("Money Before", response.MoneyBefore.ToString()),
                new("Money After", response.MoneyAfter.ToString()),
            };
            Embed embed = new(Title:"Economy Setup Money", Description:response.Message, Fields: fields);
            await _channels.CreateMessageAsync(_context.Message.ChannelID.Value, embeds: new []{embed});
        }
        
        [Command("economy setup resource")]
        [Description("sets up the economy game")]
        public async Task EconomySetupResourceAsync(int starstone, int chromotite, int zoridium)
        {
            EconomySetupResourceResponse response = await _economyGameService.SetupResources(new EconomySetupResourceInput
            {
                Starstone = starstone,
                Chromotite = chromotite,
                Zoridium = zoridium
            });
            EmbedField[] fields = {
                new("Starstone Before", response.StarstoneBefore.ToString()),
                new("Starstone After", response.StarstoneAfter.ToString()),
                new("Chromotite Before", response.ChromotiteBefore.ToString()),
                new("Chromotite After", response.ChromotiteAfter.ToString()),
                new("Zoridium Before", response.ZoridiumBefore.ToString()),
                new("Zoridium After", response.ZoridiumAfter.ToString()),
            };
            Embed embed = new(Title:"Economy Setup Resources", Description:response.Message, Fields: fields);
            await _channels.CreateMessageAsync(_context.Message.ChannelID.Value, embeds: new []{embed});
        }
        
        [Command("economy setup shop")]
        [Description("sets up the economy game")]
        public async Task EconomySetupShopAsync(float multiplier)
        {
            EconomySetupShopResponse response = await _economyGameService.SetupShop(new EconomySetupShopInput
            {
                ShopEffectMultiplier = multiplier
            });
            EmbedField[] fields = {
                new("Multiplier Before", response.ShopEffectMultiplierBefore.ToString(CultureInfo.InvariantCulture)),
                new("Multiplier After", response.ShopEffectMultiplierAfter.ToString(CultureInfo.InvariantCulture)),
            };
            Embed embed = new(Title:"Economy Setup Shop", Description:response.Message, Fields: fields);
            await _channels.CreateMessageAsync(_context.Message.ChannelID.Value, embeds: new []{embed});
        }
        
        [Command("economy join")]
        [Description("joins the economy game")]
        public async Task EconomyJoinAsync()
        {
            EconomyNewPlayerResponse response = await _economyGameService.NewPlayer(new EconomyNewPlayerInput
            {
                UserId = long.Parse(_context.Message.Author.Value.ID.Value.ToString())
            });
            EmbedField[] fields = {
                new("Money", response.Money.ToString()),
                new("Starstone", response.Starstone.ToString()),
                new("Chromotite", response.Chromotite.ToString()),
                new("Zoridium", response.Zoridium.ToString()),
                new("Planet Name", response.PlanetName),
                new("Planet Id", response.PlanetId.ToString()),
            };
            Embed embed = new(Title:"Economy Join", Description:response.Message, Fields: fields);
            await _channels.CreateMessageAsync(_context.Message.ChannelID.Value, embeds: new []{embed});
        }
    }
}