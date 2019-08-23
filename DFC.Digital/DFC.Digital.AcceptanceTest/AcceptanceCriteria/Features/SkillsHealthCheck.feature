Feature: SkillsHealthCheck
As a Citizen, I want to be able to do skills health checks tests with the National Careers Service

Scenario Outline: Complete a 'Skills Health Check' 
	Given I navigate to the Skills Health Check Home page
	When I click on Start a new Skills Health Check button
	Then I am directed to the Your Assessments List page
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


Scenario Outline: Save and delete a 'Skills Health Check' when signed in
	Given I navigate to the Skills Health Check Home page
	And I click on the SignIn button
	Then I am directed to the SignIn Page
	When I sign in with an existing user name and password
	Then I am directed back to the Skills Health Check Home page
	And I am now signed in
	When I click on Start a new Skills Health Check button
	Then I am directed to the Your Assessments List page
	When I select to start the <TypeOfSkillsHealthCheck> check
	Then I am redirected to the start page for a <TypeOfSkillsHealthCheck> check with <QuestionsToAnswer> questions to answer
	When I answer all <QuestionsToAnswer> questions 
	Then on the last question I see the Return to Skills health check page button
	When I click on the Return to Skills health check page button 
	Then I am taken back to the Your Assessments page
	And II can see my completed checks
Examples: 
| TypeOfSkillsHealthCheck	| QuestionsToAnswer	| 
| Motivation				| 42				|  

