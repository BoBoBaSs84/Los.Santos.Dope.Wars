using GTA;
using GTA.Math;
using Los.Santos.Dope.Wars.Interfaces.Base;
using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Classes.BaseTypes;

/// <summary>
/// The <see cref="PlayerProperty"/> class is the base class for player properties. Implements the members of the <see cref="IPlayerProperty"/> interface.
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
	public Vector3 EntranceMarker { get; private set; }
	/// <inheritdoc/>
	[XmlIgnore]
	public MarkerType EntranceMarkerType { get; private set; }
	/// <inheritdoc/>
	[XmlIgnore]
	public bool EntranceMarkerCreated { get; private set; }
	/// <inheritdoc/>
	[XmlIgnore]
	public Vector3 MissionMarker { get; private set; }
	/// <inheritdoc/>
	[XmlIgnore]
	public MarkerType MissionMarkerType { get; private set; }
	/// <inheritdoc/>
	[XmlIgnore]
	public bool MissionMarkerCreated { get; private set; }
	#endregion

	#region ctor
	/// <summary>
	/// The standard constructor for the <see cref="PlayerProperty"/> class
	/// </summary>
	/// <param name="position"></param>
	/// <param name="entranceMarker"></param>
	/// <param name="missionMarker"></param>
	public PlayerProperty(Vector3 position, Vector3 entranceMarker, Vector3 missionMarker)
	{
		Position = position;
		EntranceMarker = entranceMarker;
		EntranceMarkerType = MarkerType.VerticalCylinder;
		MissionMarker = missionMarker;
		MissionMarkerType = MarkerType.VerticalCylinder;
	}

	/// <summary>
	/// The empty constructor for the <see cref="PlayerProperty"/> class
	/// </summary>
	public PlayerProperty()
	{
	}
	#endregion

	#region IPlayerProperty methods
	/// <inheritdoc/>
	public void CreateBlip(BlipSprite blipSprite = BlipSprite.BusinessForSale, BlipColor blipColor = BlipColor.White)
	{
		if (!BlipCreated)
		{
			Blip = World.CreateBlip(Position);
			Blip.Sprite = blipSprite;
			Blip.Color = blipColor;
			Blip.Scale = 0.8f;
			Blip.IsFlashing = false;
			Blip.IsShortRange = true;
			BlipCreated = !BlipCreated;
		}
	}
	/// <inheritdoc/>
	public void DeleteBlip()
	{
		if (Blip is not null && BlipCreated)
		{
			Blip.Delete();
			BlipCreated = !BlipCreated;
		}
	}
	/// <inheritdoc/>
	public void DrawEntranceMarker(Vector3 markerLocation, Color markerColor) =>
		World.DrawMarker(EntranceMarkerType, markerLocation, Vector3.Zero, Vector3.Zero, Statics.EntranceMarkerScale, markerColor);
	/// <inheritdoc/>
	public void DrawMissionMarker(Vector3 markerLocation, Color markerColor) =>
		World.DrawMarker(MissionMarkerType, markerLocation, Vector3.Zero, Vector3.Zero, Statics.MissionMarkerScale, markerColor);
	/// <inheritdoc/>
	public void UpdateBlip(BlipSprite blipSprite = BlipSprite.BusinessForSale, BlipColor blipColor = BlipColor.White)
	{
		if (Blip is not null && BlipCreated)
		{
			Blip.Sprite = blipSprite;
			Blip.Color = blipColor;
		}
	}
	#endregion
}