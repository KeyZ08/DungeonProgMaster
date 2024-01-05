using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompileController : MonoBehaviour
{
    [Header("Input Field")]
    [SerializeField] private TMP_InputField inputField;
    [Header("Script to compile")]
    [SerializeField] private TextAsset _asset;

    private Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>()
    {
        { "forward", new MoveForwardCommand() },
        { "turn_right", new RotateRightCommand() },
        { "turn_left", new RotateLeftCommand() },
        { "attack", new AttackCommand() }
    };

    public List<ICommand> Compile()
    {
        var result = new List<ICommand>();
        var list = Compiler.Compile(inputField.text, _asset.text);
        for (int i = 0; i < list.Count; i++)
            result.Add(commands[list[i]]);
        return result;
    }

    public void Test()
    {
        Compiler.TestCompiling(_asset.text);
    }
}
