using Engine.Filter;
using MightyGm2.RPG.L5R4.Data;
using MightyGm2.Engine.RpgDatabase;

namespace L5R.Model.Skill
{
	public static class OptCompetenceInstantiate
	{
		public static OptCompetence Instanciate(ApprentissageOptionnelExemplar ex)
		{
            OptCompetence ret = null;
            var model = JsonDatabase.Data.OptionnalTraining.Get(ex.Model_Tag);

            switch (ex.Model_Tag)
			{
				case "OPA0001": //AuChoix
					ret = new DefaultOptCompetence() { Filter = new NoFilter<Competence>() };
                    break;
                case "OPA0002": //Art
					ret = new DefaultOptCompetence() { Filter = new FilterCompetenceModelByGlobalTag("GPC0001") };
                    break;
                case "OPA0003": //Connaissance
					ret = new DefaultOptCompetence() { Filter = new FilterCompetenceModelByGlobalTag("GPC0002") };
                    break;
                case "OPA0004": //Jeu
					ret = new DefaultOptCompetence() { Filter = new FilterCompetenceModelByGlobalTag("GPC0003") };
                    break;
                case "OPA0005": //Spectacle
					ret = new DefaultOptCompetence() { Filter = new FilterCompetenceModelByGlobalTag("GPC0004") };
                    break;
                case "OPA0006": //Arme
					ret = new DefaultOptCompetence() { Filter = new FilterCompetenceModelByGlobalTag("GPC0005") };
                    break;
                case "OPA0007": //Artisanat
					ret = new DefaultOptCompetence() { Filter = new FilterCompetenceModelByGlobalTag("GPC0006") };
                    break;
                case "OPA0107": //Artisanat2 : Artisanat au rang 2
					ret = new DefaultOptCompetence() { Filter = new FilterCompetenceModelByGlobalTag("GPC0006"), CompetenceRank = 2 };
                    break;
                case "OPA0008": //Noble
					ret = new DefaultOptCompetence() { Filter = new FilterCompetenceModelByGroup(GroupeCompetence.Noble) };
                    break;
                case "OPA0009": //Bugei
					ret = new DefaultOptCompetence() { Filter = new FilterCompetenceModelByGroup(GroupeCompetence.Bugei) };
                    break;
                case "OPA0010": //Marchand
					ret = new DefaultOptCompetence() { Filter = new FilterCompetenceModelByGroup(GroupeCompetence.Marchand) };
                    break;
                case "OPA0011": //Degradante
					ret = new DefaultOptCompetence() { Filter = new FilterCompetenceModelByGroup(GroupeCompetence.Degradante) };
                    break;
                case "OPA0012": //NobleOuMarchandOuDégradante
					ret = new DefaultOptCompetence()
					{
						Filter = new SetFilter<Competence>(new IFilter<Competence>[]{
						new FilterCompetenceModelByGroup(GroupeCompetence.Noble),
						new FilterCompetenceModelByGroup(GroupeCompetence.Marchand),
						new FilterCompetenceModelByGroup(GroupeCompetence.Degradante),
					})};
                    break;
                case "OPA0013": //NobleOuBugei
					ret = new DefaultOptCompetence()
					{
						Filter = new SetFilter<Competence>(new IFilter<Competence>[]{
						new FilterCompetenceModelByGroup(GroupeCompetence.Noble),
						new FilterCompetenceModelByGroup(GroupeCompetence.Bugei),
					})};
                    break;
                case "OPA0014": //NobleOuBugeiOuMarchand
					ret = new DefaultOptCompetence()
					{
						Filter = new SetFilter<Competence>(new IFilter<Competence>[]{
						new FilterCompetenceModelByGroup(GroupeCompetence.Noble),
						new FilterCompetenceModelByGroup(GroupeCompetence.Bugei),
						new FilterCompetenceModelByGroup(GroupeCompetence.Marchand),
					})};
                    break;
                case "OPA0015": //BugeiOuDégradante
					ret = new DefaultOptCompetence()
					{
						Filter = new SetFilter<Competence>(new IFilter<Competence>[]{
						new FilterCompetenceModelByGroup(GroupeCompetence.Bugei),
						new FilterCompetenceModelByGroup(GroupeCompetence.Degradante),
					})};
                    break;
                case "OPA0016": //MarchandOuConnaissance
					ret = new DefaultOptCompetence()
					{
						Filter = new SetFilter<Competence>(new IFilter<Competence>[]{
						new FilterCompetenceModelByGroup(GroupeCompetence.Marchand),
						new FilterCompetenceModelByGlobalTag("GPC0002"),
					})};
                    break;
                case "OPA0017": //NobleOuSpectacle
					ret = new DefaultOptCompetence()
					{
						Filter = new SetFilter<Competence>(new IFilter<Competence>[]{
						new FilterCompetenceModelByGroup(GroupeCompetence.Noble),
						new FilterCompetenceModelByGlobalTag("GPC0004"),
					})};
                    break;
                case "OPA0018": //ArtOuSpectacle
					ret = new DefaultOptCompetence()
					{
						Filter = new SetFilter<Competence>(new IFilter<Competence>[]{
						new FilterCompetenceModelByGlobalTag("GPC0001"),
						new FilterCompetenceModelByGlobalTag("GPC0004")
					})};
                    break;
                case "OPA0019": //NonDegradante
					ret = new DefaultOptCompetence(){ Filter = new PropFilterBool<Competence>("Degradante", false) };
                    break;
                case "OPA0020": //Theologie specialisation
					ret = new DefaultOptSpecialisation() { Filter = new PropFilterString<Competence>("Tag", "CPT2027") };
                    break;
				default:
					ret = new DefaultOptCompetence() { Filter = new NoFilter<Competence>() };
                    break;
            }

            ret.Number = ex.Nombre;
            ret.Description = model.Description;

            return ret;
		}

		public class FilterCompetenceModelByGroup : AbsDefaultFilter<Competence>
		{
			public GroupeCompetence Groupe{ get; set; }
			public FilterCompetenceModelByGroup(GroupeCompetence groupe)
			{
				Groupe = groupe;
			}

			public override bool ValidItem(Competence item)
            {
                return true;
                //TODO
                //return item.Groupe == Groupe;
			}
		}

		public class FilterCompetenceModelByGlobalTag : AbsDefaultFilter<Competence>
		{
			public string GlobalTag { get; set; }
			public FilterCompetenceModelByGlobalTag(string globalTag)
			{
				GlobalTag = globalTag;
			}

			public override bool ValidItem(Competence item)
			{
                return item.Global_Tag != null
                    && item.Global_Tag == GlobalTag;
            }
		}
	}
}
