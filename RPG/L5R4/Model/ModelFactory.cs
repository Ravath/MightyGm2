using Engine.Filter;
using Engine.RpgLogic;
using L5R.Model.Agent;
using L5R.Model.Capacity;
using L5R.Model.Object;
using L5R.Model.School;
using L5R.Model.Skill;
using L5R.Model.Trait;
using L5R4.JdrCore;
using MightyGm2.Engine.RpgDatabase;
using MightyGm2.RPG.L5R4.Data;
using MightyGm2.RPG.L5R4.Model.Trait;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyGm2.RPG.L5R4.Model
{
	public class ModelFactory
	{
		#region Singleton
		private static ModelFactory _this;
		public static ModelFactory Factory
		{
			get
			{
				if (_this == null)
				{
					_this = new ModelFactory();
				}
				return _this;
			}
		}
        #endregion

        public readonly JsonDatabase data;

		private ModelFactory()
		{
			data = JsonDatabase.Data;
			data.Load();
		}

		public L5R_Trait InstantiateNaturalPower(PouvoirNaturelExemplar ex)
		{
			L5R_Trait model = new L5R_Trait();
			PouvoirNaturelModel pouvoirData = data.PouvoirNaturel.Get(ex.Model_Tag);
			model.SetDataModel(pouvoirData);
            //todo this is workaround
            if(!string.IsNullOrWhiteSpace(ex.Complement))
                model.Name += " (" + ex.Complement + ")";
            IAgentEffect effect;

			switch (ex.Model_Tag)
			{
				case "SPE0001": // Enorme
					effect = new DummyAgentEffect();
					break;
				case "SPE0002": // Esprit
					effect = new DummyAgentEffect();
					break;
				case "SPE0003": // Invulnérabilité
					effect = new DummyAgentEffect();
					break;
				case "SPE0004": // Invulnérabilité Partielle
					effect = new DummyAgentEffect();
					break;
				case "SPE0005": // Invulnérabilité Supérieure
					effect = new DummyAgentEffect();
					break;
				case "SPE0006": // Mort Vivant
					effect = new DummyAgentEffect();
					break;
				case "SPE0007": // Peur
					effect = new DummyAgentEffect();
					break;
				case "SPE0008": // Résistance Magique
					effect = new DummyAgentEffect();
					break;
				case "SPE0009": // Vivacité
					effect = new Vivacity();
					break;
				case "SPE0010": // Charge Furieuse
					effect = new DummyAgentEffect();
					break;
				case "SPE0011": // Odorat
					effect = new DummyAgentEffect();
					break;
				case "SPE0012": // Charge
					effect = new DummyAgentEffect();
					break;
				case "SPE0013": // Attaque aux Yeux
					effect = new DummyAgentEffect();
					break;
				case "SPE0014": // Drainage Sanguin(Gaki)
					effect = new DummyAgentEffect();
					break;
				case "SPE0015": // Immortalité(Gaki)
					effect = new DummyAgentEffect();
					break;
				case "SPE0016": // Invisibilité(Gaki)
					effect = new DummyAgentEffect();
					break;
				case "SPE0017": // Métamorphose(Gaki)
					effect = new DummyAgentEffect();
					break;
				case "SPE0018": // Possession(Gaki)
					effect = new DummyAgentEffect();
					break;
				case "SPE0019": // Invulnérabilité(Gaki)
					effect = new DummyAgentEffect();
					break;
				case "SPE0020": // Immunité à la Souillure
					effect = new DummyAgentEffect();
					break;
				case "SPE0021": // Nom
					effect = new DummyAgentEffect();
					break;
				case "SPE0022": // Duperie
					effect = new DummyAgentEffect();
					break;
				case "SPE0023": // Eau Vitale
					effect = new DummyAgentEffect();
					break;
				case "SPE0024": // Immunité aux sorts
					effect = new DummyAgentEffect();
					break;
				case "SPE0025": // Immunité aux flèches
					effect = new DummyAgentEffect();
					break;
				case "SPE0026": // Décapitation
					effect = new DummyAgentEffect();
					break;
				case "SPE0027": // Aquatique
					effect = new DummyAgentEffect();
					break;
				case "SPE0028": // Maladie
					effect = new DummyAgentEffect();
					break;
				case "SPE0029": // Modification d'apparence
					effect = new DummyAgentEffect();
					break;
				case "SPE0030": // Lame Tsuno
					effect = new DummyAgentEffect();
					break;
				case "SPE0031": // Attaques Spectrales
					effect = new DummyAgentEffect();
					break;
				default:
					throw new NotImplementedException("Natural Power not implemented for tag : " + ex.Model_Tag);
			}

			if(effect != null)
			{
				effect.SetComplement(ex.Complement);
				model.AddEffect(effect);
			}

			return model;
        }

        public Competence InstantiateSkillModel(CompetenceModel cptData)
        {
            Competence icpt = new Competence();
            CompetenceGlobaleModel gloData = null;

            // Get Global definition if any
            if (!string.IsNullOrWhiteSpace(cptData.Global_Tag))
            {
                gloData = data.CompetenceGlobale.Get(cptData.Global_Tag);
                if (gloData == null)
                    ReportError("Can't find GlobalSkill Tag " + cptData.Global_Tag);
            }

            // Set Competence
            icpt.SetCompetence(cptData, gloData);

            // Add Specialties
            foreach (var speData in cptData.Specialisations)
            {
                icpt.AddSpecialisation(new Specialisation(speData));
            }

            // return competence
            return icpt;
        }

        public Competence InstantiateSkill(CompetenceExemplar cpt)
        {
            Competence icpt = new Competence();
            CompetenceModel cptData = data.Competence.Get(cpt.Model_Tag);
            CompetenceGlobaleModel gloData = null;

            // Get Global definition if any
            if (!string.IsNullOrWhiteSpace(cptData.Global_Tag))
            {
                gloData = data.CompetenceGlobale.Get(cptData.Global_Tag);
                if (gloData == null)
                    ReportError("Can't find GlobalSkill Tag " + cptData.Global_Tag);
            }

            // Set Competence
            icpt.SetCompetence(cptData, gloData);
            icpt.Rank = cpt.Rang;

            // Add Specialties if any
            foreach (string spmodel in cpt.SpecialisationsChoisies_Tag)
            {
                SpecialisationModel speData = cptData.Specialisations.Find(s => s.Tag == spmodel);
                if (speData == null)
                    ReportError("Can't find Speciality Tag " + cpt.Model_Tag + "/" + spmodel);
                icpt.AddSpecialisation(new Specialisation(speData));
            }

            // return competence
            return icpt;
        }

        //TODO replace every occurence of CompetenceStatus by a CompetenceExemplar, for god's sake
        public Competence InstantiateSkill(CompetenceStatus cpt)
        {
            // Convert to competence exemplar
            CompetenceExemplar cptEx = new CompetenceExemplar()
            {
                Model_Tag = cpt.Competence_Tag,
                Rang = cpt.Rang
            };

            // Get specialisation if any
            if (!string.IsNullOrWhiteSpace(cpt.Specialite_Tag))
            {
                cptEx.SpecialisationsChoisies_Tag.Add(cpt.Specialite_Tag);
            }

            return InstantiateSkill(cptEx);
        }

        public OptCompetence InstantiateSkillOption(ApprentissageOptionnelExemplar opt)
        {
            return OptCompetenceInstantiate.Instanciate(opt);
        }

        public IImplementedCapacity InstantiateSpell(string tag)
		{
			switch (tag)
			{
				default:
					return null;
			}
		}

		public IImplementedCapacity InstantiateKiho(string tag)
		{
			switch (tag)
			{
				default:
					return null;
			}
		}
		public IImplementedCapacity InstantiateKata(string tag)
		{
			switch (tag)
			{
				default:
					return null;
			}
        }

        public AgentCondition InstanciateCondition(AgentConditionExemplar exemplar)
        {
            AgentCondition ret;
            AgentConditionModel model = data.AgentCondition.Get(exemplar.Model_Tag);

            switch (exemplar.Model_Tag)
            {
                case "CON0001": // FireMin
                    ret = new RingNeed(Anneau.Feu, model);
                    break;
                case "CON0002": // AirMin
                    ret = new RingNeed(Anneau.Air, model);
                    break;
                case "CON0003": // EarthMin
                    ret = new RingNeed(Anneau.Terre, model);
                    break;
                case "CON0004": // WaterMin
                    ret = new RingNeed(Anneau.Eau, model);
                    break;
                case "CON0005": // VoidMin
                    ret = new RingNeed(Anneau.Vide, model);
                    break;
                case "CON0006": // ReflexesMin
                    ret = new AttributeNeed(Data.Trait.Reflexes, model);
                    break;
                case "CON0007": // AwarenessMin
                    ret = new AttributeNeed(Data.Trait.Intuition, model);
                    break;
                case "CON0008": // AgilityMin
                    ret = new AttributeNeed(Data.Trait.Agilite, model);
                    break;
                case "CON0009": // IntelligenceMin
                    ret = new AttributeNeed(Data.Trait.Intelligence, model);
                    break;
                case "CON0010": // PerceptionMin
                    ret = new AttributeNeed(Data.Trait.Perception, model);
                    break;
                case "CON0011": // StrengthMin
                    ret = new AttributeNeed(Data.Trait.Force, model);
                    break;
                case "CON0012": // WillpowerMin
                    ret = new AttributeNeed(Data.Trait.Volonte, model);
                    break;
                case "CON0013": // StaminaMin
                    ret = new AttributeNeed(Data.Trait.Constitution, model);
                    break;
                case "CON0014": // BushiNeed
                    ret = new SchoolBaliseNeed(BaliseEcole.Bushi, model);
                    break;
                case "CON0015": // CourtisanNeed
                    ret = new SchoolBaliseNeed(BaliseEcole.Courtisan, model);
                    break;
                case "CON0016": // ShugenjaNeed
                    ret = new SchoolBaliseNeed(BaliseEcole.Shugenja, model);
                    break;
                case "CON0017": // NinjaNeed
                    ret = new SchoolBaliseNeed(BaliseEcole.Ninja, model);
                    break;
                case "CON0018": // MoineNeed
                    ret = new SchoolBaliseNeed(BaliseEcole.Moine, model);
                    break;
                case "CON0019": // ArtisanNeed
                    ret = new SchoolBaliseNeed(BaliseEcole.Artisan, model);
                    break;
                case "CON0020": // CompetenceNeed
                    ret = new SkillNeed(model);
                    break;
                case "CON0021": // KenshinzenVictory
                    ret = new GMApprobation(model);
                    break;
                case "CON0022": // DeathLordFan
                    ret = new GMApprobation(model);
                    break;
                case "CON0023": // HonneurMin
                    ret = new HonnorMin(model);
                    break;
                case "CON0024": // GloryMin
                    ret = new GloryMin(model);
                    break;
                case "CON0025": // StatusMin
                    ret = new StatusMin(model);
                    break;
                case "CON0026": // TaintMin
                    ret = new TaintMin(model);
                    break;
                case "CON0027": // WomenOnly
                    ret = new SexeNeed(false, model);
                    break;
                case "CON0028": // MaleOnly
                    ret = new SexeNeed(true, model);
                    break;
                case "CON0029": // ClanNeed
                    ret = new ClanNeed(model);
                    break;
                    //TODO 30->56
                case "CON0057": // Tournament
                    ret = new GMApprobation(model);
                    break;
                default:
                    ret = new AgentCondition(model);
                    break;
            }

            ret.SetExemplar(exemplar);

            return ret;
        }

        public Augmentation InstantiateAugmentation(AugmentationSortExemplar exemplar)
		{
            Augmentation ret;
            AugmentationSortModel model = data.AugmentationSort.Get(exemplar.Model_Tag);
			switch (exemplar.Model_Tag)
			{
				case "AGS0001":
					ret =  new PorteeAugmentation(model);
                    break;
				case "AGS0002":
					ret =  new DureeAugmentation(model);
                    break;
                case "AGS0003":
					ret =  new ZoneAugmentation(model);
                    break;
                case "AGS0004":
					ret =  new CibleAugmentation(model);
                    break;
                case "AGS0005":
					ret =  new DommageAugmentation(model);
                    break;
                case "AGS0013":
					ret =  new CiblerAutruiSupplementaireAugmentation(model);
                    break;
                case "AGS0016":
					ret =  new CiblerAutruiAugmentation(model);
                    break;
                case "AGS0023":
					ret =  new AnneauAugmentation(model);
                    break;
                case "AGS0024":
					ret =  new OppositionAugmentation(model);
                    break;
                case "AGS0040":
					ret =  new JourAugmentation(model);
                    break;
                default:
					ret =  new Augmentation(model);
                    break;
            }

            ret.SetExemplar(exemplar);

            return ret;
		}

		public ObjectSpecial InstantiateObjectSpecial(SpecialObjetExemplar spo)
		{
            ObjectSpecial ret = null;
            var model = data.SpecialObjet.Get(spo.Model_Tag);

            switch (spo.Model_Tag)
			{
                case "SPO0001":
                    ret = new LightArmorMalus();
                    break;
                case "SPO0002":
                    ret = new HeavyArmorMalus();
                    break;
                case "SPO0003":
                    ret = new HeavyCavalryArmorMalus();
                    break;
                default:
					ret = new DefaultObjectSpecial();
                    break;
            }

            // Name and description
            ret.Name = model.Name;
            if(spo.Complement == null)
            {
                ret.Description = model.Description;
            }
            else
            {
                ret.Description = String.Format(model.Description, spo.Complement.Split(';'));
            }

            return ret;
        }

		public OptEquipment InstantiateEquipmentOption(EquipementOptionnelExemplar ex)
		{
            OptEquipment ret = null;
            var model = data.EquipementOptionnel.Get(ex.Model_Tag);

            switch (ex.Model_Tag)
			{
				case "OPE0001": //ArmureAshigaruOuLegere
					ret = new DefaultOptEquipment()
					{
						ArmorFilter = new SetFilter<Armure>(new IFilter<Armure>[] {
						new TagFilter<Armure>("ARU0001"),
						new TagFilter<Armure>("ARU0002")
					})
					};
                    break;
                case "OPE0002": //ArmureLegereOuLourde
					ret = new DefaultOptEquipment()
					{
						ArmorFilter = new SetFilter<Armure>(new IFilter<Armure>[] {
						new TagFilter<Armure>("ARU0002"),
						new TagFilter<Armure>("ARU0003")
					})
					};
                    break;
                case "OPE0003": //ArmureLegereOuCavalerie
					ret = new DefaultOptEquipment()
					{
						ArmorFilter = new SetFilter<Armure>(new IFilter<Armure>[] {
						new TagFilter<Armure>("ARU0002"),
						new TagFilter<Armure>("ARU0004")
					})
					};
                    break;
                case "OPE0004": //ArmureLourdeOuCavalerie
					ret = new DefaultOptEquipment()
					{
						ArmorFilter = new SetFilter<Armure>(new IFilter<Armure>[] {
						new TagFilter<Armure>("ARU0003"),
						new TagFilter<Armure>("ARU0004")
					})
					};
                    break;
                case "OPE0005": //ArmeAuChoix
					ret = new DefaultOptEquipment()
					{
						WeaponFilter = new NoFilter<Arme>()
					};
                    break;
                case "OPE0006": //ArmeLourdeOuHast
					ret = new DefaultOptEquipment()
					{
						WeaponFilter = new SetFilter<Arme>(new IFilter<Arme>[] {
						new WeaponTypeFilter(TypeArme.Lourde),
						new WeaponTypeFilter(TypeArme.Hast)
					})
					};
                    break;
                case "OPE0007": //ArcOuCouteau
					ret = new DefaultOptEquipment()
					{
						WeaponFilter = new SetFilter<Arme>(new IFilter<Arme>[] {
						new WeaponTypeFilter(TypeArme.Arc),
						new WeaponTypeFilter(TypeArme.Couteau)
					})
					};
                    break;
                case "OPE0008": //LanceAuChoixOuMaiChong
					ret = new DefaultOptEquipment()
					{
						WeaponFilter = new SetFilter<Arme>(new IFilter<Arme>[] {
						new WeaponTypeFilter(TypeArme.Lance),
						new TagFilter<Arme>("ARM0036")
					})
					};
                    break;
                case "OPE0009": //BoOuPaireDeJo
					ret = new DefaultOptEquipment()
					{
						WeaponFilter = new SetFilter<Arme>(new IFilter<Arme>[] {
						new TagFilter<Arme>("ARM0024"),
						new TagFilter<Arme>("ARM0025")//TODO paire?
					})
					};
                    break;
                case "OPE0010": //ArmeAuChoixOuPaireDeCouteaux
					ret = new DefaultOptEquipment()
					{
						WeaponFilter = new NoFilter<Arme>()//TODO paire?
					};
                    break;

                default:
					ret = new DefaultOptEquipment();
                    break;
			}

            ret.Number = ex.Nombre;
            ret.Description = String.Format(model.Description, ex.Nombre);

            return ret;
        }

        private Avantage InstantiateAncestor(AncetreModel item)
        {
            // Convert to Avantage
            AvantageModel model = new AvantageModel()
            {
                Tag = item.Tag,
                Name = item.Name,
                SousType = SousTypeAvantage.Spirituel,
                Cout = item.Cout,
                Description = item.Description+"\n\nExigences : "+item.Exigences
            };
            Avantage ret =  InstantiateAdvantage(model);

            // Add Clan Condition
            AgentConditionExemplar cond = new AgentConditionExemplar(){
                Model_Tag = "CON0029",
                Complement = item.Clan_Tag
            };
            ret.AddCondition(InstanciateCondition(cond));

            return ret;
        }

        public Avantage InstantiateAdvantage(AvantageModel dataModel)
		{
			Avantage a = new Avantage
            {
                Tag = dataModel.Tag,
                Name = dataModel.Name,
				Cout = dataModel.Cout,
				RankMax = dataModel.RangMax,
				Description = dataModel.Description,
				// Get in avantage list or desavantage list accordingly
				Desavantage = !data.Avantage.ContainsKey(dataModel.Tag),
			};
			IAgentEffect effect;

			// Get Groupe Description if any
			if (dataModel.Groupe_Tag != null)
			{
				GroupeAvantageModel gModel = data.GroupeAvantage.Get(dataModel.Groupe_Tag);
				a.Description = gModel.Description + "\n\n" + a.Description;
			}

			// Get Effects
			switch (dataModel.Tag)
			{
				//TODO implement
				default:
					effect = new DummyAgentEffect();
					break;
			}

			if (effect != null)
			{
				//TODO (when using avantageExemplar?) : effect.SetComplement(ex.Complement); (See Augmentation)
				a.AddEffect(effect);
			}

			return a;
        }

        public IAgentEffect InstantiateTatooPower(string tag)
        {
            switch (tag)
            {
                default:
                    return null;
            }
        }

        public IAgentEffect InstantiateShadowlandPower(string tag)
		{
			switch (tag)
			{
				default:
					return null;
			}
        }

        public Technique InstantiateTechnique(TechniqueModel model)
        {
            // Instantiate basics
            Technique tech = new Technique()
            {
                Tag = model.Tag,
                Name = model.Name + " (Rank "+model.Rang+")",//TODO remove rank notation?
                Rank = model.Rang,
                Description = model.Description
            };

            // Get Effect implementation
            IAgentEffect techEffect = null;
            switch (model.Tag)
			{
				default:
                    //TODO techniques ecole
                    //TODO techniques ecole avancée
                    //TODO techniques ecole alternative
                    break;
			}

            // Add implementation if any
            if(techEffect != null)
                tech.AddEffect(techEffect);

            // return
            return tech;
        }

        public Ecole InstantiateSchool(EcoleModel model)
        {
            // Instantiate basics
            Ecole school = new Ecole()
            {
                Tag = model.Tag,
                Name = model.Name,
                Description = model.Description,
                BonusTrait = model.BonusInitial,
                KokuInitial = (int)model.ArgentInitial,
                BuInitial = ((int)model.ArgentInitial * 10) % 10,
                ZeniInitial = ((int)model.ArgentInitial * 100) % 10,
                MotClef = model.Balise,
                Spells = model.Sorts,
                Devotion = model.Devotion,
            };
            school.InitialHonnor.Points.BaseValue = (int)model.Honneur * 10;

            // Add techniques
            foreach (var tech in data.Technique.Items.Where(t=>t.Ecole_Tag == model.Tag).OrderBy(t => t.Rang))
            {
                Technique t = ModelFactory.Factory.InstantiateTechnique(tech);
                school.Techniques.Add(t);
            }

            // Add skill init
            foreach (var skill in model.Competences)
            {
                Competence cpt = ModelFactory.Factory.InstantiateSkill(skill);
                school.Skills.Add(cpt);
            }
            if(model.CompetencesOpt != null)
                foreach (var skillOpt in model.CompetencesOpt)
                {
                    OptCompetence cpt = ModelFactory.Factory.InstantiateSkillOption(skillOpt);
                    school.OptSkills.Add(cpt);
                }

            // Add equipment init
            foreach (var eqptModel in model.Equipements)
            {
                L5R_Trait obj = ModelFactory.Factory.InstantiateEquipment(eqptModel);
                school.Equipments.Add(obj);
            }
            if (model.EquipementsOpt != null)
                foreach (var optModel in model.EquipementsOpt)
                {
                    OptEquipment opt = ModelFactory.Factory.InstantiateEquipmentOption(optModel);
                    school.OptEquipments.Add(opt);
                }

            return school;
        }

        public AlternativeSchool InstantiateAdvancedSchool(EcoleAvanceeModel model)
        {
            AlternativeSchool ret = new AlternativeSchool()
            {
                Tag = model.Tag,
                Name = model.Name,
                Description = model.Description,
                MotClef = model.Balise,
                RequiredRank = 5
            };

            // Add techniques
            foreach (var tech in data.TechniqueAvancee.Items.Where(t => t.Ecole_Tag == model.Tag).OrderBy(t => t.Rang))
            {
                Technique t = ModelFactory.Factory.InstantiateTechnique(tech);
                ret.Techniques.Add(t);
            }

            // Add Conditions : Clan
            AgentConditionExemplar cond = new AgentConditionExemplar()
            {
                Model_Tag = "CON0029",
                Complement = model.Clan_Tag
            };
            ret.Conditions.Add(InstanciateCondition(cond));

            // Add Conditions : Skills
            foreach (var item in model.CompetencesRequises)
            {
                cond = new AgentConditionExemplar()
                {
                    Model_Tag = "CON0020",
                    Complement = item.Competence_Tag+";"+item.Rang
                };
                ret.Conditions.Add(InstanciateCondition(cond));
            }
            // Add Conditions : Defaults
            foreach (var item in model.Conditions)
            {
                ret.Conditions.Add(InstanciateCondition(item));
            }

            return ret;
        }

        public AlternativeSchool InstantiateAdvancedSchool(VoieAlternativeModel model)
        {
            AlternativeSchool ret = new AlternativeSchool()
            {
                Tag = model.Tag,
                Name = model.Name,
                Description = model.Description,
                MotClef = model.Balise,
                RequiredRank = model.Rang
            };

            // Add techniques
            TechniqueModel techModel = new TechniqueModel()
            {
                Tag = model.Tag,
                Name = model.NomTechnique,
                Rang = model.Rang,
                Description = model.DescriptionTechnique
            };
            ret.Techniques.Add(InstantiateTechnique(techModel));

            // Add Conditions : Clan
            if (!string.IsNullOrWhiteSpace(model.ClanRequis_Tag))
            {
                AgentConditionExemplar cond = new AgentConditionExemplar()
                {
                    Model_Tag = "CON0029",
                    Complement = model.ClanRequis_Tag
                };
                ret.Conditions.Add(InstanciateCondition(cond));
            }

            // Add Conditions : Skills
            foreach (var item in model.CompetencesRequises)
            {
                AgentConditionExemplar cond = new AgentConditionExemplar()
                {
                    Model_Tag = "CON0020",
                    Complement = item.Competence_Tag + ";" + item.Rang
                };
                ret.Conditions.Add(InstanciateCondition(cond));
            }
            // Add Conditions : Defaults
            foreach (var item in model.Conditions)
            {
                ret.Conditions.Add(InstanciateCondition(item));
            }

            return ret;
        }

        public L5R_Trait InstantiateEquipment(DataExemplaire<AbsObjetModel> ex)
        {
            return InstantiateEquipment(ex.Model_Tag);
        }

        public L5R_Object InstantiateEquipment(string tag)
        {
            L5R_Object ret = null;
            AbsObjetModel model = null;

            // Get if Weapon
            if (data.Arme.ContainsKey(tag))
            {
                ArmeModel armeModel = data.Arme.Get(tag);
                model = armeModel;
                ret = new Arme()
                {
                    Tag = armeModel.Tag,
                    Name = armeModel.Name,
                    Description = armeModel.Description,
                    Degats = new RollAndKeep(armeModel.VD.Roll, armeModel.VD.Keep),
                    Type = armeModel.Type,
                    Taille = armeModel.Taille,
                    ArmeSamurai = armeModel.Samurai,
                    ArmePaysan = armeModel.Paysan,
                    Brisee = false,
                };
            }
            // Get if Armor
            else if (data.Armure.ContainsKey(tag))
            {
                ArmureModel armureModel  = data.Armure.Get(tag);
                model = armureModel;
                Armure armure = new Armure()
                {
                    Tag = armureModel.Tag,
                    Name = armureModel.Name,
                    Description = armureModel.Description,
                };
                armure.ND.BaseValue = armureModel.BonusND;
                armure.Reduction.BaseValue = armureModel.Reduction;
                ret = armure;
            }
            // Get if miscellaneous object
            else if (data.Objet.ContainsKey(tag))
            {
                ObjetModel objetModel = data.Objet.Get(tag);
                model = objetModel;
                ret = new L5R_Object()
                {
                    Tag = model.Tag,
                    Name = model.Name,
                    Description = model.Description,
                };
                if(objetModel.CoutMax != null)
                {
                    ret.ValeurMax = new ValeurMonetaire((int)objetModel.CoutMax, objetModel.Cout.Unit);
                }
            }
            // Can't find any
            else
            {
                ReportError("Can't find and instantiate model from tag :" + tag);
                return null;
            }

            // Get Value
            ret.Valeur.Value = model.Cout.Value;
            ret.Valeur.Unit = model.Cout.Unit;

            //get specials
            foreach (SpecialObjetExemplar sp in model.Special)
            {
                ret.AddObjectSpecial(ModelFactory.Factory.InstantiateObjectSpecial(sp));
            }

            return ret;
        }

        #region Error Mngnt
        internal static void ReportError(string message)
        {
            // Get stack trace for the exception with source file information
            StackTrace st = new StackTrace();
            // Get calling method
            int frameId = 0;
            var frame = st.GetFrame(frameId);
            while (frame.GetMethod().Name.Contains("ReportError"))
            {
                frame = st.GetFrame(++frameId);
            }
            var method = frame.GetMethod();
            // Transmit error message
            string origin = method.ReflectedType.Namespace + "." + method.ReflectedType.Name + "." + method.Name + " : ";
            Godot.GD.Print(origin + message);
        }

        internal static void ReportError_ArgumentNumber(int argumentsNumber, int min, int max, string arguments)
        {
            string message = "Received:" + argumentsNumber;
            if (min != -1) message = (min + " < " + message);
            if (max != -1) message = (message + " < " + max);
            message = message + " : " + arguments;
            ReportError("Wrong number of arguments : " + message);
        }
        #endregion


        public IEnumerable<DataCollection> GetAllData()
        {
            List<DataCollection> allData = new List<DataCollection>();

            DataCollection clans = new DataCollection() { CollectionName = "Clans" };
            DataCollection monsters = new DataCollection() { CollectionName = "Monstres" };
            DataCollection weapons = new DataCollection() { CollectionName = "Armes" };

            allData.Add(clans);
            allData.Add(monsters);
            allData.Add(weapons);

            // Add clans
            List<Clan> clansData = new List<Clan>();
            clans.Collection = clansData;
            foreach (var item in data.Clan.Items)
            {
                Clan clan = new Clan();
                clan.SetModel(item);
                clansData.Add(clan);
            }

            // Add monsters
            List<Agent> monstersData = new List<Agent>();
            monsters.Collection = monstersData;
            foreach (var item in data.Figurant.Items)
            {
                Figurant p = new Figurant();
                p.SetPersonnage(item);
                monstersData.Add(p);
            }

            // Add weapons
            List<Arme> weaponsData = new List<Arme>();
            weapons.Collection = weaponsData;
            foreach (var item in data.Arme.Items)
            {
                Arme p = (Arme)InstantiateEquipment(item.Tag);
                weaponsData.Add(p);
            }

            // Add armors
            List<Armure> armorsData = new List<Armure>();
            DataCollection armors = new DataCollection()
            {
                CollectionName = "Armures",
                Collection = armorsData
            };

            allData.Add(armors);
            foreach (var item in data.Armure.Items)
            {
                Armure eqpnt = (Armure)InstantiateEquipment(item.Tag);
                armorsData.Add(eqpnt);
            }

            // Add objects
            List<L5R_Object> objectsData = new List<L5R_Object>();
            DataCollection objects  = new DataCollection()
            {
                CollectionName = "Objets",
                Collection = objectsData
            };

            allData.Add(objects);
            foreach (var item in data.Objet.Items)
            {
                L5R_Object eqpmt = InstantiateEquipment(item.Tag);
                objectsData.Add(eqpmt);
            }

            // Add advantages
            List<Avantage> advantagesData = new List<Avantage>();
            DataCollection advantages = new DataCollection() {
                CollectionName = "Avantages",
                Collection = advantagesData
            };

            allData.Add(advantages);
            foreach (var item in data.Avantage.Items)
            {
                Avantage av = InstantiateAdvantage(item);
                advantagesData.Add(av);
            }

            // Add disadvantages
            List<Avantage> disadvantagesData = new List<Avantage>();
            DataCollection disadvantages = new DataCollection()
            {
                CollectionName = "Désavantages",
                Collection = disadvantagesData
            };
            allData.Add(disadvantages);
            foreach (var item in data.Desavantage.Items)
            {
                Avantage av = InstantiateAdvantage(item);
                disadvantagesData.Add(av);
            }

            // Add disadvantages
            List<Avantage> ancestorsData = new List<Avantage>();
            DataCollection ancestors = new DataCollection()
            {
                CollectionName = "Ancêtres",
                Collection = ancestorsData
            };
            allData.Add(ancestors);
            foreach (var item in data.Ancestor.Items)
            {
                Avantage av = InstantiateAncestor(item);
                ancestorsData.Add(av);
            }

            // Add Katas
            List<Kata> katasData = new List<Kata>();
            DataCollection katas = new DataCollection()
            {
                CollectionName = "Kata",
                Collection = katasData
            };

            allData.Add(katas);
            foreach (var item in data.Kata.Items)
            {
                Kata pwr = new Kata();
                pwr.SetDataModel(item);
                katasData.Add(pwr);
            }

            // Add Kihos
            List<Kiho> kihosData = new List<Kiho>();
            DataCollection kihos = new DataCollection()
            {
                CollectionName = "Kiho",
                Collection = kihosData
            };

            allData.Add(kihos);
            foreach (var item in data.Kiho.Items)
            {
                Kiho pwr = new Kiho();
                pwr.SetDataModel(item);
                kihosData.Add(pwr);
            }

            // Add Spells
            List<Sort> sortsData = new List<Sort>();
            DataCollection sorts = new DataCollection()
            {
                CollectionName = "Sort",
                Collection = sortsData
            };

            allData.Add(sorts);
            foreach (var item in data.Sort.Items)
            {
                Sort pwr = new Sort();
                pwr.SetDataModel(item);
                sortsData.Add(pwr);
            }

            // Add Maho
            List<Sort> mahouData = new List<Sort>();
            DataCollection Mahou = new DataCollection()
            {
                CollectionName = "Mahou",
                Collection = mahouData
            };

            allData.Add(Mahou);
            foreach (var item in data.Mahou.Items)
            {
                Sort pwr = new Sort();
                pwr.SetDataModel(item);
                mahouData.Add(pwr);
            }

            // Add Togashi Tatoo
            List<Tatoo> tatoosData = new List<Tatoo>();
            DataCollection tatoos = new DataCollection()
            {
                CollectionName = "Tatouages",
                Collection = tatoosData
            };

            allData.Add(tatoos);
            foreach (var item in data.TatouageTogashi.Items)
            {
                Tatoo pwr = new Tatoo();
                pwr.SetDataModel(item);
                tatoosData.Add(pwr);
            }

            // Add Shadowland powers
            List<PouvoirOutremonde> shadowsData = new List<PouvoirOutremonde>();
            DataCollection shadows = new DataCollection()
            {
                CollectionName = "Pouvoirs Outremonde",
                Collection = shadowsData
            };

            allData.Add(shadows);
            foreach (var item in data.PouvoirOutremonde.Items)
            {
                PouvoirOutremonde pwr = new PouvoirOutremonde();
                pwr.SetDataModel(item);
                shadowsData.Add(pwr);
            }

            // Add Skills
            List<Competence> skillsData = new List<Competence>();
            DataCollection skills = new DataCollection()
            {
                CollectionName = "Compétences",
                Collection = skillsData
            };

            allData.Add(skills);
            foreach (var item in data.Competence.Items)
            {
                Competence cpt = InstantiateSkillModel(item);
                skillsData.Add(cpt);
            }

            // Add Advanced School
            List<AlternativeSchool> advancedSchoolData = new List<AlternativeSchool>();
            DataCollection advancedSchool = new DataCollection()
            {
                CollectionName = "Ecoles Avancées",
                Collection = advancedSchoolData
            };

            allData.Add(advancedSchool);
            foreach (var item in data.EcoleAvancee.Items)
            {
                AlternativeSchool school = InstantiateAdvancedSchool(item);
                advancedSchoolData.Add(school);
            }

            // Add Alternative School
            List<AlternativeSchool> alternateSchoolData = new List<AlternativeSchool>();
            DataCollection alternateSchool = new DataCollection()
            {
                CollectionName = "Voies Alternatives",
                Collection = alternateSchoolData
            };

            allData.Add(alternateSchool);
            foreach (var item in data.VoieAlternative.Items)
            {
                AlternativeSchool school = InstantiateAdvancedSchool(item);
                alternateSchoolData.Add(school);
            }

            // Add Heroic Opportunities
            List<OpportuniteHeroiqueModel> heroOpportunitiesData = new List<OpportuniteHeroiqueModel>();
            heroOpportunitiesData.AddRange(data.OpportuniteHeroique.Items);
            DataCollection heroOpportunities = new DataCollection()
            {
                CollectionName = "Opportunités Héroiques",
                Collection = heroOpportunitiesData
            };
            allData.Add(heroOpportunities);

            // Add inheritance tables
            // TODO use a specific control type
            List<TableHeritageModel> inheritanceData = new List<TableHeritageModel>();
            inheritanceData.AddRange(data.TableHeritage.Items);
            DataCollection inheritance = new DataCollection()
            {
                CollectionName = "Héritages",
                Collection = inheritanceData
            };
            allData.Add(inheritance);

            // TODO info/Gain tabs

            return allData;
        }
    }

    public class DataCollection
    {
        public String CollectionName { get; set; }
        public IEnumerable<INamed> Collection { get; set; }
    }
}
