using DPM.Infrastructure;

namespace DPM.App
{
    public class AttackCommand : ICommand
    {
        public void Action(GameController controller, MyCharacterController c, ICommand nextStep)
        {
            var character = c.Character;
            var nextAlsoAttack = nextStep is AttackCommand;
            var forwardInMap = character.CurrentPosition + character.Forward;
            var unit = controller.GetUnitController(forwardInMap);
            c.Visualizer.Attack(
                () =>
                {
                    if (unit != null && unit is IAttackable attackeble)
                        attackeble.OnAttack(ContactDirection.Side, controller, 50);
                }, nextAlsoAttack);
        }
    }
}
