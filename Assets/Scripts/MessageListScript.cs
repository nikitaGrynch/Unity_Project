using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MessageListScript : MonoBehaviour
{
   public static TMPro.TextMeshProUGUI messageListText;

   private void Start()
   {
         messageListText = GameObject.Find("MessagesListBlockText").GetComponent<TMPro.TextMeshProUGUI>();
         messageListText.enabled = false;
         GameState.SubscribeOn(nameof(GameState.messages), PrintMessages);
   }
   
   public void PrintMessages()
   {
       messageListText.enabled = GameState.messages.Count > 0;
       messageListText.text = String.Join("\n", GameState.messages);
   }
   
   private void OnDestroy()
   {
       GameState.UnsubscribeOn(nameof(GameState.messages), PrintMessages);
   }
}
