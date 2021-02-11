using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AnimatedValues;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(Unit))]
public class UnitLayout : Editor
{
    Unit Target;

    SerializedObject soTarget;
    SerializedProperty Paused;
    SerializedProperty Commandable;
    SerializedProperty Movable;
    SerializedProperty Invulnerable;
    SerializedProperty IsDead;
    SerializedProperty Stunned;
    SerializedProperty Silenced;
    SerializedProperty CanJump;
    SerializedProperty CanSlide;

    SerializedProperty OnGround;
    SerializedProperty OnWall;

    SerializedProperty Health;
    SerializedProperty MaxHealth;
    SerializedProperty Mana;
    SerializedProperty MaxMana;

    SerializedProperty Type;
    SerializedProperty AttackDamage;
    SerializedProperty AttackRange;
    SerializedProperty AttackCooldown;

    SerializedProperty MovementSpeed;
    SerializedProperty JumpScale;

    SerializedProperty DoubleJumpsCount;
    SerializedProperty DoubleJumpsMax;

    SerializedProperty Animator;

    SerializedProperty RigidbodyCollider;

    SerializedProperty Queue;

    bool AddAbility;
    Vector2 AddAbilityScrollPos;
    Vector2 UnitAbilitiesScrollPos;

    AnimBool AbilitiesFoldoutValue;
    bool UpdateAbility;
    AnimBool UpdateFoldoutValue;
    const int ItemHeight = 50;
    public void OnEnable()
    {
        Target = (Unit)target;
        soTarget = serializedObject;

        AbilitiesFoldoutValue = new AnimBool(false);
        AbilitiesFoldoutValue.valueChanged.AddListener(Repaint);
    }
    public override void OnInspectorGUI()
    {
        soTarget.Update();

        Paused = soTarget.FindProperty("Paused");
        Commandable = soTarget.FindProperty("Commandable");
        Invulnerable = soTarget.FindProperty("Invulnerable");
        IsDead = soTarget.FindProperty("IsDead");
        Stunned = soTarget.FindProperty("Stunned");
        Movable = soTarget.FindProperty("Movable");
        Silenced = soTarget.FindProperty("Silenced");
        CanJump = soTarget.FindProperty("CanJump");
        CanSlide = soTarget.FindProperty("CanSlide");
        OnGround = soTarget.FindProperty("OnGround");
        OnWall = soTarget.FindProperty("OnWall");
        Health = soTarget.FindProperty("Health");
        MaxHealth = soTarget.FindProperty("MaxHealth");
        Mana = soTarget.FindProperty("Mana");
        MaxMana = soTarget.FindProperty("MaxMana");
        Type = soTarget.FindProperty("Type");
        AttackDamage = soTarget.FindProperty("AttackDamage");
        AttackRange = soTarget.FindProperty("AttackRange");
        AttackCooldown = soTarget.FindProperty("AttackCooldown");
        MovementSpeed = soTarget.FindProperty("MovementSpeed");
        JumpScale = soTarget.FindProperty("JumpScale");
        DoubleJumpsMax = soTarget.FindProperty("DoubleJumpsMax");
        Animator = soTarget.FindProperty("Animator");
        DoubleJumpsMax = soTarget.FindProperty("DoubleJumpsMax");
        RigidbodyCollider = soTarget.FindProperty("RigidbodyCollider");
        Queue = soTarget.FindProperty("Queue");

        EditorGUILayout.PropertyField(Paused, new GUIContent("Остановлен"));
        EditorGUILayout.PropertyField(Commandable, new GUIContent("Управляем"));
        EditorGUILayout.PropertyField(Movable, new GUIContent("Может двигаться"));
        EditorGUILayout.PropertyField(Invulnerable, new GUIContent("Неуязвимый"));
        EditorGUILayout.PropertyField(IsDead, new GUIContent("Мёртв"));
        EditorGUILayout.PropertyField(Stunned, new GUIContent("Оглушён"));
        EditorGUILayout.PropertyField(Silenced, new GUIContent("Безмолвен"));
        EditorGUILayout.PropertyField(CanJump, new GUIContent("Может прыгать"));
        EditorGUILayout.PropertyField(CanSlide, new GUIContent("Может сползать"));
        EditorGUILayout.PropertyField(OnGround, new GUIContent("На земле"));
        EditorGUILayout.PropertyField(OnWall, new GUIContent("На стене"));
        EditorGUILayout.PropertyField(Health, new GUIContent("Здоровье"));
        EditorGUILayout.PropertyField(MaxHealth, new GUIContent("Макс. здоровье"));
        EditorGUILayout.PropertyField(Mana, new GUIContent("Мана"));
        EditorGUILayout.PropertyField(MaxMana, new GUIContent("Макс. мана"));
        EditorGUILayout.PropertyField(Type, new GUIContent("Тип боя"));
        EditorGUILayout.PropertyField(AttackDamage, new GUIContent("Урон"));
        if (Target.Type == Unit.UnitType.RANGE) EditorGUILayout.PropertyField(AttackRange, new GUIContent("Дальность атаки"));
        EditorGUILayout.PropertyField(AttackCooldown, new GUIContent("Задержка удара"));
        EditorGUILayout.PropertyField(MovementSpeed, new GUIContent("Скорость"));
        if (Target.CanJump)
        {
            EditorGUILayout.PropertyField(JumpScale, new GUIContent("Коэфф. прыжка"));
            EditorGUILayout.PropertyField(DoubleJumpsMax, new GUIContent("Кол-во доп. прыжков"));
        }
        EditorGUILayout.PropertyField(Animator, new GUIContent("Аниматор"));
        EditorGUILayout.PropertyField(RigidbodyCollider, new GUIContent("Коллайдер"));
        EditorGUILayout.PropertyField(Queue, new GUIContent("Очередь приказов"));
        if (Application.isPlaying)
        {
            if (Target.Queue.Count > 0)
            {
                for (int i = 0; i < Target.Queue.Count; i++)
                {
                    EditorGUILayout.LabelField("[" + (i + 1) + "]     Приказ:    " + Target.Queue[i].Name + "     Статус:     " + Target.Queue[i].State.ToString());
                }
            }
        }

        GUIStyle itemStyle = new GUIStyle();
        itemStyle.alignment = TextAnchor.MiddleCenter;
        itemStyle.normal.textColor = Color.black;

        GUIStyle labelStyle = new GUIStyle();
        labelStyle.normal.background = Texture2D.grayTexture;
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.normal.textColor = Color.white;

        GUIStyle textStyle = new GUIStyle();
        textStyle.normal.textColor = Color.black;
        textStyle.fontStyle = FontStyle.Bold;
        textStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.Space(20);
        Target.AbilitiesFoldout = EditorGUILayout.Foldout(Target.AbilitiesFoldout, "Способности");
        AbilitiesFoldoutValue.target = Target.AbilitiesFoldout;

        EditorGUI.BeginChangeCheck();

        if (EditorGUILayout.BeginFadeGroup(AbilitiesFoldoutValue.faded))
        {
            if (Target.Abilities.Count > 0)
            {
                AddAbilityScrollPos = EditorGUILayout.BeginScrollView(AddAbilityScrollPos, GUILayout.Height(200));
                for (int i = 0; i < Target.Abilities.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("[" + (i + 1) + "]", GUILayout.Width(20), GUILayout.Height(ItemHeight));

                    GUIStyle style = new GUIStyle();
                    style.normal.background = Target.Abilities[i].GetIcon();
                    style.alignment = TextAnchor.MiddleCenter;
                    EditorGUILayout.LabelField("", style, GUILayout.Width(ItemHeight), GUILayout.Height(ItemHeight));
                    EditorGUILayout.LabelField(Target.Abilities[i].Name, itemStyle, GUILayout.Width(120), GUILayout.Height(ItemHeight));
                    EditorGUILayout.LabelField("Перезарядка: ", itemStyle, GUILayout.Width(90), GUILayout.Height(ItemHeight));
                    EditorGUILayout.LabelField(Target.Abilities[i].Cooldown.ToString() + " сек.", itemStyle, GUILayout.Width(50), GUILayout.Height(ItemHeight));

                    if (GUILayout.Button("Удалить", GUILayout.Height(ItemHeight)))
                    {
                        Undo.RecordObject(target, "Remove Ability");
                        Target.Abilities.RemoveAt(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
            }
            else
            {
                EditorGUILayout.LabelField("У этого юнита нет способностей", labelStyle, GUILayout.Height(ItemHeight));
            }

            if (GUILayout.Button("Добавить способность")) AddAbility = !AddAbility;

            if (AddAbility)
            {
                DataObject data = GameData.Load(GameData.DataBasePath);
                EditorGUILayout.Space(20);
                EditorGUILayout.BeginVertical();
                if (data.Abilities.Count > 0)
                {
                    EditorGUILayout.LabelField("Выберите способность", textStyle);
                    UnitAbilitiesScrollPos = EditorGUILayout.BeginScrollView(UnitAbilitiesScrollPos, GUILayout.Height(200));
                    for (int i = 0; i < data.Abilities.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("[" + (i + 1) + "]", GUILayout.Width(20), GUILayout.Height(ItemHeight));
                        GUIStyle style = new GUIStyle();
                        style.normal.background = data.Abilities[i].GetIcon();
                        EditorGUILayout.LabelField("", style, GUILayout.Width(ItemHeight), GUILayout.Height(ItemHeight));
                        if (GUILayout.Button(data.Abilities[i].Name, GUILayout.Width(150), GUILayout.Height(ItemHeight)))
                        {
                            Undo.RecordObject(target, "Add Ability");
                            Target.Abilities.Add(data.Abilities[i]);
                            switch (data.Abilities[i].TargetType)
                            {
                                case Ability.AbilityTarget.CASTER:
                                    Debug.Log(Target.Abilities[Target.Abilities.Count - 1].Name);
                                    Target.Abilities[Target.Abilities.Count - 1].Action.Target = Target;
                                    Target.Abilities[Target.Abilities.Count - 1].Action.Method = () =>
                                    {
                                        Target.Heal(Target, 10);
                                    };
                                    Debug.Log(Target.Abilities[Target.Abilities.Count - 1].Action.Target.ToString());
                                    break;
                                case Ability.AbilityTarget.HERO:                                    
                                    
                                    break;
                                case Ability.AbilityTarget.UNIT:

                                    break;
                                case Ability.AbilityTarget.HEROANDUNIT:
                                    
                                    break;
                            }
                            
                        }

                        switch (data.Abilities[i].TargetType)
                        {
                            case Ability.AbilityTarget.CASTER:
                                EditorGUILayout.LabelField("Цель: Заклинатель", itemStyle, GUILayout.Width(100), GUILayout.Height(ItemHeight));
                                break;
                            case Ability.AbilityTarget.HERO:
                                EditorGUILayout.LabelField("Цель: Герой", itemStyle, GUILayout.Width(100), GUILayout.Height(ItemHeight));
                                break;
                            case Ability.AbilityTarget.UNIT:
                                EditorGUILayout.LabelField("Цель: Юнит", itemStyle, GUILayout.Width(100), GUILayout.Height(ItemHeight));
                                break;
                            case Ability.AbilityTarget.HEROANDUNIT:
                                EditorGUILayout.LabelField("Цель: Юнит и Герой", itemStyle, GUILayout.Width(100), GUILayout.Height(ItemHeight));
                                break;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndScrollView();
                }
                else EditorGUILayout.HelpBox("В базе данных отсутствуют способности.", MessageType.Warning);
                EditorGUILayout.EndVertical();
            }
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUI.EndChangeCheck();

        soTarget.ApplyModifiedProperties();
    }
}
#endif