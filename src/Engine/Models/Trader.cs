namespace Engine.Models
{
    /// <summary>
    /// 交易员
    /// </summary>
    public class Trader : LivingEntity
    {
        public Trader(string name) : base(name, 9999, 9999, 9999)
        {
        }
    }
}
