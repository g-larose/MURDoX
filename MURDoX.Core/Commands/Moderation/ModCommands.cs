using BenchmarkDotNet.Loggers;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MURDoX.Core.Extensions;
using MURDoX.Data;
using MURDoX.Data.Factories;
using MURDoX.Services.Helpers;
using MURDoX.Services.Interfaces;
using MURDoX.Services.Models;
using MURDoX.Services.Services;


namespace MURDoX.Core.Commands.Moderation
{
    public class ModCommands : BaseCommandModule
    {
        private readonly AppDbContextFactory _dbFactory;
        private ILoggerService _logger;
        private readonly UtilityHelper _utilityHelper;

        public ModCommands(AppDbContextFactory dbFactory, ILoggerService logger)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _utilityHelper = new UtilityHelper(dbFactory, logger);

        }

        #region WARN
        [Command("warn")]
        [Description("warns a Mentioned Discord Member")]
        //[RequireRoles(RoleCheckMode.MatchNames, "mod")]
        [RequirePermissions(Permissions.ModerateMembers)]
        public async Task Warn(CommandContext ctx, [RemainingText] string args)
        {
            var user = ctx.Message.MentionedUsers.First();
            var userId = user.Id;
            var reason = args.Split(">")[1].Trim();
            var db = _dbFactory.CreateDbContext();
            var validUser = _utilityHelper.IsValidUser(userId);
            var warnings = 0;

            if (validUser) // user is in the db , update the warnings
            {
                warnings = db.Users!.Where(x => x.DiscordId == userId).Select(y => y.Warnings).FirstOrDefault() + 1;
                await db.SaveChangesAsync();
            }
            else // user is not in the db, create new user and add a warnng
            {
                var newUser = new ServerMember()
                {
                    DiscordId = userId,
                    Username = user.Username,
                    Rank = Rank.NEWB,
                    AvatarUrl = user.AvatarUrl,
                    Thanks = 0,
                    BankAccountTotal = 0,
                    Warnings = 1,
                    XP = 0,
                    Created = DateTime.Now.ToUniversalTime(),
                };
                await _utilityHelper.CreateNewServerMember(newUser);
                warnings = 1;
            }

            await ctx.Channel.SendMessageAsync($"{user.Mention} has been warned : Reason **[{reason}]** total warnings : **{warnings}**");
        }
        #endregion

        #region TIMEOUT
        [Command("timeout")]
        [RequireRoles(RoleCheckMode.Any, "mod", "Admin")]
        public async Task TimeoutMember(CommandContext ctx, DiscordUser user, [RemainingText] string args)
        {
            Random random= new Random();
            var userId = user.Id;
            var mem = ctx.Guild.GetMemberAsync(userId);
            var details = args.Split(',');
            var embedBuilder = new EmbedBuilderHelper();
            var embed = new Embed();
            var _reason = details[1] ?? "no reason given";
            var duration = args[0].ToString();
            if (mem != null)
            {
                double.TryParse(duration, out double d);
                var timeOut = DateTime.Now.AddMinutes(d);
                embed = new Embed()
                {
                    Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                    Desc = $"**[{mem.Result.Username}]** has a {duration}m timeout, **Reason [{_reason}]**",
                };

                try
                {
                    await mem.Result.TimeoutAsync(timeOut, _reason);
                }
                catch (Exception ex)
                {

                    var message = ex.Message;
                }
                await ctx.Channel.SendMessageAsync(embed: embedBuilder.Build(embed));

               
            }

        }
        #endregion

        #region KICK

        #endregion

        #region BAN

        #endregion

        #region PURGE
        [Command("purge")]
        [Description("delete a set number of channel messages")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task Purge(CommandContext ctx, [RemainingText] string count)
        {
            var success = int.TryParse(count, out int newCount);
            int mCount = 0;

            try
            {
                if (success)
                {
                    IEnumerable<DiscordMessage> allMessages = await ctx.Channel.GetMessagesAsync(newCount + 1).ConfigureAwait(false);
                    mCount = allMessages.Count();

                    await ((DiscordChannel)ctx.Channel).DeleteMessagesAsync(allMessages);
                    const int delay = 1500;
                    DiscordMessage m = await ctx.Channel.SendMessageAsync($"```Delete {mCount - 1} messages [SUCESS!]```").ConfigureAwait(false);
                    await Task.Delay(delay);
                    await m.DeleteAsync();
                }
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }
        #endregion

        
    }
}
