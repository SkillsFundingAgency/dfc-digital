Feature: PreSearchFilter

Scenario Outline: End to End on Pre Search Filter
	Given that I am viewing the Home page
	When I click on the PSF Continue button
	Then I am redirected to the 'JobLevel' page

	When I select the Job Level tags <jobLevelTags>
	And I press continue
	Then I am redirected to the 'PersonalInterest' page

	When I select the Personal Interest tags <interestTags>
	And I press continue
	Then I am redirected to the 'PreferredTasks' page
	
	When I select the Preferred Tasks tags <tasksTags>
	And I press continue
	Then I am redirected to the 'WaysToGetIn' page

	When I select the Ways To Get In tags <waysToGetInTags>
	And I press continue
	Then I am redirected to the 'FilterSearchResults' page
	And Job Profiles are displayed
	
	When I click on result title no '1'
	Then I am redirected to the correct job profile page

	Examples: 
	| jobLevelTags | interestTags | tasksTags | waysToGetInTags |
	| test         |              |           |                 |

Scenario Outline: PSF - No Results returned messaging
	Given that I am viewing the Home page
	When I click on the PSF Continue button
		And I select the Job Level tags <jobLevelTags>
		And I press continue
		And I select the Personal Interest tags <interestTags>
		And I press continue
		And I select the Preferred Tasks tags <tasksTags>
		And I press continue
		And I select the Ways To Get In tags <waysToGetInTags>
		And I press continue
	Then I am redirected to the 'FilterSearchResults' page
	And the no filter results message is displayed

	Examples: 
	| jobLevelTags | interestTags                   | tasksTags | waysToGetInTags          |
	| Job level 4  | broadcasting & performing arts | Selling   | Learn through experience |

Scenario: PSF - Back button redirects to correct Filter
	Given that I am viewing the Home page
	When I click on the PSF Continue button
	Then I am redirected to the 'JobLevel' page
	When I press continue
		And I press continue
		And I press continue
		And I press continue
	Then I am redirected to the 'FilterSearchResults' page
	
	When I press the Back link
	Then I am redirected to the 'WaysToGetIn' page
	When I press the Back link
	Then I am redirected to the 'PreferredTasks' page
	When I press the Back link
	Then I am redirected to the 'PersonalInterest' page
	When I press the Back link
	Then I am redirected to the 'JobLevel' page
	When I press the Back link
	Then I am redirected to the homepage

TEST to Cover the None functionality and whether selections are remembered
