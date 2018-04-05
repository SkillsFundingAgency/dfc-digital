Feature: PreSearchFilter
As a Citizen, I want to be able to search with relevant filters, So that I can explore job profiles that are relevant to me

Scenario Outline: PSF - End to End on Pre Search Filter
	Given that I am viewing the Home page
	When I click on the PSF Continue button
	Then I am redirected to the 'Job level' page

	When I select the tags <jobLevelTags>
	And I press continue on the page
	Then I am redirected to the 'Personal interests' page

	When I select the tags  <interestTags>
	And I press continue on the page
	Then I am redirected to the 'Preferred tasks' page
	
	When I select the tags  <tasksTags>
	And I press continue on the page
	Then I am redirected to the 'Ways to get in' page

	When I select the tags  <waysToGetInTags>
	And I press continue on the page
	Then I am redirected to the 'Filter search results' page
	And Job Profiles are displayed

	#When I select the tags  <jobAreasTags>
	#And I press continue on the page
	#Then I am redirected to the 'Ways to get in' page
#	When I select the tags <careerFocusTags>
#	And I press continue on the page
#	Then I am redirected to the 'Restrictions' page
#
#	When I select the tags <restrictionsTags>
#	And I press continue on the page
#	Then I am redirected to the 'Filter search results' page
#	And Job Profiles are displayed
	
	When I click on filter results no '1'
	Then I am redirected to the correct job profile page

	Examples: 
	| jobLevelTags            | interestTags  | tasksTags | jobAreasTags         | waysToGetInTags         | careerFocusTags | restrictionsTags |
	| Job level 3,Job level 4 | Animals or pets,Computing and tech | computer,Listening  | Healthcare | college or university | None            | None             |

#TODO: Discuss this, Not sure this is a valid test??
Scenario Outline: PSF - No Results returned messaging
	Given that I am viewing the Home page
	When I click on the PSF Continue button
		And I select the tags <jobLevelTags>
		And I press continue on the page
		And I select the tags <interestTags>
		And I press continue on the page
		And I select the tags <tasksTags>
		And I press continue on the page
		And I select the tags <waysToGetInTags>
		And I press continue on the page
		#And I select the tags <jobAreasTags>
		#And I press continue on the page
		#And I select the tags <careerFocusTags>
		#And I press continue on the page
		#And I select the tags <restrictionsTags>
		#And I press continue on the page

	Then I am redirected to the 'Filter search results' page
	And the no filter results message is displayed

	Examples: 
	| jobLevelTags | interestTags | tasksTags | jobAreasTags | waysToGetInTags  | careerFocusTags | restrictionsTags |
	| Job level 1  | Music        | Managing  | Transport    | In-work training | None            | None             |

Scenario Outline: PSF - Back button redirects to correct Filter
	Given that I am viewing the Home page
	When I click on the PSF Continue button
	Then I am redirected to the 'Job level' page
	When I select the tags <jobLevelTags>
		And I press continue on the page
		And I select the tags <interestTags>
		And I press continue on the page
		And I select the tags <tasksTags>
		And I press continue on the page
		And I select the tags <waysToGetInTags>
		And I press continue on the page
	Then I am redirected to the 'Filter search results' page
	
		#And I select the tags <jobAreasTags>
		#And I press continue on the page
		#And I select the tags <careerFocusTags>
		#And I press continue on the page
		#And I select the tags <restrictionsTags>
		#And I press continue on the page

	When I press the back link on the page
	Then I am redirected to the 'Ways to get in' page
		And the <waysToGetInTags> tags are still selected
	When I press the back link on the page
	Then I am redirected to the 'Preferred tasks' page
		And the <tasksTags> tags are still selected
	When I press the back link on the page
	Then I am redirected to the 'Personal interests' page
		And the <interestTags> tags are still selected
	When I press the back link on the page
	Then I am redirected to the 'Job level' page
		And the <jobLevelTags> tags are still selected
	When I press the back link on the page
	Then I am redirected to the homepage

	Examples: 
	| jobLevelTags            | interestTags | tasksTags | jobAreasTags | waysToGetInTags  | careerFocusTags | restrictionsTags            |
	| Job level 1,Job level 2 | Music        | Managing  | Transport    | In-work training | None            | physically fit,Enhanced DBS |

	#Then I am redirected to the 'Restrictions' page
	#	And the <restrictionsTags> tags are still selected
	#When I press the back link on the page
	#Then I am redirected to the 'Your career focus' page
	#	And the <careerFocusTags> tags are still selected
	#When I press the back link on the page
	#Then I am redirected to the 'Job areas' page
	#	And the <jobAreasTags> tags are still selected
	#When I press the back link on the page

Scenario Outline: PSF - Selecting None removes already selected tags
	Given that I am viewing the Home page
	When I click on the PSF Continue button
	Then I am redirected to the 'Job level' page
	When I select the tags <jobLevelTags>
	Then the <jobLevelTags> tags are still selected
	When I select the tags <noneTag>
	Then the <jobLevelTags> tags are not selected

	Examples: 
	| jobLevelTags                        | noneTag |
	| Job level 1,Job level 3,Job level 4 | None    |
