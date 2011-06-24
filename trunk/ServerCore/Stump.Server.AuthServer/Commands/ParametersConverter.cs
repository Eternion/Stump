
using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;

namespace Stump.Server.AuthServer.Commands
{
    public static class ParametersConverter
    {
        public static ConverterHandler<RoleEnum> RoleConverter = (entry, trigger) =>
        {
            RoleEnum result;
            if (Enum.TryParse(entry, true, out result))
            {
                return result;
            }

            byte value;
            if (byte.TryParse(entry, out value))
                return (RoleEnum) Enum.ToObject(typeof (RoleEnum), value);

            throw new ArgumentException("entry is not RoleEnum");
        };
    }
}