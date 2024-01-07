using UnityEngine;
using Zenject;

public class UnitInstaller : MonoInstaller
{
    [SerializeField] private CoinController coinControllerPrefab;
    [SerializeField] private SkeletonController skeletonControllerPrefab;

    public override void InstallBindings()
    {
        //������������ �������
        Container.Bind<CoinController>().FromInstance(coinControllerPrefab).AsSingle();
        Container.Bind<SkeletonController>().FromInstance(skeletonControllerPrefab).AsSingle();

        //WARNING
        //��� ����������� ������ �����������
        //�� ������ �������� ���� if � AbstractUnitControllerFactory
        //WARNING

        //������������ ������� (����� ��� ����)
        Container.BindFactory<Unit, TransformParameters, BaseUnitController, BaseUnitController.Factory>()
            .FromFactory<AbstractUnitControllerFactory>();
    }
}