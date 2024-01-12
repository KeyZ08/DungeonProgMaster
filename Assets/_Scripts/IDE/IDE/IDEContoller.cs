using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;
using DPM.Domain;

namespace DPM.App
{
    public class IDEContoller : MonoBehaviour
    {
        [Inject] CommandsInstaller commands;
        private TMP_InputField inputField;

        ISyntacticConstruction[] syntacticConstructions;

        public void Start()
        {
            inputField = GetComponent<TMP_InputField>();
            SyntacticConstructionsFill();
        }

        private void SyntacticConstructionsFill()
        {
            syntacticConstructions = new ISyntacticConstruction[]
            {
            new ArraySyntacticConstruction(
                "Control Constructions",
                new string[] { "for", "if", "else", "break", "return", "foreach" },
                new Color32(208, 106, 221, 255)
                ),
            new ArraySyntacticConstruction(
                "Static Classes",
                new string[] { "Player" },
                new Color32(34, 226, 187, 255)
                ),
            new ArraySyntacticConstruction(
                "Methods",
                commands.GetAllCommands().ToArray(),
                new Color32(229, 229, 112, 255)
                ),
            new ArraySyntacticConstruction(
                "Types",
                new string[] { "var", "int", "string", "new" },
                new Color32(64, 150, 222, 255)
                ),
            new FuncSyntacticConstruction(
                "Numbers",
                word => int.TryParse(word, out _),
                new Color32(162, 209, 138, 255)
                )
            };
        }

        public void Update()
        {
            foreach (var wordInfo in inputField.textComponent.textInfo.wordInfo)
            {
                if (wordInfo.characterCount <= 0)
                    continue;

                foreach (var syntacticConstruction in syntacticConstructions)
                    if (syntacticConstruction.CheckWord(GetWord(wordInfo, inputField.text)))
                    {
                        ChangeColor(wordInfo, syntacticConstruction.Color);
                        break;
                    }
            }

            inputField.ActivateInputField();
        }

        private string GetWord(TMP_WordInfo wordInfo, string text)
        {
            return string.Join("", text.Skip(wordInfo.firstCharacterIndex).Take(wordInfo.characterCount).ToArray());
        }

        private void ChangeColor(TMP_WordInfo wordInfo, Color32 color)
        {
            for (int i = 0; i < wordInfo.characterCount; ++i)
            {
                int charIndex = wordInfo.firstCharacterIndex + i;
                int meshIndex = inputField.textComponent.textInfo.characterInfo[charIndex].materialReferenceIndex;
                int vertexIndex = inputField.textComponent.textInfo.characterInfo[charIndex].vertexIndex;
                Color32[] vertexColors = inputField.textComponent.textInfo.meshInfo[meshIndex].colors32;
                vertexColors[vertexIndex + 0] = color;
                vertexColors[vertexIndex + 1] = color;
                vertexColors[vertexIndex + 2] = color;
                vertexColors[vertexIndex + 3] = color;
            }
            inputField.textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }
    }
}