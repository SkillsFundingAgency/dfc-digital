Feature: JobProfileSearch
As a Citizen, I want to be able to search for job profiles So that I can view specific results

# Scenarios demonstrating the correct application of [DFC-165 - A1] 
Scenario Outline: [DFC-165 - A1] When the search result item title is clicked then the user is directed to the specific job profile page
	Given that I am viewing the Home page
	When I search using '<SearchTerm>'
	And I click on result title no '1'
	Then I am redirected to the correct job profile page
Examples: 
	| SearchTerm   |
	| Dental Nurse |
	| Adult nurse  |

Scenario: [DFC-340 - A1] Performing a search with text within escaped characters that will return a result
	Given that I am viewing the Home page
	When I search using '<Police Officer>'
	Then the first result is 'shown' on the page

Scenario: Perform a search that will return 0 results
	Given that I am viewing the Home page
	When I search using 'return0results'
	Then the no results message is displayed

Scenario: [DFC-1342 - A1] Search Page displays the correct breadcrumb and links to the Homepage
	Given that I am viewing the Home page
	When I search using 'analyst'
	Then the first result is 'shown' on the page
	And the correct 'Search results' breadcrumb is displayed

	When I click the Explore career breadcrumb on 'Search results'
	Then I am redirected to the homepage

@EndToEnd
Scenario: Search End To End Test
	Given that I am viewing the Home page
	When I search using 'Nurse'
	Then the first result is 'shown' on the page

	When I click on result title no '1'
	Then I am redirected to the correct job profile page
	And the correct sections should be displayed

	When I click the Explore careers link
	And I search using 'manager'
	Then the first result is 'shown' on the page

	When I click on result title no '2'
	Then I am redirected to the correct job profile page
	And the correct sections should be displayed
  
Scenario: [DFC-1496 - A1] Starting a search displays Auto Suggest, and when selected, populates the searchbox
	Given that I am viewing the Home page
	When I enter the term 'Te'
	Then the suggested results should appear
	When I select suggested result no '1' and search
	Then the first result is 'shown' on the page
	And the search box should populate with the selected result

Scenario: [DFC-1494 - A1] Performing incorrectly spelled search suggests a Did You Mean option
	Given that I am viewing the Home page
	When I search using 'nusre'
	Then search should display the Did You Mean text
	When I click the suggested text
	Then I am redirected to the correct search results page
	And search should not display the Did You Mean text

Scenario: [DFC-1495 - A1] Performing a search displays Job categories and clicking one takes you to the categories page
	Given that I am viewing the Home page
	When I search using 'manager'
	Then the first result is 'shown' on the page
	And the results display the Found In category text
	When I click the first category link on result no '1'
	Then I am redirected to the correct job category page