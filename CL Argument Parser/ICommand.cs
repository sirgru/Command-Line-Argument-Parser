namespace CL_Argument_Parser
{
	public interface ICommand
	{
		void AddArgument(string arg);
		void AddSwitch(Switch sw);
	}
}
