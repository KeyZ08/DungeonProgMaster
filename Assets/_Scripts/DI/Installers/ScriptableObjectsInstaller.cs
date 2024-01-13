using UnityEngine;
using Zenject;

namespace DPM.App
{
    public class ScriptableObjectsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var levels = Resources.Load<LevelsHandlerScriptableObject>("LevelsHandler");
            Container.Bind<LevelsHandlerScriptableObject>().FromInstance(levels).AsSingle().NonLazy();

            var unitPrefabs = Resources.Load<UnitPrefabsHandlerScriptableObject>("UnitPrefabsHandler");
            Container.Bind<UnitPrefabsHandlerScriptableObject>().FromInstance(unitPrefabs).AsSingle().NonLazy();
        }
    }
}