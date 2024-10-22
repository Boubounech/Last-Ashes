using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    private static TransitionManager instance;

    [Serializable]
    public class Transition
    {
        public string toSceneName;
        public string toEntryName;
    }

    public static UnityAction<Transition> OnPlayerEnterTransition;

    [SerializeField] private Transition lastTransition;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OnPlayerEnterTransition += ChargeNewScene;
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(Scene current, Scene next)
    {
        TransitionDetector[] transitions = FindObjectsOfType<TransitionDetector>();
        for (int i = 0; i < transitions.Length; i++)
        {
            if (transitions[i].transitionName == lastTransition.toEntryName)
            {
                transitions[i].gameObject.SetActive(false);
                PlayerMovementRays player = FindObjectOfType<PlayerMovementRays>();
                player.OnPlayerRegainControl += delegate { transitions[i].gameObject.SetActive(true); ; };
                player.transform.position = new Vector3(transitions[i].transform.position.x, transitions[i].playerSpawnPoint.position.y, player.transform.position.z);
                player.TakeControlAndMoveTo(transitions[i].playerSpawnPoint.position);
                break;
            }
        }
    }

    private void ChargeNewScene(Transition transition)
    {
        PlayerMovementRays player = FindObjectOfType<PlayerMovementRays>();
        lastTransition = transition;
        SceneManager.LoadScene(transition.toSceneName);
    }
}
