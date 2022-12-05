using System;
using System.Collections.Generic;
using System.Data.Entity;
using MightyGm2.Engine.Database;
using MightyGm2.Engine.Process;
using L5R.Model.Agent;

namespace L5R4.Control
{
	public class PersonnageProcess : IProcess
	{
		private PersonnageParameters _parameters;
		private List<IProcessStep> _processes = new List<IProcessStep>();

		public event Action<IProcessEndArguments> EndOfProcess;

		public Personnage Personnage { get; private set; }
		public IProcessParameters Parameters { get { return _parameters; } set { _parameters = (PersonnageParameters)value; } }
		public int NbrSteps { get { return _processes.Count; } }
		public MightyDb Data { get; private set; }

		public IProcessStep GetStep(int stepIndex) { return _processes[stepIndex]; }

		public PersonnageProcess(MightyDb data, PersonnageParameters parameters)
		{
			_parameters = parameters;
			_processes.Add(new ClanStep());
			_processes.Add(new AdvantageStep());
			_processes.Add(new SpellStep());
			_processes.Add(new OptionStep());
			_processes.Add(new BackpackStep());
			_processes.Add(new XPStep());
			Data = data;
		}

		public void InitializeProcess()
		{
			Personnage = new Personnage();
		}

		public void FinalizeProcess(IProcessEndArguments endArgs)
		{
			if(endArgs.FinishedState == FinishedState.Done)
			{
				//TODO agent saving in DB
				//Agent.Save();
			}
			EndOfProcess?.Invoke(endArgs);
		}
	}
}
