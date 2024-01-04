using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompileController : MonoBehaviour
{
    [Header("Input Field")]
    [SerializeField] private TMP_InputField inputField;
    [Header("Script to compile")]
    [SerializeField] private TextAsset _asset;

    public List<string> Compile()
    {
        return Compiler.Compile(inputField.text, _asset.text);
    }

    public void Test()
    {
        Compiler.TestCompiling(_asset.text);
    }
}
