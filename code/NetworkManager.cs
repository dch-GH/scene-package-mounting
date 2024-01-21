using Sandbox.Network;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Channels;

namespace Phystest;

public sealed class NetworkManager : Component, Component.INetworkListener
{
	public static NetworkManager Instance;

	/// <summary>
	/// Create a server (if we're not joining one)
	/// </summary>
	/// 
	[Property] public bool StartServer { get; set; } = true;

	/// <summary>
	/// The prefab to spawn for the player to control.
	/// </summary>
	[Property] public GameObject PlayerPrefab { get; set; }

	/// <summary>
	/// A list of points to choose from randomly to spawn the player in. If not set, we'll spawn at the
	/// location of the NetworkHelper object.
	/// </summary>
	[Property] public List<SpawnPoint> SpawnPoints { get; set; }

	public Dictionary<Connection, ClientComponent> Clients { get; private set; }

	protected override async Task OnLoad()
	{
		if ( Scene.IsEditor )
			return;

		if ( StartServer && !GameNetworkSystem.IsActive )
		{
			Clients = new();
			LoadingScreen.Title = "Creating Lobby";
			await Task.DelayRealtimeSeconds( 0.1f );
			GameNetworkSystem.CreateLobby();
		}
	}


	protected override void OnAwake()
	{
		Instance = this;
	}

	public void OnActive( Connection conn )
	{
		if ( PlayerPrefab is null )
			return;

		SpawnPlayer( conn, Vector3.Up * 32 );
	}

	private GameObject SpawnPlayer( Connection conn, Vector3 startLocation )
	{
		// Spawn this object and make the client the owner
		var player = PlayerPrefab.Clone( startLocation );
		player.Name = $"Player - {conn.DisplayName}";
		player.BreakFromPrefab();

		var client = player.Components.GetOrCreate<ClientComponent>();
		client.Pawn = player;
		client.Connect( conn );
		Clients.Add( conn, client );

		// Everything we just set on the client component in client.Connect
		// won't be synced unless we Network.Spawn AFTER we set all that stuff.
		player.NetworkSpawn( conn );
		return player;
	}

	public void OnDisconnected( Connection conn )
	{
		if ( Clients.TryGetValue( conn, out var client ) )
		{
			client.OnDisconnectClient( conn.Id, conn.DisplayName, conn.SteamId, conn.IsHost );
			client.Pawn.Destroy();
			Clients.Remove( conn );
		}
	}
}
