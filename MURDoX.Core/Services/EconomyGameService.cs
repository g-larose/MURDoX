using MURDoX.Core.Data;
using MURDoX.Core.Models.Games.EconomyGame.ContextModels;
using MURDoX.Core.Models.Games.EconomyGame.Inputs;
using MURDoX.Core.Models.Games.EconomyGame.Responses;

namespace MURDoX.Core.Services
{
    public class EconomyGameService
    {
        private readonly ApplicationDbContext _dbContext;

        public EconomyGameService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EconomySetupMoneyResponse> SetupMoney(EconomySetupMoneyInput input)
        {
            int moneyBefore = _dbContext.EconomySettings.FirstOrDefault()!.MoneySet;
            
            if (input.Money < 0)
            {
                return new EconomySetupMoneyResponse
                {
                    Message = "You cannot have negative money.",
                    MoneyBefore = moneyBefore,
                    MoneyAfter = moneyBefore
                };
            }
            
            _dbContext.EconomySettings.FirstOrDefault()!.MoneySet = input.Money;
            await _dbContext.SaveChangesAsync();
            return new EconomySetupMoneyResponse
            {
                Message = "Money set successfully.",
                MoneyBefore = moneyBefore,
                MoneyAfter = input.Money
            };
        }

        public async Task<EconomySetupResourceResponse> SetupResources(EconomySetupResourceInput input)
        {
            int starstoneBefore = _dbContext.EconomySettings.FirstOrDefault()!.StarstoneSet;
            int chromotiteBefore = _dbContext.EconomySettings.FirstOrDefault()!.ChromotiteSet;
            int zoridiumBefore = _dbContext.EconomySettings.FirstOrDefault()!.ZoridiumSet;

            if (input.Starstone < 0 || input.Chromotite < 0 || input.Zoridium < 0)
            {
                return new EconomySetupResourceResponse
                {
                    Message = "You cannot have negative Resources.",
                    StarstoneBefore = starstoneBefore,
                    StarstoneAfter = starstoneBefore,
                    ChromotiteBefore = chromotiteBefore,
                    ChromotiteAfter = chromotiteBefore,
                    ZoridiumBefore = zoridiumBefore,
                    ZoridiumAfter = zoridiumBefore
                };
            }

            _dbContext.EconomySettings.FirstOrDefault()!.StarstoneSet = input.Starstone;
            _dbContext.EconomySettings.FirstOrDefault()!.ChromotiteSet = input.Chromotite;
            _dbContext.EconomySettings.FirstOrDefault()!.ZoridiumSet = input.Zoridium;
            await _dbContext.SaveChangesAsync();
            
            return new EconomySetupResourceResponse
            {
                Message = "Resources set successfully.",
                StarstoneBefore = starstoneBefore,
                StarstoneAfter = input.Starstone,
                ChromotiteBefore = chromotiteBefore,
                ChromotiteAfter = input.Chromotite,
                ZoridiumBefore = zoridiumBefore,
                ZoridiumAfter = input.Zoridium
            };
        }
        
        public async Task<EconomySetupShopResponse> SetupShop(EconomySetupShopInput input)
        {
            float shopBefore = _dbContext.EconomySettings.FirstOrDefault()!.ShopMultiplier;
            
            if (input.ShopEffectMultiplier < 0)
            {
                return new EconomySetupShopResponse
                {
                    Message = "You cannot have negative shops.",
                    ShopEffectMultiplierBefore = shopBefore,
                    ShopEffectMultiplierAfter = shopBefore
                };
            }
            
            _dbContext.EconomySettings.FirstOrDefault()!.ShopMultiplier = input.ShopEffectMultiplier;
            await _dbContext.SaveChangesAsync();
            return new EconomySetupShopResponse
            {
                Message = "Shop set successfully.",
                ShopEffectMultiplierBefore = shopBefore,
                ShopEffectMultiplierAfter = input.ShopEffectMultiplier
            };
        }
        
        public async Task<EconomyNewPlayerResponse> NewPlayer(EconomyNewPlayerInput input)
        {
            EconomySettings settings = _dbContext.EconomySettings.FirstOrDefault()!;
            string planetName = GetPlanetName(input.PlayerName);
            EconomyPlayer player = new()
            {
                UserId = input.UserId,
                Money = settings.MoneySet,
                Starstone = settings.StarstoneSet,
                Chromotite = settings.ChromotiteSet,
                Zoridium = settings.ZoridiumSet,
                OwnedPlanetIds = new List<Guid> { Guid.NewGuid() },
            };
            await _dbContext.EconomyPlayers.AddAsync(player);
            await _dbContext.SaveChangesAsync();
            EconomyPlayer newPlayer = _dbContext.EconomyPlayers.FirstOrDefault(x => x.UserId == input.UserId)!;
            return new EconomyNewPlayerResponse
            {
                Message = "Player created successfully.",
                Chromotite = newPlayer.Chromotite,
                Starstone = newPlayer.Starstone,
                Zoridium = newPlayer.Zoridium,
                Money = newPlayer.Money,
                PlanetName = planetName,
            };
        }
        
        private string GetPlanetName(string playerName)
        {
            string[] planetNames = {
                "Atria",
                "Azura",
                "Caelum",
                "Celestia",
                "Chronos",
                "Cosmos",
                "Elysium",
                "Gaia",
                "Helios",
                "Horizon",
                "Hydra",
                "Indigo",
                "Lyra",
                "Nova",
                "Orion",
                "Phoenix",
                "Polaris",
                "Pyxis",
                "Sol",
                "Titan"
            };
            return planetNames[new Random().Next(0, planetNames.Length)] + playerName;
        }
    }
}