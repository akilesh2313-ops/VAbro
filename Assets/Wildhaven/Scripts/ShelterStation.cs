using UnityEngine;

namespace Wildhaven
{
    public sealed class ShelterStation : MonoBehaviour
    {
        [SerializeField] private int woodCost = 8;
        [SerializeField] private int stoneCost = 4;
        [SerializeField] private GameObject[] upgradeStages;
        private int level;

        public bool TryUpgrade()
        {
            var game = WildhavenGameManager.Instance;
            if (game == null || level >= upgradeStages.Length) return false;
            if (!game.SpendResource(ResourceType.Wood, woodCost) ||
                !game.SpendResource(ResourceType.Stone, stoneCost)) return false;

            if (level > 0 && upgradeStages[level - 1] != null)
                upgradeStages[level - 1].SetActive(false);
            if (upgradeStages[level] != null)
                upgradeStages[level].SetActive(true);
            level++;
            game.SetShelterLevel(level);
            return true;
        }
    }
}