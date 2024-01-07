using UnityEngine;
using Zenject;

public class LevelsInstaller : MonoInstaller
{
    public override void InstallBindings()
    { 
        var levels = Resources.Load<LevelsHandlerScriptableObject>("LevelsHandler");
        Container.Bind<LevelsHandlerScriptableObject>().FromInstance(levels).AsSingle().NonLazy();
    }
}