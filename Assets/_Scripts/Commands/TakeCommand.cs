using DPM.Infrastructure;

namespace DPM.App
{
    public class TakeCommand : ICommand
    {
        public void Action(GameController controller, MyCharacterController c, ICommand nextStep)
        {
            var character = c.Character;
            var forwardInMap = character.CurrentPosition + character.Forward;
            var unit = controller.GetUnitController(forwardInMap);
            if (unit != null && unit is ITakeable takeable)
                takeable.OnTake(ContactDirection.Side, controller);
        }
    }
}
