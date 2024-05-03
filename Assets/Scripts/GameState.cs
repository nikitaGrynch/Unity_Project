using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static bool IsNight = false;
    private static int maxMessages = 5;
    private static float _characterWater;
    public static float CharacterWater {
        get => _characterWater;
        set
        {
            if(value <= 0.2f && _characterWater > 0.2f)
            {
                SendMessage("You are thirsty");
            }
            else if(value > _characterWater)
            {
                SendMessage("You drank water");
            }
            _characterWater = value;
        }
    }
    
    public static List<String> messages = new List<String>();
    
    public static void SendMessage(String message)
    {
        if (messages.Count >= maxMessages)
        {
            messages.RemoveAt(0);
        }
        messages.Add(message);
        NotifySubscribers(nameof(messages));
    }

    private static Dictionary<string, List<Action>> subscribers = new Dictionary<string, List<Action>>();

    public static void SubscribeOn(String propertyName, Action action)
    {

        if (!subscribers.ContainsKey(propertyName))
        {
            subscribers[propertyName] = new();
        }
        subscribers[propertyName].Add(action);
    }

    public static void UnsubscribeOn(String propertyName, Action action)
    {

        if (!subscribers.ContainsKey(propertyName))
        {
            subscribers[propertyName].Remove(action);
        }
    }

    private static void NotifySubscribers(string propertyName)
    {
        if (subscribers.ContainsKey(propertyName)){
            subscribers[propertyName].ForEach(action => action());
        }
    }
    
}
