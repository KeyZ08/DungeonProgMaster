using UnityEngine;
using Zenject;

public class UnitInstaller : MonoInstaller
{
    [SerializeField] private GameController gameController;
    [SerializeField] private CoinController coinControllerPrefab;
    [SerializeField] private SkeletonController skeletonControllerPrefab;
    [SerializeField] private MyCharacterController characterPrefab;

    public override void InstallBindings()
    {
        var levels = Resources.Load<LevelsHandlerScriptableObject>("LevelsHandler");
        Container.Bind<LevelsHandlerScriptableObject>().FromInstance(levels).AsSingle().NonLazy();
        Container.Bind<Level>().FromInstance(levels.GetLevel(1));

        Container.Bind<GameController>().FromInstance(gameController).AsSingle().NonLazy();

        Container.Bind<CoinController>().FromComponentInNewPrefab(coinControllerPrefab).AsSingle();
        Container.Bind<SkeletonController>().FromComponentInNewPrefab(skeletonControllerPrefab).AsSingle();

        Container.BindFactory<Character, Map, GameController, MyCharacterController, MyCharacterController.Factory>()
            .FromComponentInNewPrefab(characterPrefab).AsSingle();
    }
}