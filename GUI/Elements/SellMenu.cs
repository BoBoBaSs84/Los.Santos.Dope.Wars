using GTA.UI;
using LemonUI.Menus;

namespace Los.Santos.Dope.Wars.GUI.Elements;

/// <summary>
/// The <see cref="SellMenu"/> class serves as the sell menu at the left of the screen, abstracts from <see cref="NativeMenu"/>
/// </summary>
public class SellMenu : NativeMenu
{
	#region properties
	/// <summary>
	/// The <see cref="SellMenuShown"/> property
	/// </summary>
	public bool SellMenuShown { get; private set; }

	/// <summary>
	/// The <see cref="SellMenuClosed"/> property
	/// </summary>
	public bool SellMenuClosed { get; private set; }
	#endregion

	#region ctor
	/// <summary>
	/// The standard constructor with standard values
	/// </summary>
	/// <param name="title"></param>
	/// <param name="subtitle"></param>
	/// <param name="color"></param>
	public SellMenu(string title, string subtitle, Color color) : base(title, subtitle)
	{
		Alignment = Alignment.Right;
		Banner.Color = color;
		ItemCount = CountVisibility.Always;
		Offset = new PointF(Statics.ScreeSize.Width / 64, Statics.ScreeSize.Height / 36);
		UseMouse = false;
		TitleFont = GTA.UI.Font.Pricedown;
		Closed += SellMenu_Closed;
		Shown += SellMenu_Shown;
	}
	#endregion

	#region private methods
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
	#endregion
}
