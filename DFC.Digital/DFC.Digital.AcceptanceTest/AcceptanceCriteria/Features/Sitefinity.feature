Feature: Sitefinity

@mytag
Scenario: Navigate to Reporting page
	Given I am logged into Sitefinity
	Then I can see the Dashboard page

	When I go to the CMS Reports page
	Then the Job Profile report should be displayed

	When I filter by contains on SOC using code '2113'
	Then the filter should be applied