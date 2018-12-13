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
		Given there was atleast 1 job profile which have a title '<Title>' exist:
		When I search for search term '<SearchTerm>'
		Then the result will contain more than 1 result and '<Title>' should be in the first page
		Examples: 
			| SearchTerm     | Title                              |
			| Astronomy      | Astronomer                         |
			| Criminology    | Criminal psychologist              |
			| childminding   | Childminder                        |
			| counselling    | Counsellor                         |
			| Optometry      | Optomotrist                        |
			| policing       | Police constable                   |
			| Advertiser     | Advertising copywriter             |
			| Advertiser     | Advertising art director           |
			| Advertiser     | Advertising media planner          |
			| Advertiser     | Advertising account executive      |
			| Archaeology    | Archaeologist                      |
			| pharmacology   | Pharmacologist                     |
			| zoology        | Zoologist                          |
			| administer     | Payroll administrator              |
			| administer     | Sales administrator                |
			| administer     | Pensions administrator             |
			| administer     | Database administrator             |
			| administer     | Arts administrator                 |
			| biology        | Biologist                          |
			| biology        | Plant biologist                    |
			| diplomacy      | Diplomatic service officer         |
			| ecology        | Ecologist                          |
			| farming        | Farmer                             |
			| farming        | Farm worker                        |
			| farming        | Farm secretary                     |
			| farming        | Fish farmer                        |
			| fisheries      | Fish farmer                        |
			| Hydrotherapy   | Colon hydrotherapist               |
			| hypnotherapy   | Hypnotherapist                     |
			| investor       | Road traffic accident investigator |
			| investor       | Private investigator               |
			| investor       | Investment analyst                 |
			| landscaping    | Landscape architect                |
			| landscaping    | Landscaper                         |
			| landscaping    | Landscape gardener                 |
			| laundrette     | Laundry worker                     |
			| Meteorology    | Meteorologist                      |
			| Nanotechnology | Nanotechnologist                   |
			| offenders      | Youth offending service officer    |
			| performance    | Stunt performer                    |
			| performance    | Circus performer                   |
			| printing       | Print room operator                |
			| printing       | Print finisher                     |
			| promoter       | Health promotion specialist        |
			| promoter       | Music promotions manager           |
			| screenwriting  | Screenwriter                       |
			| Tattooer       | Tattooist                          |
			| Technology     | Architectural technologist         |
			| Technology     | Garment technologist               |
			| Technology     | Critical care technologist                  |
			| Technology     | Leather technologist               |
			| Technology     | Packaging technologist             |
