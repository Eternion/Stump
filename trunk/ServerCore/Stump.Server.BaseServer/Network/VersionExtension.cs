using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;

namespace Stump.Server.BaseServer.Network
{
    public enum VersionCheckingSeverity
    {
        /// <summary>
        /// Do not check version
        /// </summary>
        None,
        /// <summary>
        /// Check major minor and release values
        /// </summary>
        Light,
        /// <summary>
        /// Check revision value too
        /// </summary>
        Medium,
        /// <summary>
        /// Check all values
        /// </summary>
        Heavy,
    }

    public static class VersionExtension
    {
        /// <summary>
        ///   Define the severity of the client version checking. Set to Light/NoCheck if you have any bugs with it.
        /// </summary>
        [Variable]
        public static VersionCheckingSeverity Severity = VersionCheckingSeverity.Light;

        /// <summary>
        /// Version for the client. 
        /// </summary>
        [Variable]
        public static Version VersionRequired = new Version(2, 3, 7, 35100, 1, (byte)BuildTypeEnum.RELEASE);

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
            switch (Severity)
            {
                case VersionCheckingSeverity.None:
                    return true;
                case VersionCheckingSeverity.Light:
                    return VersionRequired.major == versionToCompare.major &&
                           VersionRequired.minor == versionToCompare.minor &&
                           VersionRequired.release == versionToCompare.release;
                case VersionCheckingSeverity.Medium:
                    return VersionRequired.major == versionToCompare.major &&
                           VersionRequired.minor == versionToCompare.minor &&
                           VersionRequired.release == versionToCompare.release &&
                           VersionRequired.revision == versionToCompare.revision;
                case VersionCheckingSeverity.Heavy:
                    return VersionRequired.major == versionToCompare.major &&
                           VersionRequired.minor == versionToCompare.minor &&
                           VersionRequired.release == versionToCompare.release &&
                           VersionRequired.revision == versionToCompare.revision &&
                           VersionRequired.patch == versionToCompare.patch;
            }

            return false;
        }
    }
}