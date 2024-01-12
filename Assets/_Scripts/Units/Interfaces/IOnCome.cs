using DPM.Infrastructure;

namespace DPM.App
{
    public interface IOnCome
    {
        public abstract void OnCome(ContactDirection contact, GameController controller);//наступили
    }

}