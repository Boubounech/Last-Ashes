using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private SpriteRenderer swordRenderer;

    private void Awake()
    {
        PlayerEvents.OnPlayerAttack.AddListener(HideBasicSword);
        PlayerEvents.OnPlayerFinishAttack.AddListener(ShowBasicSword);
        PlayerEvents.OnPlayerDash.AddListener(Dash);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void HideBasicSword(PlayerEvents.Attack attack)
    {
        swordRenderer.enabled = false;
    }

    public void ShowBasicSword()
    {
        swordRenderer.enabled = true;
    }

    private void Dash()
    {
        animator.SetTrigger("Dash");
    }
}
