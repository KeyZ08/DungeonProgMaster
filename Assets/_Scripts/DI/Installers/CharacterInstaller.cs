using Zenject;
using DPM.Domain;

namespace DPM.App
{
    public class CharacterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindFactory<Character, Map, TransformParameters, MyCharacterController, MyCharacterController.Factory>()
                .FromFactory<CharacterFactory>();
        }
    }
}