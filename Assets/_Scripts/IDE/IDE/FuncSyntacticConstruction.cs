using System;
using UnityEngine;

namespace DPM.Domain.IDE
{
    public class FuncSyntacticConstruction : ISyntacticConstruction
    {
        private readonly string title;
        private readonly Func<string, bool> func;
        private readonly Color32 color;

        public FuncSyntacticConstruction(string title, Func<string, bool> func, Color32 color)
        {
            this.title = title;
            this.func = func;
            this.color = color;
        }

        string ISyntacticConstruction.Title => title;
        Color32 ISyntacticConstruction.Color => color;

        bool ISyntacticConstruction.CheckWord(string word)
        {
            return func(word);
        }
    }
}