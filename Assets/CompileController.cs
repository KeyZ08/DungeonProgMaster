using System.Collections.Generic;
using UnityEngine;

public class CompileController : MonoBehaviour
{
    [SerializeField] TextAsset _asset;

    public List<string> Compile(string script)
    {
        return Compiler.Compile(script, _asset.text);
    }

    public void Test()
    {
        Compiler.TestCompiling(_asset.text);
    }
}
