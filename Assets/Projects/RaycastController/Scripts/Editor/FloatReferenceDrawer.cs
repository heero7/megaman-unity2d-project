using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(FloatReference))]
public class FloatReferenceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        bool useConstant = property.FindPropertyRelative("UseConstant").boolValue;


        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var rect = new Rect(position.position, Vector2.one * 20);
        var texture =  EditorGUIUtility.IconContent("animationdopesheetkeyframe");
        var guiContent =  new GUIContent(texture);
        var guiStyle = new GUIStyle
        {
            fixedWidth = 50f,
            border = new RectOffset(1,1,1,1)
        };
        if (EditorGUI.DropdownButton(rect, guiContent, FocusType.Keyboard, guiStyle))
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Constant"), useConstant, () => SetProperty(property, true));
            menu.AddItem(new GUIContent("Variable"), !useConstant, () => SetProperty(property, false));
            menu.ShowAsContext();
        }

        position.position += Vector2.right * 15;
        var value = property.FindPropertyRelative("ConstantValue").floatValue;

        if (useConstant)
        {
            var newValue = EditorGUI.TextField(position, value.ToString());
            float.TryParse(newValue, out value);
            property.FindPropertyRelative("ConstantValue").floatValue = value;
        }
        else
        {
            EditorGUI.ObjectField(position, property.FindPropertyRelative("Variable"), GUIContent.none);
        }

        EditorGUI.EndProperty();
    }

    private void SetProperty(SerializedProperty property, bool v)
    {
        var propertyRelative = property.FindPropertyRelative("UseConstant");
        propertyRelative.boolValue = v;
        property.serializedObject.ApplyModifiedProperties();
    }
}