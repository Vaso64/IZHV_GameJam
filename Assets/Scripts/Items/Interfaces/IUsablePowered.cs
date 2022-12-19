using GameJam.Player;

namespace GameJam.Items
{
    public interface IUsablePowered : IGrabbable
    {
        void Use(Battery battery){}
        
        void StopUse(){}
    }
}