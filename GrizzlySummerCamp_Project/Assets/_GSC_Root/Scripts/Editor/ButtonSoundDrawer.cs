using UnityEngine;
using UnityEditor;
/*
    Este script es para asignar los nombres que queramos a los "Elementos" del script ButtonClickSound.
    La verdad, no sabría explicarlo al 100%
 
 */


[CustomPropertyDrawer(typeof(ButtonClickSound.ButtonSound))]
public class ButtonSoundDrawer : PropertyDrawer
                                 //Funciona distindo al MonoBehaviour, sirve para "costumizar" propiedades, desde este archivos se "arrastran" los nuevos valores con los que se mostrarán
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Busca los valores a referenciar, "button" y "soundkey"
        SerializedProperty buttonProp = property.FindPropertyRelative("button");
        SerializedProperty soundKeyProp = property.FindPropertyRelative("soundKey");


        string buttonName = buttonProp.objectReferenceValue != null
                            ? buttonProp.objectReferenceValue.name
                            : "Sin botón asignado";

        string soundName = string.IsNullOrEmpty(soundKeyProp.stringValue)
                            ? "Sin clave asignada"
                            : soundKeyProp.stringValue;

        //Esto es el texto que se mostrará, "Button: " + "El nombre que se asigne anteriormente" + " | Sound: " + "El nombre de la clave que se haya asignado", si no tiene valor asignado se muestra el escrito en este código.
        label.text = $"Button: {buttonName}  |  Sound: {soundName}";

        EditorGUI.PropertyField(position, property, new GUIContent(label.text), true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }
}
