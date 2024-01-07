using Zenject;

public class CharacterFactory : IFactory<Character, Map, GameController, TransformParameters, MyCharacterController>
{
    private DiContainer container;

    public CharacterFactory(DiContainer container)
    {
        this.container = container;
    }

    public MyCharacterController Create(Character character, Map map, GameController controller, TransformParameters trp)
    {
        var prefab = container.Resolve<MyCharacterController>();
        var instance = container.InstantiatePrefabForComponent<MyCharacterController>(prefab, trp.Position, trp.Rotation, trp.Parent);
        instance.Construct(character, map, controller);
        return instance;
    }
}