using DPM.Infrastructure;

namespace DPM.App
{
    public class RotateLeftCommand : ICommand
    {
        public void Action(GameController controller, MyCharacterController c, ICommand nextStep)
        {
            var character = c.Character;
            var currentDirection = character.CurrentDirection.ToInt();
            character.CurrentDirection = (Direction)((currentDirection + 1) % 4);
            c.Visualizer.TurnTo(character.CurrentDirection);
        }
    }
}
