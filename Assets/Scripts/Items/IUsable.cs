namespace GameJam.Items
{
    public interface IUsable : IGrabbable
    {
        void Use(){}
        
        void StopUse(){}
    }
}