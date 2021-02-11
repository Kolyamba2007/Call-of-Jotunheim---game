using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class Action
{
    public enum ActionTier1Type { HEALING, DESTRUCTION, BUFF, DEBUFF, CREATION, PROTECTION }
    public ActionTier1Type ActionType;

    public float Value = 0;
    public float Time = 0;

    public object Target;
    public System.Action Method;

    public void Cast()
    {
        Method.Invoke();
    }
}

[Serializable]
public class HealingAction : Action
{
    public enum Healing { INSTANCE, PERIODIC }
    public Healing HealingType;
    public HealingAction(byte value)
    {        
        ActionType = ActionTier1Type.HEALING;
        HealingType = Healing.INSTANCE;
        Value = value;
        Method = () =>
        {
            Unit dummy = new Unit();
            dummy.Heal(dummy, (byte)Value);
        };
    }
    public HealingAction(byte value, float time)
    {
        ActionType = ActionTier1Type.HEALING;
        HealingType = Healing.PERIODIC;
        Value = value;
        Time = time;
    }
}

[Serializable]
public class DestructionAction : Action
{
    public enum Destruction { INSTANCE, PERIODIC }
    public Destruction DestructionType;

    System.Action<Unit> DestructionMethod;

    public DestructionAction(byte value, Ability.AbilityTarget target)
    {        
        ActionType = ActionTier1Type.DESTRUCTION;
        DestructionType = Destruction.INSTANCE;
        Value = value;
        Target = target;
    }
    public DestructionAction(byte value, float time, Ability.AbilityTarget target)
    {
        ActionType = ActionTier1Type.DESTRUCTION;
        DestructionType = Destruction.PERIODIC;
        Value = value;
        Time = time;
        Target = target;
    }
}

[Serializable]
public class BuffAction : Action
{
    public enum Buff { HEALTH, MANA, DAMAGE, ATTACKRANGE, SPEED }
    public Buff BuffType;

    public BuffAction(byte value, Buff buffType, Ability.AbilityTarget target)
    {
        ActionType = ActionTier1Type.BUFF;
        BuffType = buffType;
        Value = value;
        Target = target;
    }
}

[Serializable]
public class DebuffAction : Action
{
    public enum Debuff { HEALTH, MANA, DAMAGE, ATTACKRANGE, SPEED }
    public Debuff DebuffType;

    public DebuffAction(byte value, Debuff debuffType, Ability.AbilityTarget target)
    {
        ActionType = ActionTier1Type.BUFF;
        DebuffType = debuffType;
        Value = value;
        Target = target;
    }
}

[Serializable]
public class CreationAction : Action
{
    public enum Creation { MAGIC }
    public Creation CreationType;

    public CreationAction(int index)
    {
        CreationType = (Creation)index;
        ActionType = ActionTier1Type.CREATION;
    }
}

[Serializable]
public class ProtectionAction : Action
{
    public enum Protection { TELEPORT, ABSORPTION }
    public Protection ProtectionType;

    public ProtectionAction(int index)
    {
        ProtectionType = (Protection)index;
        ActionType = ActionTier1Type.PROTECTION;
    }
}



