using Microsoft.Xrm.Sdk;
using System;
using System.Linq;

namespace P2171_Project6_Plugin
{
	public class CalculatePGSISurveyScores : IPlugin
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

			tracingService.Trace("CalculatePGSISurveyScores: Plugin execution started.");


			if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
			{
				Entity entity = (Entity)context.InputParameters["Target"];
				tracingService.Trace("Attribute value: {0}", entity.LogicalName);

				if (entity.LogicalName == "ark_pgsiproblemgamblingseverityindex")
				{
					Entity surveyPGSI;

					if (context.MessageName == "Create")
					{
						surveyPGSI = entity;
					}
					else
					{
						surveyPGSI = (Entity)context.PostEntityImages["PostImage"];
					}

					var questionOne = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question1")?.Value ?? 0;
					var questionTwo = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question2")?.Value ?? 0;
					var questionThree = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question3")?.Value ?? 0;
					var questionFour = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question4")?.Value ?? 0;
					var questionFive = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question5")?.Value ?? 0;
					var questionSix = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question6")?.Value ?? 0;
					var questionSeven = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question7")?.Value ?? 0;
					var questionEight = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question8")?.Value ?? 0;
					var questionNine = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question9")?.Value ?? 0;

					//Set the question scores
					entity["ark_q1score"] = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question1")?.Value;
					entity["ark_q2score"] = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question2")?.Value;
					entity["ark_q3score"] = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question3")?.Value;
					entity["ark_q4score"] = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question4")?.Value;
					entity["ark_q5score"] = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question5")?.Value;
					entity["ark_q6score"] = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question6")?.Value;
					entity["ark_q7score"] = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question7")?.Value;
					entity["ark_q8score"] = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question8")?.Value;
					entity["ark_q9score"] = surveyPGSI.GetAttributeValue<OptionSetValue>("ark_question9")?.Value;

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
					}.Sum(); 

					service.Update(entity);
				}
			}
		}
	}
}


