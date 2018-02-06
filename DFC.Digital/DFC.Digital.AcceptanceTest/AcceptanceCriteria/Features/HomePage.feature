Feature: HomePage
As a Citizen I want to be able to view the Homepage So that I can be signposted to search

	#Acceptance Test
Scenario: [DFC-274 - A2] Job profile category page
	Given that I am viewing the Home page
	When I click on job profile category no '5'
	Then display the job profile category page


@EndToEnd
Scenario: Check the cookie page loads
	Given that I am viewing the Home page
	When I click on the Cookies link
	Then I am redirected to the cookies page

@EndToEnd
Scenario: 404 Page is displayed when navigating to a profile that doesn't exist
	Given that I am viewing the 'profile-does-not-exist' job profile page
	Then I am redirected to the 404 page

	When I click on the Back To Homepage link
	Then I am redirected to the homepage
	And display the "Explore by job category" text
	And display a list of job profile categories