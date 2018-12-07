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
	Scenario Outline: [DFC-5229] All JobProfiles which have search term derivative against the Title on DIFFERENT profiles
		Given there was atleast <ProfileCount> job profile which have a title '<Title>' exist:
		When I search for search term '<SearchTerm>'
		Then the result will contain <ProfileCount> profile greater than or equal to 1
		Examples: 
			| SearchTerm   | Title                 | ProfileCount |
			| Astronomy    | Astronomer            | 1            |
			| Criminology  | Criminal psychologist | 1            |
			| hospitality  | Hospital porter       | 1            |
			| childminding | Childminder           | 1            |
			| counselling  | Counsellor            | 1            |
			| Optometry    | Optomotrist           | 1            |
			| policing     | Police constable      | 1            |
			| Advertiser   | Advertising copywriter, Advertising art director, Advertising media planner, Advertising account executive	| 1			   |
			| Archaeology   | Archaeologist				| 1			   |
			| pharmacology  | Pharmacologist			| 1			   |
			| sociologist   | Social worker				| 1			   |
			| zoology		| Zoologist					| 1			   |
			| acupuncture  |  Acupunturist| 1			   |
			| administer |Admin Assistant, Payroll administrator,Sales administrator, Pensions administrator, Database administrator, Arts administrator| 1			   |
			| biology	|Biologist, Plant biologist| 1			   |
			| dietetics	|Dietitian| 1			   |
			| diplomacy	|Diplomatic service officer| 1			   |
			| ecology	|Ecologist| 1			   | 
			| farming	|Farmer, Farm worker, Farm secretary, Fish farmer| 1			   |
			| fisheries	|Fish farmer| 1			   |
			| Hydrotherapy	|Colon hydrotherapist| 1			   |
			| hypnotherapy	|Hypnotherapist| 1			   |
			| investor|	Road traffic accident investigator, Private investigator,Investment analyst, Clinical psychologist| 1			   |
			| Labortorian	|Laboratory technician| 1			   |
			| landscaping	|Landscape architect, Landscaper, Landscape gardener| 1			   |
			| laundrette	|Laundry worker| 1			   |
			| Meteorology|	Meteorologist| 1			   | 
			| Nanotechnology	|Nanotechnologist| 1			   |
			| offenders	|Youth offending service officer| 1			   |
			| performance|	Stunt performer, Circus performer| 1			   |
			| printing	|Print finisher, Print room operator| 1			   |
			| promoter	|Health promotion specialist, Music promotions manager| 1			   |
			| publicist	|Public finance accountant, Public relations officer, Publican| 1			   |
			| screenwriting|	Screenwriter| 1			   |
			| Tattooer|	Tattooist| 1			   |
			| Technology	|Architectural technologist, Garment technologist, Food technologist, Leather technologist, Packaging technologist| 1			   |


	 