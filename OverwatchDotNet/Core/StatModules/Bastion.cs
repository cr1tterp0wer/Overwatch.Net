using OverwatchAPI.Internal;
using System;
using System.Collections.Generic;

//-- A U T O   G E N E R A T E D --//

namespace OverwatchAPI.Data
{
	public class Bastion : IStatGroup
	{
		public HeroSpecificStats HeroSpecific { get; private set; }
		public CombatStats Combat { get; private set; }
		public BestStats Best { get; private set; }
		public AverageStats Average { get; private set; }
		public DeathsStats Deaths { get; private set; }
		public MatchAwardsStats MatchAwards { get; private set; }
		public GameStats Game { get; private set; }

		public void SendPage(IEnumerable<OverwatchDataTable> tableCollection)
		{
			foreach(var item in tableCollection)
			{
				var prop = GetType().GetProperty(item.Name.Replace(" ", ""));
				if (typeof(IStatModule).IsAssignableFrom(prop.PropertyType))
				{
					IStatModule statModule = (IStatModule)Activator.CreateInstance(prop.PropertyType);
					statModule.SendTable(item);
					prop.SetValue(this, statModule);
				}
			}
		}

		public class HeroSpecificStats : IStatModule
		{
			public float ReconKills { get; private set; }
			public float SentryKills { get; private set; }
			public float TankKills { get; private set; }
			public float SentryKillsMostinGame { get; private set; }
			public float ReconKillsMostinGame { get; private set; }
			public float TankKillsMostinGame { get; private set; }
			public float TankKillsAverage { get; private set; }
			public float SentryKillsAverage { get; private set; }
			public float ReconKillsAverage { get; private set; }

			public void SendTable(OverwatchDataTable table)
			{
				ReconKills = table.Stats["Recon Kills"].OWValToFloat();
				SentryKills = table.Stats["Sentry Kills"].OWValToFloat();
				TankKills = table.Stats["Tank Kills"].OWValToFloat();
				SentryKillsMostinGame = table.Stats["Sentry Kills - Most in Game"].OWValToFloat();
				ReconKillsMostinGame = table.Stats["Recon Kills - Most in Game"].OWValToFloat();
				TankKillsMostinGame = table.Stats["Tank Kills - Most in Game"].OWValToFloat();
				TankKillsAverage = table.Stats["Tank Kills - Average"].OWValToFloat();
				SentryKillsAverage = table.Stats["Sentry Kills - Average"].OWValToFloat();
				ReconKillsAverage = table.Stats["Recon Kills - Average"].OWValToFloat();
			}
		}

		public class CombatStats : IStatModule
		{
			public float Eliminations { get; private set; }
			public float FinalBlows { get; private set; }
			public float SoloKills { get; private set; }
			public float ShotsFired { get; private set; }
			public float ShotsHit { get; private set; }
			public float CriticalHits { get; private set; }
			public float DamageDone { get; private set; }
			public float ObjectiveKills { get; private set; }
			public float CriticalHitsperMinute { get; private set; }
			public float CriticalHitAccuracy { get; private set; }
			public float EliminationsperLife { get; private set; }
			public float WeaponAccuracy { get; private set; }

			public void SendTable(OverwatchDataTable table)
			{
				Eliminations = table.Stats["Eliminations"].OWValToFloat();
				FinalBlows = table.Stats["Final Blows"].OWValToFloat();
				SoloKills = table.Stats["Solo Kills"].OWValToFloat();
				ShotsFired = table.Stats["Shots Fired"].OWValToFloat();
				ShotsHit = table.Stats["Shots Hit"].OWValToFloat();
				CriticalHits = table.Stats["Critical Hits"].OWValToFloat();
				DamageDone = table.Stats["Damage Done"].OWValToFloat();
				ObjectiveKills = table.Stats["Objective Kills"].OWValToFloat();
				CriticalHitsperMinute = table.Stats["Critical Hits per Minute"].OWValToFloat();
				CriticalHitAccuracy = table.Stats["Critical Hit Accuracy"].OWValToFloat();
				EliminationsperLife = table.Stats["Eliminations per Life"].OWValToFloat();
				WeaponAccuracy = table.Stats["Weapon Accuracy"].OWValToFloat();
			}
		}

		public class BestStats : IStatModule
		{
			public float EliminationsMostinLife { get; private set; }
			public float MostScorewithinoneLife { get; private set; }
			public float DamageDoneMostinLife { get; private set; }
			public float WeaponAccuracyBestinGame { get; private set; }
			public float KillStreakBest { get; private set; }
			public float DamageDoneMostinGame { get; private set; }
			public float EliminationsMostinGame { get; private set; }
			public float FinalBlowsMostinGame { get; private set; }
			public float ObjectiveKillsMostinGame { get; private set; }
			public float ObjectiveTimeMostinGame { get; private set; }
			public float SoloKillsMostinGame { get; private set; }
			public float CriticalHitsMostinGame { get; private set; }
			public float CriticalHitsMostinLife { get; private set; }

			public void SendTable(OverwatchDataTable table)
			{
				EliminationsMostinLife = table.Stats["Eliminations - Most in Life"].OWValToFloat();
				MostScorewithinoneLife = table.Stats["Most Score within one Life"].OWValToFloat();
				DamageDoneMostinLife = table.Stats["Damage Done - Most in Life"].OWValToFloat();
				WeaponAccuracyBestinGame = table.Stats["Weapon Accuracy - Best in Game"].OWValToFloat();
				KillStreakBest = table.Stats["Kill Streak - Best"].OWValToFloat();
				DamageDoneMostinGame = table.Stats["Damage Done - Most in Game"].OWValToFloat();
				EliminationsMostinGame = table.Stats["Eliminations - Most in Game"].OWValToFloat();
				FinalBlowsMostinGame = table.Stats["Final Blows - Most in Game"].OWValToFloat();
				ObjectiveKillsMostinGame = table.Stats["Objective Kills - Most in Game"].OWValToFloat();
				ObjectiveTimeMostinGame = table.Stats["Objective Time - Most in Game"].OWValToFloat();
				SoloKillsMostinGame = table.Stats["Solo Kills - Most in Game"].OWValToFloat();
				CriticalHitsMostinGame = table.Stats["Critical Hits - Most in Game"].OWValToFloat();
				CriticalHitsMostinLife = table.Stats["Critical Hits - Most in Life"].OWValToFloat();
			}
		}

		public class AverageStats : IStatModule
		{
			public float DeathsAverage { get; private set; }
			public float SoloKillsAverage { get; private set; }
			public float ObjectiveTimeAverage { get; private set; }
			public float ObjectiveKillsAverage { get; private set; }
			public float FinalBlowsAverage { get; private set; }
			public float EliminationsAverage { get; private set; }
			public float DamageDoneAverage { get; private set; }

			public void SendTable(OverwatchDataTable table)
			{
				DeathsAverage = table.Stats["Deaths - Average"].OWValToFloat();
				SoloKillsAverage = table.Stats["Solo Kills - Average"].OWValToFloat();
				ObjectiveTimeAverage = table.Stats["Objective Time - Average"].OWValToFloat();
				ObjectiveKillsAverage = table.Stats["Objective Kills - Average"].OWValToFloat();
				FinalBlowsAverage = table.Stats["Final Blows - Average"].OWValToFloat();
				EliminationsAverage = table.Stats["Eliminations - Average"].OWValToFloat();
				DamageDoneAverage = table.Stats["Damage Done - Average"].OWValToFloat();
			}
		}

		public class DeathsStats : IStatModule
		{
			public float Deaths { get; private set; }
			public float EnvironmentalDeaths { get; private set; }

			public void SendTable(OverwatchDataTable table)
			{
				Deaths = table.Stats["Deaths"].OWValToFloat();
				EnvironmentalDeaths = table.Stats["Environmental Deaths"].OWValToFloat();
			}
		}

		public class MatchAwardsStats : IStatModule
		{
			public float MedalsBronze { get; private set; }
			public float MedalsSilver { get; private set; }
			public float MedalsGold { get; private set; }
			public float Medals { get; private set; }

			public void SendTable(OverwatchDataTable table)
			{
				MedalsBronze = table.Stats["Medals - Bronze"].OWValToFloat();
				MedalsSilver = table.Stats["Medals - Silver"].OWValToFloat();
				MedalsGold = table.Stats["Medals - Gold"].OWValToFloat();
				Medals = table.Stats["Medals"].OWValToFloat();
			}
		}

		public class GameStats : IStatModule
		{
			public TimeSpan TimePlayed { get; private set; }
			public float GamesPlayed { get; private set; }
			public float GamesWon { get; private set; }
			public float Score { get; private set; }
			public float ObjectiveTime { get; private set; }
			public float TimeSpentonFire { get; private set; }
			public float WinPercentage { get; private set; }

			public void SendTable(OverwatchDataTable table)
			{
				TimePlayed = table.Stats["Time Played"].OWValToTimeSpan();
				GamesPlayed = table.Stats["Games Played"].OWValToFloat();
				GamesWon = table.Stats["Games Won"].OWValToFloat();
				Score = table.Stats["Score"].OWValToFloat();
				ObjectiveTime = table.Stats["Objective Time"].OWValToFloat();
				TimeSpentonFire = table.Stats["Time Spent on Fire"].OWValToFloat();
				WinPercentage = table.Stats["Win Percentage"].OWValToFloat();
			}
		}
	}
}