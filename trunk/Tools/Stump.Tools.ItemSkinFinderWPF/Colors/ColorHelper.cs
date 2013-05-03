#region License GNU GPL
// ColorConverter.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;

namespace Stump.Tools.ItemSkinFinderWPF.Colors
{
    public class ColorHelper
    {
        public static CIEXYZ RGBtoXYZ(int red, int green, int blue)
        {
            // normalize red, green, blue values
            double rLinear = (double)red / 255.0;
            double gLinear = (double)green / 255.0;
            double bLinear = (double)blue / 255.0;

            // convert to a sRGB form
            double r = ( rLinear > 0.04045 ) ? Math.Pow(( rLinear + 0.055 ) / (
                1 + 0.055 ), 2.2) : ( rLinear / 12.92 );
            double g = ( gLinear > 0.04045 ) ? Math.Pow(( gLinear + 0.055 ) / (
                1 + 0.055 ), 2.2) : ( gLinear / 12.92 );
            double b = ( bLinear > 0.04045 ) ? Math.Pow(( bLinear + 0.055 ) / (
                1 + 0.055 ), 2.2) : ( bLinear / 12.92 );

            // converts
            return new CIEXYZ(
                ( r * 0.4124 + g * 0.3576 + b * 0.1805 ),
                ( r * 0.2126 + g * 0.7152 + b * 0.0722 ),
                ( r * 0.0193 + g * 0.1192 + b * 0.9505 )
                );
        }

        /// <summary>
        /// Converts RGB to CIELab.
        /// </summary>
        public static CIELAB RGBtoLAB(int red, int green, int blue)
        {
            var xyz = RGBtoXYZ(red, green, blue);
            return XYZtoLAB(xyz.X, xyz.Y, xyz.Z);
        }

        /// <summary>
        /// XYZ to L*a*b* transformation function.
        /// </summary>
        private static double Fxyz(double t)
        {
            return ( ( t > 0.008856 ) ? Math.Pow(t, ( 1.0 / 3.0 )) : ( 7.787 * t + 16.0 / 116.0 ) );
        }

        /// <summary>
        /// Converts CIEXYZ to CIELab.
        /// </summary>
        public static CIELAB XYZtoLAB(double x, double y, double z)
        {
            CIELAB lab = CIELAB.Empty;

            lab.L = 116.0 * Fxyz(y / CIEXYZ.D65.Y) - 16;
            lab.A = 500.0 * ( Fxyz(x / CIEXYZ.D65.X) - Fxyz(y / CIEXYZ.D65.Y) );
            lab.B = 200.0 * ( Fxyz(y / CIEXYZ.D65.Y) - Fxyz(z / CIEXYZ.D65.Z) );

            return lab;
        }

        public static double GetColorsDistance(RGB color1, RGB color2)
        {
            var lab1 = RGBtoLAB(color1.Red, color1.Green, color1.Blue);
            var lab2 = RGBtoLAB(color2.Red, color2.Green, color2.Blue);

            return Math.Sqrt(( lab1.L - lab2.L ) * ( lab1.L - lab2.L ) + ( lab1.A - lab2.A ) * ( lab1.A - lab2.A ) + ( lab1.B - lab2.B ) * ( lab1.B - lab2.B ));
        }
    }
}