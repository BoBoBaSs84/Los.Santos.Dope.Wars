using GTA;
using GTA.Math;

namespace Los.Santos.Dope.Wars.Contracts
{
	/// <summary>
	/// The <see cref="IPlayerProperty"/> interface is for player properties like warehouses etc.
	/// </summary>
	public interface IPlayerProperty
	{
		/// <summary>
		/// The<see cref="Blip"/> property, the blip on the map
		/// </summary>
		Blip? Blip { get; }

		/// <summary>
		/// The <see cref="BlipCreated"/> property, is the blip created
		/// </summary>
		bool BlipCreated { get; set; }

		/// <summary>
		/// The <see cref="Position"/> property, the position of the property and the blip on the map
		/// </summary>
		Vector3 Position { get; }

		/// <summary>
		/// The <see cref="Entrance"/> property, the position of the property entrance or interaction spot
		/// </summary>
		public Vector3 Entrance { get; }

		/// <summary>
		/// The <see cref="EntranceMarkerType"/> property
		/// </summary>
		public MarkerType EntranceMarkerType { get; }

		/// <summary>
		/// The <see cref="MissionMarker"/> property, the position where mission goin to start
		/// </summary>
		public Vector3 MissionMarker { get; }

		/// <summary>
		/// The <see cref="MissionMarkerType"/> property
		/// </summary>
		public MarkerType MissionMarkerType { get; }

		/// <summary>
		/// Creates the blip for the property on the map
		/// </summary>
		/// <param name="blipSprite"></param>
		/// <param name="blipColor"></param>
		void CreateBlip(BlipSprite blipSprite = BlipSprite.BusinessForSale, BlipColor blipColor = BlipColor.White);

		/// <summary>
		/// Deletes the blip for the property on the map
		/// </summary>
		void DeleteBlip();

		/// <summary>
		/// Changes the blip for the property on the map
		/// </summary>
		/// <param name="blipSprite"></param>
		/// <param name="blipColor"></param>
		void ChangeBlip(BlipSprite blipSprite = BlipSprite.BusinessForSale, BlipColor blipColor = BlipColor.White);
	}
}
