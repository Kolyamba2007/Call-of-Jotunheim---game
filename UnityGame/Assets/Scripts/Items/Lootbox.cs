using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using UnityEngine;
using Utilities;

[ExecuteAlways]
[Serializable]
public class Lootbox : MonoBehaviour
{
    public List<Item> Items = new List<Item>();

    public bool ItemsFoldout;
    public float FoldoutHeight = 100;

    public Animator Animator;

    public void OnEnable()
    {
        if (Items.Count > 0)
        {
            DataObject data = GameData.Load(GameData.DataBasePath);
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Icon = data.Items[i].Icon;
            }
        }
    }

    public void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            Item random = GetRandomLoot();
            Debug.Log("Испытание № " + (i + 1) + " Выпавший предмет: " + random.Name + " Вероятность: " +  random.Probability);
        }
    }

    public Item GetRandomLoot()
    {
        RandomNumberGenerator.Create();
        double random = UnityEngine.Random.Range(0.0000f, 1.0000f);
        float Pmax = 0;
        int Imax = 0;
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Probability > Pmax)
            {
                Pmax = Items[i].Probability;
                Imax = i;
            }
        }

        for (int i = 0; i < Items.Count; i++)
        {
            if (random < Items[i].Probability) return Items[i];
        }
        return Items[Imax];
    }
}
