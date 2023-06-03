using LSDW.Domain.Constants;
using LSDW.Domain.Factories;
using LSDW.Domain.Interfaces.Actors;
using LSDW.Domain.Interfaces.Models;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LSDW.Domain.Classes.Persistence;

[XmlRoot("Game", Namespace = XmlConstants.NameSpace)]
public sealed class GameState
{
	public GameState()
	{
		Player = new();
		Dealers = new();
	}

	internal GameState(IPlayer player, IEnumerable<IDealer> dealers)
	{
		Player = DomainFactory.CreatePlayerState(player);
		Dealers = DomainFactory.CreateDealerStates(dealers);
	}

	[XmlElement("Player", Form = XmlSchemaForm.Qualified)]
	public PlayerState Player { get; set; }

	[XmlArray("Dealers", Form = XmlSchemaForm.Qualified)]
	[XmlArrayItem("Dealer", Form = XmlSchemaForm.Qualified)]
	public List<DealerState> Dealers { get; set; }
}
