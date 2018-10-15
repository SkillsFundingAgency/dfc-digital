Feature: VocSurvey
As a Citizen, I want to be presented with a survey So that I can leave feedback on the service

Scenario: [DFC-610-A1] View Survey - Check Cookie
	Given that I am viewing the Home page
	Then the survey banner should be displayed

	When I click the close survey link
	Then the survey banner should not be displayed
	And a cookie should appear saying dismissed

	When I delete the cookie and refresh the page
	Then the survey banner should be displayed

Scenario: [DFC-685-A2] Select fill in short survey, Select Online survey and confirm survey is loaded
	Given that I am viewing the Home page
	When I select to fill in the online survey
	Then I am redirected to the survey page

	When I delete the cookie and refresh the page
	Then the survey banner should be displayed