using System.Diagnostics;
using MURDoX.Core.Data;
using System.Timers;
using MURDoX.Core.Models.Games;

namespace MURDoX.Core.Services;

public class DiceRollerGameService
{
    private readonly ApplicationDbContext _db;
    private DiceRollerInput _input;
    private System.Timers.Timer timer;
    private double timerSeconds;
    
    public DiceRollerGameService(ApplicationDbContext db, DiceRollerInput input)
    {
        _db = db;
        _input = input;
        timer = new System.Timers.Timer(1000);
        timer.Elapsed += OnTimerEvent;
        timer.Enabled = true;
    }

    private void OnTimerEvent(object? sender, ElapsedEventArgs e)
    {
        //this is where we check to see if the time elapsed matches the duration set in the command.
        timerSeconds++;

        if (timerSeconds >= _input.Duration.Seconds)
        {
            //the duration has expired , send a dice embed to the channel and reset timerSeconds.
        }
    }
}