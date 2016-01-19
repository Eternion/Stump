namespace Stump.DofusProtocol.Enums.Extensions
{
    public static class DirectionsEnumExtensions
    {
        public static DirectionsEnum GetOpposedDirection(this DirectionsEnum direction) => (DirectionsEnum)(((int)direction + 4) % 8);
    }
}