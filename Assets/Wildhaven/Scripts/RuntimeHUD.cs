using UnityEngine;
using UnityEngine.UI;

namespace Wildhaven
{
    public sealed class RuntimeHUD : MonoBehaviour
    {
        [SerializeField] private Text inventoryText;
        [SerializeField] private Text shelterText;

        private void Update()
        {
            var game = WildhavenGameManager.Instance;
            if (game == null) return;
            if (inventoryText != null)
                inventoryText.text = $"Berries: {game.GetResource(ResourceType.Berries)}   Wood: {game.GetResource(ResourceType.Wood)}   Stone: {game.GetResource(ResourceType.Stone)}";
            if (shelterText != null)
                shelterText.text = $"Shelter Lv. {game.ShelterLevel}";
        }
    }
}