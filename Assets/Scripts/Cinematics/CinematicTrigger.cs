using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool alreadyPlayed = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!alreadyPlayed && other.gameObject.tag == "Player") {
                GetComponent<PlayableDirector>().Play();
                alreadyPlayed = !alreadyPlayed;
            }
        }
    }
}