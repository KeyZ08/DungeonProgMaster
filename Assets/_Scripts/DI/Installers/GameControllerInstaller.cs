using UnityEngine;
using Zenject;
using DPM.UI;
using DPM.App.IDE;
//не должно использовать UI :(

namespace DPM.App
{
    public class GameControllerInstaller : MonoInstaller
    {
        [Header("Controllers")]
        [SerializeField] private GameController controller;
        [SerializeField] private UIController ui;
        [SerializeField] private MapVisualizer mapV;
        [SerializeField] private CompileController compiler;

        public override void InstallBindings()
        {
            Container.Bind<UIController>().FromInstance(ui).AsSingle();
            Container.Bind<MapVisualizer>().FromInstance(mapV).AsSingle();
            Container.Bind<CompileController>().FromInstance(compiler).AsSingle();
            Container.Bind<GameController>().FromInstance(controller).AsSingle().NonLazy();
        }
    }
}
