using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CraftRecipe))]
public class CraftingRecioe_CustomEditor : Editor
{
    private SerializedProperty recipeItems; // Array Recipe Items 
    private SerializedProperty outputItem;
    private SerializedProperty outputStack;

    private void OnEnable()
    {
      recipeItems = serializedObject.FindProperty("recipeItems");
      outputItem = serializedObject.FindProperty("outputItem");
      outputStack = serializedObject.FindProperty("outputStack");
      
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //Display 3x3 Grid
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Recipe Items", EditorStyles.boldLabel, GUILayout.Width(80));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);

        DisplayGrid(recipeItems, 3);

        EditorGUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        DisplayItemDetails(outputItem);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Output Stack", EditorStyles.boldLabel, GUILayout.Width(80));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.Space(0);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.PropertyField(outputStack, GUIContent.none, GUILayout.Width(80));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();


        
    }

    private void DisplayGrid(SerializedProperty array, int gridSize)
    {
        int arraySize = array.arraySize;

        for ( int row = 0; row < gridSize; row++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            for (int col = 0; col < gridSize; col ++)
            {
                int index = row * gridSize + col;
                if(index < arraySize)
                {
                    EditorGUILayout.BeginVertical();
                    SerializedProperty itemProperty = array.GetArrayElementAtIndex(index);
                    DisplayItemDetails(itemProperty, index);
                    //Display
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space(10);
                }
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
        }
    }
    private void DisplayItemDetails(SerializedProperty itemProperty, int index = -1)
    {
        ItemSO item = itemProperty.objectReferenceValue as ItemSO;
        EditorGUILayout.BeginVertical(GUILayout.Width(80));
        if(index != -1)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Width(80));
            EditorGUILayout.LabelField("Index : " + index, EditorStyles.boldLabel, GUILayout.Width(80));
            EditorGUILayout.EndHorizontal();
        }
        Rect spriteRect = GUILayoutUtility.GetRect(80, 80);
        if(item != null)
        {
            EditorGUI.DrawTextureTransparent(spriteRect, item.icon.texture, ScaleMode.ScaleToFit);
        }
        else
        {
            EditorGUI.DrawRect(spriteRect, new Color(0, 0, 0, 0.1f));
        }
        EditorGUILayout.PropertyField(itemProperty, GUIContent.none, GUILayout.Width(80));
        EditorGUILayout.EndVertical();
    }

}
