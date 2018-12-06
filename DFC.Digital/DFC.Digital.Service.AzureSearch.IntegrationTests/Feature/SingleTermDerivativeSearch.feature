			#| Title		 | SearchTerm				| ProfileCount |
			#| Astronomer	 | Astronomy				| 1			   |                                                                                                              
			#| Criminology   | Criminal psychologist	| 1			   |
			#| hospitality   | Hospital porter			| 1			   |
			#| hospitality   | Hospital porter			| 1			   |
			#| childminding  | Childminder				| 1			   |
			#| counselling   | Counsellor				| 1			   |
			#| Optometry     | Optomotrist				| 1			   |
			#| policing      | Police constable			| 1			   |
			#| Advertiser    | Advertising copywriter	| 1			   |
			#| Archaeology   | Archaeologist			| 1			   |
			#| counciller    | Counsellor				| 1			   |
			#| criminalogist | Criminal psychologist	| 1			   |
			#| Criminologist | Criminal psychologist	| 1			   |
			#| pharmacology  | Pharmacologist			| 1			   |
			#| sociologist   | Social worker			| 1			   |
			#| sociology     | Social worker			| 1			   |
			#| zoology       | Zoologist				| 1			   |

Feature: SingleTermDerivatives with suffix returns jobprofiles based upon relevance to serch term entered
	Scenario Outline: [DFC-5229] All JobProfiles which have search term derivative against the <Title> on DIFFERENT profiles
		Given there was atleast <ProfileCount> job profile which have a title '<Title>' exist:
			| SearchTerm	| Title						| ProfileCount |
			| Astronomy		| Astronomer					| 1			   |                                                                                                              
			| Criminology   | Criminal psychologist		| 1			   |
			| hospitality   | Hospital porter			| 1			   |
			| hospitality   | Hospital porter			| 1			   |
			| childminding  | Childminder				| 1			   |
			| counselling   | Counsellor				| 1			   |
			| Optometry     | Optomotrist				| 1			   |
			| policing      | Police constable			| 1			   |
			| Advertiser    | Advertising copywriter	| 1			   |
			| Archaeology   | Archaeologist				| 1			   |
			| counciller    | Counsellor				| 1			   |
			| criminalogist | Criminal psychologist		| 1			   |
			| Criminologist | Criminal psychologist		| 1			   |
			| pharmacology  | Pharmacologist			| 1			   |
			| sociologist   | Social worker				| 1			   |
			| sociology     | Social worker				| 1			   |
			| zoology       | Zoologist					| 1			   |

		When I search for search term '<SearchTerm>'
		Then the result will contain '<ProfileCount>' profile(s) greater than or equal to 1
		Examples: 
			| Title			| ProfileCount |
			| Astronomer	| 1			   |                                                                                                              
			| Criminology   | 1			   |
			| hospitality   | 1			   |
			| childminding  | 1			   |
			| counselling   | 1			   |
			| Optometry     | 1			   |
			| policing      | 1			   |
			| Advertiser    | 1			   |
			| Archaeology   | 1			   |
			| counciller    | 1			   |
			| criminalogist | 1			   |
			| Criminologist | 1			   |
			| pharmacology  | 1			   |
			| sociologist   | 1			   |
			| sociology     | 1			   |
			| zoology       | 1			   |


	 