Feature: JobProfile
As a Citizen I want to be able to view the JobProfile Page So that I can view the job profile details

Scenario: [DFC-1342 - A2] Profile Page displays the correct breadcrumb and links to the Homepage
	Given that I am viewing the 'dental-nurse' job profile page
	Then the correct 'Job profile' breadcrumb is displayed

	When I click the Find a Career breadcrumb on 'Job profile'
	Then I am redirected to the homepage

Scenario: [DFC-1335 - A1] Profile Page displays Related careers with no more than 5 shown and takes you to the correct carers page
	Given that I am viewing the 'plumber' job profile page
	Then the Related Careers section is displayed on the page
	And there should be no more than 5 careers

	When I click on career title '1'
	Then I am redirected to the correct job profile page

Scenario: [DFC-1508 - A1] Profile Page displays related courses. Select the course title and ensure you are redirected to the Course Directory
	Given that I am viewing the 'Electrician' job profile page
	When I click on the training course title no '1'
	Then take me to the training course page on Course Directory
	When I click to go back
	Then take me back to the job profile I had been viewing

Scenario: [DFC-1508 - A2] Selecting the Find courses near you link on the job profile page for courses should take me to the search page on Course Directory
	Given that I am viewing the 'nurse' job profile page
	When I click Find courses near you
	Then take me to the search page on Course Directory
	When I click to go back
	Then take me back to the job profile I had been viewing

#@EndToEnd
#Scenario: Useful Links Test on Job Profile Page
#	Given that I am viewing the 'dental-nurse' job profile page
#	Then the Useful Links section is displayed on the page
#
#	When I click the 'Find a course' link
#	Then I am redirected to the correct 'nationalcareersservice.direct.gov.uk/course-directory/home' url
#
#	When I click the 'Apprenticeships' link
#	Then I am redirected to the correct 'findapprenticeship.service.gov.uk/apprenticeshipsearch' url
#
#	When I click the 'Thinking about university' link
#	Then I am redirected to the correct 'ucas.com' url
#
#	When I click the 'Help with funding' link
#	Then I am redirected to the correct 'gov.uk/further-education-courses' url
#
#	When I click the 'Become a volunteer' link
#	Then I am redirected to the correct 'gov.uk/volunteering' url

@EndToEnd
Scenario: Apprenticeship and courses displayed on Job Profile Page
	Given that I am viewing the 'plumber' job profile page
	Then the correct sections should be displayed
	And '4' apprenticeships should be displayed with valid data

@EndToEnd
Scenario: Apprenticeship are not displayed on Job Profile Page and correct message shown
	Given that I am viewing the 'border-force-officer' job profile page
	Then the correct sections should be displayed
	And the no apprenticeships message should be shown

@EndToEnd
Scenario: Valid Search on Job Profile Page
	Given that I am viewing the 'Chef' job profile page
	Then the search section should be displayed
	When I search using 'nurse' on the profile page
	Then the first result is 'shown' on the page

Scenario: [DFC-1763 & DFC-1761] SignPost banner is displayed on JP page and redirects to the corresponding BAU JP Page
	Given that I am viewing the 'GP' job profile page
	Then the 'profile' page signpost banner is displayed
	When I click on the 'profile' page banner link
	Then I am redirected to corresponding 'BAU' profile page

	When I click the BAU JP signpost banner
	Then I am redirected to corresponding 'Beta' profile page



