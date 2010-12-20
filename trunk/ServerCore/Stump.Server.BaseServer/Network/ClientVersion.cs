// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using Stump.BaseCore.Framework.Attributes;
using Stump.DofusProtocol.Classes;

namespace Stump.Server.BaseServer.Network
{
    public enum VersionCheckingSeverity
    {
        NoCheck,
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
        public static VersionCheckingSeverity Severity = VersionCheckingSeverity.Medium;

        [Variable]
        public static string VersionRequired = "2.2.3.36341.1";

        [Variable]
        public static uint ActualVersion = 1280;

        [Variable]
        public static uint RequiredVersion = 1285;

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
            if (Severity == VersionCheckingSeverity.NoCheck)
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
            if (Severity == VersionCheckingSeverity.NoCheck)
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