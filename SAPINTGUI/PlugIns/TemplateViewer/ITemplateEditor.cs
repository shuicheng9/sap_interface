#region License

// Copyright 2005-2009 Paul Kohler (http://pksoftware.net/MiniSqlQuery/). All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (Ms-PL)
// http://minisqlquery.codeplex.com/license
#endregion

using System;
using MiniSqlQuery.Core;

namespace SAPINT.Gui.PlugIns.TemplateViewer
{
	/// <summary>The i template editor.</summary>
	public interface ITemplateEditor : IPerformTask
	{
		/// <summary>The run template.</summary>
		void RunTemplate();
	}
}