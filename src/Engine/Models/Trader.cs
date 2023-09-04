namespace Engine.Models
{
    /// <summary>
    /// 交易员
    /// </summary>
    public class Trader : LivingEntity
    {
        public int Id { get; set; }
        public Trader(int id, string name) : base(name, 9999, 9999, 9999)
        {
            Id = id;
        }
    }
}
