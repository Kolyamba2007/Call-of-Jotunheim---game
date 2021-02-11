using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Item
{
    public enum ItemType { ACTIVE, PASSIVE }
    public enum ItemCharacteristic { PERMANENT, CONSUMABLE }
    public ItemCharacteristic Characteristic = ItemCharacteristic.PERMANENT;
    public string Name;
    public string Description;
    public byte Charges;
    public ItemType Type;
    public float Cooldown;
    public float Probability;
    public SerializedSprite Icon;
    public Action Action;
    public Item(string _name)
    {
        Name = _name;
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
    public void Use()
    {
        
    }
}
