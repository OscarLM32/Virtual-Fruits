using UnityEngine;

namespace Level.DynamicDifficulty
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelSector : MonoBehaviour
    {
        public DifficultySetting[] difficultySettings;
    }
}