Feature: Test

Background: 
	Given I log into Sitefinity

@mytag
Scenario: Navigate to Job Profile Content page
	When I open the Content dropdown
	And I select the 'Job Profiles' link
	Then I am redirected to the 'Job Profiles' content page
	Then log off sitefinity
