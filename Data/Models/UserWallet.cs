namespace Safira.Data.Models;

public class UserWallet
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public string GuildId { get; set; } = null!;
    public long Balance { get; set; }
    public DateTime? LastDaily { get; set; }
}
