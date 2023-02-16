using System.Diagnostics;
using MURDoX.Core.Data;
using System.Timers;
using Microsoft.EntityFrameworkCore.Query;
using MURDoX.Core.Models.Games;
using Remora.Results;

namespace MURDoX.Core.Services;

public class DiceRollerGameService
{
    private readonly ApplicationDbContext _db;
    private DiceRollerInput _input;
    private System.Timers.Timer timer;
    private double timerSeconds;
    
    public DiceRollerGameService(ApplicationDbContext db)
    {
        _db = db;
        timer = new System.Timers.Timer(1000);
        timer.Elapsed += OnTimerEvent;
        timer.Enabled = true;
    }

    private void OnTimerEvent(object? sender, ElapsedEventArgs e)
    {
        //this is where we check to see if the time elapsed matches the duration set in the command.
        timerSeconds++;
        if (timerSeconds >= _input.Duration.Seconds)
            DoRoll(_input);
    }

    public async Task<DiceRollerResponse> DoRoll(DiceRollerInput input)
    {
        _input = input;
        timer.Start();
        return new DiceRollerResponse(); //this is just to get rid of the error.
    }
}