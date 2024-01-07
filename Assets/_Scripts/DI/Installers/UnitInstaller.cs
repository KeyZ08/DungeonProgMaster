using UnityEngine;
using Zenject;

public class UnitInstaller : MonoInstaller
{
    [SerializeField] private CoinController coinControllerPrefab;
    [SerializeField] private SkeletonController skeletonControllerPrefab;

    public override void InstallBindings()
    {
        //регистрируем префабы
        Container.Bind<CoinController>().FromInstance(coinControllerPrefab).AsSingle();
        Container.Bind<SkeletonController>().FromInstance(skeletonControllerPrefab).AsSingle();

        //WARNING
        //при регистрации нового контроллера
        //не забудь добавить блок if в AbstractUnitControllerFactory
        //WARNING

        //регистрируем фабрику (общую для всех)
        Container.BindFactory<Unit, TransformParameters, BaseUnitController, BaseUnitController.Factory>()
            .FromFactory<AbstractUnitControllerFactory>();
    }
}