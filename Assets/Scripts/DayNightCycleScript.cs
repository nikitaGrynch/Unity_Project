using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycleScript : MonoBehaviour
{
    private float dayPeriod = 20f;
    private float nightFactor = 0.25f;
    private float dayPhase;

    [SerializeField]
    private Light sun;
    [SerializeField]
    private Light moon;
    [SerializeField]
    private Material daySkybox;
    [SerializeField]
    private Material nightSkybox;

    // Start is called before the first frame update
    void Start()
    {
        dayPhase = 0f;
        RenderSettings.skybox = daySkybox;
    }

    // Update is called once per frame
    void Update()
    {
        dayPhase += Time.deltaTime / dayPeriod;
        if(dayPhase > 1f)
        {
            dayPhase -= 1f;
        }
        GameState.IsNight = dayPhase > 0.25f && dayPhase < 0.75f;
        if (GameState.IsNight)
        {
            if(RenderSettings.skybox != nightSkybox)
            {
                RenderSettings.skybox = nightSkybox;
            }
        }
        else
        {
            if (RenderSettings.skybox != daySkybox)
            {
                RenderSettings.skybox = daySkybox;
            }
        }
        float luxFactor = LuxFactor(dayPhase);

        sun.intensity = GameState.IsNight ? 0f : luxFactor;
        moon.intensity = GameState.IsNight ? nightFactor * luxFactor : 0f;

        RenderSettings.skybox.SetFloat("_Exposure", luxFactor);
        RenderSettings.ambientIntensity = GameState.IsNight ? nightFactor * luxFactor : luxFactor;
        this.transform.eulerAngles = new Vector3(0, 0, 360f * dayPhase);
        RenderSettings.skybox.SetFloat("_Rotation", 360f * dayPhase);
    }

    private float LuxFactor(float t)
    {
        return 0.5f * (1f + Mathf.Cos(4f * Mathf.PI * t));
    }
}
