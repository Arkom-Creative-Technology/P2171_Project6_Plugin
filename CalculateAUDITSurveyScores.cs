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
						surveyAUDIT = (Entity)context.PostEntityImages["PostImage"];
					}

					var questionOne = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question1")?.Value ?? 0;
					var questionTwo = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question2")?.Value ?? 0;
					var questionThree = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question3")?.Value ?? 0;
					var questionFour = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question4")?.Value ?? 0;
					var questionFive = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question5")?.Value ?? 0;
					var questionSix = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question6")?.Value ?? 0;
					var questionSeven = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question7")?.Value ?? 0;
					var questionEight = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question8")?.Value ?? 0;
					var questionNine = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question9")?.Value ?? 0;
					var questionTen = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question10")?.Value ?? 0;

					//Set the question scores
					entity["ark_q1score"] = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question1")?.Value;
					entity["ark_q2score"] = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question2")?.Value;
					entity["ark_q3score"] = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question3")?.Value;
					entity["ark_q4score"] = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question4")?.Value;
					entity["ark_q5score"] = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question5")?.Value;
					entity["ark_q6score"] = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question6")?.Value;
					entity["ark_q7score"] = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question7")?.Value;
					entity["ark_q8score"] = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question8")?.Value;
					entity["ark_q9score"] = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question9")?.Value;
					entity["ark_q10score"] = surveyAUDIT.GetAttributeValue<OptionSetValue>("ark_question10")?.Value;

					//Calculate overall score
					entity["ark_overallscore"] = new[]
					{
						questionOne,
						questionTwo,
						questionThree,
						questionFour,
						questionFive,
						questionSix,
						questionSeven,
						questionEight,
						questionNine,
						questionTen
					}.Sum(); 

					service.Update(entity);
				}
			}
		}
	}
}


