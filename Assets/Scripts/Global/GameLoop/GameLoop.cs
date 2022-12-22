using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace GameJam.Global
{
    public class GameLoop : MonoBehaviour
    {
        private GameObject currentLevelInstance;

        private void Start() => ToMenu();

        public IEnumerator Win()
        {
            yield return StartCoroutine(FadeTo(Color.black, 0.5f));
            yield return new WaitForSeconds(0.5f);
            ToMenu();
            GlobalReferences.Player.battery.Charge(Mathf.Infinity);
            yield return FadeTo(Color.white, 0.25f);
        }

        public IEnumerator GameOver()
        {
            Time.timeScale = 0.5f;
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(FadeTo(Color.black, 1f));
            yield return new WaitForSeconds(1f);
            ToMenu();
            FindObjectOfType<Volume>().sharedProfile.TryGet<ColorAdjustments>(out var colorAdjustments); 
            colorAdjustments.saturation.value = 0;
            GlobalReferences.Player.battery.Charge(Mathf.Infinity);
            Time.timeScale = 1f;
            yield return StartCoroutine(FadeTo(Color.white, 0.5f));
        }

        public IEnumerator Pause()
        {
            yield return StartCoroutine(FadeTo(Color.black, 0.5f));
            GlobalReferences.Player.transform.position = GlobalReferences.StartPoint.position;
            ToMenu();
            yield return StartCoroutine(FadeTo(Color.white, 0.25f));
            yield return new WaitForSeconds(1f);
        }

        public void Resume()
        {
            
        }

        public void StartLevel(Level level)
        {
            if(currentLevelInstance != null)
                Destroy(currentLevelInstance);
            currentLevelInstance = Instantiate(level.levelPrefab);
            FromMenu();
        }
        
        private IEnumerator FadeTo(Color color, float length)
        {
            FindObjectOfType<Volume>().sharedProfile.TryGet(out ColorAdjustments colorAdjustments);
            var t = 0f;
            var startColor = colorAdjustments.colorFilter.value;
            while (t < length)
            {
                t += Time.deltaTime;
                colorAdjustments.colorFilter.value = Color.Lerp(startColor, color, t / length);
                yield return null;
            }
        }

        private void ToMenu()
        {
            GlobalReferences.Player.SetRoomScaleMovement(GlobalReferences.StartPoint);
            GlobalReferences.Player.passiveDrain = 0;
            currentLevelInstance?.SetActive(false);
        }

        private void FromMenu()
        {
            GlobalReferences.Player.SetRoomScaleMovement(null);
            GlobalReferences.Player.passiveDrain = 0.5f;
            currentLevelInstance?.SetActive(true);
        }
    }

}
