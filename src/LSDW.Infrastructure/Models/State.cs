using LSDW.Abstractions.Domain.Models;
using LSDW.Infrastructure.Constants;
using System.Xml.Schema;
using System.Xml.Serialization;
using static LSDW.Infrastructure.Factories.InfrastructureFactory;

namespace LSDW.Infrastructure.Models;

/// <summary>
/// The state class.
/// </summary>
[XmlRoot(XmlConstants.StateRootName, Namespace = XmlConstants.NameSpace)]
public sealed class State
{
	/// <summary>
	/// Initializes a instance of the state class.
	/// </summary>
	public State()
	{
		Dealers = new();
		Player = new();
	}

	/// <summary>
	/// Initializes a instance of the state class.
	/// </summary>
	/// <param name="dealers">The dealer instance colection to save.</param>
	/// <param name="player">The player instance to save.</param>
	internal State(ICollection<IDealer> dealers, IPlayer player)
	{
		Dealers = CreateDealerStates(dealers);
		Player = CreatePlayerState(player);
	}

	/// <summary>
	/// The dealers property of the state.
	/// </summary>
	[XmlArray(nameof(Dealers), Form = XmlSchemaForm.Qualified)]
	[XmlArrayItem(XmlConstants.DealerStateRootName, Form = XmlSchemaForm.Qualified)]
	public List<DealerState> Dealers { get; set; }

	/// <summary>
	/// The player property of the state.
	/// </summary>
	[XmlElement(nameof(Player), Form = XmlSchemaForm.Qualified)]
	public PlayerState Player { get; set; }

	/// <summary>
	/// Should the <see cref="Dealers"/> property be serialized?
	/// </summary>
	public bool ShouldSerializeDealers() => Dealers.Count != default;
}
