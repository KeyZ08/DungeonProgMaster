using UnityEngine;
using Zenject;
using DPM.Domain;

namespace DPM.App
{
    public class CharacterFactory : IFactory<Character, Map, TransformParameters, MyCharacterController>
    {
        private DiContainer container;
        private MyCharacterController prefab;

        public CharacterFactory(DiContainer container)
        {
            this.container = container;
        }

        public MyCharacterController Create(Character character, Map map, TransformParameters trp)
        {
            if (prefab == null)
                prefab = Resources.Load<MyCharacterController>("Character");
            var instance = container.InstantiatePrefabForComponent<MyCharacterController>(prefab, trp.Position, trp.Rotation, trp.Parent);
            instance.Construct(character, map);
            return instance;
        }
    }
}