using Microsoft.Xrm.Sdk;
using System;
using System.Linq;

namespace P2171_Project6_Plugin
{
	public class CalculateWEMWBSSurveyScores : IPlugin
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

			tracingService.Trace("CalculateWEMWBSSurveyScores: Plugin execution started.");


			if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
			{
				Entity entity = (Entity)context.InputParameters["Target"];
				tracingService.Trace("Attribute value: {0}", entity.LogicalName);

				if (entity.LogicalName == "ark_wemwbswarwickedinburghmentalwellbeingscale")
				{
					Entity surveyWEMWBS;

					if (context.MessageName == "Create")
					{
						surveyWEMWBS = entity;
					}
					else
					{
						surveyWEMWBS = (Entity)context.PostEntityImages["PostImage"];
					}

					var questionOne = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question1")?.Value ?? 0;
					var questionTwo = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question2")?.Value ?? 0;
					var questionThree = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question3")?.Value ?? 0;
					var questionFour = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question4")?.Value ?? 0;
					var questionFive = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question5")?.Value ?? 0;
					var questionSix = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question6")?.Value ?? 0;
					var questionSeven = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question7")?.Value ?? 0;
					var questionEight = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question8")?.Value ?? 0;
					var questionNine = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question9")?.Value ?? 0;
					var questionTen = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question10")?.Value ?? 0;
					var questionEleven = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question11")?.Value ?? 0;
					var questionTwelve = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question12")?.Value ?? 0;
					var questionThirteen = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question13")?.Value ?? 0;
					var questionFourteen = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question14")?.Value ?? 0;

					//Set the question scores
					entity["ark_q1score"] = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question1")?.Value;
					entity["ark_q2score"] = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question2")?.Value;
					entity["ark_q3score"] = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question3")?.Value;
					entity["ark_q4score"] = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question4")?.Value;
					entity["ark_q5score"] = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question5")?.Value;
					entity["ark_q6score"] = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question6")?.Value;
					entity["ark_q7score"] = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question7")?.Value;
					entity["ark_q8score"] = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question8")?.Value;
					entity["ark_q9score"] = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question9")?.Value;
					entity["ark_q10score"] = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question10")?.Value;
					entity["ark_q11score"] = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question11")?.Value;
					entity["ark_q12score"] = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question12")?.Value;
					entity["ark_q13score"] = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question13")?.Value;
					entity["ark_q14score"] = surveyWEMWBS.GetAttributeValue<OptionSetValue>("ark_question14")?.Value;

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
						questionTen,
						questionEleven,
						questionTwelve,
						questionThirteen,
						questionFourteen
					}.Sum(); 

					service.Update(entity);
				}
			}
		}
	}
}


