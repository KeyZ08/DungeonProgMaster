using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private CharacterVisualizer characterV;
    private Character character;

    private GameContoller controller;
    
    private MapVisualizer mapV;
    private Map map;

    private bool isPlayed;

    public Vector2Int Position => character.CurrentPosition;
    public Direction Direction => character.CurrentDirection;

    public void Construct(Character character, MapVisualizer mapV, Map map, GameContoller controller)
    {
        this.character = character;
        this.mapV = mapV;
        this.map = map;
        this.controller = controller;
        characterV.Constructor(character.StartDirection);
    }

    public void Play(List<string> steps)
    {
        isPlayed = true;
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

            Come(controller.GetUnitController(character.CurrentPosition));

            //если следующее дествие - forward, то проверяем, что мы можем его выполнить
            //(никуда не выпали и не уперлись)
            if (index + 1 < playerSteps.Count && playerSteps[index + 1] == "forward")
            {
                if (!map.IsGround(character.CurrentPosition))
                    break;
                if (!controller.IsNextMoveFree(character.CurrentPosition, character.CurrentDirection))
                    break;
            }
        }

        isPlayed = false;
        controller.OnCharacterMoveEnd();
    }

    private void CharacterWork(string step, string nextStep = null)
    {
        if (step == "forward")
        {
            Command.MoveForward(character);
            var isNextMoveFree = nextStep == "forward" && controller.IsNextMoveFree(character.CurrentPosition, character.CurrentDirection);
            characterV.MoveTo(mapV.GetCellCenter(character.CurrentPosition), isNextMoveFree);
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
            var forwardInMap = character.CurrentPosition + character.Forward;
            var forwardUnit = controller.GetUnitController(forwardInMap);
            Attack(forwardUnit, nextStep == "attack");
        }
        else
            throw new NotImplementedException(step);
    }

    public void Attack(UnitController unit, bool nextAlsoAttack)
    {
        characterV.Attack(
            () => {
                if (unit != null && unit is IAttackable attackeble)
                    attackeble.OnAttack(ContactDirection.Side, controller);
            }, nextAlsoAttack);
    }

    public void Come(UnitController unit)
    {
        if (unit != null && unit is IOnCome onComeable)
            onComeable.OnCome(ContactDirection.Directly, controller);
    }

    public void Take(UnitController unit)
    {
        if (unit != null && unit is ITakeable takeable)
            takeable.OnTake(ContactDirection.Directly, controller);
    }
}
