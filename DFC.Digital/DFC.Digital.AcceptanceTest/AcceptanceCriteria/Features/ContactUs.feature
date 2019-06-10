Feature: ContactUs
As a Citizen, I want to be able to use the various methods to get in contact with the National Careers Service

Scenario Outline: Submit a 'Contact An Adviser' form
	Given I navigate to the Contact us select option page
	When I select the 'contact-adviser' option
		And I press continue
	Then I am redirected to the first 'adviser' contact form
	When I complete the first form with <ContactReason> option and <AdviserQuery> query
		And I press continue
	Then I am redirected to the second 'adviser' contact form
	When I complete the details form with the details <Firstname>, <Lastname>, <EmailAddress>, <ConfirmationEmail>, <DOB>, <PostCode>
		And I press send
	Then I am redirected to the confirmation page

Examples: 
| ContactReason | AdviserQuery            | Firstname      | Lastname       | EmailAddress                     | ConfirmationEmail                | DOB        | PostCode |
| Funding       | contact an Adviser Form | Automated Test | Automated Test | automatedtestesfa@mailinator.com | automatedtestesfa@mailinator.com | 20/11/2000 | CV3 5FE  |

Scenario Outline: Submit a 'Give Feedback' form
	Given I navigate to the Contact us select option page
	When I select the 'give-feedback' option
		And I press continue on feecback form
	Then I am redirected to the first 'feedback' contact form
	When I complete the first feedback form with <ContactReason> option and <AdviserQuery> query
		And I press continue on feecback form
	Then I am redirected to the second 'feedback' contact form
	When I complete the give feedback details form with the details <Firstname>, <Lastname>, <EmailAddress>, <ConfirmationEmail>, <Contact>
		And I press send
	Then I am redirected to the confirmation page

Examples: 
| ContactReason | AdviserQuery       | Firstname      | Lastname       | EmailAddress                     | ConfirmationEmail                | Contact |
| Funding       | Give Feedback Forn | Automated Test | Automated Test | automatedtestesfa@mailinator.com | automatedtestesfa@mailinator.com | yes     |


Scenario Outline: Submit a 'Technical Feedback' form
	Given I navigate to the Contact us select option page
	When I select the 'technical-issue' option
		And I press continue on tecnical form
	Then I am redirected to the first 'technical' contact form
	When I complete the first technical form with <AdviserQuery> query
		And I press continue on tecnical form
	Then I am redirected to the second 'technical' contact form
	When I complete the give technical details form with the details <Firstname>, <Lastname>, <EmailAddress>, <ConfirmationEmail>, <Contact>
		And I press send
	Then I am redirected to the confirmation page

Examples: 
| AdviserQuery       | Firstname      | Lastname       | EmailAddress                     | ConfirmationEmail                | Contact |
| Give Feedback Forn | Automated Test | Automated Test | automatedtestesfa@mailinator.com | automatedtestesfa@mailinator.com | yes     |
