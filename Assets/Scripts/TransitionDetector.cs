using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDetector : MonoBehaviour
{
    [SerializeField] public string transitionName;
    [SerializeField] private TransitionManager.Transition transition;
    [SerializeField] public Transform playerSpawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TransitionManager.OnPlayerEnterTransition.Invoke(transition);
    }
}
