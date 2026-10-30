using System.Collections.Generic;
using UnityEngine;

namespace Wildhaven
{
    public sealed class WildhavenGameManager : MonoBehaviour
    {
        public static WildhavenGameManager Instance { get; private set; }
        private readonly Dictionary<ResourceType, int> inventory = new Dictionary<ResourceType, int>();
        public int ShelterLevel { get; private set; } = 1;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
                inventory[type] = 0;
        }

        public int GetResource(ResourceType type) => inventory.TryGetValue(type, out int value) ? value : 0;

        public void AddResource(ResourceType type, int amount)
        {
            if (amount > 0) inventory[type] = GetResource(type) + amount;
        }

        public bool SpendResource(ResourceType type, int amount)
        {
            if (amount < 0 || GetResource(type) < amount) return false;
            inventory[type] = GetResource(type) - amount;
            return true;
        }

        public void SetShelterLevel(int level) => ShelterLevel = Mathf.Max(1, level);
    }
}