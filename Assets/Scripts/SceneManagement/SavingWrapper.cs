using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;


namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {

        const string defaultSaveFile = "save";

        [SerializeField] float fadeInTime = 0.2f;

        IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmidiate();

            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save()
        {
            // Saving to /Users/stefan.dudasko/Library/Application Support/DefaultCompany/Pro Benders/save.sav
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
    }

}