using UnityEngine;
using Zenject;

public class CharacterInstaller : MonoInstaller
{
    [SerializeField] private MyCharacterController characterPrefab;
    public override void InstallBindings()
    {
        Container.Bind<MyCharacterController>().FromInstance(characterPrefab).AsSingle();

        Container.BindFactory<Character, Map, GameController, TransformParameters, MyCharacterController, MyCharacterController.Factory>()
            .FromFactory<CharacterFactory>();
    }
}