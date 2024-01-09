using UnityEngine;

public class ChestController : UnitController<Chest>, ITakeable
{
    [SerializeField] private Animator animator;

    public void OnTake(ContactDirection contact, GameController controller)
    {
        if(contact == ContactDirection.Side)
        {
            Open();
            controller.coins += unit.Cost;
        }
    }

    private void Open()
    {
        animator.SetTrigger("Open");
    }
}
