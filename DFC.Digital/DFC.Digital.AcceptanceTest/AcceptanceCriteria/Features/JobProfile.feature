Feature: JobProfile
As a Citizen I want to be able to view the JobProfile Page So that I can view the job profile details

Scenario: [DFC-1342 - A2] Profile Page displays the correct breadcrumb and links to the Homepage
	Given that I am viewing the 'dental-nurse' job profile page
	Then the correct 'Job profile' breadcrumb is displayed

	When I click the Explore career breadcrumb on 'Job profile'
	Then I am redirected to the explore careers page

Scenario: [DFC-1335 - A1] Profile Page displays Related careers with no more than 5 shown and takes you to the correct carers page
	Given that I am viewing the 'gp' job profile page
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
	Given that I am viewing the 'bin-worker' job profile page
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
Scenario: Apprenticeships are displayed on Job Profile Page
	Given that I am viewing the 'plumber' job profile page
	Then the correct sections should be displayed
	And '2' apprenticeships should be displayed with valid data

@EndToEnd
Scenario: Apprenticeship are not displayed on Job Profile Page and correct message shown
	Given that I am viewing the 'gp' job profile page
	Then the correct sections should be displayed
	And the no apprenticeships message should be shown

@EndToEnd
Scenario: Valid Search on Job Profile Page
	Given that I am viewing the 'Chef' job profile page
	Then the search section should be displayed
	When I search using 'nurse' on the profile page
	Then the first result is 'shown' on the page

Scenario: JP Survey - Answering YES to the survey
	Given that I am viewing the 'careers-adviser' job profile page
	Then the job profile survey banner is displayed on the page
	When I select 'yes' on the survey
	Then the thank you message is displayed
	And the job profile cookie is set

	When I delete the job profile survey cookie
	Then the job profile survey banner is displayed on the page

Scenario: JP Survey - Answering NO to the survey
	Given that I am viewing the 'assistant-immigration-officer' job profile page
	When I select 'no' on the survey
	Then the alternate message is displayed

	When I click the jp click here feedback survey link
	Then I am redirected to the JP survey page

	When I click to go back
	And I delete the job profile survey cookie
	Then the job profile survey banner is displayed on the page
