using GTA.UI;
using LemonUI.Menus;

namespace Los.Santos.Dope.Wars.GUI
{
	/// <summary>
	/// The <see cref="NativeMenuRight"/> class serves as the standard right menu class, abstracts from <see cref="NativeMenu"/>
	/// </summary>
	public class NativeMenuRight : NativeMenu
	{
		/// <summary>
		/// The standard constructor with standard values
		/// </summary>
		/// <param name="title"></param>
		/// <param name="subtitle"></param>
		/// <param name="color"></param>
		public NativeMenuRight(string title, string subtitle, System.Drawing.Color color) : base(title, subtitle)
		{
			Alignment = Alignment.Right;
			Banner.Color = color;
			Offset = new System.Drawing.PointF(-Constants.ScreeSize.Width / 64, Constants.ScreeSize.Height / 36);
			UseMouse = false;
			TitleFont = Font.Pricedown;
		}
	}
}
