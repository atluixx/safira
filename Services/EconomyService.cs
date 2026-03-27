using Microsoft.EntityFrameworkCore;
using Safira.Data;
using Safira.Data.Models;

namespace Safira.Services;

public class EconomyService(SafiraDbContext db)
{
    private const long DailyAmount = 100;
    private static readonly TimeSpan DailyCooldown = TimeSpan.FromHours(24);

    public async Task<UserWallet> GetOrCreateWalletAsync(ulong userId, ulong guildId)
    {
        var uid = userId.ToString();
        var gid = guildId.ToString();

        var wallet = await db.Wallets
            .FirstOrDefaultAsync(w => w.UserId == uid && w.GuildId == gid);

        if (wallet is not null)
            return wallet;

        wallet = new UserWallet { UserId = uid, GuildId = gid, Balance = 0 };
        db.Wallets.Add(wallet);
        await db.SaveChangesAsync();
        return wallet;
    }

    public async Task<(bool Success, TimeSpan? TimeRemaining)> TryClaimDailyAsync(ulong userId, ulong guildId)
    {
        var wallet = await GetOrCreateWalletAsync(userId, guildId);

        if (wallet.LastDaily.HasValue)
        {
            var elapsed = DateTime.UtcNow - wallet.LastDaily.Value;
            if (elapsed < DailyCooldown)
                return (false, DailyCooldown - elapsed);
        }

        wallet.Balance += DailyAmount;
        wallet.LastDaily = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> TryTransferAsync(
        ulong fromUserId, ulong toUserId, ulong guildId, long amount)
    {
        if (amount <= 0)
            return (false, "Amount must be greater than zero.");

        if (fromUserId == toUserId)
            return (false, "You cannot transfer coins to yourself.");

        var sender = await GetOrCreateWalletAsync(fromUserId, guildId);
        if (sender.Balance < amount)
            return (false, $"Insufficient balance. You have **{sender.Balance:N0}** coins.");

        var recipient = await GetOrCreateWalletAsync(toUserId, guildId);

        sender.Balance -= amount;
        recipient.Balance += amount;
        await db.SaveChangesAsync();
        return (true, null);
    }
}
