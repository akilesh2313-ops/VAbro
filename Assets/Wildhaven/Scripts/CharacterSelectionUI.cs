using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Wildhaven
{
    /// <summary>Runtime character setup panel. Uses the existing placeholder avatar until model assets are added.</summary>
    public sealed class CharacterSelectionUI : MonoBehaviour
    {
        [SerializeField] private CharacterCustomizer target;
        [SerializeField] private GameObject panel;
        [SerializeField] private InputField nameField;
        [SerializeField] private Text statusText;

        private void Start()
        {
            if (target == null) target = FindObjectOfType<CharacterCustomizer>();
            if (nameField != null) nameField.text = PlayerPrefs.GetString("Wildhaven.CharacterName", "Alex");
            if (target != null)
            {
                target.SetCharacterName(PlayerPrefs.GetString("Wildhaven.CharacterName", "Alex"));
                target.SetAvatarStyle((AvatarStyle)PlayerPrefs.GetInt("Wildhaven.AvatarStyle", 0));
                target.SetOutfitStyle((OutfitStyle)PlayerPrefs.GetInt("Wildhaven.OutfitStyle", 0));
            }
            SetStatus("Choose your explorer and outfit.");
        }

        public void SelectAvatar(int value)
        {
            if (target == null) return;
            target.SetAvatarStyle(value == 0 ? AvatarStyle.Male : AvatarStyle.Female);
            PlayerPrefs.SetInt("Wildhaven.AvatarStyle", value == 0 ? 0 : 1);
            PlayerPrefs.Save();
            SetStatus(value == 0 ? "Male explorer selected." : "Female explorer selected.");
        }

        public void SelectOutfit(int value)
        {
            if (target == null) return;
            int safeValue = Mathf.Clamp(value, 0, 2);
            target.SetOutfitStyle((OutfitStyle)safeValue);
            PlayerPrefs.SetInt("Wildhaven.OutfitStyle", safeValue);
            PlayerPrefs.Save();
            SetStatus("Outfit preset updated.");
        }

        public void SaveAndClose()
        {
            if (target != null && nameField != null)
            {
                string cleanName = string.IsNullOrWhiteSpace(nameField.text) ? "Alex" : nameField.text.Trim();
                target.SetCharacterName(cleanName);
                PlayerPrefs.SetString("Wildhaven.CharacterName", target.CharacterName);
                PlayerPrefs.Save();
            }
            if (panel != null) panel.SetActive(false);
            SetStatus("Character setup saved.");
        }

        public void Open() { if (panel != null) panel.SetActive(true); }

        private void SetStatus(string value)
        {
            if (statusText != null) statusText.text = value;
        }
    }
}