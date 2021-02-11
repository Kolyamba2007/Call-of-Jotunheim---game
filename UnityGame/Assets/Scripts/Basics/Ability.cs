using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[Serializable]
public class Ability
{
    public enum AbilityType { TARGET, NONTARGET, POINT, PASSIVE }
    public enum AbilityTarget { HERO, UNIT, CASTER, HEROANDUNIT }
    public AbilityType Type = AbilityType.NONTARGET;
    public AbilityTarget TargetType;
    public string Name;
    public string Description;
    public float Cooldown = 0;
    public byte Range = 0;
    public SerializedSprite Icon;
    public Action Action;

    public Ability(string _name)
    {
        Name = _name;
    }

    public void SetType(AbilityType _type)
    {
        Type = _type;
    }
    public void SetCooldown(float _value)
    {
        Cooldown = _value;
    }
    public Texture2D GetIcon()
    {
        if (Icon != null)
        {
            if (Icon.ptr != IntPtr.Zero)
            {
                Texture2D convert = Texture2D.CreateExternalTexture(Icon.width, Icon.height, TextureFormat.ARGB32, false, false, Icon.ptr);
                return convert;
            }
            else return Texture2D.grayTexture;
        }
        else return Texture2D.grayTexture;
    }
    public void Cast()
    {
        if (Action != null && Action.Method != null) Action.Method.Invoke();
    }
}