Feature: SkillsHealthCheck
As a Citizen, I want to be able to do skills health checks tests with the National Careers Service

Scenario Outline: Complete and download 'Skills Health Check' 
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


Scenario Outline: Sign in complete a check, sign out and back in to access completed saved checks in my account 
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
	And I can see my completed <TypeOfSkillsHealthCheck> check
	When I click on the Your Account link
	Then I am redirected to the Your Account Home page
	And I can see the saved Skills health check
	When I click on Sign out
	Then I am signed out of my account
	Given I navigate to the Skills Health Check Home page
	When I click on the SignIn button
	Then I am directed to the SignIn Page
	When I sign in with an existing user name and password
	Then I am directed back to the Skills Health Check Home page
	And I am shown the section I have already started a Skills Health Check
	When I click on the Show my Skills Health Check documents 
	Then I am redirected to the Your Account Home page
	When I click the View link
	Then I am taken back to the Your Assessments page
	And I can see my completed <TypeOfSkillsHealthCheck> check
	When I click on the Your Account link
	Then I am redirected to the Your Account Home page
	And I can see the saved Skills health check
	When I delete my Skills health check
	Then I am asked to confirm the delete
	When I confirm the delete
	Then I am redirected to the Your Account Home page
	And I have no saved Skills health checks
	When I click on Sign out
	Then I am signed out of my account
	
Examples: 
| TypeOfSkillsHealthCheck	| QuestionsToAnswer	| 
| Motivation				| 42				|  

