Feature: Test

Background: 
	Given I log into Sitefinity

@mytag
Scenario: login to sitefinity
	When I open the Content dropdown
	And I select the 'Job Profiles' link
	Then log off sitefinity
