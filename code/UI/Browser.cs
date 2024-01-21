using Sandbox.UI;

namespace Phystest;
public partial class Browser : Panel
{
	async void SpawnPackage( Package p )
	{
		NetworkManager.Instance.SpawnPackage( p.FullIdent );
	}

	public override void Tick()
	{
		SetClass( "active", Input.Down( GameInputActions.Menu ) );
	}

}
