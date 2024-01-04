public static class Command
{
    public static void MoveForward(Character character)
    {
        character.CurrentPosition += character.Forward;
    }

    public static void RotateRight(Character character)
    {
        var currentDirection = character.CurrentDirection.ToInt();
        character.CurrentDirection = (Direction)((currentDirection + 4 - 1) % 4);
    }

    public static void RotateLeft(Character character)
    {
        var currentDirection = character.CurrentDirection.ToInt();
        character.CurrentDirection = (Direction)((currentDirection + 1) % 4);
    }
}
