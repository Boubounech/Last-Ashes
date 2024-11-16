using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AngelBoss : MonoBehaviour
{
    public float attackDelay;

    public string[] attacks;

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
    public float ballLife;
    public int ballAmount;

    [Header("Events")]
    public static UnityEvent OnStartAttack = new UnityEvent();
    public static UnityEvent OnEndAttack = new UnityEvent();
    public static UnityEvent OnMidLife = new UnityEvent();

    private void Awake()
    {
        OnEndAttack.AddListener(SelectNextAttack);
    }

    private void Start()
    {
        SelectNextAttack();
    }

    private void SelectNextAttack()
    {
        string attack = attacks[Random.Range(0, attacks.Length)];
        Invoke("EmitAttacking", attackDelay);
        Invoke(attack, attackDelay);
    }

    private void EmitAttacking()
    {
        OnStartAttack.Invoke();
        Debug.Log("Start Attack");
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
            rayPos.RemoveAt(Random.Range(0, rayPos.Count));
        }

        foreach(float xPos in rayPos)
        {
            GameObject ray = Instantiate(
                vrayPrefab,
                new Vector3(xPos, 0, 0),
                Quaternion.identity
                );
        }
        OnEndAttack.Invoke();
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
        OnEndAttack.Invoke();
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
        yield return new WaitForSeconds(ballLife);
        OnEndAttack.Invoke();
    }
}
