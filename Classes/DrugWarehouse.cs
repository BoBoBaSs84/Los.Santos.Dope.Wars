using GTA;
using GTA.Math;
using Los.Santos.Dope.Wars.Contracts;
using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Classes
{
	/// <summary>
	/// The <see cref="DrugWarehouse"/> class is the root element for the warehouse drug stash of the player
	/// Implements the members of the <see cref="IPlayerProperty"/> interface
	/// </summary>
	[XmlRoot(ElementName = nameof(DrugWarehouse), IsNullable = false)]
	public class DrugWarehouse : IPlayerProperty
	{
		#region ctor
		/// <summary>
		/// The <see cref="DrugWarehouse"/> empty constructor
		/// </summary>
		public DrugWarehouse()
		{
			Blip = null!;
			DrugStash = new DrugStash();
			DrugStash.Init();
		}

		/// <summary>
		/// The <see cref="DrugWarehouse"/> standard constructor with standard values
		/// </summary>
		/// <param name="location"></param>
		/// <param name="entrance"></param>
		/// <param name="missionMarker"></param>
		public DrugWarehouse(Vector3 location, Vector3 entrance, Vector3 missionMarker)
		{
			Position = location;
			EntrancePosition = entrance;
			MissionMarkerPosition = missionMarker;
			DrugStash = new DrugStash();
			DrugStash.Init();
		}
		#endregion

		#region properties
		/// <inheritdoc/>
		[XmlIgnore]
		public Blip? Blip { get; set; }

		/// <inheritdoc/>
		[XmlIgnore]
		public bool BlipCreated { get; set; }

		/// <inheritdoc/>
		[XmlIgnore]
		public Vector3 Position { get; private set; }

		/// <inheritdoc/>
		[XmlIgnore]
		public Vector3 EntrancePosition { get; private set; }

		/// <inheritdoc/>
		[XmlIgnore]
		public Vector3 MissionMarkerPosition { get; private set; }

		/// <inheritdoc/>
		[XmlIgnore]
		public MarkerType EntranceMarkerType { get; private set; }

		/// <inheritdoc/>
		[XmlIgnore]
		public MarkerType MissionMarkerType { get; private set; }

		/// <summary>
		/// The <see cref="DrugStash"/> property, securely stashes the brought in player drugs and drug money
		/// </summary>
		[XmlElement(ElementName = nameof(DrugStash), IsNullable = false)]
		public DrugStash DrugStash { get; set; }
		#endregion

		#region public methods
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
		public void ChangeBlip(BlipSprite blipSprite = BlipSprite.BusinessForSale, BlipColor blipColor = BlipColor.White)
		{
			if (BlipCreated)
			{
				Blip!.Sprite = blipSprite;
				Blip!.Color = blipColor;				
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
