using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDisplayer : MonoBehaviour
{
    [SerializeField] private Image lifeImage;
    [SerializeField] private List<Image> lifes;

    private void Awake()
    {
        LifePointsManager.OnHpChanged.AddListener(ActOnHpChanged);
    }

    private void Start()
    {
        ActOnHpChanged(LifePointsManager.instance.GetHp());
    }

    private void ActOnHpChanged(int newHp)
    {
        if (newHp < lifes.Count)
        {
            do
            {
                Image life = lifes.ToArray()[lifes.Count - 1];
                lifes.RemoveAt(lifes.Count - 1);
                Destroy(life.gameObject);
            } while (newHp < lifes.Count);
        } else if (newHp > lifes.Count)
        {
            do
            {
                Image newLife = Instantiate(lifeImage, transform);
                lifes.Insert(lifes.Count, newLife);
            } while (newHp > lifes.Count);
        }
    }
}
