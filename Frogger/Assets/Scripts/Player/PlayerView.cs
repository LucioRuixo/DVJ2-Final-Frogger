using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Animator animator;

    public void Run()
    {
        animator.SetTrigger("Run");
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }

    public void Idle()
    {
        animator.SetTrigger("Idle");
    }
}