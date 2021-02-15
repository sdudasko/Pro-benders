using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{

    public class Fader : MonoBehaviour
    {
        CanvasGroup cvGroup;

        private void Awake()
        {
            cvGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmidiate()
        {
            cvGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time = 2f)
        {
            while (cvGroup.alpha < 1)
            {
                cvGroup.alpha += Time.deltaTime / time;

                yield return null;
            }
        }

        public IEnumerator FadeIn(float time = 2f)
        {
            while (cvGroup.alpha > 0)
            {
                cvGroup.alpha -= Time.deltaTime / time;

                yield return null;
            }
        }
    }

}