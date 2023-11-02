using Level.DynamicDifficulty;
using UnityEditor;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(DifficultySetting))]
public class DifficultySettingsDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        return base.CreatePropertyGUI(property);
    }
}
