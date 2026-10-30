using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Wildhaven
{
    // Runtime setup menu; uses the current placeholder avatar until production models are added.
    public sealed class CharacterSelectionUI : MonoBehaviour
    {
        private CharacterCustomizer customizer;
        private GameObject panel;
        private InputField nameInput;
        private Text status;

        private void Start()
        {
            customizer = FindObjectOfType<CharacterCustomizer>();
            if (FindObjectOfType<EventSystem>() == null)
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            BuildUI();
            if (customizer != null)
            {
                customizer.SetCharacterName(PlayerPrefs.GetString("Wildhaven.CharacterName", "Alex"));
                customizer.SetAvatarStyle((AvatarStyle)PlayerPrefs.GetInt("Wildhaven.AvatarStyle", 0));
                customizer.SetOutfitStyle((OutfitStyle)PlayerPrefs.GetInt("Wildhaven.OutfitStyle", 0));
            }
            nameInput.text = PlayerPrefs.GetString("Wildhaven.CharacterName", "Alex");
        }

        private void BuildUI()
        {
            var canvasObject = new GameObject("Character Selection Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);

            panel = MakePanel(canvasObject.transform);
            MakeLabel(panel.transform, "WILDHAVEN • CHARACTER SETUP", 30, new Vector2(0.5f, 0.88f), new Vector2(0.9f, 0.12f));
            nameInput = MakeInput(panel.transform, new Vector2(0.5f, 0.70f));
            MakeButton(panel.transform, "Explorer", new Vector2(0.30f, 0.53f), () => SelectStyle(0));
            MakeButton(panel.transform, "Adventurer", new Vector2(0.70f, 0.53f), () => SelectStyle(1));
            MakeButton(panel.transform, "Forest", new Vector2(0.25f, 0.36f), () => SelectOutfit(0));
            MakeButton(panel.transform, "Ocean", new Vector2(0.50f, 0.36f), () => SelectOutfit(1));
            MakeButton(panel.transform, "Sunset", new Vector2(0.75f, 0.36f), () => SelectOutfit(2));
            MakeButton(panel.transform, "SAVE & PLAY", new Vector2(0.5f, 0.16f), SaveAndClose);
            status = MakeLabel(panel.transform, "Choose a style and outfit.", 20, new Vector2(0.5f, 0.04f), new Vector2(0.9f, 0.08f));
        }

        private void SelectStyle(int value)
        {
            if (customizer != null) customizer.SetAvatarStyle(value == 0 ? AvatarStyle.Explorer : AvatarStyle.Adventurer);
            PlayerPrefs.SetInt("Wildhaven.AvatarStyle", value);
            status.text = value == 0 ? "Explorer style selected." : "Adventurer style selected.";
        }

        private void SelectOutfit(int value)
        {
            if (customizer != null) customizer.SetOutfitStyle((OutfitStyle)Mathf.Clamp(value, 0, 2));
            PlayerPrefs.SetInt("Wildhaven.OutfitStyle", value);
            status.text = "Outfit preset selected.";
        }

        private void SaveAndClose()
        {
            string chosen = string.IsNullOrWhiteSpace(nameInput.text) ? "Alex" : nameInput.text.Trim();
            if (customizer != null) customizer.SetCharacterName(chosen);
            PlayerPrefs.SetString("Wildhaven.CharacterName", chosen);
            PlayerPrefs.Save();
            panel.SetActive(false);
        }

        private GameObject MakePanel(Transform parent)
        {
            var go = new GameObject("Character Setup Panel", typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(930, 920);
            go.GetComponent<Image>().color = new Color(0.04f, 0.09f, 0.12f, 0.94f);
            return go;
        }

        private Text MakeLabel(Transform parent, string value, int fontSize, Vector2 anchor, Vector2 size)
        {
            var go = new GameObject("Label", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = anchor;
            rt.sizeDelta = new Vector2(size.x * 900, size.y * 900);
            var text = go.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.text = value;
            text.fontSize = fontSize;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;
            return text;
        }

        private InputField MakeInput(Transform parent, Vector2 anchor)
        {
            var root = new GameObject("Name Input", typeof(RectTransform), typeof(Image), typeof(InputField));
            root.transform.SetParent(parent, false);
            var rt = root.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = anchor;
            rt.sizeDelta = new Vector2(600, 76);
            root.GetComponent<Image>().color = Color.white;
            var text = MakeLabel(root.transform, "Alex", 24, new Vector2(0.5f, 0.5f), new Vector2(0.95f, 0.9f));
            text.color = Color.black;
            var input = root.GetComponent<InputField>();
            input.textComponent = text;
            input.targetGraphic = root.GetComponent<Image>();
            input.text = "Alex";
            return input;
        }

        private void MakeButton(Transform parent, string caption, Vector2 anchor, UnityEngine.Events.UnityAction action)
        {
            var go = new GameObject(caption, typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = anchor;
            rt.sizeDelta = new Vector2(250, 86);
            go.GetComponent<Image>().color = new Color(0.12f, 0.38f, 0.34f, 1f);
            go.GetComponent<Button>().onClick.AddListener(action);
            var label = MakeLabel(go.transform, caption, 22, new Vector2(0.5f, 0.5f), new Vector2(0.95f, 0.9f));
            label.raycastTarget = false;
        }
    }
}