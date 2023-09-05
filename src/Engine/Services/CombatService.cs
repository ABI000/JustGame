using Engine.Models;

namespace Engine.Services
{
    /// <summary>
    /// 战斗计算
    /// </summary>
    public static class CombatService
    {
        /// <summary>
        /// 判断是否发动攻击
        /// </summary>
        /// <param name="player"></param>
        /// <param name="opponent"></param>
        /// <returns></returns>
        public static Combatant FirstAttacker(Player player, Monster opponent)
        {
            // Formula is: ((Dex(player)^2 - Dex(monster)^2)/10) + Random(-10/10)
            // For dexterity values from 3 to 18, this should produce an offset of +/- 41.5
            int playerDexterity = player.Dexterity * player.Dexterity;
            int opponentDexterity = opponent.Dexterity * opponent.Dexterity;
            decimal dexterityOffset = (playerDexterity - opponentDexterity) / 10m;
            int randomOffset = RandomNumberGenerator.NumberBetween(-10, 10);
            decimal totalOffset = dexterityOffset + randomOffset;
            return RandomNumberGenerator.NumberBetween(0, 100) <= 50 + totalOffset
                       ? Combatant.Player
                       : Combatant.Opponent;
        }
        /// <summary>
        /// 判断攻击命中
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool AttackSucceeded(LivingEntity attacker, LivingEntity target)
        {
            // Currently using the same formula as FirstAttacker initiative.
            // This will change as we include attack/defense skills,
            // armor, weapon bonuses, enchantments/curses, etc.
            int playerDexterity = attacker.Dexterity * attacker.Dexterity;
            int opponentDexterity = target.Dexterity * target.Dexterity;
            decimal dexterityOffset = (playerDexterity - opponentDexterity) / 10m;
            int randomOffset = RandomNumberGenerator.NumberBetween(-10, 10);
            decimal totalOffset = dexterityOffset + randomOffset;
            return RandomNumberGenerator.NumberBetween(0, 100) <= 50 + totalOffset;
        }
    }
    public enum Combatant
    {
        /// <summary>
        /// 玩家
        /// </summary>
        Player,
        /// <summary>
        /// 对手
        /// </summary>
        Opponent
    }
}
