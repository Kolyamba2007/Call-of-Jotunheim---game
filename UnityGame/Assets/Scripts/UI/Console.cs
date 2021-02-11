using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    private static string str;

    private static string command, argument, value1, value2;
    public InputField Command, Chat;
    private static int index;
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            str = Command.text;
            Chat.text += str + Environment.NewLine;

            if (str.Length > 0 && str != " ")
            {
                if (str.Substring(0, 1) == "-")
                {
                    str = str.Remove(0, 1);
                    command = Extract();
                    argument = Extract();
                    value1 = Extract();
                    value2 = Extract();

                    Process(command, argument, value1, value2);
                    Debug.Log("COMMAND = [" + command + "]");
                    Debug.Log("ARGUMENT = [" + argument + "]");
                    Debug.Log("VALUE1 = [" + value1 + "]");
                    Debug.Log("VALUE2 = [" + value2 + "]");
                }
                else
                {
                    Message("Ошибка: служебная команда должна начинаться со спецсимволов '-'.");
                }

                Command.text = "";
                Chat.Select();
            }
        }
    }

    private string Extract()
    {
        string command = "";
        if (str.Length > 0)
        {
            if (str.IndexOf(' ') > 0)
            {
                command = str.Substring(0, str.IndexOf(' '));
                str = str.Remove(0, str.IndexOf(' ') + 1);
            }
            else
            {
                command = str.Substring(0, str.Length);
                str = str.Remove(0, str.Length);
            }

        }
        return command;
    }
    private void Message(string _message)
    {
        Chat.text += _message + Environment.NewLine;
    }

    private const string ADD = "add";
    private const string CREATE = "create";
    private const string REMOVE = "remove";
    private const string SET = "set";
    private const string CLEAR = "clear";
    private const string HELP = "help";
    private void Process(string _message, string _argument, string _value1, string _value2)
    {
        switch (_message)
        {
            case ADD:

                break;
            case CREATE:
                
                break;            
            case SET:
                SetFunction(_argument, _value1, _value2);
                break;
            case CLEAR:
                Chat.text = "";
                break;
            case HELP:
                Message("Список команд:");
                Message("   [1] -add");
                Message("   [2] -create");
                Message("   [3] -remove");
                Message("   [4] -set");
                Message("   [5] -clear");
                Message("   [6] -help");
                break;
        }
    }


    private const string HEALTH = "health";
    private const string MAXHEALTH = "maxhealth";
    private const string MANA = "mana";
    private const string MAXMANA = "maxmana";
    private const string SPEED = "speed";
    private void SetFunction(string _argument, string _value1, string _value2)
    {
        if (_value1 != "")
        {
            switch (_argument)
            {
                case HEALTH:
                    if (Unit.GetUnitByName(_value1))
                    {
                        Unit.GetUnitByName(_value1).Health = System.Convert.ToByte(_value2);
                        Message("Увеличено здоровье у " + _value1 + " стало " + _value2);
                    }
                    else Message("Ошибка: юнита " + _value1 + " не существует.");
                    break;
                case MAXHEALTH:
                    if (Unit.GetUnitByName(_value1))
                    {
                        Unit.GetUnitByName(_value1).MaxHealth = System.Convert.ToByte(_value2);
                        Message("Увеличено макс. здоровье у " + _value1 + " стало " + _value2);
                    }
                    else Message("Ошибка: юнита " + _value1 + " не существует.");
                    break;
                case MANA:
                    if (Unit.GetUnitByName(_value1))
                    {
                        Unit.GetUnitByName(_value1).Mana = System.Convert.ToByte(_value2);
                        Message("Увеличена мана у " + _value1 + " стало " + _value2);
                    }
                    else Message("Ошибка: юнита " + _value1 + " не существует.");
                    break;
                case MAXMANA:
                    if (Unit.GetUnitByName(_value1))
                    {
                        Unit.GetUnitByName(_value1).MaxMana = System.Convert.ToByte(_value2);
                        Message("Увеличена макс. мана у " + _value1 + " стало " + _value2);
                    }
                    else Message("Ошибка: юнита " + _value1 + " не существует.");
                    break;
                case SPEED:
                    if (Unit.GetUnitByName(_value1))
                    {
                        Unit.GetUnitByName(_value1).MovementSpeed = System.Convert.ToByte(_value2);
                        Message("Увеличена скорость у " + _value1 + " стало " + _value2);
                    }
                    else Message("Ошибка: юнита " + _value1 + " не существует.");
                    break;
            }
        }
        else Message("Отсутствует аргумент у команды -set, либо аргумент не корректен.");
    }
}
