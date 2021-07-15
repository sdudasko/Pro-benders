using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{

    public class Fader : MonoBehaviour
    {
        CanvasGroup cvGroup;
        Coroutine currentlyActiveFade = null;

        private void Awake()
        {
            cvGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmidiate()
        {
            cvGroup.alpha = 1;
        }

        public Coroutine FadeOut(float time = 2f)
        {
            return Fade(1, time);
        }

        public Coroutine FadeIn(float time = 2f)
        {
            return Fade(0, time);
        }

        public Coroutine Fade(float target, float time)
        {
            if (currentlyActiveFade != null)
            {
                StopCoroutine(currentlyActiveFade);
            }
            currentlyActiveFade = StartCoroutine(FadeRoutine(target, time));

            return currentlyActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(cvGroup.alpha, target))
            {
                cvGroup.alpha = Mathf.MoveTowards(cvGroup.alpha, target, Time.deltaTime / time);

                yield return null;
            }
        }
    }

}