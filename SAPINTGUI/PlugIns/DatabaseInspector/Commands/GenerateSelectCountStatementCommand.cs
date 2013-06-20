#region License

// Copyright 2005-2009 Paul Kohler (http://pksoftware.net/MiniSqlQuery/). All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (Ms-PL)
// http://minisqlquery.codeplex.com/license
#endregion

using System;
using System.IO;
using MiniSqlQuery.Core;
using MiniSqlQuery.Core.DbModel;

namespace SAPINT.Gui.PlugIns.DatabaseInspector.Commands
{
	/// <summary>The generate select count statement command.</summary>
	public class GenerateSelectCountStatementCommand : GenerateStatementCommandBase
	{
		/// <summary>Initializes a new instance of the <see cref="GenerateSelectCountStatementCommand"/> class.</summary>
		public GenerateSelectCountStatementCommand()
			: base("Generate Select COUNT(*) Statement")
		{
		}

		/// <summary>Execute the command.</summary>
		public override void Execute()
		{
			IQueryEditor editor = ActiveFormAsSqlQueryEditor;
			string tableName = HostWindow.DatabaseInspector.RightClickedTableName;
			DbModelInstance model = HostWindow.DatabaseInspector.DbSchema;

			if (tableName != null && editor != null)
			{
				StringWriter sql = new StringWriter();
				SqlWriter.WriteSelectCount(sql, GetTableOrViewByName(model, tableName));
				editor.InsertText(sql.ToString());
			}
		}
	}
}