using System;
using Zenject;

public class UnitInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //������������ �������
        AddBindFactory<Coin, CoinController>();
        AddBindFactory<Skeleton, OneShotSkeletonController>();
        AddBindFactory<Chest, ChestController>();
        //AddBindFactory<Skeleton, SkeletonController>();

        /*
        WARNING
            ��� ����������� ����������� ��� ������ �����
            �� ������ �������� � UnitControllerFactory
            inject ���� ������� � ���� if

            � ��� ����������� ������ ����������� - �������� ������ � UnitControllersHandler

            P.S. ������ Unit ������������� ���� UnitController
            ����������� �������� ������ ������������ ����, �� ���������� ����� ������������
            �������� ���� SkeletonController � OneShotSkeletonController, ������������ �� ����� ������ ���� �� ���
        WARNING
        */

        //����������� ����� �������
        Container.Bind<IUnitControllerFactory>().To<UnitControllerFactory>().AsSingle();
    }

    private void AddBindFactory<TUnit, TController>() where TController : UnitController<TUnit> where TUnit : Unit
    {
        //������������ ����������� �������� ������ ������������ ��� ������ ���� Unit
        Container.BindFactory<TUnit, TransformParameters, BaseUnitController, PlaceholderFactory<TUnit, TransformParameters, BaseUnitController>>()
            .FromFactory<ConcreteUnitControllerFactory<TController, TUnit>>();
    }

    /// <summary>
    /// ��������� �� �� ����� ������ �������� Create ��������� ������� Unit � TransformParameters,
    /// ����� ��� ����������� ������ ������� � ���������� � Create
    /// </summary>
    public class UnitControllerFactory : IUnitControllerFactory
    {
        [Inject] PlaceholderFactory<Coin, TransformParameters, BaseUnitController> coinFactory;
        [Inject] PlaceholderFactory<Skeleton, TransformParameters, BaseUnitController> skeletonFactory;
        [Inject] PlaceholderFactory<Chest, TransformParameters, BaseUnitController> chestFactory;

        public BaseUnitController Create(Unit unit, TransformParameters trp)
        {
            if (unit is Coin coin)
                return coinFactory.Create(coin, trp);
            else if (unit is Skeleton skeleton)
                return skeletonFactory.Create(skeleton, trp);
            else if (unit is Chest chest)
                return chestFactory.Create(chest, trp);
            else
                throw new NotImplementedException();
        }
    }
}