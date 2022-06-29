using GTA.UI;
using LemonUI.Menus;

namespace Los.Santos.Dope.Wars.GUI
{
	/// <summary>
	/// The <see cref="NativeMenuMiddle"/> class serves as the standard middle menu class, abstracts from <see cref="NativeMenu"/>
	/// </summary>
	public class NativeMenuMiddle : NativeMenu
	{
		/// <summary>
		/// The standard constructor with standard values
		/// </summary>
		/// <param name="title"></param>
		/// <param name="color"></param>
		public NativeMenuMiddle(string title, System.Drawing.Color color) : base(title)
		{
			Alignment = Alignment.Left;
			Banner.Color = color;
			Offset = new System.Drawing.PointF(Constants.ScreeSize.Width / 3, Constants.ScreeSize.Width / 3);
			UseMouse = false;
			//TitleFont = Font.Pricedown;
			Width = Constants.ScreeSize.Width / 3;
		}
	}
}
