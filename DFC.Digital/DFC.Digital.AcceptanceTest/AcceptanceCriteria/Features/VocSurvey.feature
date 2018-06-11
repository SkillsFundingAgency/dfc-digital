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

Scenario: [DFC-685-A1] Select fill in short survey, Enter email and	press Send
	Given that I am viewing the Home page
	When I enter the email 'simulate-delivered@notifications.service.gov.uk' and press send
	Then the success message is displayed

	When I delete the cookie and refresh the page
	Then the survey banner should be displayed

Scenario: [DFC-685-A2] Select fill in short survey, Select Online survey and confirm survey is loaded
	Given that I am viewing the Home page
	When I select to fill in the online survey
	Then I am redirected to the survey page

	When I delete the cookie and refresh the page
	Then the survey banner should be displayed

Scenario: [DFC-1562-1808] Personalised Survey - Check Cookies update with last JP visited and GA tag is not null
	Given that I am viewing the Home page
	And I have not visited the job profile page previously
	Then the survey banner should be displayed
	And the vocPersonalisation cookie should not be displayed

	When I search using 'nurse'
	And I click on result title no '1'
	Then I am redirected to the correct job profile page
	And the vocPersonaliation cookie should display the last job profile title
	And the GA client ID should not be empty

	When I enter the email 'simulate-delivered@notifications.service.gov.uk' and press send
	Then the success message is displayed