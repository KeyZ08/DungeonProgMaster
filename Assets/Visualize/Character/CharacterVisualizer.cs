using System;
using UnityEngine;
using DPM.Infrastructure;

namespace DPM.UI
{
    public class CharacterVisualizer : MonoBehaviour
    {
        [SerializeField] private CharacterRenderer chRenderer;
        private Rigidbody2D rb;
        private Vector2 targetPosition;
        private Direction targetDiretion;
        private float movementSpeed = 1f;

        private float rotatedTimer;
        private readonly float rotatedTimerDefault = 0.3f;//seconds
        private bool isRotated;
        private bool isMovement;
        private bool nextAlsoMove;
        private bool isAttacked;
        private bool nextAlsoAttack;

        public bool IsAnimated => isMovement || isRotated || isAttacked;

        public Vector2 Position => transform.position;

        public void Constructor(Direction spawnDirection)
        {
            rb = GetComponent<Rigidbody2D>();
            targetPosition = transform.position;
            targetDiretion = spawnDirection;
            chRenderer.SetDirection(targetDiretion);
            rotatedTimer = rotatedTimerDefault;
        }

        private void Start()
        {
            chRenderer.PlayMove(false);
        }

        public void TurnTo(Direction direction)
        {
            targetDiretion = direction;
            isRotated = true;
        }

        public void MoveTo(Vector2 target, bool nextAlsoMove)
        {
            targetPosition = target;
            isMovement = true;
            this.nextAlsoMove = nextAlsoMove;
        }

        void FixedUpdate()
        {
            if (isAttacked)
                return;

            if (isRotated)
            {
                rotatedTimer -= Time.fixedDeltaTime;
                if (rotatedTimer < 0)
                {
                    chRenderer.SetDirection(targetDiretion);
                    isRotated = false;
                    rotatedTimer = rotatedTimerDefault;
                }
            }

            if (isMovement)
            {
                Vector2 currentPos = transform.position;
                Vector2 inputVector = targetPosition - (Vector2)transform.position;
                Vector2 movement = Vector2.ClampMagnitude(inputVector, movementSpeed * Time.fixedDeltaTime);
                Vector2 newPos = currentPos + movement;
                rb.MovePosition(newPos);
                if (movement.magnitude < .01f)
                    isMovement = false;
            }

            if (nextAlsoMove)
            {
                chRenderer.PlayMove(true);
                return;
            }

            //костыль дабы исправить прерывание анимации атаки (иногда в 1 кадр срабатывает и Move и Attack из-за чего всё ломается)
            if (nextAlsoAttack)
                return;

            chRenderer.PlayMove(isMovement);
        }

        Action OnHit;
        public void Attack(Action onHit, bool nextAlsoAttack)
        {
            isAttacked = true;
            this.nextAlsoAttack = nextAlsoAttack;
            OnHit = onHit;
            chRenderer.PlayAttack();
        }

        //анимация атаки закончилась
        public void AttackEnd()
        {
            isAttacked = false;
        }

        //нанесен удар (соответственно анимации)
        public void Hit()
        {
            OnHit?.Invoke();
            OnHit = null;
        }
    }
}
