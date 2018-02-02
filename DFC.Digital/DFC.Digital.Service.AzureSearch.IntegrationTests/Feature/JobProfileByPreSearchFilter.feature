@JobProfileByPreSearchFilter
Feature: List job profiles by the PSF they are tagged with.

Scenario: [DFC-1940 - A1] List job profiles by the PSF they are tagged with.

Given Given I have the following profiles tagged with the following PSF tags
	| Title                     | Interests            | TrainingRoutes | Enablers           | EntryQualifications | JobAreas | PreferredTaskTypes |
	| A_DFC_1940_Title_One_A1   | food_A1,prison_A1    | College_A1     | driving-license_A1 | level-3_A1          |          |                    |
	| A_DFC_1940_Title_Two_A1   | sport_A1,law_A1      |                | driving-license_A1 | level-8_A1          |          |                    |
	| A_DFC_1940_Title_Three_A1 | travel_A1,science_A1 | College_A1     |                    | level-6_A1          |          |                    |
	| A_DFC_1940_Title_Four_A1  | law_A1               | university_A1  | driving-license_A1 | level-2 _A1         |          |                    |
When I filter with the following PSF items
	| Interests         | TrainingRoutes | Enablers | EntryQualifications | JobAreas | PreferredTaskTypes |
	| food_A1,travel_A1 |                |          |                     |          |                    |
Then the psf search profiles are listed in no specific order:
	| Title                     |
	| A_DFC_1940_Title_Three_A1 |
	| A_DFC_1940_Title_One_A1   |  

Scenario: [DFC-1940 - A2] List job profiles by the PSF they are tagged with.

Given Given I have the following profiles tagged with the following PSF tags
	| Title                     | Interests            | TrainingRoutes | Enablers           | EntryQualifications | JobAreas | PreferredTaskTypes |
	| A_DFC_1940_Title_One_A2   | food_A2,prison_A2    | College_A2     | driving-license_A2 | level-3_A2          |          |                    |
	| A_DFC_1940_Title_Two_A2   | sport_A2,law_A2      |                | driving-license_A2 | level-8_A2          |          |                    |
	| A_DFC_1940_Title_Three_A2 | travel_A2,science_A2 | College_A2     |                    | level-6_A2          |          |                    |
	| A_DFC_1940_Title_Four_A2  | law_A2               | university_A2  | driving-license_A2 | level-2_A2          |          |                    | 
When I filter with the following PSF items
	| Interests | TrainingRoutes | Enablers | EntryQualifications | JobAreas | PreferredTaskTypes |
	|           | College_A2     |          |                     |          |                    |
Then the psf search profiles are listed in no specific order:
	| Title                     |
	| A_DFC_1940_Title_Three_A2 |
	| A_DFC_1940_Title_One_A2   |  

Scenario: [DFC-1940 - A3] List job profiles by the PSF they are tagged with.

Given Given I have the following profiles tagged with the following PSF tags
	| Title                     | Interests            | TrainingRoutes | Enablers           | EntryQualifications | JobAreas | PreferredTaskTypes |
	| A_DFC_1940_Title_One_A3   | food_A3,prison_A3    | College_A3     |                    | level-3_A3          |          |                    |
	| A_DFC_1940_Title_Two_A3   | sport_A3,law_A3      |                |                    | level-8_A3          |          |                    |
	| A_DFC_1940_Title_Three_A3 | travel_A3,science_A3 | College_A3     |                    | level-6_A3          |          |                    |
	| A_DFC_1940_Title_Four_A3  | law_A3               | university_A3  | driving-license_A3 | level-2_A3          |          |                    |
When I filter with the following PSF items
	| Interests | TrainingRoutes | Enablers           | EntryQualifications | JobAreas | PreferredTaskTypes |
	|           |                | driving-license_A3 |                     |          |                    |
Then the psf search profiles are listed in no specific order:
	| Title                    |
	| A_DFC_1940_Title_Four_A3 |
  

Scenario: [DFC-1940 - A4] List job profiles by the PSF they are tagged with.

Given Given I have the following profiles tagged with the following PSF tags
	| Title                     | Interests            | TrainingRoutes | Enablers           | EntryQualifications | JobAreas | PreferredTaskTypes |
	| A_DFC_1940_Title_One_A4   | food_A4,prison_A4    | College_A4     | driving-license_A4 | level-3_A4          |          |                    |
	| A_DFC_1940_Title_Two_A4   | sport_A4,law_A4      |                | driving-license_A4 | level-8_A4          |          |                    |
	| A_DFC_1940_Title_Three_A4 | travel_A4,science_A4 | College_A4     |                    | level-6_A4          |          |                    |
	| A_DFC_1940_Title_Four_A4  | law_A4               | university_A4  | driving-license_A4 | level-2_A4          |          |                    |
When I filter with the following PSF items
	| Interests | TrainingRoutes | Enablers | EntryQualifications | JobAreas | PreferredTaskTypes |
	|           |                |          | level-8_A4          |          |                    |
Then the psf search profiles are listed in no specific order:
	| Title                   |
	| A_DFC_1940_Title_Two_A4 |
 

 Scenario: [DFC-1940 - A5] List job profiles by the PSF they are tagged with.

Given Given I have the following profiles tagged with the following PSF tags
	| Title                     | Interests            | TrainingRoutes        | Enablers                  | EntryQualifications | JobAreas | PreferredTaskTypes |
	| A_DFC_2011_Title_One_A5   | food_A5,prison_A5    | College_A5            | driving-license_A5        | level-3_A5          |          |                    |
	| A_DFC_2011_Title_Two_A5   | sport_A5,law_A5      |                       | driving-license_A5        | level-8_A5          |          |                    |
	| A_DFC_2011_Title_Three_A5 | travel_A5,science_A5 | College_A5            |                           | level-6_A5          |          |                    |
	| A_DFC_2011_Title_Four_A5  | lawall_A5new         | universitythree_A5new | driving-licensefour_A5new | level-2five_A5new   |          |                    |
When I filter with the following PSF items
	| Interests    | TrainingRoutes        | Enablers                  | EntryQualifications | JobAreas | PreferredTaskTypes |
	| lawall_A5new | universitythree_A5new | driving-licensefour_A5new | level-2five_A5new   |          |                    |
Then the psf search profiles are listed in no specific order:
	| Title                    |
	| A_DFC_2011_Title_Four_A5 |
 
Scenario: [DFC-2011 - A1] List job profiles by the PSF job area they are tagged with.

Given Given I have the following profiles tagged with the following PSF tags
	| Title                         | Interests                    | TrainingRoutes    | Enablers               | EntryQualifications | JobAreas    | PreferredTaskTypes         |
	| A_DFC_2011_Title_One_2011A1   | food_2011A1,prison_2011A1    | College_2011A1    | driving-license_2011A1 | level-3_2011A1      | admin_2011A | workingwithkids_2011A1     |
	| A_DFC_2011_Title_Two_2011A1   | sport_2011A1,law_2011A1      |                   | driving-license_2011A1 | level-8_2011A1      |             |                            |
	| A_DFC_2011_Title_Three_2011A1 | travel_2011A1,science_2011A1 | College_2011A1    |                        | level-6_2011A1      | admin_2011A |                            |
	| A_DFC_2011_Title_Four_2011A1  | law_2011A1                   | university_2011A1 | driving-license_2011A1 | level-2 _2011A1     |             | workingwithmachines_2011A1 |
When I filter with the following PSF items
	| Interests | TrainingRoutes | Enablers | EntryQualifications | JobAreas | PreferredTaskTypes                                 | JobAreas | PreferredTaskTypes |
	|           |                |          |                     |          | workingwithkids_2011A1, workingwithmachines_2011A1 |          |                    |
Then the psf search profiles are listed in no specific order:
	| Title                        |
	| A_DFC_2011_Title_One_2011A1  |
	| A_DFC_2011_Title_Four_2011A1 |

Scenario: [DFC-2011 - A2] List job profiles by the PSF preferred Task they are tagged with.

Given Given I have the following profiles tagged with the following PSF tags
	| Title                         | Interests                    | TrainingRoutes    | Enablers               | EntryQualifications | JobAreas        | PreferredTaskTypes        |
	| A_DFC_2011_Title_One_2011A2   | food_2011A2,prison_2011A2    | College_2011A2    | driving-license_2011A2 | level-3_2011A2      | admin_2011A     | workingwithkids_2011A     |
	| A_DFC_2011_Title_Two_2011A2   | sport_2011A2,law_2011A2      |                   | driving-license_2011A2 | level-8_2011A2      | law_2011A2      |                           |
	| A_DFC_2011_Title_Three_2011A2 | travel_2011A2,science_2011A2 | College_2011A2    |                        | level-6_2011A2      | security_2011A2 |                           |
	| A_DFC_2011_Title_Four_2011A2  | law_2011A2                   | university_2011A2 | driving-license_2011A2 | level-2 _2011A2     |                 | workingwithmachines_2011A |
When I filter with the following PSF items
	| Interests | TrainingRoutes | Enablers | EntryQualifications | JobAreas                   | PreferredTaskTypes |
	|           |                |          |                     | law_2011A2,security_2011A2 |                    |
Then the psf search profiles are listed in no specific order:
	| Title                         |
	| A_DFC_2011_Title_Two_2011A2   |
	| A_DFC_2011_Title_Three_2011A2 |
 
 
Scenario: [DFC-2011 - A3] List job profiles by the PSF preferred Task they are tagged with.

Given Given I have the following profiles tagged with the following PSF tags
	| Title                         | Interests                    | TrainingRoutes    | Enablers               | EntryQualifications | JobAreas       | PreferredTaskTypes        |
	| A_DFC_2011_Title_One_2011A3   | food_2011A3,prison_2011A3    | College_2011A3    | driving-license_2011A3 | level-3_2011A3      | admin_2011A    | workingwithkids_2011A     |
	| A_DFC_2011_Title_Two_2011A3   | sport_2011A3,law_2011A3      |                   | driving-license_2011A3 | level-8_2011A3      | law_2011A      |                           |
	| A_DFC_2011_Title_Three_2011A3 | travel_2011A3,science_2011A3 | College_2011A3    |                        | level-6_2011A3      | security_2011A |                           |
	| A_DFC_2011_Title_Four_2011A3  | law_2011A3                   | university_2011A3 | driving-license_2011A3 | level-2 _2011A3     | security_2011All | workingwithmachines_2011All |
When I filter with the following PSF items
	| Interests | TrainingRoutes | Enablers | EntryQualifications | JobAreas         | PreferredTaskTypes          |
	|           |                |          |                     | security_2011All | workingwithmachines_2011All |
Then the psf search profiles are listed in no specific order:
	| Title                        |
	| A_DFC_2011_Title_Four_2011A3 |
 
