using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Row))]
public class LevelScriptable : PropertyDrawer
{
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty coloum1 = property.FindPropertyRelative("Coloum1");
        SerializedProperty coloum2 = property.FindPropertyRelative("Coloum2");
        SerializedProperty coloum3 = property.FindPropertyRelative("Coloum3");
        SerializedProperty coloum4 = property.FindPropertyRelative("Coloum4");
        SerializedProperty coloum5 = property.FindPropertyRelative("Coloum5");
        SerializedProperty coloum6 = property.FindPropertyRelative("Coloum6");
        SerializedProperty coloum7 = property.FindPropertyRelative("Coloum7");
        SerializedProperty coloum8 = property.FindPropertyRelative("Coloum8");
        SerializedProperty coloum9 = property.FindPropertyRelative("Coloum9");

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        float widthSize = position.width / 9;

        Rect pos1 = new Rect(position.x, position.y, widthSize, position.height);
        Rect pos2 = new Rect(position.x + widthSize, position.y, widthSize, position.height);
        Rect pos3 = new Rect(position.x + widthSize*2, position.y, widthSize, position.height);
        Rect pos4 = new Rect(position.x + widthSize*3, position.y, widthSize, position.height);
        Rect pos5 = new Rect(position.x + widthSize*4, position.y, widthSize, position.height);
        Rect pos6 = new Rect(position.x + widthSize*5, position.y, widthSize, position.height);
        Rect pos7 = new Rect(position.x + widthSize*6, position.y, widthSize, position.height);
        Rect pos8 = new Rect(position.x + widthSize*7, position.y, widthSize, position.height);
        Rect pos9 = new Rect(position.x + widthSize*8, position.y, widthSize, position.height);

        EditorGUI.PropertyField(pos1, coloum1, GUIContent.none);
        EditorGUI.PropertyField(pos2, coloum2, GUIContent.none);
        EditorGUI.PropertyField(pos3, coloum3, GUIContent.none);
        EditorGUI.PropertyField(pos4, coloum4, GUIContent.none);
        EditorGUI.PropertyField(pos5, coloum5, GUIContent.none);
        EditorGUI.PropertyField(pos6, coloum6, GUIContent.none);
        EditorGUI.PropertyField(pos7, coloum7, GUIContent.none);
        EditorGUI.PropertyField(pos8, coloum8, GUIContent.none);
        EditorGUI.PropertyField(pos9, coloum9, GUIContent.none);

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}