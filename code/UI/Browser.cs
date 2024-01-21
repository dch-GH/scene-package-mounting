using Sandbox.UI;

namespace Phystest;
public partial class Browser : Panel
{
	async void SpawnPackage( Package p )
	{
		var package = await Package.FetchAsync( p.FullIdent, false );
		Log.Info( $"Spawning package: {package}" );

		var primaryType = package.GetMeta( "PrimaryAsset", "" );
		Log.Info( $"Package PrimaryType: {primaryType}" );

		await package.MountAsync( true );

		var type = TypeLibrary.GetType( primaryType );
		Log.Info( $"Package PrimaryType from TypeLibrary: {type}" );

		if ( type is null )
			return;

		var go = NetworkManager.Instance.Scene.CreateObject();
		go.Name = primaryType;
		go.Components.Create( type, true );
	}
}
