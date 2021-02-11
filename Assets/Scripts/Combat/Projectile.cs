using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        Health target = null;
        [SerializeField] float speed = 20;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        float damage = 0;

        private Vector3 aimLocation;

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
         
        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
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

            if (hitEffect != null) Instantiate(hitEffect, GetAimLocation(), transform.rotation);

            target.TakeDamange(damage);

            Destroy(gameObject);
        }
    }
}
