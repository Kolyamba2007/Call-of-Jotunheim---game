using System.Collections;
using System.Collections.Generic;
using System.Numerics;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(Platform))]
public class PlatofrmLayout : Editor
{
    Platform Target;

    public void OnEnable()
    {
        Target = (Platform)target;
    }
    public override void OnInspectorGUI()
    {       
        GUIStyle header = new GUIStyle();
        header.fontStyle = FontStyle.Bold;

        GUILayout.Label("Маршрут платформы", header);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Тип маршрута");
        Target.Orbit = (Platform.OrbitType)EditorGUILayout.EnumPopup(Target.Orbit);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        switch (Target.Orbit)
        {
            case Platform.OrbitType.LINE:
                if (!Application.isPlaying)
                {
                    GUILayout.Label("Перемещение", header);
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Скорость");
                    Target.MovementSpeed = EditorGUILayout.Slider(Target.MovementSpeed, 0, 5);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                    Target.Points.Clear();
                    switch (Target.LineOrbit)
                    {
                        case Platform.LineOrbitType.HORIZONTAL:
                            Target.Points.Add(Target.transform.position + UnityEngine.Vector3.left * Target.LineOrbitDistance);
                            Target.Points.Add(Target.transform.position + UnityEngine.Vector3.right * Target.LineOrbitDistance);
                            break;
                        case Platform.LineOrbitType.VERTICAL:
                            Target.Points.Add(Target.transform.position + UnityEngine.Vector3.up * Target.LineOrbitDistance);
                            Target.Points.Add(Target.transform.position + UnityEngine.Vector3.down * Target.LineOrbitDistance);
                            break;
                    }
                }
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Траектория");
                Target.LineOrbit = (Platform.LineOrbitType)EditorGUILayout.EnumPopup(Target.LineOrbit);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Дистанция");
                Target.LineOrbitDistance = EditorGUILayout.FloatField(Target.LineOrbitDistance);
                EditorGUILayout.EndHorizontal();
                break;
            case Platform.OrbitType.ELLIPSE:
                GUILayout.Label("Перемещение", header);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Скорость");
                Target.MovementSpeed = EditorGUILayout.Slider(Target.MovementSpeed, 0, 0.2f);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Движение");
                Target.OrbitOrientation = (Platform.OrbitOrientationType)EditorGUILayout.EnumPopup(Target.OrbitOrientation);
                EditorGUILayout.EndHorizontal();

                Target.Points.Clear();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Большой диаметр");
                Target.EllipseBigRadius = EditorGUILayout.Slider(Target.EllipseBigRadius, 0, 30);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Малый диаметр");
                Target.EllipseSmallRadius = EditorGUILayout.Slider(Target.EllipseSmallRadius, 0, 30);
                EditorGUILayout.EndHorizontal();
                break;
            case Platform.OrbitType.RECTANGLE:
                GUILayout.Label("Перемещение", header);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Скорость");
                Target.MovementSpeed = EditorGUILayout.Slider(Target.MovementSpeed, 0, 5);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Движение");
                Target.OrbitOrientation = (Platform.OrbitOrientationType)EditorGUILayout.EnumPopup(Target.OrbitOrientation);
                EditorGUILayout.EndHorizontal();

                if (!Application.isPlaying)
                {
                    Target.Points.Clear();
                    Target.Points.Add(Target.transform.position);
                    Target.Points.Add(Target.transform.position + UnityEngine.Vector3.right * Target.RectangleBigSize);
                    Target.Points.Add(Target.transform.position + UnityEngine.Vector3.right * Target.RectangleBigSize + UnityEngine.Vector3.down * Target.RectangleSmallSize);
                    Target.Points.Add(Target.transform.position + UnityEngine.Vector3.down * Target.RectangleSmallSize);
                }
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Большая сторона");
                Target.RectangleBigSize = EditorGUILayout.FloatField(Target.RectangleBigSize);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Малая сторона");
                Target.RectangleSmallSize = EditorGUILayout.FloatField(Target.RectangleSmallSize);
                EditorGUILayout.EndHorizontal();
                break;
            case Platform.OrbitType.SQUARE:
                GUILayout.Label("Перемещение", header);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Скорость");
                Target.MovementSpeed = EditorGUILayout.Slider(Target.MovementSpeed, 0, 5);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Движение");
                Target.OrbitOrientation = (Platform.OrbitOrientationType)EditorGUILayout.EnumPopup(Target.OrbitOrientation);
                EditorGUILayout.EndHorizontal();

                if (!Application.isPlaying)
                {
                    Target.Points.Clear();
                    Target.Points.Add(Target.transform.position);
                    Target.Points.Add(Target.transform.position + UnityEngine.Vector3.right * Target.SquareSize);
                    Target.Points.Add(Target.transform.position + UnityEngine.Vector3.right * Target.SquareSize + UnityEngine.Vector3.down * Target.SquareSize);
                    Target.Points.Add(Target.transform.position + UnityEngine.Vector3.down * Target.SquareSize);
                }
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Сторона квадрата");
                Target.SquareSize = EditorGUILayout.FloatField(Target.SquareSize);
                EditorGUILayout.EndHorizontal();
                break;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(Target.gameObject);
            EditorSceneManager.MarkSceneDirty(Target.gameObject.scene);
        }
    }
}
#endif
