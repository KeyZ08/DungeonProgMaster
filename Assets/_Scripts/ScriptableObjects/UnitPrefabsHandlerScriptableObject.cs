using System;
using System.Collections.Generic;
using UnityEngine;


namespace DPM.App
{
    [CreateAssetMenu(fileName = "UnitPrefabsHandler", menuName = "ScriptableObjects/UnitPrefabsHandler")]
    public class UnitPrefabsHandlerScriptableObject : ScriptableObject
    {
        private Dictionary<Type, BaseUnitController> controllerByType;
        [SerializeField] private List<BaseUnitController> controllers;

        public BaseUnitController GetPrefab(Type type)
        {
            return controllerByType[type];
        }

        private void OnEnable()
        {
            controllerByType = new Dictionary<Type, BaseUnitController>();
            for (int i = 0; i < controllers.Count; i++)
            {
                var type = controllers[i].GetType();
                controllerByType.Add(type, controllers[i]);
            }
        }

        private void OnValidate()
        {
            var hashSet = new HashSet<Type>();
            for (int i = 0; i < controllers.Count; i++)
            {
                var type = controllers[i].GetType();
                if (hashSet.Contains(type))
                    throw new Exception($"Повторяющийся тип: {type.Name}");
                hashSet.Add(type);
            }
        }
    }
}
