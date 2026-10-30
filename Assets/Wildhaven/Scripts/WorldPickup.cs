using UnityEngine;

namespace Wildhaven
{
    public enum ResourceType { Berries, Wood, Stone, FreshWater }

    public sealed class WorldPickup : MonoBehaviour
    {
        [SerializeField] private ResourceType resourceType;
        [SerializeField, Min(1)] private int amount = 1;
        [SerializeField] private float spinSpeed = 45f;

        private void Update() => transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (WildhavenGameManager.Instance != null)
                WildhavenGameManager.Instance.AddResource(resourceType, amount);
            Destroy(gameObject);
        }
    }
}