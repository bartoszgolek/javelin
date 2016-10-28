namespace Javelin.Base.Config
{
	public interface IConfigReader
	{
		string this[string path] { get; }

		IConfigReader GetSubconfig(string path);

		IConfigReader[] GetSubconfigs(string path);
	}
}