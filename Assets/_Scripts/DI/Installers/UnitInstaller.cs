using System;
using Zenject;
using DPM.Domain;

namespace DPM.App
{
    public class UnitInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            //������������ �������
            InstallFactory<Coin, CoinController>();
            InstallFactory<Chest, ChestController>();
            InstallFactory<Skeleton, SkeletonController>();
            InstallFactory<Box, BoxController>();
            InstallFactory<Barrel, BarrelController>();
            InstallFactory<Torch, TorchController>();
            InstallFactory<Finish, FinishController>();

            /*
            WARNING
                ��� ����������� ����������� ��� ������ �����
                �� ������ �������� � UnitControllerFactory
                inject ���� ������� � ���� if

                � ��� ����������� ������ ����������� - �������� ������ � UnitPrefabsHandler

                P.S. ������ Unit ������������� ���� UnitController
            WARNING
            */

            //����������� ����� �������
            Container.Bind<IUnitControllerFactory>().To<UnitControllerFactory>().AsSingle();
        }

        private void InstallFactory<TUnit, TController>() where TController : UnitController<TUnit> where TUnit : Unit
        {
            Container.Bind<IUnitControllerFactory<TController, TUnit>>().To<ConcreteUnitControllerFactory<TController, TUnit>>().AsSingle();
        }

        /// <summary>
        /// ��������� �� �� ����� ������ �������� Create ��������� ������� Unit,
        /// ����� ��� ����������� ������ ������� � ���������� � Create
        /// </summary>
        public class UnitControllerFactory : IUnitControllerFactory
        {
            [Inject] IUnitControllerFactory<CoinController, Coin> coinFactory;
            [Inject] IUnitControllerFactory<SkeletonController, Skeleton> skeletonFactory;
            [Inject] IUnitControllerFactory<ChestController, Chest> chestFactory;
            [Inject] IUnitControllerFactory<BoxController, Box> boxFactory;
            [Inject] IUnitControllerFactory<BarrelController, Barrel> barrelFactory;
            [Inject] IUnitControllerFactory<TorchController, Torch> torchFactory;
            [Inject] IUnitControllerFactory<FinishController, Finish> finishFactory;

            public BaseUnitController Create(Unit unit, TransformParameters trp)
            {
                if (unit is Coin coin)
                    return coinFactory.Create(coin, trp);
                else if (unit is Skeleton skeleton)
                    return skeletonFactory.Create(skeleton, trp);
                else if (unit is Chest chest)
                    return chestFactory.Create(chest, trp);
                else if (unit is Box box)
                    return boxFactory.Create(box, trp);
                else if (unit is Barrel barrel)
                    return barrelFactory.Create(barrel, trp);
                else if (unit is Torch torch)
                    return torchFactory.Create(torch, trp);
                else if (unit is Finish finish)
                    return finishFactory.Create(finish, trp);
                else
                    throw new NotImplementedException();
            }
        }
    }
}