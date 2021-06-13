using System;
using Wolf_Hack.SDK.Interfaces.Client.Entity.Structures;

namespace Wolf_Hack.SDK.Interfaces.Client.Entity
{
    internal unsafe class IClientEntity
    {
        private readonly IntPtr m_Class;

        private IClientEntity(IntPtr classPtr) => m_Class = classPtr;

        public static implicit operator IClientEntity(IntPtr Value) => new IClientEntity(Value);

        public static explicit operator IntPtr(IClientEntity Value) => Value.m_Class;

        public static implicit operator bool(IClientEntity value) => value.m_Class != IntPtr.Zero;

        public PlayerBase GetPlayer => m_Class;
    }
}