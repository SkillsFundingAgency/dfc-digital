Feature: SingleTermDerivatives with suffix returns jobprofiles based upon relevance to serch term entered
	Scenario Outline: [DFC-5229] All JobProfiles which have search term derivative against the Title on DIFFERENT profiles
		Given the following job profiles exist:
			| Title                              | AlternativeTitle           |
			| Astronomer                         |                            |
			| Criminal psychologist              |                            |
			| Childminder                        |                            |
			| Counsellor                         | Therapist, psychotherapist |
			| Optomotrist                        |                            |
			| Police constable                   |                            |
			| Advertising copywriter             |                            |
			| Advertising art director           |                            |
			| Advertising media planner          |                            |
			| Advertising account executive      |                            |
			| Archaeologist                      |                            |
			| Pharmacologist                     |                            |
			| Zoologist                          |                            |
			| Payroll administrator              |                            |
			| Sales administrator                |                            |
			| Pensions administrator             |                            |
			| Database administrator             |                            |
			| Arts administrator                 |                            |
			| Biologist                          |                            |
			| Plant biologist                    |                            |
			| Diplomatic service officer         |                            |
			| Ecologist                          |                            |
			| Farmer                             |                            |
			| Farm worker                        |                            |
			| Farm secretary                     |                            |
			| Fish farmer                        |                            |
			| Colon hydrotherapist               |                            |
			| Hypnotherapist                     |                            |
			| Road traffic accident investigator |                            |
			| Private investigator               |                            |
			| Investment analyst                 |                            |
			| Landscape architect                |                            |
			| Landscaper                         |                            |
			| Landscape gardener                 |                            |
			| Laundry worker                     |                            |
			| Meteorologist                      |                            |
			| Nanotechnologist                   |                            |
			| Youth offending service officer    |                            |
			| Stunt performer                    |                            |
			| Circus performer                   |                            |
			| Print room operator                |                            |
			| Print finisher                     |                            |
			| Health promotion specialist        |                            |
			| Screenwriter                       |                            |
			| Tattooist                          |                            |
			| Architectural technologist         |                            |
			| Garment technologist               |                            |
			| Critical care technologist         |                            |
			| Leather technologist               |                            |
			| Packaging technologist             |                            |
			| Music promotions manager           |                            |
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
			| Technology     | Critical care technologist         |
			| Technology     | Leather technologist               |
			| Technology     | Packaging technologist             |
