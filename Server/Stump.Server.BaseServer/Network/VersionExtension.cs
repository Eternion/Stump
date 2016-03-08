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
        [Variable(true)]
        public static VersionCheckingSeverity Severity = VersionCheckingSeverity.Light;

        /// <summary>
        /// Version for the client. 
        /// </summary>
        [Variable(true)]
        public static Version ExpectedVersion = new Version(2, 33, 0, 101615, 2, (sbyte) BuildTypeEnum.RELEASE);

        /// <summary>
        /// Actual version
        /// </summary>
        [Variable(true)]
        public static int ActualProtocol = 1692;

        /// <summary>
        /// Required version
        /// </summary>
        [Variable(true)]
        public static int ProtocolRequired = 1692;

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
                    return ExpectedVersion.major == versionToCompare.major &&
                           ExpectedVersion.minor == versionToCompare.minor &&
                           ExpectedVersion.release == versionToCompare.release;
                case VersionCheckingSeverity.Medium:
                    return ExpectedVersion.major == versionToCompare.major &&
                           ExpectedVersion.minor == versionToCompare.minor &&
                           ExpectedVersion.release == versionToCompare.release &&
                           ExpectedVersion.revision == versionToCompare.revision;
                case VersionCheckingSeverity.Heavy:
                    return ExpectedVersion.major == versionToCompare.major &&
                           ExpectedVersion.minor == versionToCompare.minor &&
                           ExpectedVersion.release == versionToCompare.release &&
                           ExpectedVersion.revision == versionToCompare.revision &&
                           ExpectedVersion.patch == versionToCompare.patch;
            }
            
            return false;
        }

        public static VersionExtended ToVersionExtended(this Version version, sbyte install, sbyte technology)
        {
            return new VersionExtended(version.major, version.minor, version.release, version.revision, version.patch,
                version.buildType, install, technology);
        }
    }
}