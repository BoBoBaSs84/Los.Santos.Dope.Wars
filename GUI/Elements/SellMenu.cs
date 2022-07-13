using GTA.UI;
using LemonUI.Menus;
using System;

namespace Los.Santos.Dope.Wars.GUI.Elements
{
	/// <summary>
	/// The <see cref="SellMenu"/> class serves as the sell menu at the left of the screen, abstracts from <see cref="NativeMenu"/>
	/// </summary>
	public class SellMenu : NativeMenu
	{
		/// <summary>
		/// The <see cref="SellMenuShown"/> property
		/// </summary>
		public bool SellMenuShown { get; private set; }

		/// <summary>
		/// The <see cref="SellMenuClosed"/> property
		/// </summary>
		public bool SellMenuClosed { get; private set; }

		/// <summary>
		/// The standard constructor with standard values
		/// </summary>
		/// <param name="title"></param>
		/// <param name="subtitle"></param>
		/// <param name="color"></param>
		public SellMenu(string title, string subtitle, System.Drawing.Color color) : base(title, subtitle)
		{
			Alignment = Alignment.Right;
			Banner.Color = color;
			ItemCount = CountVisibility.Always;
			Offset = new System.Drawing.PointF(-Constants.ScreeSize.Width / 64, Constants.ScreeSize.Height / 36);
			UseMouse = false;
			TitleFont = Font.Pricedown;
			Closed += SellMenu_Closed;
			Shown += SellMenu_Shown;
		}

		private void SellMenu_Shown(object sender, EventArgs e)
		{
			SellMenuClosed = false;
			SellMenuShown = true;
		}

		private void SellMenu_Closed(object sender, EventArgs e)
		{
			SellMenuClosed = true;
			SellMenuShown = false;
		}
	}
}
