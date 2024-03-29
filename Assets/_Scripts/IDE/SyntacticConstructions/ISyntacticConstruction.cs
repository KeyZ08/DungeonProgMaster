﻿using UnityEngine;

namespace DPM.Domain.IDE
{
    public interface ISyntacticConstruction
    {
        string Title { get; }
        Color32 Color { get; }
        bool CheckWord(string word);
    }
}

