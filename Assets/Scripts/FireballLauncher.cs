using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballLauncher : MonoBehaviour
{
    public GameObject fireballPrefab;
    public float cooldownFireball = 0.4f;
    public float fireballVitalEnergyCost = 10;

    private void Awake()
    {
        PlayerEvents.OnPlayerLaunchFireball.AddListener(LaunchFireball);
    }

    private void LaunchFireball(float direction)
    {
        Vector3 position = new Vector3(
            direction >= 0 ? transform.position.x : transform.position.x - 2 * transform.localPosition.x,
            transform.position.y,
            transform.position.z);
        Fireball fb = Instantiate(fireballPrefab, position, Quaternion.identity).GetComponent<Fireball>();
        fb.SetDirection(Vector3.right * direction);

        VitalEnergyManager.instance.RemoveTime(fireballVitalEnergyCost);

        Invoke("AllowNewFireball", cooldownFireball);
    }

    private void AllowNewFireball()
    {
        PlayerEvents.OnPlayerCanLaunchFireball.Invoke();
    }
}
