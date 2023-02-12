namespace MURDoX.Core.Models
{
    public class Embed
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Desc { get; set; }
        public string? LinkUrl { get; set; }
        public string? Footer { get; set; }
        public string? AuthorAvatar { get; set; }
        public string? ImgUrl { get; set; }
        public string? ThumbnailImgUrl { get; set; }
        public string? FooterImgUrl { get; set; }
        public string? Color { get; set; }
        public DateTimeOffset? TimeStamp { get; set; }
        public EmbedField[]? Fields { get; set; }
    }
}