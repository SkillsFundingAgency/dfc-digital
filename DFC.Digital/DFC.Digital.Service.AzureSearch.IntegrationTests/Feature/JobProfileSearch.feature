@JobProfile
Feature: List job profiles based upon relevance to the "search term" entered
[DFC-31] List job profiles based upon relevance to the "search term" entered
[DFC-31 - A1] Job profiles which possess a <Title> or an <AltTitle> which exactly matches the "search term" should be listed first in the list
	1. <Title> and <AltTitle> are equivalent in weighting. (i.e. if the term exactly matches either <Title> or <AltTitle> these are considered equivalent - see [A3])
	2. The <Title> and <AltTitle> should be unique (i.e. it doesn't make sense if the same <Title> or <AltTitle> is used across different profiles - although this isn't prohibited.)
[DFC-31 - A2] Job profiles which possess a <Title> or an <AltTitle> which contains the "search term" should be listed second in the list
[DFC-31 - A3] Job profiles which satisfy the same condition, to the same level, are not listed in any particular order

[DFC-164] I want to be able to see Job Profile/s in my search results
	[DFC-164 - A1] Job profiles which possess a <Title> which exactly matches the "search term" should be listed in the list
	[DFC-336] Bugfix - User uses invalid characters as search term should not see anything
	[DFC-349] Bugfix - User search by a term that contains more than 50 results

	[DFC-167] Navigate using the Next & Previous hyperlinks
	[DFC-167 - A1] The maximum number of results shown on a page should be limited to the <page limit> (i.e. 10)
Covered in UI - [DFC-167 - A2] Only display the "Next" pagination control if the last result is NOT shown on the page
Covered in UI - [DFC-167 - A3] Only display the "Previous" pagination control if the first result is NOT shown on the page
Covered in UI - [DFC-167 - A4] The "Next" pagination control design should match the prototype and contain the following elements: "Next " <current page number>+1 " of " <total page number>
Covered in UI - [DFC-167 - A5] The "Previous" pagination control design should match the prototype and contain the following elements: "Previous " <current page number>-1 " of " <total page number>
Covered in UI - [DFC-167 - A6] Clicking the "Next" pagination control should redirect the user to the next page of the search results
Covered in UI - [DFC-167 - A7] Clicking the "Previous" pagination control should redirect the user to the previous page of the search results

# Scenarios demonstrating the correct application of [DFC-167 - A1]
Scenario Outline: [DFC-167 - A1 - 1] The number of job profiles displayed equals the <TotalNumberOfResults> if the <TotalNumberOfResults> is less than or equal to the <page limit> (i.e. 10)
	Given there are '<NumberOfResults>' profiles which have a Title of '<SearchTerm>'
	When I search using the search term '<SearchTerm>'
	Then the number of job profiles shown on the page is less than or equal to '10'. (i.e. the page limit)
Examples: 
| SearchTerm     | NumberOfResults |
| DFC-167-A1-1-1 | 3               |
| DFC-167-A1-1-2 | 10              |

Scenario Outline: [DFC-167 - A1 - 2] The number of job profiles displayed equals the <page limit> if the <TotalNumberOfResults> is greater than the <page limit> (i.e. 10)
	Given there are '<NumberOfResults>' profiles which have a Title of '<SearchTerm>'
	When I search using the search term '<SearchTerm>' 
	Then the number of job profiles shown on the page is equal to '10'. (i.e. the page limit)
Examples: 
| SearchTerm     | NumberOfResults |
| DFC-167-A1-2-1 | 11              |
| DFC-167-A1-2-2 | 101             |

# REFERENCE: Data Initialisation
#  This is the data which will be added to the index once all of the scenarios have run.
#  If this data is insufficient to support a new scenario, please add to this list
#  prior to including within a scenario's GIVEN statement.
 
#| Title                                 | AlternativeTitle                         | JobProfileSpecialism   | HiddenAlternativeTitle | Category   |
#| Aerospace engineering technician      |                                          |                        |                        |            |
#| CBT practitioners                     | Talking therapist, behavioural therapist |                        |                        |            |
#| Children nurse                        |                                          |                        |                        |            |
#| Children's nurse                      |                                          |                        |                        |            |
#| Counsellor                            | Therapist, psychotherapist               |                        |                        |            |
#| First Job Profile                     | Extra Start                              |                        |                        |            |
#| General practitioner (GP)             | Doc                                      |                        |                        |            |
#| Hotel based job                       |                                          |                        |                        |            |
#| Last Job Profile                      | Extra End                                |                        |                        |            |
#| Last-Job Profile                      | Last-Job-Profile                         |                        |                        |            |
#| Lift engineer                         | Lift technician                          | engineerkw             |                        |            |
#| Marine engineering technician         | Shipbuilding technician                  |                        |                        |            |
#| Middle Job Profile                    | Extra Middle                             |                        |                        |            |
#| Money adviser                         | Debt counsellor                          |                        |                        |            |
#| Motel based job                       |                                          |                        |                        |            |
#| Nail technician                       |                                          | nailingtk, technicaltk |                        |            |
#| Nurse                                 | Adult nurse                              |                        |                        | Healthcare |
#| Pharmacy technician                   | Pharmaceutical technician                |                        |                        |            |
#| Speech and language therapy assistant |                                          |                        |                        |            |
#| Stage technician                      | Stagehand, Stage manager                 |                        |                        |            |
#| Stagehand                             | Stage technician                         |                        |                        |            |
#| Therapist                             | UITest Exact Title                       |                        |                        |            |
#| UITest Exact AltTitle                 | Therapist                                |                        |                        |            |
#| UITest Partial AltTitle               | Physical therapist                       |                        |                        |            |
#| Veterinary nurse                      |                                          |                        |                        |            |
#| Veterinary physiotherapist            | Animal physiotherapist                   |                        |                        |            |
#| Vocal Therapist                       | UITest Partial Title                     |                        |                        |            |
#| Zookeeper                             | Keeper, animal keeper                    |                        |                        |            |
#| Movie operator                        | Addition                                 | Projectionist          |                        |            |
#| Projectionist                         | Operators                                |                        |                        |            |
#| Theater operators                     | Projectionist, film projector            |                        |                        |            |
#| Another technician                    |                                          |                        | engineerhat            |            |
#| Technologist                          |                                          | engineerhat            |                        |            |
 

#[A1] Job profiles which possess a <Title> which exactly matches the "search term" should be listed in the list
# Scenarios demonstrating the correct application of [A1] 
	Scenario: [A1] User uses a search term which contains <Title> on profile(s)
		Given the following job profiles exist:
			| Title              | AlternativeTitle |
			| First Job Profile  | Extra Start      |
			| Nurse              | Adult nurse      |
			| Middle Job Profile | Extra Middle     |
			| Veterinary nurse   |                  |
			| Last Job Profile   | Extra End        |
			| Children nurse     |                  |
			| Children's nurse   |                  |
		When I search using the search term 'Nurse'
		Then the result list will contain '4' profile(s)
		And the profiles are listed in no specific order:
			| Title            | AlternativeTitle |
			| Nurse            | Adult nurse      |
			| Veterinary nurse |                  |
			| Children nurse   |                  |
			| Children's nurse |                  |


# Scenarios demonstrating the correct application of [DFC-31 - A3] for [DFC-31 - A1]
	Scenario: [DFC-31 - A1, A3 - 1] User uses a search term which exactly matches <Title> or <AltTitle> on mutliple profiles
	# This should not happen in practice (see note 2.) but this test is needed to demonstrate the equivalence of terms within [A1]
		Given the following job profiles exist:
			|Title                                 |AlternativeTitle                                |
			|First Job Profile                     |Extra Start                                     |
			|Stagehand                             |Stage technician                                |
			|Middle Job Profile                    |Extra Middle                                    |
			|Stage technician                      |Stagehand, Stage manager                        |
			|Last Job Profile                      |Extra End                                       |
		When I search using the search term 'Stagehand'
		Then the result list will contain '2' profile(s)
		And the profiles are listed in no specific order:
			|Title                                 |AlternativeTitle                                |
			|Stagehand                             |Stage technician                                |
			|Stage technician                      |Stagehand, Stage manager                        |	

	
# Scenarios demonstrating the correct application of [DFC-31 - A2] 	
	Scenario: [DFC-31 - A2 - 1] User uses a search term which is contained within the <Title> on a single profile
		Given the following job profiles exist:
			|Title                                 |AlternativeTitle                                |
			|First Job Profile                     |Extra Start                                     |
			|Aerospace engineering technician      |                                                |
			|Last Job Profile                      |Extra End                                       |
		When I search using the search term 'Aerospace'
		Then the result list will contain '1' profile(s)
		And the profiles are listed in no specific order:
			|Title                                 |AlternativeTitle                                |
			|Aerospace engineering technician      |                                                |	
	
	Scenario: [DFC-31 - A2 - 2] User uses a search term which is contained within the <AltTitle> on a single profile
		Given the following job profiles exist:
			|Title                                 |AlternativeTitle                                |
			|First Job Profile                     |Extra Start                                     |
			|Marine engineering technician         |Shipbuilding technician                         |
			|Last Job Profile                      |Extra End                                       |
		When I search using the search term 'Shipbuilding'
		Then the result list will contain '1' profile(s)
		And the profiles are listed in no specific order:
			|Title                                 |AlternativeTitle                                |
			|Marine engineering technician         |Shipbuilding technician                         |	

# Scenarios demonstrating the correct application of [DFC-31 - A3] for [DFC-31 - A2]	
	Scenario: [DFC-31 - A2, A3 - 1] User uses a search term which is contained within the <Title> on multiple profiles
		Given the following job profiles exist:
			|Title                                 |AlternativeTitle                                |
			|First Job Profile                     |Extra Start                                     |
			|Veterinary nurse                      |                                                |
			|Middle Job Profile                    |Extra Middle                                    |
			|Veterinary physiotherapist            |Animal physiotherapist                          |
			|Last Job Profile                      |Extra End                                       |
		When I search using the search term 'Veterinary'
		Then the result list will contain '2' profile(s)
		And the profiles are listed in no specific order:
			|Title                                 |AlternativeTitle                                |
			|Veterinary nurse                      |                                                |		
			|Veterinary physiotherapist            |Animal physiotherapist                          |
	
	Scenario: [DFC-31 - A2, A3 - 2] User uses a search term which is contained within the <AltTitle> on multiple profiles
		Given the following job profiles exist:
			|Title                                 |AlternativeTitle                                |
			|First Job Profile                     |Extra Start                                     |
			|Veterinary physiotherapist            |Animal physiotherapist                          |
			|Middle Job Profile                    |Extra Middle                                    |
			|Zookeeper                             |Keeper, animal keeper                           |
			|Last Job Profile                      |Extra End                                       |
		When I search using the search term 'animal'
		Then the result list will contain '2' profile(s)
		And the profiles are listed in no specific order:
			|Title                                 |AlternativeTitle                                |
			|Veterinary physiotherapist            |Animal physiotherapist                          |		
			|Zookeeper                             |Keeper, animal keeper                           |	

	Scenario: [DFC-31 - A2, A3 - 3] User uses a search term which is contained within the <Title> OR <AltTitle> on DIFFERENT profiles
		Given the following job profiles exist:
			| Title                            | AlternativeTitle          | JobProfileSpecialism   | HiddenAlternativeTitle |
			| First Job Profile                | Extra Start               |                        |                        |
			| Nail technician                  |                           | nailingtk, technicaltk |                        |
			| Aerospace engineering technician |                           |                        |                        |
			| Pharmacy technician              | Pharmaceutical technician |                        |                        |
			| Middle Job Profile               | Extra Middle              |                        |                        |
			| Marine engineering technician    | Shipbuilding technician   |                        |                        |
			| Lift engineer                    | Lift technician           | engineerkw             |                        |
			| Stagehand                        | Stage technician          |                        |                        |
			| Stage technician                 | Stagehand, Stage manager  |                        |                        |
			| Last Job Profile                 | Extra End                 |                        |                        |
			| Another technician               |                           |                        | engineerhat            |
		When I search using the search term 'technician'
		Then the result list will contain '8' profile(s)
		And the profiles are listed in no specific order:
			| Title                            | AlternativeTitle          | JobProfileSpecialism   | HiddenAlternativeTitle |
			| Nail technician                  |                           | nailingtk, technicaltk |                        |
			| Aerospace engineering technician |                           |                        |                        |
			| Pharmacy technician              | Pharmaceutical technician |                        |                        |
			| Marine engineering technician    | Shipbuilding technician   |                        |                        |
			| Lift engineer                    | Lift technician           | engineerkw             |                        |
			| Stagehand                        | Stage technician          |                        |                        |
			| Stage technician                 | Stagehand, Stage manager  |                        |                        |
			| Another technician               |                           |                        | engineerhat            |            
# Scenarios demonstrating the prioritisation of [DFC-31 - A1] over [DFC-31 - A2]
	Scenario: [DFC-31 - A1, A2 - 1] User uses a search term which exactly matches <Title> AND is contained within the <AltTitle> on DIFFERENT profiles
		Given the following job profiles exist:
			| Title              | AlternativeTitle           |
			| First Job Profile  | Extra Start                |
			| Counsellor         | Therapist, psychotherapist |
			| Middle Job Profile | Extra Middle               |
			| Money adviser      | Debt counsellor            |
			| Last Job Profile   | Extra End                  |
		When I search using the search term 'counsellor'
		Then the result list will contain '2' profile(s)
		And the profiles are listed in no specific order:
			| Title         | AlternativeTitle           |
			| Counsellor    | Therapist, psychotherapist |
			| Money adviser | Debt counsellor            |

	Scenario: [DFC-31 - A1, A2 - 2] User uses a search term which exactly matches <AltTitle> AND is contained within the <Title> on DIFFERENT profiles
	  # The "search term" is literal and no 'varients' will be considered as a partial match. Hence the use of 'contains' rather than 'partial match'
      #    i.e. "engineer" will be considered a partial match against "Lift engineer" or "Marine engineering technician" but NOT "Engine driver"
      #    NOTE: The presence of "Speech and language therapy assistant" is used to demonstrate this against the 'therapist' search - and will not be returned
		Given the following job profiles exist:
			| Title                                 | AlternativeTitle                         |
			| First Job Profile                     | Extra Start                              |
			| Veterinary physiotherapist            | Animal physiotherapist                   |
			| Counsellor                            | Therapist, psychotherapist               |
			| Middle Job Profile                    | Extra Middle                             |
			| CBT practitioners                     | Talking therapist, behavioural therapist |
			| Speech and language therapy assistant |                                          |
			| Last Job Profile                      | Extra End                                |
			| Therapist                             | UITest Exact Title                       |
			| UITest Exact AltTitle                 | Therapist                                |
			| Vocal Therapist                       | UITest Partial Title                     |
			| UITest Partial AltTitle               | Physical therapist                       |
		When I search using the search term 'therapist'
		Then the result list will contain '7' profile(s)
		And the profiles are listed in no specific order:
			| Title                      | AlternativeTitle                         |
			| Counsellor                 | Therapist, psychotherapist               |
			| CBT practitioners          | Talking therapist, behavioural therapist |
			| Veterinary physiotherapist | Animal physiotherapist                   |
			| Therapist                  | UITest Exact Title                       |
			| UITest Exact AltTitle      | Therapist                                |
			| Vocal Therapist            | UITest Partial Title                     |
			| UITest Partial AltTitle    | Physical therapist                       |

	#Scenario: [DFC-31 - A1, A2, A3 - MK1] User uses a search term which is found to be an EXACT match, and a PARTIAL match, against both <Title> and <AlternativeTitle>, on mutliple different profiles
	#	Given the following job profiles exist:
	#		| Title                      | AlternativeTitle                         |
	#		| Veterinary physiotherapist | Animal physiotherapist                   |
	#		| Counsellor                 | Therapist, psychotherapist               |
	#		| CBT practitioners          | Talking therapist, behavioural therapist |
	#		| Therapist                  | UITest Exact Title                       |
	#		| Vocal Therapist            | UITest Partial Title                     |
	#		| UITest Exact AltTitle      | Therapist                                |
	#		| UITest Partial AltTitle    | Physical therapist                       |
	#	When I search using the search term 'Therapist'
	#	Then the result list will contain '6' profile(s)
	#	And the profiles are listed first in no specific order:
	#		| Title                                               | AlternativeTitle                                 |
	#		| Therapist                                 | UITest Exact Title |
	#		| UITest Exact AltTitle | Therapist                              |
	#	And the following profiles are listed in no specific order skip '2' results:
	#		| Title                   | AlternativeTitle                         |
	#		| Counsellor              | Therapist, psychotherapist               |
	#		| CBT practitioners       | Talking therapist, behavioural therapist |
	#		| Vocal Therapist         | UITest Partial Title                     |
	#		| UITest Partial AltTitle | Physical therapist                       |


	Scenario Outline: [DFC-31 - B1] Bugfix - User uses invalid characters as search term should not see anything
	Given the following job profiles exist:
			| Title						| AlternativeTitle	|
			| Extra Start				|					|
			| Nurse                     | Adult nurse		|
			| Middle Job Profile        | Extra Middle		|
			| Veterinary nurse          |					|
			| Last Job Profile          | Extra End			|
			| General practitioner (GP) | Doc				|
			| Hotel based job           |					|
			| Motel based job           |					|
		When I search using the search term '<SearchTerm>'
		Then the result list will contain '<TotalCount>' profile(s)
		Examples: 
			| SearchTerm                | TotalCount |
			| *                         | 10         |
			| Nurs?                     | 4          |
			| Nurse^3                   | 4          |
			| +Profile +Job             | 6          |
			| Nurs~                     | 4          |
			| /[mh]otel/                | 2          |
			
	Scenario Outline: [DFC-31 - B1 - 2 ] Bugfix - User uses non-alphabetic characters within search term should no longer impact results
	Given the following job profiles exist:
			| Title                     | AlternativeTitle |
			| Extra Start               |                  |
			| Nurse                     | Adult nurse      |
			| Middle Job Profile        | Extra Middle     |
			| Veterinary nurse          |                  |
			| Last Job Profile          | Extra End        |
			| General practitioner (GP) | Doc              |
			| Hotel based job           |                  |
			| Motel based job           |                  |
			| Co-ordinator              | Extra End        |
			| Pilot                     | Co-Pilot         |
When I search using the search term '<SearchTerm>'
		Then the result list will contain '<TotalCount>' profile(s)
		Examples: 
			| SearchTerm           | TotalCount |
			| Nurse !Veterinary    | 5          |
			| Nurse - Veterinary   | 5          |
			| Nurse + Veterinary   | 5          |
			| Nurse OR Profile     | 10         |
			| Profile && Job       | 6          |
			| Profile & Job        | 6          |
			| Profile \|\| Job     | 6          |
			| Veterinary AND nurse | 10          |
			| (GP)                 | 1          |
			| Co-ordinator         | 2          |

Scenario Outline: [DFC-1128] Bugfix - Performing a search with text which contains an apostrophe (') is causing 'Server error' 
	Given the following job profiles exist:
			| Title						| AlternativeTitle	|
			| Extra Start				|					|
			| Children's nurse	    	|					|			
			| Middle Job Profile        | Extra Middle		|
			| Children nurse	    	|					|			
		When I search using the search term '<SearchTerm>'
		Then the result list will contain '<TotalCount>' profile(s)
		Examples: 
			| SearchTerm                | TotalCount |			
			| Children's			    | 2          |

Scenario Outline: [DFC-340] Bugfix Performing a search with text within a "<" and ">" causes a "Server Error" (A potentially dangerous Request.Form value was detected) 
	Given the following job profiles exist:
			| Title						| AlternativeTitle	|
			| Extra Start				|					|
			| Children's nurse	    	|					|			
			| Middle Job Profile        | Extra Middle		|
		When I search using the search term '<SearchTerm>'
		Then the result list will contain '<TotalCount>' profile(s)
		Examples: 
			| SearchTerm                | TotalCount |	
			| <Children's nurse>		| 4          |
		   
Scenario: [DFC-1572] Bugfix - Search term contains "-" should return results
		Given the following job profiles exist:
			| Title            | AlternativeTitle |
			| Pilot            | Co-Pilot         |
			| Co-ordinator     | Extra End        |
			| Last-Job Profile | Last-Job-Profile |
		When I search using the search term 'co-o'
		Then the result list will contain '2' profile(s)
		And the profiles are listed in no specific order:
			| Title           | AlternativeTitle |
			| Pilot           | Co-Pilot         |
			| Co-ordinator    | Extra End        |
			
# Scenarios demonstrating the fix of [DFC-349]
	Scenario Outline: [DFC-349] Bugfix - User search by a term that contains more than 50 results
		Given that '<CountOfDummies>' job profiles exist with '<SearchTerm>':
		When I search using the search term '<SearchTerm>'
		Then the result count should match '<CountOfDummies>' 
	Examples:
	| SearchTerm | CountOfDummies |
	| Everything | 1000           |

Scenario: [DFC-1493 - A1] Partial Match - search and match with Title only
	Given the following job profiles exist:
         | Title                     | AlternativeTitle |
         | Money adviser             | Debt counsellor  |
         | General practitioner (GP) | Doc              |
	When I search using the search term 'advis'
	Then the result list will contain '1' profile(s)
	And the profiles are listed in no specific order:
         | Title         | AlternativeTitle |
         | Money adviser | Debt counsellor  | 

Scenario: [DFC-1493 - A2] Partial Match - search and match with Alternative Title only
	Given the following job profiles exist:
         | Title                         | AlternativeTitle          |
         | Stage technician              | Stagehand, Stage manager  |
         | Pharmacy technician           | Pharmaceutical technician |
         | Marine engineering technician | Shipbuilding technician   |
	When I search using the search term 'manag'
    Then the result list will contain '1' profile(s)
	And the profiles are listed in no specific order:
         | Title            | AlternativeTitle         |
         | Stage technician | Stagehand, Stage manager |

Scenario: [DFC-1493 - A3] Partial Match - search and match with both Title Alternative Title only
	Given the following job profiles exist:
         | Title                            | AlternativeTitle           | JobProfileSpecialism   | HiddenAlternativeTitle |
         | Nail technician                  |                            | nailingtk, technicaltk |                        |
         | Lift engineer                    | Lift technician            | engineerkw             |                        |
         | Stagehand                        | Stage technician           |                        |                        |
         | Stage technician                 | Stagehand, Stage manager   |                        |                        |
         | Last Job Profile                 | Extra End                  |                        |                        |
         | Therapist                        | UITest Exact Title         |                        |                        |
         | Aerospace engineering technician |                            |                        |                        |
         | Pharmacy technician              | Pharmaceutical technician  |                        |                        |
         | Counsellor                       | Therapist, psychotherapist |                        |                        |
         | Middle Job Profile               | Extra Middle               |                        |                        |
         | Marine engineering technician    | Shipbuilding technician    |                        |                        |
         | Another technician               |                            |                        | engineerhat             |
	When I search using the search term 'techni'
	Then the result list will contain '8' profile(s)
	And the profiles are listed in no specific order:
         | Title                            | AlternativeTitle          | JobProfileSpecialism   | HiddenAlternativeTitle |
         | Nail technician                  |                           | nailingtk, technicaltk |                        |
         | Lift engineer                    | Lift technician           | engineerkw             |                        |
         | Stagehand                        | Stage technician          |                        |                        |
         | Stage technician                 | Stagehand, Stage manager  |                        |                        |
         | Aerospace engineering technician |                           |                        |                        |
         | Pharmacy technician              | Pharmaceutical technician |                        |                        |
         | Marine engineering technician    | Shipbuilding technician   |                        |                        |
         | Another technician               |                           |                        | engineerhat            |

Scenario: [DFC-1493 - A4] Partial Match - search and dont match both Title Alternative Title only
	Given the following job profiles exist:
         | Title                    | AlternativeTitle            |
         | Vocal Therapist | UITest Partial Title        |
         | UITest Exact AltTitle    | Therapist          |
         | UITest Partial AltTitle  | Physical therapist |
	When I search using the search term 'Program'
	Then the result count should match '0'

Scenario: [DFC-1635 - A1] Match with data that has single Keyword only
	Given the following job profiles exist:
         | Title              | AlternativeTitle | JobProfileSpecialism   | HiddenAlternativeTitle |
         | Nail technician    |                  | nailingtk, technicaltk |                        |
         | Lift engineer      | Lift technician  | engineerkw             |                        |
	When I search using the search term 'engineerkw'
	Then the result list will contain '1' profile(s)
	And the profiles are listed in no specific order:
         | Title              | AlternativeTitle | JobProfileSpecialism | HiddenAlternativeTitle |
         | Lift engineer      | Lift technician  | engineerkw           |                        |


Scenario: [DFC-1635 - A2] Match with data that has multiple Keywords
	Given the following job profiles exist:
         | Title           | AlternativeTitle | JobProfileSpecialism   | HiddenAlternativeTitle |
         | Nail technician |                  | nailingtk, technicaltk |                        |
         | Lift engineer   | Lift technician  | engineerkw             |                        |
	When I search using the search term 'nailingtk, technicaltk'
	Then the result list will contain '1' profile(s)
	And the profiles are listed in no specific order:
         | Title           | AlternativeTitle | JobProfileSpecialism   | HiddenAlternativeTitle |
         | Nail technician |                  | nailingtk, technicaltk |                        |


Scenario: [DFC-1617 - A1] Fuzzy Search - Match incorrectly spelled term against Title, Alt Title and Keyword
	Given the following job profiles exist:
         | Title                         | AlternativeTitle              | JobProfileSpecialism | HiddenAlternativeTitle |
         | Middle Job Profile            | Extra Middle                  |                      |                        |
         | Marine engineering technician | Shipbuilding technician       |                      |                        |
         | Movie operator                | Addition                      | Projectionist        |                        |
         | Projectionist                 | Operators                     |                      |                        |
         | Theater operators             | Projectionist, film projector |                      |                        |
	When I search using the search term 'Projeionist'
    Then the result list will contain '3' profile(s)
	And the profiles are listed in no specific order:
         | Title             | AlternativeTitle              | JobProfileSpecialism | HiddenAlternativeTitle |
         | Movie operator    | Addition                      | Projectionist        |                        |
         | Projectionist     | Operators                     |                      |                        |
         | Theater operators | Projectionist, film projector |                      |                        |

Scenario: [DFC-1495] I want to see all the Job Categories the JP is found in, in search results
Given the following job profiles exist:
	| Title                  | JobProfileCategoriesWithUrl                                                                     |
	| A_DFC_1495_Title_One   | DFC_1495_CategoryOneURL\|DFC_1495_CategoryOne,DFC_1495_CategoryTwoURL\|DFC_1495_CategoryTwo     |
	| B_DFC_1495_Title_Two   | DFC_1495_CategoryOneURL\|DFC_1495_CategoryOne,DFC_1495_CategoryThreeURL\|DFC_1495_CategoryThree |
	| C_DFC_1495_Title_Three | DFC_1495_CategoryTwoURL\|DFC_1495_CategoryTwo |
When I search using the search term 'DFC_1495'
Then the profiles are listed in no specific order:
    | Title                  | JobProfileCategoriesWithUrl																		|
	| A_DFC_1495_Title_One   | DFC_1495_CategoryOneURL\|DFC_1495_CategoryOne,DFC_1495_CategoryTwoURL\|DFC_1495_CategoryTwo     |
	| B_DFC_1495_Title_Two   | DFC_1495_CategoryOneURL\|DFC_1495_CategoryOne,DFC_1495_CategoryThreeURL\|DFC_1495_CategoryThree |
	| C_DFC_1495_Title_Three | DFC_1495_CategoryTwoURL\|DFC_1495_CategoryTwo |


Scenario: [DFC-1987 - A1] JP is tagged by 'JobProfileSpecialism' and 'HiddenAlternativeTitle' with equal search weight. 
	Given the following job profiles exist:
         | Title              | AlternativeTitle | JobProfileSpecialism | HiddenAlternativeTitle |
         | Lift engineer      | Lift technician  | engineerkw           |                        |
         | Another technician |                  |                      | engineerhat            |
         | Technologist       |                  | engineerhat          |                        |
When I search using the search term 'engineerhat'
	Then the result list will contain '2' profile(s)
	And the profiles are listed in no specific order:
         | Title              | AlternativeTitle | JobProfileSpecialism | HiddenAlternativeTitle |
         | Technologist       |                  | engineerhat          |                        |
         | Another technician |                  |                      | engineerhat            |	

Scenario: [DFC-5954 - A1] JP is tagged by 'Overview' and 'JobProfileCategories' with equal search weight. 
	Given the following job profiles exist:
         | Title           | AlternativeTitle | Overview     | JobProfileCategories     |
         | search master   | Lift technician  | enhancements |                          |
         | search analyst  |                  |              | enhancements,derivatives |
         | search engineer |                  |              | e                        |
When I search using the search term 'enhancements'
	Then the result list will contain '2' profile(s)
	And the profiles are listed in no specific order:
         | Title          | AlternativeTitle | Overview     | JobProfileCategories     |
         | search master  | Lift technician  | enhancements |                          |
         | search analyst |                  |              | enhancements,derivatives |
   