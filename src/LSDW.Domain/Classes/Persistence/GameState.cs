using LSDW.Domain.Constants;
using LSDW.Domain.Factories;
using LSDW.Domain.Interfaces.Actors;
using LSDW.Domain.Interfaces.Models;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LSDW.Domain.Classes.Persistence;

[XmlRoot(XmlConstants.GameStateRootName, Namespace = XmlConstants.NameSpace)]
public sealed class GameState
{
	public GameState()
	{
		Player = new();
		Dealers = new();
	}

	internal GameState(IPlayer player, ICollection<IDealer> dealers)
	{
		Player = DomainFactory.CreatePlayerState(player);
		Dealers = DomainFactory.CreateDealerStates(dealers);
	}

	[XmlElement(nameof(Player), Form = XmlSchemaForm.Qualified)]
	public PlayerState Player { get; set; }

	[XmlArray(nameof(Dealers), Form = XmlSchemaForm.Qualified)]
	[XmlArrayItem(XmlConstants.DealerStateRootName, Form = XmlSchemaForm.Qualified)]
	public List<DealerState> Dealers { get; set; }

	public bool ShouldSerializeDealers() => Dealers.Any();
}
