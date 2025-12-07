using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ButtonClickSound.ButtonSoundGroup))]
public class ButtonSoundGroupDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty foldoutProp = property.FindPropertyRelative("foldout");
        SerializedProperty soundsProp = property.FindPropertyRelative("sounds");

        float height = EditorGUIUtility.singleLineHeight + 4;

        if (foldoutProp.boolValue)
        {
            height += EditorGUIUtility.singleLineHeight + 4; // groupName field
            height += EditorGUI.GetPropertyHeight(soundsProp, true);
        }

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty groupNameProp = property.FindPropertyRelative("groupName");
        SerializedProperty soundsProp = property.FindPropertyRelative("sounds");
        SerializedProperty foldoutProp = property.FindPropertyRelative("foldout");

        Rect foldoutRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        foldoutProp.boolValue = EditorGUI.Foldout(foldoutRect, foldoutProp.boolValue, groupNameProp.stringValue);

        if (foldoutProp.boolValue)
        {
            Rect nameRect = new(position.x, position.y + 20, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(nameRect, groupNameProp);

            Rect listRect = new(position.x, position.y + 45, position.width, EditorGUI.GetPropertyHeight(soundsProp, true));
            EditorGUI.PropertyField(listRect, soundsProp, true);
        }
    }
}
