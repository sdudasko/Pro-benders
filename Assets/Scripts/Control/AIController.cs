using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 2f;
        [SerializeField] float suspicionTime = 5f;

        Fighter enemyFighter;
        Health health;
        GameObject player;

        Vector3 guardInitialPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;

        void Start()
        {
            enemyFighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();

            guardInitialPosition = transform.position;
        }

        void Update()
        {
            if (health.isDead()) return;
            print(timeSinceLastSawPlayer);

            if (InAttackRangeOfPlayer() && enemyFighter.CanAttack(player))
            {
                AttackBehaviour();
                timeSinceLastSawPlayer = 0;

            } else if (timeSinceLastSawPlayer < suspicionTime) {
                SuspicionBehaviour();
            } else
            {
                //enemyFighter.Cancel();
                GuardBehaviour();
            }
            timeSinceLastSawPlayer += Time.deltaTime;

        }

        private void GuardBehaviour()
        {
            GetComponent<Mover>().StartMoveAction(guardInitialPosition);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            enemyFighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance;
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
