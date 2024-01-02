public interface IAttackeble
{
    public abstract void OnAttack(ContactDirection contact, GameContoller controller);//ударили
}

public interface ITakeable
{
    public abstract void OnTake(ContactDirection contact, GameContoller controller);//подобрали
}

public interface IOnCome
{
    public abstract void OnCome(ContactDirection contact, GameContoller controller);//наступили
}


public enum Tangibility //осязаемость
{
    Obstacle, //препятствие
    None
}

public enum ContactDirection
{
    Side, //сбоку (сверху, снизу, справа, слева)
    Directly, //стоя на этом объекте
}