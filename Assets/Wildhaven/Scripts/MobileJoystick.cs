using UnityEngine;
using UnityEngine.EventSystems;

namespace Wildhaven
{
    public sealed class MobileJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform handle;
        [SerializeField, Range(0.1f, 1f)] private float handleRange = 0.65f;
        public Vector2 Value { get; private set; }

        public void Configure(RectTransform baseRect, RectTransform handleRect)
        {
            background = baseRect;
            handle = handleRect;
        }

        public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

        public void OnDrag(PointerEventData eventData)
        {
            if (background == null || handle == null) return;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background, eventData.position, eventData.pressEventCamera, out var localPoint)) return;

            var half = background.rect.size * 0.5f;
            var normalized = new Vector2(half.x > 0 ? localPoint.x / half.x : 0,
                half.y > 0 ? localPoint.y / half.y : 0);
            Value = Vector2.ClampMagnitude(normalized, 1f);
            handle.anchoredPosition = new Vector2(Value.x * half.x * handleRange, Value.y * half.y * handleRange);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Value = Vector2.zero;
            if (handle != null) handle.anchoredPosition = Vector2.zero;
        }
    }
}