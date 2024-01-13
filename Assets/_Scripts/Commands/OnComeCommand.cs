using DPM.Infrastructure;

namespace DPM.App
{
    public class OnComeCommand : ICommand
    {
        public void Action(GameController controller, MyCharacterController c, ICommand nextStep)
        {
            var character = c.Character;
            var unit = controller.GetUnitController(character.CurrentPosition);
            if (unit != null && unit is IOnCome onComeable)
                onComeable.OnCome(ContactDirection.Directly, controller);
        }
    }
}