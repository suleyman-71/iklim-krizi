using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField, Header("Managed Objects")] 
    private Light DirectionalLight = null;

    [SerializeField] private LightPreset DayNightPreset, LampPreset;
    private List<Light> SpotLights = new List<Light>();

    [SerializeField, Range(0, 1440), Header("Modifiers"), Tooltip("The game's current time of day")] 
    private float TimeOfDay;

    [SerializeField, Tooltip("Angle to rotate the sun")] 
    private float SunDirection = 90f;

    [SerializeField, Tooltip("How fast time will go")] 
    private float TimeMultiplier = 25;

    [SerializeField] private bool ControlLights = true;

    private const float inverseDayLength = 1f / 1440f;

    private void Start()
    {
        if (ControlLights)
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach (Light li in lights)
            {
                switch (li.type)
                {
                    case LightType.Disc:
                    case LightType.Point:
                    case LightType.Rectangle:
                    case LightType.Spot:
                        SpotLights.Add(li);
                        break;
                    case LightType.Directional:
                    default:
                        break;
                }
            }
        }
    }

    private void Update()
    {
        if (DayNightPreset == null)
            return;

        TimeOfDay = TimeOfDay + (Time.deltaTime * TimeMultiplier);
        TimeOfDay = TimeOfDay % 1440;
        UpdateLighting(TimeOfDay * inverseDayLength);
    }

    /// <param name="timePercent"></param>
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = DayNightPreset.AmbientColour.Evaluate(timePercent);
        RenderSettings.fogColor = DayNightPreset.FogColour.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            if (DirectionalLight.enabled == true)
            {
                DirectionalLight.color = DayNightPreset.DirectionalColour.Evaluate(timePercent);
                DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, SunDirection, 0));
            }
        }

        foreach (Light lamp in SpotLights)
        {
            if (lamp != null)
            {
                if (lamp.isActiveAndEnabled && lamp.shadows != LightShadows.None && LampPreset != null)
                {
                    lamp.color = LampPreset.DirectionalColour.Evaluate(timePercent);
                }
            }
        }
    }
}