Feature: SkillsHealthCheck
As a Citizen, I want to be able to do skills health checks tests with the National Careers Service

Scenario Outline: Complete and download 'Skills Health Check' 
	Given I navigate to the Skills Health Check Home page
	When I click on Start a new Skills Health Check button
	Then I am directed to the Your Assessments List page
	When I select to start the <TypeOfSkillsHealthCheck> check
	Then I am redirected to the start page for a <CheckTitle> check with <QuestionsToAnswer> questions to answer
	When I answer all <QuestionsToAnswer> questions 
	Then on the last question I see the Return to Skills health check page button
	When I click on the Return to Skills health check page button 
	Then I am taken back to the Your Assessments page
	And I can download my completed assessment as a PDF 
	And I can download my completed assessment as a Word document 
	Then I clear my session to remove my temp Skills Health Check
Examples: 
| CheckTitle			| TypeOfSkillsHealthCheck	| QuestionsToAnswer |
| Working with shapes	| Spatial					| 17                |  

Scenario Outline: Must answer a question before you can continue to the next one.
	Given I navigate to the Skills Health Check Home page
	When I click on Start a new Skills Health Check button
	Then I am directed to the Your Assessments List page
	When I select to start the <TypeOfSkillsHealthCheck> check
	Then I am redirected to the start page for a <CheckTitle> check with <QuestionsToAnswer> questions to answer
	When I try to continue with out answering a question
	Then I am shown an error message and not allowed to continue
	When I answer all <QuestionsToAnswer> questions 
	Then on the last question I see the Return to Skills health check page button
	When I click on the Return to Skills health check page button 
	Then I am taken back to the Your Assessments page	
	Then I clear my session to remove my temp Skills Health Check

	
Examples: 
| CheckTitle					| TypeOfSkillsHealthCheck	| QuestionsToAnswer |
|Solving mechanical problems	| Mechanical				| 14               |  
