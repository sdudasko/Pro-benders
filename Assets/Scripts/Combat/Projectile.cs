using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        Health target = null;
        [SerializeField] float speed = 20;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10;
        [SerializeField] GameObject[] destroyOnHit = null;

        [SerializeField] float lifeAfterImpact = 3;

        float damage = 0;
        private Vector3 aimLocation;

        GameObject instigator = null;

        [SerializeField] UnityEvent onHit;

        private void Start()
        {
            if (target == null) return;

            transform.LookAt(GetAimLocation());

        }

        void Update()
        {
            if (target == null) return;

            if (isHoming && !target.isDead())
            {
                transform.LookAt(aimLocation);
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);

        }
         
        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();

            if (targetCapsule == null) return target.transform.position;

            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;

            if (target.isDead()) return;

            target.TakeDamange(instigator, damage);

            speed = 0;

            onHit.Invoke();

            if (hitEffect != null) Instantiate(hitEffect, GetAimLocation(), transform.rotation);

            foreach(GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
