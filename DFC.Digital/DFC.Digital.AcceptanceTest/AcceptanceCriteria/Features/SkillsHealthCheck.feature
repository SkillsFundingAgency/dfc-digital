Feature: SkillsHealthCheck
As a Citizen, I want to be able to do skills health checks tests with the National Careers Service

Scenario Outline: Complete a 'Skills Health Check' 
	Given I navigate to the Your Assessments page
	When I select to start the <TypeOfSkillsHealthCheck> check
	Then I am redirected to the start page for a <TypeOfSkillsHealthCheck> check with <QuestionsToAnswer> questions to answer
	When I answer all <QuestionsToAnswer> questions 
	Then on the last question I see the Return to Skills health check page button
	When I click on the Return to Skills health check page button 
	Then I am taken back to the Your Assessments page
	And I can down load my completed assessment as a PDF 
	And I can down load my completed assessment as a Word document 
Examples: 
| TypeOfSkillsHealthCheck	| QuestionsToAnswer	| 
| Motivation				| 42				|  

