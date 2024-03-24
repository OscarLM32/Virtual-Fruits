using Enemies;
using System;

namespace Extensions
{
    public static class EnemyTypeExtensions
    {
        [Obsolete] //This can now be replaced by the EnumToPascalCase extension
        //At the current time all enemy addressables are going to have their address written as their type in PascalCase
        public static string GetAddressableKey(this EnemyType type)
        {
            string address = type.ToString();
            char aux = address[0];

            address = address.ToLower();
            address = aux + address.Substring(1);

            return address;
        }
    }
}