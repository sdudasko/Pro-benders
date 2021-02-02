using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmosRadius = 0.3f;

        private void OnDrawGizmos()
        {
            int len = transform.childCount;

            for (int i = 0; i < len; i++)
            {
                int j = GetNextIndex(i);

                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(GetWayPoint(i), waypointGizmosRadius);
                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));
            }
        }

        public Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }

        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }
    }
}
