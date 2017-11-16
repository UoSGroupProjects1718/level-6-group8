using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This is an editor script which allows us to see and edit a level layout
/// within the Unity editor.
/// </summary>
//[CustomPropertyDrawer(typeof(LevelData))]
//public class CustomLevelModule : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        EditorGUI.PrefixLabel(position, label);

//        Rect newPosition = position;
//        newPosition.y += 18f;

//        SerializedProperty levelHeight = property.FindPropertyRelative("levelHeight");
//        SerializedProperty levelWidth = property.FindPropertyRelative("levelWidth");

//        EditorGUI.IntField(new Rect(newPosition.x, newPosition.y, 25, 25), levelHeight.intValue);
//        newPosition.x += 20;
//        EditorGUI.IntField(new Rect(newPosition.x, newPosition.y, 25, 25), levelWidth.intValue);
//        newPosition.x = position.x;
//        newPosition.y += 20;

//        if (levelHeight.intValue > 0 && levelWidth.intValue > 0)
//        {
//            SerializedProperty rows = property.FindPropertyRelative("rows");

//            for (int i = 0; i < 5; i++)
//            {
//                SerializedProperty row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("row");
//                newPosition.height = 20;

//                if (row.arraySize != 10) row.arraySize = 10;

//                newPosition.width = 70;

//                for (int j = 0; j < 5; j++)
//                {
//                    EditorGUI.PropertyField(newPosition, row.GetArrayElementAtIndex(j), GUIContent.none);
//                    newPosition.x += 15;
//                }

//                newPosition.x = position.x;
//                newPosition.y += 20;
//            }
//        }
//    }

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        return 20 * 12;
//    }
//}
