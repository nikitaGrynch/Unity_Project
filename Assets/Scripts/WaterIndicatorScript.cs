using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WaterIndicatorScript : MonoBehaviour
{
    private Image indicator;
    
    void Start()
    {
        indicator = GameObject.Find("Indicator").GetComponent<Image>();
    }
    void Update()
    {
        indicator.fillAmount = GameState.CharacterWater;
    }
}
