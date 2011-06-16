using Stump.Core.Attributes;
using Stump.DofusProtocol.Classes;

namespace Stump.Server.BaseServer.Network
{
    public enum VersionCheckingSeverity
    {
        None,
        Light, // check major minor and release values
        Medium, // check revision too
        Heavy, // check patch
    }

    public class ClientVersion
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
        public static string VersionRequired = "2.3.3.40497.1";

        /// <summary>
        /// Actual version
        /// </summary>
        [Variable]
        public static uint ActualVersion = 1304;

        /// <summary>
        /// Required version
        /// </summary>
        [Variable]
        public static uint RequiredVersion = 1304;

        public static ClientVersion ClientVersionRequired = Parse(VersionRequired);
                                    // version can be found in games_base.xml


        public byte BuildType;
        public byte Major;
        public byte Minor;
        public byte Patch;
        public byte Release;
        public ushort Revision;

        public ClientVersion(byte major, byte minor, byte release, ushort revision, byte patch, byte buildtype)
        {
            Major = major;
            Minor = minor;
            Release = release;
            Revision = revision;
            Patch = patch;
            BuildType = buildtype;
        }

        public bool CompareVersion(Version versionToCompare)
        {
            if (Severity == VersionCheckingSeverity.None)
                return true;

            else if (Severity == VersionCheckingSeverity.Light)
                return Major == versionToCompare.major &&
                       Minor == versionToCompare.minor &&
                       Release == versionToCompare.release;

            else if (Severity == VersionCheckingSeverity.Medium)
                return Major == versionToCompare.major &&
                       Minor == versionToCompare.minor &&
                       Release == versionToCompare.release &&
                       Revision == versionToCompare.revision;

            else if (Severity == VersionCheckingSeverity.Heavy)
                return Major == versionToCompare.major &&
                       Minor == versionToCompare.minor &&
                       Release == versionToCompare.release &&
                       Revision == versionToCompare.revision &&
                       Patch == versionToCompare.patch;

            return false;
        }

        public bool CompareVersion(ClientVersion versionToCompare)
        {
            if (Severity == VersionCheckingSeverity.None)
                return true;

            else if (Severity == VersionCheckingSeverity.Light)
                return Major == versionToCompare.Major &&
                       Minor == versionToCompare.Minor &&
                       Release == versionToCompare.Release;

            else if (Severity == VersionCheckingSeverity.Medium)
                return Major == versionToCompare.Major &&
                       Minor == versionToCompare.Minor &&
                       Release == versionToCompare.Release &&
                       Revision == versionToCompare.Revision;

            else if (Severity == VersionCheckingSeverity.Heavy)
                return Major == versionToCompare.Major &&
                       Minor == versionToCompare.Minor &&
                       Release == versionToCompare.Release &&
                       Revision == versionToCompare.Revision &&
                       Patch == versionToCompare.Patch;

            return false;
        }

        public Version ToVersion()
        {
            return new Version(Major, Minor, Release, Revision, Patch, BuildType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ClientVersion)) return false;
            return Equals((ClientVersion) obj);
        }

        public static ClientVersion Parse(string str)
        {
            string[] split = str.Split('.');

            return new ClientVersion(byte.Parse(split[0]),
                                     byte.Parse(split[1]),
                                     byte.Parse(split[2]),
                                     ushort.Parse(split[3]),
                                     byte.Parse(split[4]),
                                     0);
        }

        public bool Equals(ClientVersion other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.BuildType == BuildType && other.Major == Major && other.Minor == Minor && other.Patch == Patch &&
                   other.Release == Release && other.Revision == Revision;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = BuildType.GetHashCode();
                result = (result*397) ^ Major.GetHashCode();
                result = (result*397) ^ Minor.GetHashCode();
                result = (result*397) ^ Patch.GetHashCode();
                result = (result*397) ^ Release.GetHashCode();
                result = (result*397) ^ Revision.GetHashCode();
                return result;
            }
        }
    }
}