using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using Zenject;

public class CompileController : MonoBehaviour
{
    [Header("Input Field")]
    [SerializeField] private TMP_InputField inputField;
    [Header("Script to compile")]
    [SerializeField] private TextAsset _asset;

    [Inject] CommandsInstaller commandsInstaller;

    public List<ICommand> Compile()
    {
        try
        {
            var commands = commandsInstaller.GetAllCommands();
            var methods = MethodsGenerate(commands);

            var result = new List<ICommand>();
            var list = Compiler.Compile(inputField.text, _asset.text, methods);

            for (int i = 0; i < list.Count; i++)
                if (commandsInstaller.TryGetCommand(list[i], out var command))
                    result.Add(command);
                else Debug.LogWarning($"Команда не найдена: {list[i]}");
            return result;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return new List<ICommand>();
        }
    }

    private string MethodsGenerate(List<string> commands)
    {
        var strBuilder = new StringBuilder();
        for (int i = 0;i < commands.Count;i++)
        {
            strBuilder.Append($"public static void {commands[i]}() => AddMove(\"{commands[i]}\");");
            strBuilder.Append("\n");
        }
        return strBuilder.ToString();
    }
}
