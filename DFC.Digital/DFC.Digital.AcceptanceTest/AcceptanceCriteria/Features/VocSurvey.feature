Feature: VocSurvey
As a Citizen, I want to be presented with a survey So that I can leave feedback on the service

Scenario: Complete Feedback Survey
	Given that I am viewing the Home page
	When I click the feedback link
	Then I am redirected to the survey page
