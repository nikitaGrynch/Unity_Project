using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellScript : MonoBehaviour
{
    private Collider otherCollider;
    private GameObject actionDescriptionBlock;
    private TMPro.TextMeshProUGUI actionDescriptionMessageUI;
    private bool isPlayerNear = false;
    void Start()
    {
        actionDescriptionBlock = GameObject.Find("ActionDescriptionBlock");
        actionDescriptionMessageUI = GameObject.Find("ActionDescriptionBlockText").GetComponent<TMPro.TextMeshProUGUI>();
        actionDescriptionBlock.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F) && isPlayerNear)
        {
            DrinkWater();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        isPlayerNear = true;
        actionDescriptionMessageUI.text = "Press [F] to drink water";
        actionDescriptionBlock.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        isPlayerNear = false;
        actionDescriptionBlock.SetActive(false);
    }
    private void DrinkWater()
    {
        GameState.CharacterWater = 1.0f;
    }
}
