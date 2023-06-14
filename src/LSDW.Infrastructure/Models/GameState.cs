using LSDW.Abstractions.Domain.Models;
using LSDW.Infrastructure.Constants;
using System.Xml.Schema;
using System.Xml.Serialization;
using static LSDW.Infrastructure.Factories.InfrastructureFactory;

namespace LSDW.Infrastructure.Models;

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
	internal GameState(IPlayer player, ICollection<IDealer> dealers)
	{
		Player = CreatePlayerState(player);
		Dealers = CreateDealerStates(dealers);
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
