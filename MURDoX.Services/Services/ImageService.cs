using DSharpPlus.Entities;
using MURDoX.Data.Factories;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace MURDoX.Services.Services
{
    public class ImageService
    {
        private readonly AppDbContextFactory _dbFactory;

        public ImageService(AppDbContextFactory contextFactory)
        {
            _dbFactory = contextFactory;
        }

        public async Task<SixLabors.ImageSharp.Image> GenerateRankImage(DiscordMember member)
        {
            var db = _dbFactory.CreateDbContext(); 
            var ct = new CancellationTokenSource();
            SixLabors.ImageSharp.Image rankImage;

            if (ct.IsCancellationRequested) return null;

            var baseImagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Images", "player_rank.png");

            using (rankImage = SixLabors.ImageSharp.Image.Load(baseImagePath))
            {
                return rankImage;
               
            }
           
        }  
    }
}
