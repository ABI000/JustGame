using Engine.Models;

namespace Engine.Actions
{
    /// <summary>
    /// 武器战斗类
    /// </summary>
    public class AttackWithWeapon
    {
        private readonly GameItem _weapon;
        private readonly int _maximumDamage;
        private readonly int _minimumDamage;
        public event EventHandler<string> OnActionPerformed;
        public AttackWithWeapon(GameItem weapon, int minimumDamage, int maximumDamage)
        {
            if (weapon.Category != ItemCategory.Weapon)
            {
                throw new ArgumentException($"{weapon.Name} is not a weapon");
            }
            if (_minimumDamage < 0)
            {
                throw new ArgumentException("minimumDamage must be 0 or larger");
            }
            if (_maximumDamage < _minimumDamage)
            {
                throw new ArgumentException("maximumDamage must be >= minimumDamage");
            }
            _weapon = weapon;
            _minimumDamage = minimumDamage;
            _maximumDamage = maximumDamage;
        }
        public void Execute(LivingEntity actor, LivingEntity target)
        {
            int damage = RandomNumberGenerator.NumberBetween(_minimumDamage, _maximumDamage);
            if (damage == 0)
            {
                ReportResult($"{actor.Name} missed the {target.Name.ToLower()}.");
            }
            else
            {
                ReportResult($"{actor.Name} hit the {target.Name.ToLower()} for {damage} points.");
                target.TakeDamage(damage);
            }
        }
        private void ReportResult(string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
    }
}
