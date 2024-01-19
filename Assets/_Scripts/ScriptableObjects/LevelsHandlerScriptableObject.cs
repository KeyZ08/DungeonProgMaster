using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using System;
using DPM.Domain;

namespace DPM.App
{
    [CreateAssetMenu(fileName = "LevelsHandler", menuName = "ScriptableObjects/LevelsHandler", order = 1)]
    public class LevelsHandlerScriptableObject : ScriptableObject
    {
        [SerializeField] private List<TextAsset> jsonLevels = new List<TextAsset>();

        public int LevelsCount => jsonLevels.Count;

        public Level GetLevel(int index)
        {
            if (index >= LevelsCount)
                throw new IndexOutOfRangeException();
            var level = JsonConvert.DeserializeObject<Level>(jsonLevels[index].text);
            return level;
        }
    }
}
