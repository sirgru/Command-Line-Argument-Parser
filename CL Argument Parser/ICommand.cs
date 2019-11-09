namespace CLAP
{
	public interface ICommand
	{
		void AddArgument(string arg);
		void AddSwitch(Switch sw);
	}
}
