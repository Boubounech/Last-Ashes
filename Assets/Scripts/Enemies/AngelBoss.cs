using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AngelBoss : MonoBehaviour
{
    [SerializeField] private string bossID;
    [Serializable]
    public struct AngelAttack {
        public string function;
        public float duration;
    }

    public float attackDelay;

    public AngelAttack[] attacks;
    private bool isHardmode;

    private bool isDead = false; 

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
    public static UnityEvent OnAttackCharging = new UnityEvent();
    public static UnityEvent OnAttackReady = new UnityEvent();
    public static UnityEvent OnStartAttack = new UnityEvent();
    public static UnityEvent OnEndAttack = new UnityEvent();
    public static UnityEvent OnChangePosition = new UnityEvent();

    private void Awake()
    {
        GetComponent<Damageable>().OnMidLife.AddListener(delegate { isHardmode = true; });
        GetComponent<Damageable>().OnDeath.AddListener(delegate 
        { 
            SaveManager.instance.bossesToSave.Add(this.bossID);
            isDead = true;
        });
        OnEndAttack.AddListener(delegate { Invoke("ChargeNextAttack", attackDelay); });
        OnAttackReady.AddListener(delegate { SelectNextAttack(); });
    }

    private void Start()
    {
        if (SaveManager.instance.bossesSaved.Contains(this.bossID) || SaveManager.instance.bossesToSave.Contains(this.bossID))
        {
            isDead = true;
            Debug.Log("contenu dans");
        }
        if (isDead)
        {
            Destroy(this.gameObject);
        }
        else
        {
            isHardmode = false;
            Invoke("ChargeNextAttack", attackDelay);
        }

    }

    private void ChargeNextAttack()
    {
        if (UnityEngine.Random.value <= changePosProbability || isHardmode)
        {
            transform.position = possiblePos[UnityEngine.Random.Range(0, possiblePos.Length)].position;
        }
        OnAttackCharging.Invoke();
    }

    private void SelectNextAttack()
    {
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
            Invoke(second.function, 0);
        }

        Invoke("EmitAttacking", 0);
        Invoke(attack.function, 0);
        Invoke("EmitEndAttack", duration);
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

    public bool GetIsDead()
    {
        return this.isDead;
    }

    public string GetAngelId()
    {
        return this.bossID;
    }
}
