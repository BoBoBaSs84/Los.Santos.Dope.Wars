using LSDW.Core.Interfaces.Classes;
using LSDW.Factories;
using LSDW.Interfaces.Actors;
using System.Xml.Schema;
using System.Xml.Serialization;
using static LSDW.Constants.XmlConstants.NameSpaces;

namespace LSDW.Classes.Persistence;

[XmlRoot("Game", Namespace = GameStateNameSpace)]
public sealed class GameState
{
	public GameState(IPlayer player, IEnumerable<IDealer> dealers)
	{
		Player = PersistenceFactory.CreatePlayerState(player);
		Dealers = PersistenceFactory.CreateDealerStates(dealers);
	}

	public GameState()
	{
		Player = new();
		Dealers = new();
	}

	[XmlElement("Player", Form = XmlSchemaForm.Qualified)]
	public PlayerState Player { get; set; }

	[XmlArray("Dealers", Form = XmlSchemaForm.Qualified)]
	[XmlArrayItem("Dealer", Form = XmlSchemaForm.Qualified)]
	public List<DealerState> Dealers { get; set; }
}
