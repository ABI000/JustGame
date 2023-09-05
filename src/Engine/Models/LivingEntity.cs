using Engine.Services;
using Newtonsoft.Json;

namespace Engine.Models
{
    public abstract class LivingEntity : BaseNotificationClass
    {
        private string _name = string.Empty;
        private int _currentHitPoints;
        private int _maximumHitPoints;
        private int _gold;

        private int _dexterity;

        /// <summary>
        /// 灵巧
        /// </summary>
        public int Dexterity
        {
            get => _dexterity;
            private set
            {
                _dexterity = value;
                OnPropertyChanged();
            }
        }
        private GameItem _currentWeapon;
        /// <summary>
        /// 最大生命值
        /// </summary>
        public GameItem CurrentWeapon
        {
            get => _currentWeapon;
            set
            {
                if (_currentWeapon != null)
                {
                    _currentWeapon.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }
                _currentWeapon = value;
                if (_currentWeapon != null)
                {
                    _currentWeapon.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get => _name;
            private set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 当前生命值
        /// </summary>
        public int CurrentHitPoints
        {
            get => _currentHitPoints;
            private set
            {
                _currentHitPoints = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 最大生命值
        /// </summary>
        public int MaximumHitPoints
        {
            get => _maximumHitPoints;
            protected set
            {
                _maximumHitPoints = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 金币数量
        /// </summary>
        public int Gold
        {
            get => _gold;
            private set
            {
                _gold = value;
                OnPropertyChanged();
            }
        }


        private int _level;
        /// <summary>
        /// 等级
        /// </summary>
        public int Level
        {
            get => _level;
            protected set
            {
                _level = value;
                OnPropertyChanged();
            }
        }


        private Inventory _inventory;
        /// <summary>
        /// 背包
        /// </summary>
        public Inventory Inventory
        {
            get => _inventory;
            protected set
            {
                _inventory = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否死亡
        /// </summary>
        public bool IsDead => !IsAlive;
        /// <summary>
        /// 是否存活
        /// </summary>
        public bool IsAlive => CurrentHitPoints > 0;
        /// <summary>
        /// 死亡事件
        /// 具体执行函数由具体逻辑传入
        /// </summary>
        public event EventHandler? OnKilled;

        public event EventHandler<string>? OnActionPerformed;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maximumHitPoints"></param>
        /// <param name="currentHitPoints"></param>
        /// <param name="gold"></param>
        protected LivingEntity(string name, int maximumHitPoints, int currentHitPoints, int dexterity, int gold, int level = 1)
        {
            Name = name;
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitPoints;
            Gold = gold;
            Level = level;
            Inventory = new Inventory();
            Dexterity = dexterity;
        }

        public void RaiseActionPerformedEvent(object sender, string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
        /// <summary>
        /// 使用武器
        /// </summary>
        /// <param name="target"></param>
        public void UseCurrentWeaponOn(LivingEntity target)
        {
            CurrentWeapon.PerformAction(this, target);
        }

        #region 生命计算逻辑

        /// <summary>
        /// 扣血
        /// </summary>
        /// <param name="hitPointsOfDamage"></param>
        public void TakeDamage(int hitPointsOfDamage)
        {
            CurrentHitPoints -= hitPointsOfDamage;
            if (IsDead)
            {
                CurrentHitPoints = 0;
                RaiseOnKilledEvent();
            }
        }

        /// <summary>
        /// 发送死亡信息
        /// </summary>
        private void RaiseOnKilledEvent()
        {
            OnKilled?.Invoke(this, new System.EventArgs());
        }
        /// <summary>
        /// 加血
        /// </summary>
        /// <param name="hitPointsToHeal"></param>
        public void Heal(int hitPointsToHeal)
        {
            CurrentHitPoints += hitPointsToHeal;
            if (CurrentHitPoints > MaximumHitPoints)
            {
                CurrentHitPoints = MaximumHitPoints;
            }
        }
        /// <summary>
        /// 完全恢复
        /// </summary>
        public void CompletelyHeal()
        {
            CurrentHitPoints = MaximumHitPoints;
        }

        #endregion

        #region 金币处理


        /// <summary>
        /// 增加金币
        /// </summary>
        /// <param name="amountOfGold"></param>
        public void ReceiveGold(int amountOfGold)
        {
            Gold += amountOfGold;
            RaiseMessage($"You receive {amountOfGold} gold");
        }

        /// <summary>
        /// 扣除金币
        /// </summary>
        /// <param name="amountOfGold"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void SpendGold(int amountOfGold)
        {
            if (amountOfGold > Gold)
            {
                throw new ArgumentOutOfRangeException($"{Name} only has {Gold} gold, and cannot spend {amountOfGold} gold");
            }
            Gold -= amountOfGold;
        }
        #endregion

        #region 物品逻辑

        /// <summary>
        /// 物品检查
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool HasAllTheseItems(List<ItemQuantity> items) => Inventory.HasAllTheseItems(items);
        /// <summary>
        /// 添加物品
        /// </summary>
        /// <param name="item"></param>
        public void AddItemToInventory(GameItem item)
        {
            Inventory = Inventory.AddItem(item);
        }
        /// <summary>
        /// 移除物品
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItemFromInventory(GameItem item)
        {
            Inventory = Inventory.RemoveItem(item);
        }
        /// <summary>
        /// 批量移除物品
        /// </summary>
        /// <param name="itemQuantities"></param>
        public void RemoveItemsFromInventory(List<ItemQuantity> itemQuantities)
        {
            Inventory = Inventory.RemoveItems(itemQuantities);
        }


        #region 消耗品逻辑
        private GameItem _currentConsumable;

        [JsonIgnore]
        public GameItem CurrentConsumable
        {
            get => _currentConsumable;
            set
            {
                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }
                _currentConsumable = value;
                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }
                OnPropertyChanged();
            }
        }
        public void UseCurrentConsumable()
        {
            CurrentConsumable.PerformAction(this, this);
            RemoveItemFromInventory(CurrentConsumable);
        }
        #endregion
        #endregion
    }
}
