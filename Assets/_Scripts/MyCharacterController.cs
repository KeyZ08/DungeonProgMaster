using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MyCharacterController : MonoBehaviour
{
    [SerializeField] private CharacterVisualizer characterV;

    [Inject] private GameController controller;
    private Character character;
    private Map map;

    public Vector2Int Position => character.CurrentPosition;
    public Direction Direction => character.CurrentDirection;
    public Character Character => character;
    public CharacterVisualizer Visualizer => characterV;

    public void Construct(Character character, Map map)
    {
        this.character = character;
        this.map = map;
        characterV.Constructor(character.StartDirection);
    }

    public class Factory : PlaceholderFactory<Character, Map, TransformParameters, MyCharacterController> { }

    public void Play(List<ICommand> steps)
    {
        StartCoroutine(CharacterWorkCoroutine(steps));
    }

    private IEnumerator CharacterWorkCoroutine(List<ICommand> playerSteps)
    {
        for (var index = 0; index < playerSteps.Count; index++)
        {
            playerSteps[index].Action(controller, this, index == playerSteps.Count - 1 ? null : playerSteps[index + 1]);

            while (characterV.IsAnimated)
                yield return null;

            new OnComeCommand().Action(controller, this, null);

            //если следующее действие - forward, то проверяем, что мы можем его выполнить
            //(никуда не выпали и не уперлись)
            if (index + 1 < playerSteps.Count && playerSteps[index + 1] is MoveForwardCommand)
            {
                if (!map.IsGround(character.CurrentPosition))
                    break;
                if (!IsNextMoveFree(character.CurrentPosition, character.CurrentDirection))
                    break;
            }
        }

        controller.OnCharacterMoveEnd();
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
}