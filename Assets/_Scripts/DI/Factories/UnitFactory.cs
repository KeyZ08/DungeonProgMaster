using System;
using Zenject;

public class ConcreteUnitControllerFactory<TController, TUnit> : IFactory<TUnit, TransformParameters, TController>
    where TController : UnitController<TUnit> where TUnit : Unit
{
    private DiContainer container;

    public ConcreteUnitControllerFactory(DiContainer container)
    {
        this.container = container;
    }

    public TController Create(TUnit unit, TransformParameters trp)
    {
        var prefab = container.Resolve<TController>();
        var instance = container.InstantiatePrefabForComponent<TController>(prefab, trp.Position, trp.Rotation, trp.Parent);
        instance.Construct(unit);
        return instance;
    }
}

public class AbstractUnitControllerFactory : IFactory<Unit, TransformParameters, BaseUnitController>
{
    private DiContainer container;

    public AbstractUnitControllerFactory(DiContainer container)
    {
        this.container = container;
    }

    public BaseUnitController Create(Unit unit, TransformParameters trp)
    {
        if (unit is Coin coin)
            return new ConcreteUnitControllerFactory<CoinController, Coin>(container).Create(coin, trp);
        else if (unit is Skeleton skeleton)
            return new ConcreteUnitControllerFactory<SkeletonController, Skeleton>(container).Create(skeleton, trp);
        else
            throw new NotImplementedException();
    }
}