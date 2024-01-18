using System;
using Zenject;
using DPM.Domain;
using System.Collections.Generic;

namespace DPM.App
{
    public class UnitInstaller : MonoInstaller
    {
        private static Dictionary<Type, IUnitControllerFactory> unitToFactory;

        public override void InstallBindings()
        {
            unitToFactory = new Dictionary<Type, IUnitControllerFactory>();

            //������������
            Bind<Coin, CoinController>();
            Bind<Chest, ChestController>();
            Bind<Skeleton, SkeletonController>();

            /* WARNING
                ������ Unit ������������� ���� UnitController
                
                ��� ����������� ����������� �����
                �� ������ �������� ������ � UnitPrefabsHandler
            */

            //����������� ����� �������
            Container.Bind<IUnitControllerFactory>().To<UnitControllerFactory>().AsSingle();
        }

        private void Bind<TUnit, TController>() where TController : UnitController<TUnit> where TUnit : Unit
        {
            unitToFactory.Add(typeof(TUnit), new ConcreteUnitControllerFactory<TController, TUnit>(Container));
        }

        /// <summary>
        /// ��������� �� �� ����� ������ �������� Create ��������� ������� Unit,
        /// ����� ��� ����������� ������ ������� � ���������� � Create
        /// </summary>
        public class UnitControllerFactory : IUnitControllerFactory
        {
            public BaseUnitController Create(Unit unit, TransformParameters trp)
            {
                var factory = unitToFactory[unit.GetType()];
                return factory.Create(unit, trp);
            }
        }
    }
}