using DPM.Domain;

namespace DPM.App
{
    public interface IUnitControllerFactory
    {
        BaseUnitController Create(Unit unit, TransformParameters trp);
    }

    public interface IUnitControllerFactory<TController, TUnit>
        where TController : UnitController<TUnit>
        where TUnit : Unit
    {
        TController Create(TUnit unit, TransformParameters trp);
    }
}