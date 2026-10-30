using UnityEngine;

namespace Wildhaven
{
    public enum CharacterType { Explorer, Adventurer }

    [CreateAssetMenu(menuName = "Wildhaven/Character Profile")]
    public sealed class CharacterProfile : ScriptableObject
    {
        public string displayName = "Alex";
        public CharacterType characterType;
        public GameObject characterPrefab;
        public Sprite portrait;
    }
}