using Microsoft.Xrm.Sdk;
using System;
using System.Linq;

namespace P2171_Project6_Plugin
{
	public class CalculateAUDITSurveyScores : IPlugin
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

			tracingService.Trace("CalculateAUDITSurveyScores: Plugin execution started.");

			if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
			{
				Entity entity = (Entity)context.InputParameters["Target"];
				tracingService.Trace("Attribute value: {0}", entity.LogicalName);

				if (entity.LogicalName == "ark_auditalcoholusedisordersidentificationtest")
				{
					Entity surveyAUDIT;

					if (context.MessageName == "Create")
					{
						surveyAUDIT = entity;
					}
					else
					{
						surveyAUDIT = context.PreEntityImages["PreEntityImage"];
					}

					var questionOne = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question1");
					var questionTwo = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question2");
					var questionThree = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question3");
					var questionFour = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question4");
					var questionFive = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question5");
					var questionSix = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question6");
					var questionSeven = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question7");
					var questionEight = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question8");
					var questionNine = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question9");
					var questionTen = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question10");

					//Set the question scores
					entity["ark_q1score"] = questionOne;
					entity["ark_q2score"] = questionTwo;
					entity["ark_q3score"] = questionThree;
					entity["ark_q4score"] = questionFour;
					entity["ark_q5score"] = questionFive;
					entity["ark_q6score"] = questionSix;
					entity["ark_q7score"] = questionSeven;
					entity["ark_q8score"] = questionEight;
					entity["ark_q9score"] = questionNine;
					entity["ark_q10score"] = questionTen;

					//Calculate overall score
					entity["ark_overallscore"] = new[]
					{
						questionOne.Value,
						questionTwo.Value,
						questionThree.Value,
						questionFour.Value,
						questionFive.Value,
						questionSix.Value,
						questionSeven.Value
					}.Sum(); 

					service.Update(entity);
				}
			}
		}
	}
}


