using UnityEngine;

namespace Wildhaven
{
    /// <summary>
    /// Lightweight day/night lighting cycle. Add to a scene's Directional Light.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class DayNightCycle : MonoBehaviour
    {
        [Header("Cycle")]
        [SerializeField, Min(60f)] private float fullDayLengthSeconds = 600f;
        [SerializeField, Range(0f, 1f)] private float startingTime = 0.28f;

        [Header("Lighting")]
        [SerializeField] private Gradient lightColor = new Gradient();
        [SerializeField] private AnimationCurve lightIntensity = AnimationCurve.EaseInOut(0f, 0.05f, 1f, 1f);
        [SerializeField, Min(0f)] private float maxIntensity = 1.2f;

        [Header("Optional sky")]
        [SerializeField] private Material skyboxMaterial;

        public float TimeOfDay { get; private set; }

        private void Awake()
        {
            TimeOfDay = startingTime;
            if (lightColor.colorKeys.Length == 0)
            {
                lightColor = new Gradient();
                lightColor.SetKeys(
                    new[] {
                        new GradientColorKey(new Color(0.08f, 0.12f, 0.28f), 0f),
                        new GradientColorKey(new Color(1f, 0.55f, 0.28f), 0.23f),
                        new GradientColorKey(new Color(1f, 0.95f, 0.82f), 0.5f),
                        new GradientColorKey(new Color(1f, 0.48f, 0.22f), 0.77f),
                        new GradientColorKey(new Color(0.08f, 0.12f, 0.28f), 1f)
                    },
                    new[] {
                        new GradientAlphaKey(1f, 0f),
                        new GradientAlphaKey(1f, 1f)
                    });
            }
        }

        private void Update()
        {
            TimeOfDay = Mathf.Repeat(TimeOfDay + Time.deltaTime / fullDayLengthSeconds, 1f);
            float sunAngle = TimeOfDay * 360f - 90f;
            transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

            Light sun = GetComponent<Light>();
            if (sun != null)
            {
                sun.color = lightColor.Evaluate(TimeOfDay);
                sun.intensity = Mathf.Clamp01(lightIntensity.Evaluate(TimeOfDay)) * maxIntensity;
            }

            if (skyboxMaterial != null && skyboxMaterial.HasProperty("_Rotation"))
                skyboxMaterial.SetFloat("_Rotation", TimeOfDay * 360f);
    }
}
