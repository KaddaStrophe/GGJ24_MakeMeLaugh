using System;
using System.Collections.Generic;
using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace CatsInCostumes {
    [CreateAssetMenu]
    sealed class CharacterAsset : ScriptableObject {
        static CharacterAsset m_empty;
        internal static CharacterAsset empty {
            get {
                if (!m_empty) {
                    m_empty = CreateInstance<CharacterAsset>();
                }

                return m_empty;
            }
        }

        internal static CharacterAsset GetAssetById(string id) {
            if (IsNarrator(id)) {
                return null;
            }

            return GameManager.TryGetCharacter(id, out var character)
                ? character
                : null;
        }

        internal static bool IsNarrator(string id) {
            return string.IsNullOrEmpty(id) || id == "-";
        }

        [SerializeField]
        SerializableKeyValuePairs<Mood, Sprite> sprites = new();

        internal bool TryGetSprite(Mood mood, out Sprite sprite) => sprites.TryGetValue(mood, out sprite) && sprite;

#if UNITY_EDITOR
        [ContextMenu(nameof(OnValidate))]
        void OnValidate() {
            var defaultSprite = sprites.GetValueOrDefault(Mood.Neutral);

            var newSprites = Enum
                .GetValues(typeof(Mood))
                .OfType<Mood>()
                .ToDictionary(m => m, m => sprites.TryGetValue(m, out var sprite) && sprite ? sprite : defaultSprite);

            if (!Enumerable.SequenceEqual(sprites.Values, newSprites.Values)) {
                sprites.SetItems(newSprites);
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
            }
        }
#endif
    }
}
