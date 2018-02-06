Feature: JobProfileCategory
As a Citizen, I want to be able to view Job Categories So that I can see related jobs in the field

Scenario: [DFC-1342 - A3] Job Category Page displays the correct breadcrumb and links to the Homepage
	Given I am viewing the 'Managerial' category page
	Then the correct 'Job category' breadcrumb is displayed

	When I click the Find a Career breadcrumb on 'Job category'
	Then I am redirected to the homepage


@EndToEnd
Scenario: Navigate between different categories
	Given I am viewing the 'Managerial' category page
	Then display a list of job profiles
	And display the other job categories section
	And the 'Managerial' category should not be in the other job categories section

	When I click on 'Beauty and wellbeing' under other categories
	Then display the correct category title
	And the 'Beauty and wellbeing' category should not be in the other job categories section

	When I click on 'Computing, technology and digital' under other categories
	Then display the correct category title
	And the 'Computing, technology and digital' category should not be in the other job categories section

	When I click on the no '1' job profile on the categories page
	Then display the correct job profile page

@EndToEnd
Scenario: Job Profile Category End to End Test
	Given that I am viewing the Home page
	Then display the "Explore by job category" text
	And display a list of job profile categories

	When I click on job profile category no '2'
	Then display the job profile category page

	When I click on the no '2' job profile on the categories page
	Then display the correct job profile page

Scenario: [DFC-1765 - Signpost banner on JC page is displayed and redirects to the BAU JP Landing Page
	Given I am viewing the 'Managerial' category page
	Then display a list of job profiles
	And the 'category' page signpost banner is displayed

	When I click on the 'category' page banner link
	Then I am redirected to the BAU Job Profile landing page

