using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    [SerializeField] Transform target = null;

    void Update()
    {
        FollowCamera followCamera = GetComponent<FollowCamera>();

        transform.position = target.position;


    }
}
