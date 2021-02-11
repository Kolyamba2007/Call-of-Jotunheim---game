using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AnimatedValues;
#endif
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[CustomEditor(typeof(Lootbox))]
public class Lootboxyout : Editor
{
    Lootbox Target;
    AnimBool itemsFoldoutValue;
    Vector2 LootBoxScrollPos;
    Vector2 AddItemScrollPos;

    bool AddItem;

    const int ItemHeight = 50;
    private void OnEnable()
    {
        Target = (Lootbox)target;
        itemsFoldoutValue = new AnimBool(false);
        itemsFoldoutValue.valueChanged.AddListener(Repaint);
    }

    public override void OnInspectorGUI()
    {
        GUIStyle itemStyle = new GUIStyle();
        itemStyle.alignment = TextAnchor.MiddleLeft;
        itemStyle.normal.textColor = Color.black;

        GUIStyle labelStyle = new GUIStyle();
        labelStyle.normal.background = Texture2D.grayTexture;
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.normal.textColor = Color.white;

        GUIStyle textStyle = new GUIStyle();
        textStyle.normal.textColor = Color.black;
        textStyle.fontStyle = FontStyle.Bold;
        textStyle.alignment = TextAnchor.MiddleCenter;

        Target.ItemsFoldout = EditorGUILayout.Foldout(Target.ItemsFoldout, "Предметы");
        itemsFoldoutValue.target = Target.ItemsFoldout;

        EditorGUI.BeginChangeCheck();

        if (EditorGUILayout.BeginFadeGroup(itemsFoldoutValue.faded))
        {
            if (Target.Items.Count > 0)
            {
                AddItemScrollPos = EditorGUILayout.BeginScrollView(AddItemScrollPos, GUILayout.Height(200));
                for (int i = 0; i < Target.Items.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("[" + i + "]", GUILayout.Width(20), GUILayout.Height(ItemHeight));

                    GUIStyle style = new GUIStyle();
                    style.normal.background = Target.Items[i].GetIcon();
                    EditorGUILayout.LabelField("", style, GUILayout.Width(ItemHeight), GUILayout.Height(ItemHeight));
                    EditorGUILayout.LabelField(Target.Items[i].Name, GUILayout.Width(120), GUILayout.Height(ItemHeight));
                    EditorGUILayout.LabelField("Вероятность: " + Target.Items[i].Probability, GUILayout.Width(120), GUILayout.Height(ItemHeight));
                    if (GUILayout.Button("Изменить", GUILayout.Height(ItemHeight)))
                    {
                        
                    }
                    if (GUILayout.Button("Удалить", GUILayout.Height(ItemHeight)))
                    {
                        Undo.RecordObject(target, "Remove Item");
                        Target.Items.RemoveAt(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
            }
            else
            {
                EditorGUILayout.LabelField("Сундук пуст", labelStyle, GUILayout.Height(ItemHeight));
            }

            if (GUILayout.Button("Добавить предмет")) AddItem = !AddItem;

            if (AddItem)
            {
                DataObject data = GameData.Load(GameData.DataBasePath);
                EditorGUILayout.Space(20);
                EditorGUILayout.BeginVertical();
                if (data.Items.Count > 0)
                {
                    EditorGUILayout.LabelField("Выберите предмет", textStyle);
                    LootBoxScrollPos = EditorGUILayout.BeginScrollView(LootBoxScrollPos, GUILayout.Height(200));
                    for (int i = 0; i < data.Items.Count; i++)
                    {                        
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("[" + i + "]", GUILayout.Width(20), GUILayout.Height(ItemHeight));                        
                        GUIStyle style = new GUIStyle();
                        style.normal.background = data.Items[i].GetIcon();
                        EditorGUILayout.LabelField("", style, GUILayout.Width(ItemHeight), GUILayout.Height(ItemHeight));
                        if (GUILayout.Button(data.Items[i].Name, GUILayout.Width(150), GUILayout.Height(ItemHeight)))
                        {
                            Undo.RecordObject(target, "Add Item");
                            Target.Items.Add(data.Items[i]);
                        }
                        EditorGUILayout.LabelField("Вероятность: " + data.Items[i].Probability, GUILayout.Height(ItemHeight));
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndScrollView();
                }
                else EditorGUILayout.HelpBox("В базе данных отсутствуют предметы. Для продолжения добавьте предмет в Tools/Game Data.", MessageType.Warning);              
                EditorGUILayout.EndVertical();
            }
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUI.EndChangeCheck();
    }
}
#endif