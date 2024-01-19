using Zenject;
using DPM.Domain;
using System;

namespace DPM.App
{
    public class ConcreteUnitControllerFactory<TController, TUnit> : IUnitControllerFactory<TController, TUnit>
    where TController : UnitController<TUnit> where TUnit : Unit
    {
        private IInstantiator container;
        private Type controllerType;
        UnitPrefabsHandlerScriptableObject prefabs;

        public ConcreteUnitControllerFactory(DiContainer container)
        {
            this.container = container;
            controllerType = typeof(TController);
            prefabs = container.Resolve<UnitPrefabsHandlerScriptableObject>();
            if(prefabs == null) throw new NullReferenceException("UnitPrefabsHandlerScriptableObject");
        }

        public TController Create(TUnit unit, TransformParameters trp)
        {
            var prefab = prefabs.GetPrefab(controllerType);
            var instance = container.InstantiatePrefabForComponent<TController>(prefab);
            instance.transform.SetParent(trp.Parent);
            instance.transform.position = trp.Position;
            instance.Construct(unit);
            return instance;
        }

        BaseUnitController IUnitControllerFactory.Create(Unit unit, TransformParameters trp)
        {
            return Create(unit as TUnit, trp);
        }
    }
}