using Zenject;
using DPM.Domain;

namespace DPM.App
{
    public class CharacterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CharacterFactory>().AsSingle();
        }
    }
}