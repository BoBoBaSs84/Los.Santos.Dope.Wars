using GTA.Math;

namespace LSDW.Dealers;

/// <summary>
/// One fixed dealer pitch: where he stands, which way he faces, and the id his saved
/// state is keyed under. Data only — see <see cref="DealerRoster.Spots"/> for the list
/// and the rules that govern <paramref name="Id"/>.
/// </summary>
/// <param name="Id">
/// The spot's stable identity. Never the list index, never derived from
/// <paramref name="Position"/>, never reused or renumbered — the save file keys on it.
/// </param>
/// <param name="Position">Where the blip sits and the ped spawns.</param>
/// <param name="Heading">Which way the ped faces when spawned.</param>
internal sealed record DealerSpot(
  int Id,
  Vector3 Position,
  float Heading);
