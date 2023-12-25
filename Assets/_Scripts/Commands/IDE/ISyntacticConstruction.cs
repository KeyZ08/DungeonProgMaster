using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace IDE
{
    public interface ISyntacticConstruction
    {
        string Title { get; }
        Color32 Color { get; }
        bool CheckWord(string word);
    }
}

