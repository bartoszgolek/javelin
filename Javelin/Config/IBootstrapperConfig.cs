namespace Javelin.Config
{
	internal interface IBootstrapperConfig
	{
		bool IsSlave { get; }
		bool IsMaster { get; }
		string RootUri { get; }
	}
}