namespace Javelin.Base.Config
{
	public interface IConfigReader
	{
		IConfigReader this[string path] { get; }

		string GetValue(string path);

		IConfigReader[] Children(string path);
	}
}