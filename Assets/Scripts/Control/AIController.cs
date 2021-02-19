using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 2f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] float wayPointTolerance = 1f;
        [SerializeField] float dwellingTime = 3f;
        [Range(0,1)][SerializeField] float patrolSpeedFraction = 0.3f;

        Fighter enemyFighter;
        Health health;
        GameObject player;
        Mover mover;

        LazyValue<Vector3> guardInitialPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;

        int currentWayPoint = 0;
        float currentDwellingTime = Mathf.Infinity;

        private void Awake()
        {
            enemyFighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            guardInitialPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        void Start()
        {
            guardInitialPosition.ForceInit();
        }

        void Update()
        {
            UpdateSpeeds();
            if (health.isDead()) return;

            if (InAttackRangeOfPlayer() && enemyFighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            UpdateTimers();
        }

        private void UpdateSpeeds()
        {
            
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            currentDwellingTime += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardInitialPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    currentDwellingTime = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (currentDwellingTime >= dwellingTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < wayPointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWayPoint = patrolPath.GetNextIndex(currentWayPoint);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWayPoint(currentWayPoint);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
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
