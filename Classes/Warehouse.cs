using GTA.Math;
using Los.Santos.Dope.Wars.Classes.Base;
using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Classes
{
	/// <summary>
	/// The <see cref="Warehouse"/> class is the root element for the warehouse stash of the player.
	/// Class inherits from the base class <see cref="PlayerProperty"/>
	/// </summary>
	[XmlRoot(ElementName = nameof(Warehouse), IsNullable = false)]
	public class Warehouse : PlayerProperty
	{
		#region ctor
		/// <summary>
		/// The standard constructor for the <see cref="Warehouse"/> class
		/// </summary>
		public Warehouse(Vector3 position, Vector3 entrance, Vector3 missionMarker) : base(position, entrance, missionMarker)
		{
			Stash = new();
			Stash.Init();
		}

		/// <summary>
		/// The empty constructor for the <see cref="Warehouse"/> class
		/// </summary>
		public Warehouse()
		{
			Stash = new();
			Stash.Init();
		}
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="Stash"/> property, securely stashes the brought in player drugs and drug money
		/// </summary>
		[XmlElement(ElementName = nameof(Stash), IsNullable = false)]
		public PlayerStash Stash { get; set; }
		#endregion
	}
}
