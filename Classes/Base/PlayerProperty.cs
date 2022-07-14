using GTA;
using GTA.Math;
using Los.Santos.Dope.Wars.Contracts;
using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Classes.Base
{
	/// <summary>
	/// The <see cref="PlayerProperty"/> class is the base class for player properties.
	/// Implements the members of the <see cref="IPlayerProperty"/> interface
	/// </summary>
	public abstract class PlayerProperty : IPlayerProperty
	{
		#region properties
		/// <inheritdoc/>
		[XmlIgnore]
		public Blip? Blip { get; private set; }
		/// <inheritdoc/>
		[XmlIgnore]
		public bool BlipCreated { get; set; }
		/// <inheritdoc/>
		[XmlIgnore]
		public Vector3 Position { get; private set; }
		/// <inheritdoc/>
		[XmlIgnore]
		public Vector3 Entrance { get; private set; }
		/// <inheritdoc/>
		[XmlIgnore]
		public MarkerType EntranceMarkerType { get; private set; }
		/// <inheritdoc/>
		[XmlIgnore]
		public Vector3 MissionMarker { get; private set; }
		/// <inheritdoc/>
		[XmlIgnore]
		public MarkerType MissionMarkerType { get; private set; }
		#endregion

		#region ctor
		/// <summary>
		/// The standard constructor for the <see cref="PlayerProperty"/> class
		/// </summary>
		/// <param name="position"></param>
		/// <param name="entrance"></param>
		/// <param name="missionMarker"></param>
		public PlayerProperty(Vector3 position, Vector3 entrance, Vector3 missionMarker)
		{
			Position = position;
			Entrance = entrance;
			MissionMarker = missionMarker;
		}

		/// <summary>
		/// The empty constructor for the <see cref="PlayerProperty"/> class
		/// </summary>
		public PlayerProperty()
		{
		}
		#endregion

		#region public methods
		/// <inheritdoc/>
		public void ChangeBlip(BlipSprite blipSprite = BlipSprite.BusinessForSale, BlipColor blipColor = BlipColor.White)
		{
			if (BlipCreated)
			{
				Blip!.Sprite = blipSprite;
				Blip!.Color = blipColor;
			}
		}
		/// <inheritdoc/>
		public void CreateBlip(BlipSprite blipSprite = BlipSprite.BusinessForSale, BlipColor blipColor = BlipColor.White)
		{
			if (!BlipCreated)
			{
				Blip = World.CreateBlip(Position);
				Blip.Sprite = blipSprite;
				Blip.Color = blipColor;
				Blip.IsFlashing = false;
				Blip.IsShortRange = true;
				BlipCreated = !BlipCreated;
			}
		}
		/// <inheritdoc/>
		public void DeleteBlip()
		{
			if (BlipCreated)
				Blip!.Delete();
		}
		#endregion
	}
}