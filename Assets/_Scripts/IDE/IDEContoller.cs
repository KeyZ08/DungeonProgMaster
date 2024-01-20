using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Zenject;
using DPM.Domain.IDE;
using System;

namespace DPM.App.IDE
{
    public class IDEContoller : MonoBehaviour
    {
        [Inject] CommandsInstaller commands;
        private TMP_InputField inputField;
        private List<Tuple<TMP_WordInfo, Color32>> wordColors;
        private Dictionary<string, Color32> wordHash;
        private int oldTextHash;

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
                new HashSet<string> { "for", "if", "else", "break", "return", "foreach", "while" },
                new Color32(208, 106, 221, 255)
                ),
            new ArraySyntacticConstruction(
                "Static Classes",
                new HashSet<string> { "Player" },
                new Color32(34, 226, 187, 255)
                ),
            new ArraySyntacticConstruction(
                "Methods",
                commands.GetAllCommands().ToHashSet(),
                new Color32(229, 229, 112, 255)
                ),
            new ArraySyntacticConstruction(
                "Types",
                new HashSet<string> { "var", "int", "string", "new" },
                new Color32(64, 150, 222, 255)
                ),
            new FuncSyntacticConstruction(
                "Numbers",
                word => int.TryParse(word, out _),
                new Color32(162, 209, 138, 255)
                )
            };

            oldTextHash = "".GetHashCode();
            wordColors = new List<Tuple<TMP_WordInfo, Color32>>();
            wordHash = new Dictionary<string, Color32>();
        }

        public void LateUpdate()
        {
            var tempTextHash = inputField.text.GetHashCode();
            if (oldTextHash != tempTextHash)
            {
                OnValueChanged();
                oldTextHash = tempTextHash;
            }

            foreach (var tuple in wordColors)
                ChangeColor(tuple.Item1, tuple.Item2);

            inputField.textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

            inputField.ActivateInputField();
        }

        public void OnValueChanged()
        {
            wordColors.Clear();
            foreach (var wordInfo in inputField.textComponent.textInfo.wordInfo)
            {
                if (wordInfo.characterCount <= 0)
                    continue;

                var word = GetWord(wordInfo, inputField.text);

                if (wordHash.ContainsKey(word))
                {
                    wordColors.Add(Tuple.Create(wordInfo, wordHash[word]));
                    continue;
                }

                foreach (var syntacticConstruction in syntacticConstructions)
                    if (syntacticConstruction.CheckWord(word))
                    {
                        wordHash[word] = syntacticConstruction.Color;
                        wordColors.Add(Tuple.Create(wordInfo, syntacticConstruction.Color));
                        break;
                    }
            }
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
        }
    }
}