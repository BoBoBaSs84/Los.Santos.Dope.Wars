using GTA.UI;
using LemonUI.Menus;
using System;

namespace Los.Santos.Dope.Wars.GUI.Elements
{
	/// <summary>
	/// The <see cref="BuyMenu"/> class serves as the buy menu at the right of the screen, abstracts from <see cref="NativeMenu"/>
	/// </summary>
	public class BuyMenu : NativeMenu
	{
		/// <summary>
		/// The <see cref="BuyMenuShown"/> property
		/// </summary>
		public bool BuyMenuShown { get; private set; }

		/// <summary>
		/// The <see cref="BuyMenuClosed"/> property
		/// </summary>
		public bool BuyMenuClosed { get; private set; }

		/// <summary>
		/// The standard constructor with standard values
		/// </summary>
		/// <param name="title"></param>
		/// <param name="subtitle"></param>
		/// <param name="color"></param>
		public BuyMenu(string title, string subtitle, System.Drawing.Color color) : base(title, subtitle)
		{
			Alignment = Alignment.Left;
			Banner.Color = color;
			ItemCount = CountVisibility.Always;
			Offset = new System.Drawing.PointF(Constants.ScreeSize.Width / 64, Constants.ScreeSize.Height / 36);
			UseMouse = false;
			TitleFont = Font.Pricedown;
			Closed += BuyMenu_Closed;
			Shown += BuyMenu_Shown;
		}

		private void BuyMenu_Shown(object sender, EventArgs e)
		{
			BuyMenuClosed = false;
			BuyMenuShown = true;
		}

		private void BuyMenu_Closed(object sender, EventArgs e)
		{
			BuyMenuClosed = true;
			BuyMenuShown = false;
		}
	}
}
