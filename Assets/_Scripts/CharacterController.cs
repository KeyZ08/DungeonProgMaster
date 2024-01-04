using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private CharacterVisualizer characterV;
    private Character character;
    private Map map;

    private GameContoller controller;

    public Vector2Int Position => character.CurrentPosition;
    public Direction Direction => character.CurrentDirection;

    public void Construct(Character character, Map map, GameContoller controller)
    {
        this.character = character;
        this.map = map;
        this.controller = controller;
        characterV.Constructor(character.StartDirection);
    }

    public void Play(List<string> steps)
    {
        StartCoroutine(CharacterWorkCoroutine(steps));
    }

    private IEnumerator CharacterWorkCoroutine(List<string> playerSteps)
    {
        for (var index = 0; index < playerSteps.Count; index++)
        {
            var step = playerSteps[index];
            CharacterWork(step, index == playerSteps.Count - 1 ? null : playerSteps[index + 1]);

            while (characterV.IsAnimated)
                yield return null;

            Come();

            //если следующее дествие - forward, то проверяем, что мы можем его выполнить
            //(никуда не выпали и не уперлись)
            if (index + 1 < playerSteps.Count && playerSteps[index + 1] == "forward")
            {
                if (!map.IsGround(character.CurrentPosition))
                    break;
                if (!IsNextMoveFree(character.CurrentPosition, character.CurrentDirection))
                    break;
            }
        }

        controller.OnCharacterMoveEnd();
    }

    private void CharacterWork(string step, string nextStep = null)
    {
        if (step == "forward")
        {
            Command.MoveForward(character);
            var isNextMoveFree = nextStep == "forward" && IsNextMoveFree(character.CurrentPosition, character.CurrentDirection);
            characterV.MoveTo(controller.GetPosByMap(character.CurrentPosition), isNextMoveFree);
        }
        else if (step == "turn_right")
        {
            Command.RotateRight(character);
            characterV.TurnTo(character.CurrentDirection);
        }
        else if (step == "turn_left")
        {
            Command.RotateLeft(character);
            characterV.TurnTo(character.CurrentDirection);
        }
        else if (step == "attack")
        {
            Attack(nextStep == "attack");
        }
        else
            throw new NotImplementedException(step);
    }

    /// <summary>
    /// true - свободно, false - преграда.
    /// Преградой является то на что нельзя встать, пропасть - не преграда
    /// </summary>
    public bool IsNextMoveFree(Vector2Int position, Direction direction)
    {
        var posInMap = position + direction.Vector();
        if (!map.InMapBounds(posInMap))
            return false;

        if (map.IsWall(posInMap))
            return false;

        var unit = controller.GetUnitController(posInMap);
        if (unit != null && unit.Type == Tangibility.Obstacle)
            return false;

        return true;
    }

    public void Attack(bool nextAlsoAttack)
    {
        var forwardInMap = character.CurrentPosition + character.Forward;
        var unit = controller.GetUnitController(forwardInMap);
        characterV.Attack(
            () => {
                if (unit != null && unit is IAttackable attackeble)
                    attackeble.OnAttack(ContactDirection.Side, controller);
            }, nextAlsoAttack);
    }

    public void Come()
    {
        var unit = controller.GetUnitController(character.CurrentPosition);
        if (unit != null && unit is IOnCome onComeable)
            onComeable.OnCome(ContactDirection.Directly, controller);
    }

    public void Take()
    {
        var forwardInMap = character.CurrentPosition + character.Forward;
        var unit = controller.GetUnitController(forwardInMap);
        if (unit != null && unit is ITakeable takeable)
            takeable.OnTake(ContactDirection.Directly, controller);
    }
}
