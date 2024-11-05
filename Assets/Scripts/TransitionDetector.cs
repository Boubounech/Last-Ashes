using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDetector : MonoBehaviour
{
    [SerializeField] private string playerTag;
    [SerializeField] public string transitionName;
    [SerializeField] private TransitionManager.Transition transition;
    [SerializeField] public Transform playerSpawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            TransitionManager.OnPlayerEnterTransition.Invoke(transition);
            PlayerEvents.OnPlayerChangeScene.Invoke();
        }
    }
}
