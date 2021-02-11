using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class Order : MonoBehaviour
{
    private Action<object[]> Action { set; get; }
    private List<IEnumerator<object []>> Coroutines { set; get; } = new List<IEnumerator<object []>>();
    
    public string Name { private set; get; }
    
    public enum OrderState { EXECUTED, PAUSED, PROCESSING, ABORTED, QUEUEING }
    public OrderState State { private set; get; } = OrderState.QUEUEING;
    
    public Order(Action<object[]> method, string _name)
    {
        Action = method;
        if (_name != "") Name = _name;
        else Name = "Безымянный приказ";
    }

    public static bool OverlapInList(List<Order> list, string method)
    {
        foreach (Order order in list)
        {
            if (order.Name == method) return true;
            else continue;
        }
        return false;
    }
    public void AddCoroutine(IEnumerator<object []> coroutine)
    {
        Coroutines.Add(coroutine);
    }

    public void Execute()
    {
        State = OrderState.PROCESSING;
        Action.Invoke(Action.Method.GetParameters());
    }
    public void Complete()
    {
        State = OrderState.EXECUTED;
    }
    public void Pause()
    {
        State = OrderState.PAUSED;
    }
    public void Abort()
    {
        State = OrderState.ABORTED;
        for (int i = 0; i < Coroutines.Count; i++)
        {
            StopCoroutine(Coroutines[i]);
        }
    }
}