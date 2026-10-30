using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Wildhaven
{
    /// <summary>Creates a simple touch joystick and nearby shelter interaction button at runtime.</summary>
    public sealed class MobileControlsUI : MonoBehaviour
    {
        private MobileJoystick joystick;

        private void Start()
        {
            if (FindObjectOfType<EventSystem>() == null)
                new GameObject("Touch Event System", typeof(EventSystem), typeof(StandaloneInputModule));

            var canvasObject = new GameObject("Mobile Controls Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            var baseObject = MakeUIObject("Joystick Base", canvasObject.transform, new Vector2(0, 0), new Vector2(0, 0), new Vector2(210, 210));
            baseObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.22f);
            var handleObject = MakeUIObject("Joystick Handle", baseObject.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(90, 90));
            handleObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.7f);
            joystick = baseObject.AddComponent<MobileJoystick>();
            joystick.Configure(baseObject.GetComponent<RectTransform>(), handleObject.GetComponent<RectTransform>());

            var motor = FindObjectOfType<PlayerMotor>();
            if (motor != null) motor.SetJoystick(joystick);

            var action = MakeButton(canvasObject.transform, "UPGRADE", new Vector2(1, 0), new Vector2(1, 0), new Vector2(190, 82));
            action.onClick.AddListener(TryNearbyUpgrade);
        }

        private void TryNearbyUpgrade()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;
            ShelterStation nearest = null;
            float best = 4.5f;
            foreach (var station in FindObjectsOfType<ShelterStation>())
            {
                float distance = Vector3.Distance(player.transform.position, station.transform.position);
                if (distance < best) { best = distance; nearest = station; }
            }
            if (nearest != null) nearest.TryUpgrade();
        }

        private GameObject MakeUIObject(string title, Transform parent, Vector2 anchor, Vector2 pivot, Vector2 size)
        {
            var go = new GameObject(title, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = anchor;
            rt.pivot = pivot;
            rt.anchoredPosition = new Vector2(125, 125);
            rt.sizeDelta = size;
            return go;
        }

        private Button MakeButton(Transform parent, string title, Vector2 anchor, Vector2 pivot, Vector2 size)
        {
            var go = new GameObject(title + " Button", typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = anchor;
            rt.pivot = pivot;
            rt.anchoredPosition = new Vector2(-135, 135);
            rt.sizeDelta = size;
            go.GetComponent<Image>().color = new Color(0.12f, 0.38f, 0.34f, 0.88f);
            var button = go.GetComponent<Button>();
            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            labelObject.transform.SetParent(go.transform, false);
            var label = labelObject.GetComponent<Text>();
            label.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            label.text = title;
            label.alignment = TextAnchor.MiddleCenter;
            label.fontSize = 22;
            label.color = Color.white;
            var labelRect = label.rectTransform;
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = labelRect.offsetMax = Vector2.zero;
            return button;
        }
    }
}