using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{

    public class Fader : MonoBehaviour
    {
        CanvasGroup cvGroup;

        private void Start()
        {
            cvGroup = GetComponent<CanvasGroup>();

            StartCoroutine(FadeOutIn());
        }

        IEnumerator FadeOutIn()
        {
            yield return FadeOut(2);
            yield return FadeIn(2);
        }

        public IEnumerator FadeOut(float time)
        {
            while (cvGroup.alpha < 1)
            {
                cvGroup.alpha += Time.deltaTime / time;

                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (cvGroup.alpha > 0)
            {
                cvGroup.alpha -= Time.deltaTime / time;

                yield return null;
            }
        }
    }

}