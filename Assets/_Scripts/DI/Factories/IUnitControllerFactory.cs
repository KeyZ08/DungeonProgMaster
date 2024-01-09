public interface IUnitControllerFactory
{
    BaseUnitController Create(Unit unit, TransformParameters trp);
}