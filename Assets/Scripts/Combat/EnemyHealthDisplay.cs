using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Health target;

        private void Update()
        {
            target = GameObject.FindWithTag("Player").GetComponent<Fighter>().GetTarget();

            if (target == null) {

                GetComponent<Text>().text = "No target";
            } else
            {
                GetComponent<Text>().text = String.Format("{0:0}%", target.GetPercentage());
            }
        }
    }
}