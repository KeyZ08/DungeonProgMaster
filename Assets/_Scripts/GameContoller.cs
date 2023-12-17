using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameContoller : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] private Transform characterSpawner;
    [SerializeField] private PlayerVisualizer CharacterPrefab;

    [Header("Controllers")]
    [SerializeField] private MapVisualizer mapV;
    [SerializeField] private CompileController compiler;

    [Header("Input Field")]
    [SerializeField] private TMP_InputField inputField;

    [Header("UI")]
    [SerializeField] private Transform WinCanvas;
    [SerializeField] private Transform LoseCanvas;

    [Header("Buttons")]
    [SerializeField] private Button playBtn;

    private Level level;
    private Character character;
    private PlayerVisualizer playerV;

    private List<string> playerSteps;

    private void Awake() 
    {
        level = LevelsHandler.Levels[1];
        LevelConstruct(level);

        playBtn.onClick.AddListener(PlayBtnClick);
    }

    public void LevelConstruct(Level level)
    {
        mapV.DrawMap(level.Map);

        var characterPos = (Vector2Int)mapV.FromMapToVisual(level.Player.StartPosition, level.Map);
        character = new Character(characterPos, level.Player.StartDirection);
        playerV = Instantiate(CharacterPrefab, mapV.GetCellCenter(characterPos), Quaternion.identity, characterSpawner);
        playerV.Constructor(mapV.GetCellCenter(characterPos), character.StartDirection);
    }

    public void LevelDelete()
    {
        Destroy(playerV.gameObject);
        playerV = null;
        character = null;
    }

    public void LevelReset()
    {
        WinCanvas.gameObject.SetActive(false);
        LoseCanvas.gameObject.SetActive(false);
        LevelDelete();
        LevelConstruct(level);
    }

    private void PlayBtnClick()
    {
        playerSteps = compiler.Compile(inputField.text);
        GameStart();
    }

    private void GameStart()
    {
        StartCoroutine(CharacterWorkCoroutine());
    }

    private IEnumerator CharacterWorkCoroutine()
    {
        for (var index = 0; index < playerSteps.Count; index++)
        {
            var step = playerSteps[index];
            index = CharacterWork(index, step);

            while (playerV.IsAnimated)
                yield return null;

            //если следующее дествие - forward, то проверяем, что мы можем его выполнить
            //(никуда не выпали и не уперлись)
            if (index + 1 < playerSteps.Count && playerSteps[index + 1] == "forward")
            {
                var chInMap = mapV.FromVisualToMap(character.CurrentPosition, level.Map);
                var forward = character.CurrentDirection.Vector();
                var forwardInMap = mapV.FromVisualToMap(character.CurrentPosition + forward, level.Map);
                if (!level.Map.IsGround(chInMap) || !IsFreeForMove(forwardInMap))
                    break;
            }
        }

        var characterInMap = mapV.FromVisualToMap(character.CurrentPosition, level.Map);
        if (level.Map.IsFinish(characterInMap))
            Win();
        else
            Lose();
    }

    private int CharacterWork(int index, string step)
    {
        if (step == "forward")
        {
            var count = 0;
            while (index < playerSteps.Count)
            {
                var forwardV = character.CurrentDirection.Vector();
                var chPos = mapV.FromVisualToMap(character.CurrentPosition + forwardV * count, level.Map);
                var forwardChPos = mapV.FromVisualToMap(character.CurrentPosition + forwardV * (count + 1), level.Map);
                if (!IsFreeForMove(forwardChPos) || !level.Map.IsGround(chPos))
                {
                    //если куда-то выпали или во что-то уперлись, дальше можно не идти
                    break;
                }

                count++;
                index++;
                if (playerSteps.Count - 1 >= index && playerSteps[index] != "forward")
                {
                    index--;
                    break;
                }
            }
            Command.MoveForward(character, count);
            playerV.MoveTo(mapV.GetCellCenter(character.CurrentPosition));
        }
        else if (step == "turn_right")
        {
            Command.RotateRight(character);
            playerV.TurnTo(character.CurrentDirection);
        }
        else if (step == "turn_left")
        {
            Command.RotateLeft(character);
            playerV.TurnTo(character.CurrentDirection);
        }
        else
            throw new NotImplementedException(step);
        return index;
    }

    /// <summary>
    /// true - свободно, false - преграда.
    /// Преградой является то на что нельзя встать, пропасть - не преграда
    /// </summary>
    private bool IsFreeForMove(Vector2Int mapPos)
    {
        var inBounds = level.Map.InMapBounds(mapPos);
        if (!inBounds) 
            return false;

        if (level.Map.IsWall(mapPos)) 
            return false;

        return true;
    }

    public void Win()
    {
        WinCanvas.gameObject.SetActive(true);
    }

    public void Lose()
    {
        LoseCanvas.gameObject.SetActive(true);
    }
}
