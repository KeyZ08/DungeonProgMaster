using DPM.Infrastructure;

namespace DPM.App
{
    public interface IAttackable
    {
        public abstract void OnAttack(ContactDirection contact, GameController controller, int damage);//ударили
    }
}