using UnityEngine;
using Zenject;

public class GameControllerInstaller : MonoInstaller
{
    [Header("Controllers")]
    [SerializeField] private UIController ui;
    [SerializeField] private MapVisualizer mapV;
    [SerializeField] private CompileController compiler;

    public override void InstallBindings()
    {
        Container.Bind<UIController>().FromInstance(ui).AsSingle();
        Container.Bind<MapVisualizer>().FromInstance(mapV).AsSingle();
        Container.Bind<CompileController>().FromInstance(compiler).AsSingle();
    }
}
