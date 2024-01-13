using UnityEngine;
using DPM.Infrastructure;

namespace DPM.UI
{
    public class CharacterRenderer : MonoBehaviour
    {
        public static readonly string[] idleDirections = { "IdleUp", "IdleLeft", "IdleDown", "IdleRight" };
        public static readonly string[] moveDirections = { "MoveUp", "MoveLeft", "MoveDown", "MoveRight" };
        public static readonly string[] attackDirections = { "AttackUp", "AttackLeft", "AttackDown", "AttackRight" };

        Animator animator;
        int lastDirection;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void SetDirection(Direction direction)
        {
            lastDirection = direction.ToInt();
        }

        public void PlayAttack()
        {
            animator.Play(attackDirections[lastDirection]);
        }

        public void PlayMove(bool isMove)
        {
            string[] directionArray;
            if (!isMove)
                directionArray = idleDirections;
            else
                directionArray = moveDirections;
            animator.Play(directionArray[lastDirection]);
        }
    }
}
