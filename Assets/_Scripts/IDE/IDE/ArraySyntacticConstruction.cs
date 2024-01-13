using System.Collections.Generic;
using UnityEngine;

namespace DPM.Domain.IDE
{
    public class ArraySyntacticConstruction : ISyntacticConstruction
    {
        private readonly string title;
        private readonly HashSet<string> words;
        private readonly Color32 color;

        public ArraySyntacticConstruction(string title, HashSet<string> words, Color32 color)
        {
            this.title = title;
            this.words = words;
            this.color = color;
        }

        string ISyntacticConstruction.Title => title;
        Color32 ISyntacticConstruction.Color => color;

        bool ISyntacticConstruction.CheckWord(string word)
        {
            return words.Contains(word);
        }
    }
}