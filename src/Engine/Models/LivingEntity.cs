using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public abstract class LivingEntity : BaseNotificationClass
    {
        private string _name = string.Empty;
        private int _currentHitPoints;
        private int _maximumHitPoints;
        private int _gold;
        private GameItem _currentWeapon;
        /// <summary>
        /// 最大生命值
        /// </summary>
        public GameItem CurrentWeapon
        {
            get { return _currentWeapon; }
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
            get { return _name; }
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
            get { return _currentHitPoints; }
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
            get { return _maximumHitPoints; }
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
            get { return _gold; }
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
            get { return _level; }
            protected set
            {
                _level = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 物品集合
        /// </summary>
        public ObservableCollection<GameItem> Inventory { get; }
        /// <summary>
        /// 物品分类
        /// </summary>
        public ObservableCollection<GroupedInventoryItem> GroupedInventory { get; }
        /// <summary>
        /// 拥有的武器
        /// 后期改为多装备
        /// </summary>
        public List<GameItem> Weapons => Inventory.Where(i => i.Category == ItemCategory.Weapon).ToList();


        /// <summary>
        /// 是否死亡
        /// </summary>
        public bool IsDead => CurrentHitPoints <= 0;

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
        protected LivingEntity(string name, int maximumHitPoints, int currentHitPoints, int gold, int level = 1)
        {
            Name = name;
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitPoints;
            Gold = gold;
            Level = level;
            Inventory = new ObservableCollection<GameItem>();
            GroupedInventory = new ObservableCollection<GroupedInventoryItem>();
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


        public void AddItemToInventory(GameItem item)
        {
            Inventory.Add(item);
            if (item.IsUnique)
            {
                GroupedInventory.Add(new GroupedInventoryItem(item, 1));
            }
            else
            {
                if (!GroupedInventory.Any(gi => gi.Item.ItemTypeID == item.ItemTypeID))
                {
                    GroupedInventory.Add(new GroupedInventoryItem(item, 0));
                }
                GroupedInventory.First(gi => gi.Item.ItemTypeID == item.ItemTypeID).Quantity++;
            }
            OnPropertyChanged(nameof(Weapons));
        }



        public void RemoveItemFromInventory(GameItem item)
        {
            Inventory.Remove(item);
            GroupedInventoryItem? groupedInventoryItemToRemove = item.IsUnique ? GroupedInventory.FirstOrDefault(gi => gi.Item == item) : GroupedInventory.FirstOrDefault(gi => gi.Item.ItemTypeID == item.ItemTypeID);
            if (groupedInventoryItemToRemove != null)
            {
                if (groupedInventoryItemToRemove.Quantity == 1)
                {
                    GroupedInventory.Remove(groupedInventoryItemToRemove);
                }
                else
                {
                    groupedInventoryItemToRemove.Quantity--;
                }
            }
            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumable));
        }
        public void RemoveItemsFromInventory(List<ItemQuantity> itemQuantities)
        {
            foreach (ItemQuantity itemQuantity in itemQuantities)
            {
                for (int i = 0; i < itemQuantity.Quantity; i++)
                {
                    RemoveItemFromInventory(Inventory.First(item => item.ItemTypeID == itemQuantity.ItemID));
                }
            }
        }

        /// <summary>
        /// 物品检查
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool HasAllTheseItems(List<ItemQuantity> items)
        {
            foreach (ItemQuantity item in items)
            {
                if (Inventory.Count(i => i.ItemTypeID == item.ItemID) < item.Quantity)
                {
                    return false;
                }
            }
            return true;
        }
        public void RaiseActionPerformedEvent(object sender, string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
        #endregion
        public void UseCurrentWeaponOn(LivingEntity target)
        {
            CurrentWeapon.PerformAction(this, target);
        }

        #region 物品逻辑
        public List<GameItem> Consumables => Inventory.Where(i => i.Category == ItemCategory.Consumable).ToList();
        public bool HasConsumable => Consumables.Any();
        private GameItem _currentConsumable;
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
    }
}
