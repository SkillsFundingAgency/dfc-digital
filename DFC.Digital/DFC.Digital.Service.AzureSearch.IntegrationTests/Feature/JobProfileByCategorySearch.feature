@JobProfileByCategory
Feature: List job profiles by the catogories that they are in.

Scenario: [DFC-275] List job profiles by the catogories that they are in.

Given the following job profiles in catogories  exist:
	| Title                 | JobProfileCategories                    |
	| A_DFC_275_Title_One   | DFC_275_CategoryOne                     |
	| B_DFC_275_Title_Two   | DFC_275_CategoryOne                     |
	| C_DFC_275_Title_Three | DFC_275_CategoryOne,DFC_275_CategoryTwo |
	| D_DFC_275_Title_Four  | DFC_275_CategoryTwo                     |
When I filter by the category 'DFC_275_CategoryOne'
Then the number of job profiles returned is 3
And the job profiles are listed in the following order
	| Title                 |
	| A_DFC_275_Title_One   |
	| B_DFC_275_Title_Two   |
	| C_DFC_275_Title_Three |



