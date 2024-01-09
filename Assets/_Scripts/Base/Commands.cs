public interface ICommand
{
    void Action(GameController controller, MyCharacterController c, ICommand nextStep = null);
}

public class MoveForwardCommand : ICommand
{
    public void Action(GameController controller, MyCharacterController c, ICommand nextStep)
    {
        var character = c.Character;
        character.CurrentPosition += character.Forward;
        var isNextMoveFree = nextStep is MoveForwardCommand && c.IsNextMoveFree(character.CurrentPosition, character.CurrentDirection);
        c.Visualizer.MoveTo(controller.GetPosByMap(character.CurrentPosition), isNextMoveFree);
    }
}

public class RotateRightCommand : ICommand
{
    public void Action(GameController controller, MyCharacterController c, ICommand nextStep)
    {
        var character = c.Character;
        var currentDirection = character.CurrentDirection.ToInt();
        character.CurrentDirection = (Direction)((currentDirection + 4 - 1) % 4);
        c.Visualizer.TurnTo(character.CurrentDirection);
    }
}

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
                    attackeble.OnAttack(ContactDirection.Side, controller);
            }, nextAlsoAttack);
    }
}

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