using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalRay : MonoBehaviour
{
    public float lifeTime;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
