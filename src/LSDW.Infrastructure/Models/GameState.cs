using LSDW.Abstractions.Application.Missions;
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
		Dealers = new();
		Player = new();
		Trafficking = new();
	}

	/// <summary>
	/// Initializes a instance of the game state class.
	/// </summary>
	/// <param name="dealers">The dealer instance colection to save.</param>
	/// <param name="player">The player instance to save.</param>
	/// <param name="trafficking">The trafficking instance to save.</param>
	internal GameState(ICollection<IDealer> dealers, IPlayer player, ITrafficking trafficking)
	{
		Dealers = CreateDealerStates(dealers);
		Player = CreatePlayerState(player);
		Trafficking = CreateTraffickingState(trafficking);
	}

	/// <summary>
	/// The dealers property of the game state.
	/// </summary>
	[XmlArray(nameof(Dealers), Form = XmlSchemaForm.Qualified)]
	[XmlArrayItem(XmlConstants.DealerStateRootName, Form = XmlSchemaForm.Qualified)]
	public List<DealerState> Dealers { get; set; }

	/// <summary>
	/// The player property of the game state.
	/// </summary>
	[XmlElement(nameof(Player), Form = XmlSchemaForm.Qualified)]
	public PlayerState Player { get; set; }

	/// <summary>
	/// The trafficking property of the game state.
	/// </summary>
	[XmlElement(nameof(Trafficking), Form = XmlSchemaForm.Qualified)]
	public TraffickingState Trafficking { get; set; }

	/// <summary>
	/// Should the <see cref="Dealers"/> property be serialized?
	/// </summary>
	public bool ShouldSerializeDealers() => Dealers.Any();
}
