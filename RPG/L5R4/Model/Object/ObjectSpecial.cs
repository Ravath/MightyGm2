using System;
using System.Linq;
using MightyGm2.RPG.L5R4.Data;
using L5R4.JdrCore;
using Engine.RpgLogic;
using L5R.Model.Agent;

namespace L5R.Model.Object
{
	public abstract class ObjectSpecial : IModifier<Agent.Agent>
	{
		private string _name;
		private string _desc;

		#region Prop
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public string Description
		{
			get { return _desc; }
			set { _desc = value; }
		}
		#endregion

		public int GetInt(string compl, int index)
		{
			return int.Parse(compl.Split(';')[index]);
		}
		public RollAndKeep GetDice(string compl, int index)
		{
			string pool = compl.Split(';')[index];
			int roll=0, keep = 0;
			foreach (char sep in new char[] { 'k', 'g' })
			{
				if (pool.Contains(sep))
				{
					int.TryParse(pool.Split(sep)[0], out roll);
					int.TryParse(pool.Split(sep)[1], out keep);
					break;
				}
			}
			return new RollAndKeep(roll, keep);
		}

        public abstract void AffectAgent(Agent.Agent a);
        public abstract void UnaffectAgent(Agent.Agent a);
    }

    internal class DefaultObjectSpecial : ObjectSpecial
    {
        public override void AffectAgent(Agent.Agent a) { /*NOTHING*/ }
        public override void UnaffectAgent(Agent.Agent a) { /*NOTHING*/ }
    }

    //TODO Special objects implementations
    internal class LightArmorMalus : ObjectSpecial
	{
        public override void AffectAgent(Agent.Agent a)
        {
            //TODO LightArmorMalus
        }

        public override void UnaffectAgent(Agent.Agent a)
        {
            //TODO LightArmorMalus
        }
    }
	internal class HeavyArmorMalus : ObjectSpecial
	{
        public override void AffectAgent(Agent.Agent a)
        {
            //TODO HeavyArmorMalus
        }

        public override void UnaffectAgent(Agent.Agent a)
        {
            //TODO HeavyArmorMalus
        }
    }
	internal class HeavyCavalryArmorMalus : ObjectSpecial
	{
        public override void AffectAgent(Agent.Agent a)
        {
            //TODO HeavyCavalryArmorMalus
        }

        public override void UnaffectAgent(Agent.Agent a)
        {
            //TODO HeavyCavalryArmorMalus
        }
    }

}
