using Wolf_Hack.SDK.Interfaces.Client.Entity.Structures;

namespace Wolf_Hack.SDK.Interfaces.Client.Entity
{
    internal unsafe struct IClientEntity
    {
        #region Structures
        public EntityBase* GetPlayer
        {
            get
            {
                fixed (void* Class = &this)
                {
                    return (EntityBase*)(uint)Class;
                }
            }
        }

        public BaseWeapon* GetWeapon
        {
            get
            {
                fixed (void* Class = &this)
                {
                    return (BaseWeapon*)(uint)Class;
                }
            }
        }
        #endregion
    }
}