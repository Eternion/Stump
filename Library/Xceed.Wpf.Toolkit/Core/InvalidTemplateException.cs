﻿/************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2010-2012 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus edition at http://xceed.com/wpf_toolkit

   Visit http://xceed.com and follow @datagrid on Twitter

  **********************************************************************/

using System;

namespace Xceed.Wpf.Toolkit.Core
{
  public class InvalidTemplateException : Exception
  {
    #region Constructors

    public InvalidTemplateException( string message )
      : base( message )
    {
    }

    public InvalidTemplateException( string message, Exception innerException )
      : base( message, innerException )
    {
    }

    #endregion
  }
}
