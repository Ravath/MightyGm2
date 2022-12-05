namespace  MightyGm2.RPG.L5R4.Data {

    public enum L5R4_DataCategory
    {
        Clan, Family, School,
        Weapon, Armor, Object,
        Advantage, Disadvantage,
        Skill, SkillSpecialisation, SkillMastery,
        Technique, Kata, Spell, Kiho, Mahou, 
        AdvancedSchool, AlternativeSchool, Ancestor,
        ShadowlandMutation, ExtraCapacity/*monsters and stuff*/,
        Action, Condition/*stunned, ...*/, HeroicOpportunity,
        Character,
        Plot, AdvancedTechnique
    }

	public enum Anneau{
		Feu, Air, Terre, Eau, Vide}

	public enum Affinite{
		Feu, Air, Terre, Eau, Vide, Maho, Choix, Aucun}

	public enum Devotion{
		Shintao, Fortunes}

	public enum ElementSort{
		Feu, Air, Terre, Eau, Vide, Universel}

	public enum MotClefSort{
		ArtGuerre, Artisanat, Defense, Divination, Glyphe, Illusion, Jade, Tonnerre, Voyage}

	public enum Trait{
		Reflexes, Intuition, Agilite, Intelligence, Perception, Force, Volonte, Constitution}

	public enum TraitCompetence{
		Reflexes, Intuition, Agilite, Intelligence, Perception, Force, Volonte, Constitution, Vide}

	public enum BaliseEcole{
		Bushi, Courtisan, Shugenja, Ninja, Moine, Artisan}

	public enum GroupeCompetence{
		Noble, Bugei, Marchand, Degradante}

	public enum SousTypeAvantage{
		Physique, Mental, Social, Materiel, Spirituel}

	public enum Portee{
		Personnel, Contact, PersonnelContact, Metres, Kilometres, Special}

	public enum ZoneEffet{
		Personnel, Individu, Esprit, Objet, Arme, Armure, dmCube, Metres, Kilometres, Cone, Cadavre, MortVivant, Membre, Invocation, Diametre, Lieu, Autre}

	public enum Duree{
		Instantane, Permanent, Rounds, Minutes, Heures, Jours, Semaine, DeuxSemaine, Mois, Annee, Special, Indefinie}

	public enum TypeArme{
		Fleche, Arc, Chaine, Hast, Ninjutsu, Lourde, Baton, Couteau, Eventail, Lance, Sabre}

	public enum TailleArme{
		Petite, Moyenne, Grande}

	public enum TypeKiho{
		Interieur, Karmique, Martial, Mystique}

	public enum Action{
		Rapide, Simple, Complexe}

	public enum TypeClan{
		Majeur, Mineur, Confrerie, Autre}

	public enum Monnaie{
		Zeni, Bu, Koku}

	public enum ObjectType{
		Divers, Vetement, Service, Suivant}

	public enum TypePouvoirOutremonde{
		Mutation, Mineur, Majeur, Akutenshi}

	public enum TypeActionCombat{
		Gratuite, Simple, Mouvement, Complexe, Augmentation}

	public enum TypeCombatCondition{
		Posture, Autre}

}
