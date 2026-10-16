using System;
using Cloudora.Level;

namespace Cloudora.Progression
{
    // v3.1 coin: bölüm sonunda kazanılır, başarısızlıkta "devam" için harcanır. Sayılar ilk ayar; testlerle düzenlenecek.
    public static class CoinEconomy
    {
        public const int StartingCoins = 0;   // başlangıç coini ileride tutorial görevinde verilecek
        public const int ContinueCost = 40;
        public const int MediumReward = 10;
        public const int HardReward = 15;
        public const int VeryHardReward = 20;
        public const int LegacyReward = 10;   // 101+ (eski sistem)

        public static int RewardFor(int level)
        {
            if (level < 1 || level > V3LevelPlan.LastLevel) return LegacyReward;
            return V3LevelPlan.BandFor(level) switch
            {
                DifficultyBand.Hard => HardReward,
                DifficultyBand.VeryHard => VeryHardReward,
                _ => MediumReward
            };
        }
    }

    public sealed class CoinWallet
    {
        public int Coins { get; private set; }

        public CoinWallet(int coins) => Coins = Math.Max(0, coins);

        public void Earn(int amount)
        {
            if (amount > 0) Coins += amount;
        }

        public bool TrySpend(int amount)
        {
            if (amount < 0 || amount > Coins) return false;
            Coins -= amount;
            return true;
        }
    }
}
