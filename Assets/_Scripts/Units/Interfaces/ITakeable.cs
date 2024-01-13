using DPM.Infrastructure;

namespace DPM.App
{
    public interface ITakeable
    {
        public abstract void OnTake(ContactDirection contact, GameController controller);//подобрали
    }
}