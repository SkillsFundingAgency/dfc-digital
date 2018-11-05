Feature: Test

Background: 

@mytag
Scenario: Navigate to Job Profile Content page
	Given I log into Sitefinity
	When I open the Content dropdown
	And I select the 'Job Profiles' link
	Then I am redirected to the 'Job Profiles' content page
	Then log off sitefinity

	
	
Scenario: Search Term code to check number of results returned
	Given I navigate to the homepage
	When I search for the data items