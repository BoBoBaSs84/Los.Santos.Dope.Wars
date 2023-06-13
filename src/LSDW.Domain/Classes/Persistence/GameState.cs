using LSDW.Domain.Constants;
using LSDW.Domain.Factories;
using LSDW.Domain.Interfaces.Actors;
using LSDW.Domain.Interfaces.Models;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LSDW.Domain.Classes.Persistence;

/// <summary>
/// The game state class.
/// </summary>
[XmlRoot(XmlConstants.GameStateRootName, Namespace = XmlConstants.NameSpace)]
public sealed class GameState
{
	/// <summary>
	/// Initializes a instance of the game state class.
	/// </summary>
	public GameState()
	{
		Player = new();
		Dealers = new();
	}

	/// <summary>
	/// Initializes a instance of the game state class.
	/// </summary>
	internal GameState(IPlayer player, IEnumerable<IDealer> dealers)
	{
		Player = DomainFactory.CreatePlayerState(player);
		Dealers = DomainFactory.CreateDealerStates(dealers);
	}

	/// <summary>
	/// The player property of the game state.
	/// </summary>
	[XmlElement(nameof(Player), Form = XmlSchemaForm.Qualified)]
	public PlayerState Player { get; set; }

	/// <summary>
	/// The dealers property of the game state.
	/// </summary>
	[XmlArray(nameof(Dealers), Form = XmlSchemaForm.Qualified)]
	[XmlArrayItem(XmlConstants.DealerStateRootName, Form = XmlSchemaForm.Qualified)]
	public List<DealerState> Dealers { get; set; }

	/// <summary>
	/// Should the <see cref="Dealers"/> property be serialized?
	/// </summary>
	public bool ShouldSerializeDealers() => Dealers.Any();
}
