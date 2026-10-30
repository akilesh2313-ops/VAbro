using UnityEngine;

namespace Wildhaven
{
    public enum AvatarStyle { Explorer, Adventurer }
    public enum OutfitStyle { Forest, Ocean, Sunset }

    public sealed class CharacterCustomizer : MonoBehaviour
    {
        [SerializeField] private Renderer[] outfitRenderers;
        [SerializeField] private AvatarStyle avatarStyle;
        [SerializeField] private OutfitStyle outfitStyle;
        [SerializeField] private string characterName = "Alex";

        public string CharacterName => characterName;
        public AvatarStyle CurrentAvatarStyle => avatarStyle;
        public OutfitStyle CurrentOutfitStyle => outfitStyle;

        private void Start() => ApplyOutfit();

        public void SetCharacterName(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                characterName = value.Trim().Substring(0, Mathf.Min(18, value.Trim().Length));
        }

        public void SetAvatarStyle(AvatarStyle value) => avatarStyle = value;

        public void SetOutfitStyle(OutfitStyle value)
        {
            outfitStyle = value;
            ApplyOutfit();
        }

        private void ApplyOutfit()
        {
            Color color = outfitStyle switch
            {
                OutfitStyle.Forest => new Color(0.18f, 0.38f, 0.22f),
                OutfitStyle.Ocean => new Color(0.12f, 0.32f, 0.58f),
                _ => new Color(0.72f, 0.36f, 0.16f)
            };

            if (outfitRenderers == null) return;
            foreach (var item in outfitRenderers)
            {
                if (item == null) continue;
                // Use a property block so shared materials are not modified.
                var block = new MaterialPropertyBlock();
                item.GetPropertyBlock(block);
                block.SetColor("_Color", color);
                block.SetColor("_BaseColor", color);
                item.SetPropertyBlock(block);
            }
        }
    }
}