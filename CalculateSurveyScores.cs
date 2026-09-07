using Microsoft.Xrm.Sdk;
using System;

namespace P2171_Project6_Plugin
{
	public class CalculateSurveyScores : IPlugin
	{
		private IPluginExecutionContext executionContext;
		private IOrganizationService service;
		private ITracingService tracingService;

		public void Execute(IServiceProvider serviceProvider)
		{
			Microsoft.Xrm.Sdk.IPluginExecutionContext context = (Microsoft.Xrm.Sdk.IPluginExecutionContext)
			serviceProvider.GetService(typeof(Microsoft.Xrm.Sdk.IPluginExecutionContext));

			tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
			executionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
			service = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory)))
				.CreateOrganizationService(executionContext.UserId);

			if (context.Depth > 1)
			{
				return;
			}

			tracingService.Trace("CalculateSurveyScores: Plugin execution started.");

			if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
			{
				Entity entity = (Entity)context.InputParameters["Target"];
				tracingService.Trace("Attribute value: {0}", entity.LogicalName);

				if (entity.LogicalName == "ark_gad7survey")
				{
					Entity surveyGAD;

					if (context.MessageName == "Create")
					{
						surveyGAD = entity;
					}
					else
					{
						//surveyGAD = (Entity)context.PreEntityImages["PreSurveyGAD"];
						surveyGAD = (Entity)context.PostEntityImages["SurveyGAD"];
					}

					var questionOne = surveyGAD.GetAttributeValue<OptionSetValue>("ark_feelingnervousanxiousoronedge");
					var questionTwo = surveyGAD.GetAttributeValue<OptionSetValue>("ark_notbeingabletostoporcontrolworrying");
					var questionThree = surveyGAD.GetAttributeValue<OptionSetValue>("ark_worryingtoomuchaboutdifferentthings");
					var questionFour = surveyGAD.GetAttributeValue<OptionSetValue>("ark_troublerelaxing");
					var questionFive = surveyGAD.GetAttributeValue<OptionSetValue>("ark_beingsorestlessthatitishardtositstill");
					var questionSix = surveyGAD.GetAttributeValue<OptionSetValue>("ark_becomingeasilyannoyedorirritable");
					var questionSeven = surveyGAD.GetAttributeValue<OptionSetValue>("ark_feelingafraid");

					entity["ark_totalscore"] = questionOne.Value + questionTwo.Value + questionThree.Value + questionFour.Value + questionFive.Value + questionSix.Value + questionSeven.Value;
					entity["ark_calculatescore"] = false;

					service.Update(entity);
				}

				if (entity.LogicalName == "ark_phq9survey")
				{
					Entity surveyPHQ;

					if (context.MessageName == "Create")
					{
						surveyPHQ = entity;
					}
					else
					{
						surveyPHQ = (Entity)context.PostEntityImages["SurveyPHQ"];
					}

					var questionOne = surveyPHQ.GetAttributeValue<OptionSetValue>("ark_littleinterestorpleasureindoingthings");
					var questionTwo = surveyPHQ.GetAttributeValue<OptionSetValue>("ark_feelingdowndepressedorhopeless");
					var questionThree = surveyPHQ.GetAttributeValue<OptionSetValue>("ark_troublefallingasleepstayingasleeporsleep");
					var questionFour = surveyPHQ.GetAttributeValue<OptionSetValue>("ark_feelingtiredorhavinglittleenergy");
					var questionFive = surveyPHQ.GetAttributeValue<OptionSetValue>("ark_poorappetiteorovereating");
					var questionSix = surveyPHQ.GetAttributeValue<OptionSetValue>("ark_feelingbadaboutyourself");
					var questionSeven = surveyPHQ.GetAttributeValue<OptionSetValue>("ark_troubleconcentratingonthings");
					var questionEight = surveyPHQ.GetAttributeValue<OptionSetValue>("ark_movingorspeakingsoslowly");
					var questionNine = surveyPHQ.GetAttributeValue<OptionSetValue>("ark_thoughtsthatyouwouldbebetteroffdead");

					entity["ark_totalscore"] = questionOne.Value + questionTwo.Value + questionThree.Value + questionFour.Value + questionFive.Value + questionSix.Value + questionSeven.Value + questionEight.Value + questionNine.Value;
					entity["ark_calculatescore"] = false;

					service.Update(entity);
				}
			}
		}
	}
}


