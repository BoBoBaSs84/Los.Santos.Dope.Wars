using GTA.UI;
using LemonUI.Menus;
using System;

namespace Los.Santos.Dope.Wars.GUI.Elements
{
	/// <summary>
	/// The <see cref="StatisticsMenu"/> class serves as the statistics menu in the middle of the screen, abstracts from <see cref="NativeMenu"/>
	/// </summary>
	public class StatisticsMenu : NativeMenu
	{
		/// <summary>
		/// The <see cref="StatisticsMenuShown"/> property
		/// </summary>
		public bool StatisticsMenuShown { get; private set; }

		/// <summary>
		/// The <see cref="StatisticsMenuClosed"/> property
		/// </summary>
		public bool StatisticsMenuClosed { get; private set; }


		/// <summary>
		/// The standard constructor with standard values
		/// </summary>
		/// <param name="title"></param>
		/// <param name="subtitle"></param>
		/// <param name="color"></param>
		public StatisticsMenu(string title, string subtitle, System.Drawing.Color color) : base(title, subtitle)
		{
			Alignment = Alignment.Left;
			Banner.Color = color;
			ItemCount = CountVisibility.Never;
			Offset = new System.Drawing.PointF(Constants.ScreeSize.Width / 3, Constants.ScreeSize.Width / 3);
			UseMouse = false;
			Width = Constants.ScreeSize.Width / 3;
			Closed += StatisticsMenu_Closed;
			Shown += StatisticsMenu_Shown;
		}

		private void StatisticsMenu_Shown(object sender, EventArgs e)
		{
			StatisticsMenuClosed = false;
			StatisticsMenuShown = true;
		}

		private void StatisticsMenu_Closed(object sender, EventArgs e)
		{
			StatisticsMenuClosed = true;
			StatisticsMenuShown = false;
		}
	}
}
