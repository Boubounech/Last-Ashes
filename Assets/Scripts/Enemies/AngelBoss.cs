using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AngelBoss : MonoBehaviour
{
    [Serializable]
    public struct AngelAttack {
        public string function;
        public float duration;
    }

    public float attackDelay;

    public AngelAttack[] attacks;
    private bool isHardmode;

    [Header("Positions")]
    public Transform[] possiblePos;
    [Range(0f, 1f)] public float changePosProbability;

    [Header("VerticalRays")]
    public float leftLimit;
    public float rightLimit;
    public float rayWidth;
    public GameObject vrayPrefab;
    public int vraynbHoles;

    [Header("HorizontalRays")]
    public GameObject hrayPrefab;
    public float hrayDelay;
    public int hrayAmount;

    [Header("Ball")]
    public GameObject ballPrefab;
    public float ballDelay;
    public int ballAmount;

    [Header("Events")]
    public static UnityEvent OnStartAttack = new UnityEvent();
    public static UnityEvent OnEndAttack = new UnityEvent();
    public static UnityEvent OnChangePosition = new UnityEvent();

    private void Awake()
    {
        OnEndAttack.AddListener(SelectNextAttack);
        GetComponent<Damageable>().OnMidLife.AddListener(delegate { isHardmode = true; });
    }

    private void Start()
    {
        SelectNextAttack();
        isHardmode = false;
    }

    private void SelectNextAttack()
    {
        if (UnityEngine.Random.value <= changePosProbability || isHardmode)
        {
            transform.position = possiblePos[UnityEngine.Random.Range(0, possiblePos.Length)].position;
        }

        AngelAttack attack = attacks[UnityEngine.Random.Range(0, attacks.Length)];
        float duration = attack.duration;

        if (isHardmode && attacks.Length > 1)
        {
            AngelAttack second;
            do
            {
                second = attacks[UnityEngine.Random.Range(0, attacks.Length)];
            } while (second.function == attack.function);
            duration = Mathf.Max(duration, second.duration);
            Invoke(second.function, attackDelay);
        }

        Invoke("EmitAttacking", attackDelay);
        Invoke(attack.function, attackDelay);
        Invoke("EmitEndAttack", duration + attackDelay);
    }

    private void EmitAttacking()
    {
        OnStartAttack.Invoke();
    }

    private void EmitEndAttack()
    {
        OnEndAttack.Invoke();
    }

    private void VerticalRays()
    {
        List<float> rayPos = new List<float>();
        for(float i = leftLimit; i < rightLimit; i += rayWidth)
        {
            rayPos.Add(i);
        }
        for (int i = 0; i < vraynbHoles; i++)
        {
            rayPos.RemoveAt(UnityEngine.Random.Range(0, rayPos.Count));
        }

        foreach(float xPos in rayPos)
        {
            GameObject ray = Instantiate(
                vrayPrefab,
                new Vector3(xPos, 0, 0),
                Quaternion.identity
                );
        }
    }

    private void HorizontalRays()
    {
        StartCoroutine(SpawnHorizontalRays());
    }

    private IEnumerator SpawnHorizontalRays()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        for(int i = 0; i < hrayAmount; i++)
        {
            GameObject ray = Instantiate(
                hrayPrefab,
                new Vector3(0, player.position.y, 0),
                Quaternion.identity
                );
            yield return new WaitForSeconds(hrayDelay);
        }
    }

    private void Ball()
    {
        StartCoroutine(SpawnBall());
    }

    private IEnumerator SpawnBall()
    {
        for (int i = 0; i < ballAmount; i++)
        {
            GameObject ray = Instantiate(
                ballPrefab,
                transform.position,
                Quaternion.identity
                );
            yield return new WaitForSeconds(ballDelay);
        }
    }
}
