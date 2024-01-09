using System;
using Zenject;

public class UnitInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //регистрируем фабрики
        AddBindFactory<Coin, CoinController>();
        AddBindFactory<Skeleton, OneShotSkeletonController>();
        AddBindFactory<Chest, ChestController>();
        //AddBindFactory<Skeleton, SkeletonController>();

        /*
        WARNING
            при регистрации контроллера дл€ Ќќ¬ќ√ќ юнита
            не забудь добавить в UnitControllerFactory
            inject поле фабрики и блок if

            а при регистрации нового контроллера - закинуть префаб в UnitControllersHandler

            P.S. одному Unit соостветсвует один UnitController
            возможность указани€ разных контроллеров есть, но ограничено одним контроллером
            например есть SkeletonController и OneShotSkeletonController, использовать мы можем только один из них
        WARNING
        */

        //регистраци€ общей фабрики
        Container.Bind<IUnitControllerFactory>().To<UnitControllerFactory>().AsSingle();
    }

    private void AddBindFactory<TUnit, TController>() where TController : UnitController<TUnit> where TUnit : Unit
    {
        //обеспечивает возможность указани€ разных контроллеров дл€ одного типа Unit
        Container.BindFactory<TUnit, TransformParameters, BaseUnitController, PlaceholderFactory<TUnit, TransformParameters, BaseUnitController>>()
            .FromFactory<ConcreteUnitControllerFactory<TController, TUnit>>();
    }

    /// <summary>
    /// Ѕлагодар€ ей мы можем просто вызывать Create передава€ базовый Unit и TransformParameters,
    /// далее она подт€гивает нужную фабрику и возвращает еЄ Create
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