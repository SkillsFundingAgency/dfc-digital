Feature: FindACourse

Scenario: Search for course and visit the course details page
	Given that I am viewing the Home page
	When I click the Find a Course link
	Then I am redirected to the Find a Course landing page

	When I search for the course 'Maths'
	Then I am shown course results for 'Maths'

	When I click on course result no '1'
	Then I am redirected to the correct course details page

Scenario Outline: Search for course and other information
	Given that I am viewing the Find A Course landing page
	When I search for the Course <CourseName>, provider <ProviderName>, location <Location>, show courses <Show16to19>
	Then I am shown course results for '<CourseName>'
		And the filters applied <ProviderName>, <Location>, <Show16to19>

	When I change the provider <Provider2> and location <Location2>
	Then the results should be updated with the new provider <Provider2> and location <Location2>

Examples:
| CourseName  | ProviderName      | Location  | Show16to19 | Provider2 | Location2  |
| electrician | Leicester College | Leicester |            | Skills    | Birmingham | 

Scenario: Search for course with 0 results
	Given that I am viewing the Find A Course landing page
	When I search for the course 'NoCourse'
	Then I am shown 0 results with the correct messaging

Scenario Outline: Search for course and Apply filters
	Given that I am viewing the Find A Course landing page
	When I search for the course '<CourseName>'
	Then I am shown course results for '<CourseName>'

	When I apply the filter hours <CourseHours>, type <CourseType> and start date <StartDate>
	Then the following filters <CourseHours>, <CourseType>, <StartDate> are selected

Examples: 
| CourseName | CourseHours | CourseType | StartDate |
| Maths      | Full time   | Online     | Today     |
