﻿using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace Engine.Models
{
    public class Player : LivingEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maximumHitPoints"></param>
        /// <param name="currentHitPoints"></param>
        /// <param name="gold"></param>
        public Player(string characterClass, int experiencePoints, string name, int maximumHitPoints, int currentHitPoints, int dexterity, int gold)
            : base(name, maximumHitPoints, currentHitPoints, dexterity, gold)
        {
            CharacterClass = characterClass;
            ExperiencePoints = experiencePoints;
            Quests = new ObservableCollection<QuestStatus>();
            Recipes = new ObservableCollection<Recipe>();
        }
        #region Properties
        private string _characterClass;
        private int _experiencePoints;


        /// <summary>
        /// 角色种族
        /// </summary>
        public string CharacterClass
        {
            get { return _characterClass; }
            set
            {
                _characterClass = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 经验
        /// </summary>
        public int ExperiencePoints
        {
            get { return _experiencePoints; }
            set
            {
                _experiencePoints = value;
                OnPropertyChanged();
                SetLevelAndMaximumHitPoints();
            }
        }
        public event EventHandler OnLeveledUp;
        /// <summary>
        /// 任务列表
        /// </summary>
        public ObservableCollection<QuestStatus> Quests { get; }
        #endregion
        /// <summary>
        /// 增加经验
        /// </summary>
        /// <param name="experiencePoints"></param>
        public void AddExperience(int experiencePoints)
        {
            ExperiencePoints += experiencePoints;
            RaiseMessage($"You receive {experiencePoints} experience points");
        }
        /// <summary>
        /// 设置等级
        /// </summary>
        private void SetLevelAndMaximumHitPoints()
        {
            int originalLevel = Level;
            Level = (ExperiencePoints / 100) + 1;
            if (Level != originalLevel)
            {
                MaximumHitPoints = Level * 10;
                OnLeveledUp?.Invoke(this, System.EventArgs.Empty);
            }
        }

        public ObservableCollection<Recipe> Recipes { get; }
        public void LearnRecipe(Recipe recipe)
        {
            if (!Recipes.Any(r => r.ID == recipe.ID))
            {
                Recipes.Add(recipe);
            }
        }
    }
}
