using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;

namespace Stump.Server.BaseServer.Network
{
    public enum VersionCheckingSeverity
    {
        None,
        Light, // check major minor and release values
        Medium, // check revision too
        Heavy, // check patch
    }

    public static class VersionExtension
    {
        /// <summary>
        ///   Define the severity of the client version checking. Set to Light/NoCheck if you have any bugs with it.
        /// </summary>
        [Variable]
        public static VersionCheckingSeverity Severity = VersionCheckingSeverity.Light;

        /// <summary>
        /// Version for the client
        /// </summary>
        [Variable]
        public static Version VersionRequired = new Version(2, 3, 3, 40497, 1, (byte)BuildTypeEnum.RELEASE);//"2.3.3.40497.1";

        /// <summary>
        /// Actual version
        /// </summary>
        [Variable]
        public static int ActualVersion = 1304;

        /// <summary>
        /// Required version
        /// </summary>
        [Variable]
        public static int RequiredVersion = 1304;

        /// <summary>
        /// Compare the given version and the required version
        /// </summary>
        /// <param name="versionToCompare"></param>
        /// <returns></returns>
        public static bool IsUpToDate(this Version versionToCompare)
        {
            if (Severity == VersionCheckingSeverity.None)
                return true;

            else if (Severity == VersionCheckingSeverity.Light)
                return VersionRequired.major == versionToCompare.major &&
                       VersionRequired.minor == versionToCompare.minor &&
                       VersionRequired.release == versionToCompare.release;

            else if (Severity == VersionCheckingSeverity.Medium)
                return VersionRequired.major == versionToCompare.major &&
                       VersionRequired.minor == versionToCompare.minor &&
                       VersionRequired.release == versionToCompare.release &&
                       VersionRequired.revision == versionToCompare.revision;

            else if (Severity == VersionCheckingSeverity.Heavy)
                return VersionRequired.major == versionToCompare.major &&
                       VersionRequired.minor == versionToCompare.minor &&
                       VersionRequired.release == versionToCompare.release &&
                       VersionRequired.revision == versionToCompare.revision &&
                       VersionRequired.patch == versionToCompare.patch;

            return false;
        }
    }
}