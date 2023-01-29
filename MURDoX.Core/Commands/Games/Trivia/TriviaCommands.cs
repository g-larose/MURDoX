﻿using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using MURDoX.Core.Utilities.Converters;
using MURDoX.Data.Factories;
using MURDoX.Extentions;
using MURDoX.Services.Helpers;
using MURDoX.Services.Interfaces;
using MURDoX.Services.Models;
using System.Timers;

namespace MURDoX.Core.Commands.Games.Trivia
{
    public class TriviaCommands : BaseCommandModule
    {
        private readonly IUserService _userService;
        private readonly AppDbContextFactory _dbFactory;
        private System.Timers.Timer _killSwitch;
        private double seconds;
        private string? correctAnswer;
        private bool running;
        private Dictionary<string, int> userPoints = new Dictionary<string, int>();
        public TriviaCommands(IUserService userService, AppDbContextFactory dbFactory)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _dbFactory = dbFactory;
        }


        [Command("trivia")]
        [Description("starts a new trivia game")]
        
        public async Task PlayTrivia(CommandContext ctx, [RemainingText] string cat = "")
        {
            ctx.Client.ComponentInteractionCreated += Client_ComponentInteractionCreated;
            _killSwitch = new System.Timers.Timer(55000);
            _killSwitch.Elapsed += _killSwitch_Elapsed;
            _killSwitch.Enabled = true;
            _killSwitch.Start();
            var questions = new List<Question>();
            var timer = new System.Timers.Timer();
            var bot = ctx.Client.CurrentUser;
            seconds = 0;
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
           // timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
            running = true;

            var builder = new DiscordEmbedBuilder()
                      .WithTitle("**new question is on the way!**")
                      .WithColor(DiscordColor.Grayple)
                      .WithTimestamp(null);
            await ctx.Channel.SendMessageAsync(embed: builder.Build());
            await ctx.Channel.TriggerTypingAsync();

            var questionRequest = await TriviaHttpHelper.MakeQuestionRequest(cat, "easy");
            questions = TriviaHttpHelper.HandleQuestionResponse(questionRequest);


            while (running)
            { 
                if (seconds >= 25)
                {
                    seconds = 0;
                    Random rnd = new Random();

                    if (questions.Count < 1)
                    {
                        await ctx.Channel.SendMessageAsync("Reloading questions, hang tight!");
                        await ctx.Channel.TriggerTypingAsync();

                        try
                        {
                            questionRequest = await TriviaHttpHelper.MakeQuestionRequest(cat, "easy");
                            questions = TriviaHttpHelper.HandleQuestionResponse(questionRequest);

                            if (questions.Count == 0)
                                continue;
                        }
                        catch (Exception ex)
                        {
                            var message = ex.Message;
                        }
                    }
                    var index = rnd.Next(0, questions.Count);
                    var pickedQuestion = questions[index];
                    var answers = pickedQuestion.Answers!;
                    correctAnswer = pickedQuestion.CorrectAnswer!.Replace("&rsquo;", string.Empty).Replace("&#039;", "'").Replace("&quot;", "'");
                    string answerOne = answers[0].ToString();
                    string answerTwo = answers[1].ToString();
                    string answerThree = answers[2].ToString();
                    answerOne.Replace("&rsquo;", string.Empty).Replace("&#039;", "'").Replace("&quot;", "'");
                    answerTwo.Replace("&rsquo;", string.Empty).Replace("&#039;", "'").Replace("&quot;", "'");
                    answerThree.Replace("&rsquo;", string.Empty).Replace("&#039;", "'").Replace("&quot;", "'");
                    var q = pickedQuestion?._Question!.ToString()?.Replace("&rsquo;", string.Empty).Replace("&#039;", "'").Replace("&quot;", "'");
                    var answer_one_btn = new DiscordButtonComponent(ButtonStyle.Secondary, answers[0].ToString(), answers[0].ToString());
                    var answer_two_btn = new DiscordButtonComponent(ButtonStyle.Secondary, answers[1].ToString(), answers[1].ToString());
                    var answer_three_btn = new DiscordButtonComponent(ButtonStyle.Secondary, answers[2].ToString(), answers[2].ToString());
                    var answer_correct_btn = new DiscordButtonComponent(ButtonStyle.Secondary, correctAnswer, correctAnswer);

                    var buttons = new DiscordButtonComponent[] { answer_one_btn, answer_two_btn, answer_three_btn, answer_correct_btn };
                    buttons.Shuffle();
                    questions.Remove(pickedQuestion!);
                    var embedBuilder = new EmbedBuilderHelper();

                    var embed = new Embed()
                    {
                        Title = "Trivia",
                        Author = bot.Username,
                        Desc = $"*{pickedQuestion!.Category}*\r\n**{q}**",
                        Color = await ShuffleHelper.GetRandomEmbedColorAsync(),
                        Footer = $"{bot.Username}",
                        TimeStamp = DateTime.Now,
                    };


                    var messageBuilder = new DiscordMessageBuilder();
                    messageBuilder.AddComponents(buttons)
                                    .AddEmbed(embedBuilder.Build(embed));

                    await ctx.Channel.SendMessageAsync(messageBuilder);
                    _killSwitch.Start();
                }
            }

            var points = userPoints.Take(1);
            var deleteEmbedBuilder = new EmbedBuilderHelper();
            var nameField = new EmbedField() { Name = "Member", Value = points.First().Key, Inline = true };
            var xpField = new EmbedField() { Name = "Points", Value = points.First().Value.ToString(), Inline = true };
            var fields = new EmbedField[] { nameField, xpField };
            var deleteEmbed = new Embed()
            {
                Desc = "Trivia Stopped",
                Fields = fields,
                TimeStamp = DateTime.Now,
            };
            await ctx.Channel.SendMessageAsync(embed: deleteEmbedBuilder.Build(deleteEmbed));
            //TODO: Display the score foreach player
            //something went wrong or a mod stopped the trivia game 
            //display the scores foreach player in memory.
        }


        private void _killSwitch_Elapsed(object? sender, ElapsedEventArgs e)
        {
            running = false;
            _killSwitch.Stop();  
        }

        private async Task Client_ComponentInteractionCreated(DiscordClient sender, ComponentInteractionCreateEventArgs e)
        {
            seconds = 0;
           
            var user = e.Interaction.User.Username;
            var interactionMessage = e.Message;
            var buttons = interactionMessage.Components;
            var mess = await e.Message.ModifyAsync($"**{user}** Answered - Calculating Response...");

            foreach (var component in mess.Components)
            {
                var but = component.Components.Where(x => x.Type == ComponentType.Button).ToList();

                ((DiscordButtonComponent)but[0]).Disable();
                ((DiscordButtonComponent)but[1]).Disable();
                ((DiscordButtonComponent)but[2]).Disable();
                ((DiscordButtonComponent)but[3]).Disable();
            }
            var gameStarted = e.Interaction.CreationTimestamp;
            _killSwitch.Stop();
            await Task.Delay(1000);

         
            if (e.Interaction.Data.CustomId == correctAnswer)
            {
                if (!userPoints.ContainsKey(user))
                    userPoints.Add(user, 100);
                else
                {
                    var _ = userPoints.TryGetValue(user, out int uPoints);
                    userPoints[user] = uPoints + 100;
                }
                await e.Message.ModifyAsync($"**{user}** correct - 100 points awarded");
                var followUpMessageBuilder = new DiscordFollowupMessageBuilder();
                var embedBuilder = new EmbedBuilderHelper();
                var embed = new Embed()
                {
                    Color = "chartreuse",
                    Desc = $"Correct Answer: **{correctAnswer}**",
                    Footer = e.Message.Author.Username,
                    TimeStamp = DateTime.Now,
                };
                followUpMessageBuilder.AddEmbed(embedBuilder.Build(embed));
                var responseBuilder = new DiscordInteractionResponseBuilder();
                responseBuilder.AddEmbed(embedBuilder.Build(embed));
                    
                await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);
                //await e.Interaction.CreateFollowupMessageAsync(followUpMessageBuilder);

                await _userService.UpdateOrAddXpToUser(user, 100);
            }  
            else
            {
                await e.Message.ModifyAsync($"**{user}** incorrect 0 points awarded");
                var followUpMessageBuilder1 = new DiscordFollowupMessageBuilder();
                var embedBuilder1 = new EmbedBuilderHelper();
                var embed1 = new Embed()
                {
                    Color = "magenta",
                    Desc = $"Correct Answer: **{correctAnswer}**",
                    Footer = e.Message.Author.Username,
                    TimeStamp = DateTime.Now,
                };
                followUpMessageBuilder1.AddEmbed(embedBuilder1.Build(embed1));
                var responseBuilder = new DiscordInteractionResponseBuilder();
                responseBuilder.AddEmbed(embedBuilder1.Build(embed1));

                await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);
                //await e.Interaction.CreateFollowupMessageAsync(followUpMessageBuilder1);
            }

            
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            seconds++;
        }

    }
}
