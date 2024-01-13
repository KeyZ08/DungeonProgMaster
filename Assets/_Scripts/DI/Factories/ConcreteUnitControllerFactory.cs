using Zenject;
using DPM.Domain;

namespace DPM.App
{
    public class ConcreteUnitControllerFactory<TController, TUnit> : IUnitControllerFactory<TController, TUnit>
    where TController : UnitController<TUnit> where TUnit : Unit
    {
        private IInstantiator container;
        [Inject] UnitPrefabsHandlerScriptableObject prefabs;

        public ConcreteUnitControllerFactory(IInstantiator container)
        {
            this.container = container;
        }

        public TController Create(TUnit unit, TransformParameters trp)
        {
            var prefab = prefabs.GetPrefab(typeof(TController));
            var instance = container.InstantiatePrefabForComponent<TController>(prefab, trp.Position, trp.Rotation, trp.Parent);
            instance.Construct(unit);
            return instance;
        }
    }
}