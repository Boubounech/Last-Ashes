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
        PlayerEvents.OnPlayerHeal.AddListener(Heal);
        PlayerEvents.OnPlayerCancelHeal.AddListener(CancelHeal);
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

    private void Heal()
    {
        animator.SetBool("Heal", true);
    }

    private void CancelHeal()
    {
        animator.SetBool("Heal", false);
    }
}
