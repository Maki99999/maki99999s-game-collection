using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace artgame
{
    public class WeatherController : MonoBehaviour
    {

        public static WeatherController weatherController;

        public Light mainSun;
        public Light[] sideSuns;

        public Light[] lamps;

        public Material skyMaterial;
        public Material[] areaMaterials;

        [Space(20)]

        public GameObject rain;
        public GameObject ThunderPrefab;

        [Space(20)]

        public float thunderTimeMin;
        public float thunderTimeMax;
        float nextThunder;

        [Space(20)]

        public WeatherType currentWeather = WeatherType.RAIN;

        [Range(0, 1)] public float CustomBrightnessSun;
        [Range(0, 1)] public float CustomBrightnessLamp;
        [Range(0, 1)] public float CustomBrightnessSky;
        [Range(0, 1)] public float CustomBrightnessGround;

        Transform playerTransform = null;

        private void Start()
        {
            nextThunder = Random.Range(thunderTimeMin, thunderTimeMax);
            SetWeather(currentWeather);
            weatherController = this;
        }

        private void Update()
        {
            if (currentWeather == WeatherType.CUSTOM)
            {
                UpdateValues(CustomBrightnessSun, CustomBrightnessLamp, CustomBrightnessSky, CustomBrightnessGround);
            }

            if (currentWeather == WeatherType.RAIN && Time.time >= nextThunder)
            {
                nextThunder = Time.time + Random.Range(thunderTimeMin, thunderTimeMax);
                Instantiate(ThunderPrefab, playerTransform.position + new Vector3(Random.Range(-30f, 30f), 30f, Random.Range(-30f, 30f)), Quaternion.identity, rain.transform);
            }

            if (playerTransform == null)
                playerTransform = PlayerController.playerController.transform;

            transform.position = new Vector3(playerTransform.position.x, 0f, playerTransform.position.z) + playerTransform.forward * 3.5f;
        }

        void UpdateValues(float brightnessSun, float brightnessLamp, float brightnessSky, float brightnessGround)
        {
            skyMaterial.SetColor("_Color", new Color(brightnessSky, brightnessSky, brightnessSky));
            RenderSettings.fogColor = new Color(brightnessSky, brightnessSky, brightnessSky);
            foreach (Material material in areaMaterials)
            {
                material.SetColor("_Color", new Color(brightnessGround, brightnessGround, brightnessGround));
            }
            foreach (Light lamp in lamps)
            {
                lamp.intensity = 8 * brightnessLamp;
            }
            mainSun.intensity = 8 * brightnessSun;

            foreach (Light sun in sideSuns)
            {
                sun.intensity = 4 * brightnessSun;
            }
        }

        private void SetWeather(WeatherType newWeather)
        {

            switch (newWeather)
            {
                case WeatherType.DAY:
                    rain.SetActive(false);
                    UpdateValues(0.5f, 0f, 1f, 1f);
                    break;

                case WeatherType.NIGHT:
                    rain.SetActive(false);
                    UpdateValues(0f, 0.5f, 0f, 0f);
                    break;

                case WeatherType.RAIN:
                    rain.SetActive(true);
                    nextThunder = Time.time + Random.Range(thunderTimeMin, thunderTimeMax);
                    UpdateValues(0f, 0.35f, 0.05f, 0.068f);
                    break;

                case WeatherType.CUSTOM:
                    break;

                default:
                    break;
            }

            currentWeather = newWeather;
        }

        public void SetDay()
        {
            SetWeather(WeatherType.DAY);
        }

        public void SetNight()
        {
            SetWeather(WeatherType.NIGHT);
        }

        public void SetRain()
        {
            SetWeather(WeatherType.RAIN);
        }

        public IEnumerator Lightning()
        {
            UpdateValues(0.5f, 0f, 1f, 1f);
            yield return new WaitForSeconds(0.2f);

            UpdateValues(0f, 0.5f, 0.1f, 0.14f);
            yield return new WaitForSeconds(0.3f);

            UpdateValues(0.125f, 0f, 0.5f, 0.2f);
            yield return new WaitForSeconds(0.1f);

            if (currentWeather != WeatherType.RAIN)
                SetWeather(currentWeather);
            else
                UpdateValues(0f, 0.35f, 0.05f, 0.068f);
        }

        public enum WeatherType
        {
            DAY,
            NIGHT,
            RAIN,
            CUSTOM
        }
    }
}

