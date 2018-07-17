using System;
using System.Linq;
using System.Collections.Generic;

namespace RdlEngine.Render.ExcelConverter
{
	internal class ExcelRow
	{
		public float YPosition { get; set; }
		public float yOffset { get; set; }
		public List<ExcelCell> Cells { get; set; }

		public ExcelRow(float yPos)
		{
			Cells = new List<ExcelCell>();
			YPosition = yPos;
			yOffset = 0;
		}

		public bool IsEmpty {
			get {
				return !Cells.Any();
			}
		}
	}
}
